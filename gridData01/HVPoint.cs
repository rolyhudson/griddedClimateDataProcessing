using System;
using Rhino.Geometry;
using System.Drawing;
using Accord.MachineLearning;
using Accord.Statistics;
using System.Collections.Generic;
using System.Linq;

namespace gridData01
{
    class HVPoint: DataPoint
    {
        public double[,] temp { get; set; }
        public double[,] rh { get; set; }
        public double[,] windSpd { get; set; }
        public double[,] appTemp { get; set; }
        public double[,] utci { get; set; }
        public double[,] ideamIC { get; set; }
        public int[,] ideamICClass { get; set; }
        public double altitude { get; set; }
        public int[,] beaufortScale { get; set; }
        public int totalYrs;
        public Vector3d[,] HVcontinousColor { get; set; }
        public Vector3d[,] HVdiscreteColor { get; set; }
        public Point3d[,] pointInTrScale;
        public WeatherPoint weatherP;
        public string[,] HVclassDescriptor { get; set; }
        public string[,] clusterClass { get; set; }
        public int[,] clusterIndex { get; set; }
        public double[,] silhouetteCF { get; set; }
        public double[,] cohesion { get; set; }
        public double[,][] dataVector { get; set; }
        Vector3d baseColor;
        Vector3d tColor;
        Vector3d wColor;
        Vector3d rhColor;
        double[] tmaxmin;
        double[] wmaxmin;
        double[] rhmaxmin;
        MapTools mt = new MapTools();
        public HVPoint()
        {
        }
        public void setUpClassification(WeatherPoint wp, int yrs,double a)
        {
            this.weatherP = wp;
            this.totalYrs = yrs;
            this.appTemp = new double[yrs, 12];
            this.temp = new double[yrs, 12];
            this.rh = new double[yrs, 12];
            this.windSpd = new double[yrs, 12];
            this.beaufortScale = new int[yrs, 12];
            this.HVcontinousColor = new Vector3d[this.totalYrs,12];
            this.pointInTrScale = new Point3d[this.totalYrs,12];
            this.HVclassDescriptor = new string[this.totalYrs, 12];
            this.HVdiscreteColor = new Vector3d[this.totalYrs, 12];
            this.clusterIndex = new int[this.totalYrs, 12];
            this.clusterClass = new string[this.totalYrs, 12];
             this.silhouetteCF = new double[this.totalYrs, 12];
            this.cohesion = new double[this.totalYrs, 12];
            this.dataVector = new double[yrs, 12][];
            this.utci = new double[this.totalYrs, 12];
            this.ideamIC = new double[this.totalYrs, 12];
            this.ideamICClass = new int[this.totalYrs, 12];
            this.altitude = a;
            setParamsFromWeather();
            calcApparentTemp();
            calcUTCI();
            calcIDEAMIC();
        }
        public double getDataByType(int type,int y,int m)
        {
            //cType 0 = utci
            //cType 1 = app temp
            //cTYpe 2 = ideamic
            double v = 0;
            switch (type)
            {
                case 0:
                    v = this.utci[y, m];
                    break;
                case 1:
                    v = this.appTemp[y, m];
                    break;
                case 2:
                    v = this.ideamIC[y, m];
                    break;
            }
            return v;
        }
        private double dist3d(double[]p1,double []p2)
        {
            double deltaX = p2[0] - p1[0];
            double deltaY = p2[1] - p1[1];
            double deltaZ = p2[2] - p1[2];
            double dist = Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);
            return dist;
        }
        
        public void getSilhouetteCF(List<DataPoint> dps, KMeansClusterCollection clusters)
        {
            
            HVPoint hpj;
            //calculate cohesion
           //loop years and mnths this point
            for (int y = 0; y < this.totalYrs; y++)
            {
                for (int m = 0; m < 12; m++)
                {
                    double a = 0;//average dist to points in same cluster
                    int aCount = 0;
                    double dist = 0;
                    for (int j = 0; j < dps.Count; j++)
                    {
                        if (!dps[j].isNullPoint)
                        {
                            hpj = (HVPoint)dps[j];
                            
                                    if (hpj.clusterIndex[y, m] == this.clusterIndex[y, m])
                                    {
                                        //is it a single dim clustering?
                                        if (this.dataVector[y, m].Length == 1) dist = Math.Abs(this.dataVector[y, m][0] - hpj.dataVector[y, m][0]);
                                        else dist = dist3d(this.dataVector[y, m], hpj.dataVector[y, m]);
                                        if (dist > 0)//assuming dist =0 when the points are the same.
                                        {
                                            aCount++;
                                            a += dist;
                                        }
                                
                            }

                            
                        }
                    }
                    if(aCount>0) this.cohesion[y, m] = a / aCount;

                }
                
            }
            //calculate separation
            double[] p;
            double b=0;
            int bCount = 0;
                for (int y = 0; y < this.totalYrs; y++)
                {
                    for (int m = 0; m < 12; m++)
                    {
                    double dist = 0;
                    b = 1000000;//closest other cluster
                    
                    for (int c = 0; c < clusters.Count; c++)
                    {
                        if (this.clusterIndex[y, m] == c) continue;
                        p = clusters.Centroids[c];
                        if (this.dataVector[y, m].Length == 1) dist = Math.Abs(this.dataVector[y, m][0] - p[0]);
                        else dist = dist3d(this.dataVector[y, m], p);
                        if (dist < b) b = dist;
                    }
                    bCount++;
                    if (this.cohesion[y, m] > 0)
                    {
                        this.silhouetteCF[y, m] = (b - this.cohesion[y, m]) / Math.Max(this.cohesion[y, m], b);
                        
                    }
                }
            }
            
        }
        public void getClusterIndex(KMeansClusterCollection clusters)
        {
            double[] p;
            MapTools mt = new MapTools();
            double[] pcaVector = new double[3];
            for (int i = 0; i < this.totalYrs; i++)
            {
                for (int j = 0; j < this.dataVector.GetLength(1); j++)
                {
                    this.clusterIndex[i,j] = clusters.Decide(this.dataVector[i,j]);
                }

            }
        }
        public void getAnnualAvgClusterIndex(KMeansClusterCollection clusters)
        {
            double[] p;
            MapTools mt = new MapTools();
            double[] pcaVector = new double[3];
           
            for (int j = 0; j < 12; j++) this.clusterIndex[0, j] = clusters.Decide(this.dataVector[0, 0]);
        }
        public void setColors(Vector3d noColor, Vector3d tempColor, Vector3d windColor, Vector3d humidColor)
        {
            this.baseColor = noColor;

            this.tColor = tempColor;
            this.wColor = windColor;
            this.rhColor = humidColor;
        }
        public void setRanges(double[] tempMaxMin, double[] windMaxMin, double[] rhMaxMin)
        {
            this.tmaxmin = tempMaxMin;
            this.wmaxmin = windMaxMin;
            this.rhmaxmin = rhMaxMin;
        }
        public void discreteClassification()
        {
            int bf = 0;
            int w = 0;
            int t = 0;
            int rh = 0;
            string wind = "";
            
            for (int i = 0; i < this.totalYrs; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    bf = this.mt.setBeaufort(this.weatherP.windSpd[i, j]);

                    t = (int)Math.Round(this.weatherP.temp[i, j] / 5.0) * 5;
                    w= (int)Math.Round(this.weatherP.windSpd[i, j]/2.0)*2;
                    rh = (int)Math.Round(this.weatherP.rh[i, j] / 10.0) * 10;
                    if (bf == 0 || bf == 1) wind = "calm-light breeze";
                    if (bf == 2 || bf == 3) wind = "light-gentle breeze";
                    if (bf == 4 || bf == 5) wind = "gentle-fresh breeze";
                    if(bf>5) wind = "strong breeze";
                    this.HVclassDescriptor[i, j] = "T" + t.ToString() + "_RH" + rh.ToString() + "_W" + wind;
 
                    this.HVdiscreteColor[i,j] = colorFromParams(t, w, rh);
                }

            }
        }

        public void setParamsFromWeather()
        {
            MapTools mt = new MapTools();
            for (int i = 0; i < this.totalYrs; i++)
                {
                    for (int j = 0; j < 12; j++)
                    {
                    this.temp[i, j] = this.weatherP.temp[i, j];
                    this.rh[i, j] = this.weatherP.rh[i, j];
                    this.windSpd[i, j] = this.weatherP.windSpd[i, j];
                    this.beaufortScale[i, j] = mt.setBeaufort(this.windSpd[i, j]);
                }

                }
        }
        public Vector3d colorFromParams(double t, double w, double rh)
        {
            double tParam = parameter(t, tmaxmin[1], tmaxmin[0]);
            double wParam = parameter(w, wmaxmin[1], wmaxmin[0]);
            double rhParam = parameter(rh, rhmaxmin[1], rhmaxmin[0]);

            Vector3d tCol = scaleColorInRange(tParam, this.baseColor,this.tColor);
            Vector3d wCol= scaleColorInRange(wParam, this.baseColor, this.wColor);
            Vector3d rhCol = scaleColorInRange(rhParam, this.baseColor, this.rhColor);
            Vector3d mixColor = geometricMean(tCol, wCol, rhCol);
            return mixColor;
        }
        public Vector3d scaleColorInRange(double p, Vector3d colMin, Vector3d colMax)
        {
            double xRange = colMax.X - colMin.X;
            double yRange = colMax.Y - colMin.Y;
            double zRange = colMax.Z - colMin.Z;
            double x = colMin.X+ p * xRange;
            double y = colMin.Y+ p * yRange;
            double z = colMin.Z+ p * zRange;
            Vector3d color = new Vector3d(x, y, z);
            return color;
        }
        public Vector3d geometricMean(Vector3d col1, Vector3d col2, Vector3d col3)
        {
            //http://www.handprint.com/HP/WCL/color3.html#mixprofile
            //http://scottburns.us/subtractive-color-mixture/
            if (col1.X == 0) col1.X = 1;
            if (col1.Y == 0) col1.Y = 1;
            if (col1.Z == 0) col1.Z = 1;
            if (col2.X == 0) col2.X = 1;
            if (col2.Y == 0) col2.Y = 1;
            if (col2.Z == 0) col2.Z = 1;
            if (col3.X == 0) col3.X = 1;
            if (col3.Y == 0) col3.Y = 1;
            if (col3.Z == 0) col3.Z = 1;
            var r = Math.Sqrt(col1.X * col2.X * col3.X);
            var g = Math.Sqrt(col1.Y * col2.Y * col3.Y);
            var b = Math.Sqrt(col1.Z * col2.Z * col3.Z);
            Vector3d color = new Vector3d(r, g, b);
            return color;
        }
        public double parameter(double v, double max, double min)
        {
            double range = max - min;
            double p = (v - min) / range;
            return p;
        }
        public void calcIDEAMIC()
        {
            for (int i = 0; i < this.totalYrs; i++)
            {
                for (int j = 0; j < 12; j++)
                {

                    this.ideamIC[i, j] = Math.Round(UTCI.ideamIC(this.altitude, this.temp[i, j], this.rh[i, j], this.windSpd[i, j]),2);
                    this.ideamICClass[i,j] = UTCI.ideamICClass(this.ideamIC[i, j]);
                }

            }
        }
        public void calcUTCI()
        {
            for (int i = 0; i < this.totalYrs; i++)
            {
                for (int j = 0; j < 12; j++)
                {

                    this.utci[i, j] = Math.Round(UTCI.CalcUTCI(this.temp[i,j],this.windSpd[i,j]+0.5,this.temp[i,j],this.rh[i,j]),2);
                }

            }
        }
        public void calcApparentTemp()
        {
            for (int i = 0; i < this.totalYrs; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    
                    this.appTemp[i, j] = Math.Round(UTCI.appTemp(this.weatherP.temp[i, j], this.weatherP.vp[i, j], this.weatherP.windSpd[i, j]), 2);
                    
                }
               
            }
        }
        //public void classify(TriVariateLogScale triscale)
        //{
        //    for (int i = 0; i < this.totalYrs; i++)
        //    {
        //        for (int j = 0; j < 12; j++)
        //        {
        //            Point3d p = triscale.getPointFromABCParams(this.weatherP.vp[i, j]/3+this.weatherP.temp[i,j], this.weatherP.windSpd[i, j], this.apparentTemp[i, j]);
        //            thiscolor[i,j] = triscale.getVectorColor(this.weatherP.vp[i, j] / 3 + this.weatherP.temp[i, j], this.weatherP.windSpd[i, j], this.apparentTemp[i, j]);
        //            this.pointInTrScale[i,j] = p;
        //        }
                
        //    }
        //}
    }
}
