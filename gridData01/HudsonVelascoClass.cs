using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using Rhino.Geometry;
using Accord.MachineLearning;
using Accord.Math.Distances;
using Accord.Math;
using Accord.Math.Decompositions;
using Accord.Statistics;
using Accord.Statistics.Analysis;
using Accord.Statistics.Visualizations;
using Accord.Collections;
using Accord.Controls;

using MIConvexHull;

namespace gridData01
{
    class HudsonVelascoClass : ClimateClassification
    {
        TriVariateLogScale hvScale = new TriVariateLogScale();
        public double[] windMaxMin = new double[2];
        public double[] rHumidityMaxMin = new double[2];
        public double[] tempMaxMin = new double[2];
        public double[][] clusterCentroids;
        public KMeansClusterCollection clusters;
        Vector3d noColor;
        Vector3d tempColor;
        Vector3d windColor;
        Vector3d humidColor;
        List<double[]> preObs = new List<double[]>();
        List<int[]> pointYearMonth = new List<int[]>();
        public List<string> clusterLabels = new List<string>();
        double[][] observations;
        //cluster evaluation
        public double[] silhouetteCF;//one per cluster
        public double[] distort;//one per cluster
        double[] deltaUTCITotals;
        double[] deltaIDEAMCITotals;
        double[] deltaAPPTEMPTotals;
        public int[] pointsInCluster;
        public double[] clusterUTCI_avAbsDev;
        public double[] clusterUTCISD;
        public double[] clusterIDEAMCI_avAbsDev;
        public double[] clusterIDEAMCISD;
        public double[] clusterAPPTEMP_avAbsDev;
        public double[] clusterAPPTEMPSD;
        List<List<double[]>> cvectors = new List<List<double[]>>();
        
        public int kClusters;
        public PrincipalComponentAnalysis pca;
        bool doPCA;
        bool normalise;
        bool logScale;
        //tropical
        string outputpath;
        public HudsonVelascoClass(WeatherData data, string climatedata,int k,string outPath,bool dopc, bool norm,bool logs)
        {
            //this constructor to set up the tropical clusters
            this.kClusters = k;

            this.outputpath = outPath;
            this.normalise = norm;
            this.doPCA = dopc;
            this.logScale = logs;
            setHVPoints(data.getClassification(climatedata), data.gridPoints);
            findMaxMinData();
            //setKDataPoints();
            setKDataPointsMultiDimAverage();
            //scatterPlot();
            setPrincipleComponent();
            
            if(this.doPCA)pcaTransform();
            scatterPlot();

            assignPCAdataVectors();
            defineClusters(this.kClusters);
            classify();
            createHulls(true);
            //writeClusterGeometry();
           
            defineClusterLbls();
            writeCentroids();
            setHudsonVelascoFields();
            //evaluateClusterCentroids();
        }
        public HudsonVelascoClass(WeatherData data, string climatedata, int k, string outPath, bool dopc, bool norm, bool logs,int cType,bool averaged)
        {
            //this constructor for singular cluster tests
            //cType 0 = utci
            //cType 1 = app temp
            //cTYpe 2 = ideamic
            this.kClusters = k;

            this.outputpath = outPath;
            this.normalise = norm;
            this.doPCA = dopc;
            this.logScale = logs;
            setHVPoints(data.getClassification(climatedata), data.gridPoints);
            if (averaged) setKDataPointsSingleDimAverage(cType);
            else setKDataPointsSingleDim(cType);
            //scatterPlot();
            setPrincipleComponent();

            if (this.doPCA) pcaTransform();
            //scatterPlot();

            assignPCAdataVectors();
            defineClusters(this.kClusters);
            classify();
            defineClusterSingleDim(cType);
            setHudsonVelascoFields();
            //evaluateClusterCentroidsSingleDim();
        }
        public HudsonVelascoClass(WeatherData data, string climatedata, HudsonVelascoClass hvcprevious)
        {
            //this version is performed on country level data set using another clustering from a bigger dataset
            this.outputpath = hvcprevious.outputpath;
            this.kClusters = hvcprevious.kClusters;
            this.clusterLabels = hvcprevious.clusterLabels;
            this.doPCA = hvcprevious.doPCA;
            this.normalise = hvcprevious.normalise;
            this.logScale = hvcprevious.logScale;
            this.clusters = hvcprevious.clusters;
            this.clusterCentroids = hvcprevious.clusterCentroids;
            this.pca = hvcprevious.pca;
            this.windMaxMin = hvcprevious.windMaxMin;
            this.rHumidityMaxMin = hvcprevious.rHumidityMaxMin;
            this.tempMaxMin = hvcprevious.tempMaxMin;

            setHVPoints(data.getClassification(climatedata), data.gridPoints);

            setKDataPoints();

            if (this.doPCA) pcaTransform();
            assignPCAdataVectors();
            classify();
            //createHulls();
            //writeClusterGeometry();
            setHudsonVelascoFields();
            evaluateClusterCentroids();
        }
        
        private void assignPCAdataVectors()
        {
            HVPoint hp;
            int p = 0;
            int y = 0;
            int m = 0;
            for (int i = 0; i < this.observations.Length; i++)
            {
                p = pointYearMonth[i][0];
                y = pointYearMonth[i][1];
                m = pointYearMonth[i][2];
                hp= (HVPoint)this.dataPoints[p];
                hp.dataVector[y, m] = this.observations[i];
            }
        }
        private void evaluateClusterCentroids()
        {
            silhouetteCF = new double[this.kClusters];
            pointsInCluster = new int[this.kClusters];
            distort = new double[this.kClusters];
            deltaUTCITotals = new double[this.kClusters];
            deltaIDEAMCITotals = new double[this.kClusters];
            deltaAPPTEMPTotals = new double[this.kClusters];
            clusterUTCI_avAbsDev = new double[this.kClusters];
            clusterIDEAMCI_avAbsDev = new double[this.kClusters];
            clusterUTCISD = new double[this.kClusters];
            clusterIDEAMCISD = new double[this.kClusters];
            clusterAPPTEMP_avAbsDev = new double[this.kClusters];
            clusterAPPTEMPSD = new double[this.kClusters];

            for (int i = 0; i < this.kClusters; i++) this.cvectors.Add(new List<double[]>());
            getPointsInCluster();
            getClusterDistortion();
            computeSilhouetteCFs();
            ciCheck();
        }
        private void evaluateClusterCentroidsSingleDim()
        {
            silhouetteCF = new double[this.kClusters];
            pointsInCluster = new int[this.kClusters];
            distort = new double[this.kClusters];
            for (int i = 0; i < this.kClusters; i++) this.cvectors.Add(new List<double[]>());
            getPointsInCluster();
            getClusterDistortion();
            computeSilhouetteCFs();
        }
        private void getClusterDistortion()
        {
            for (int i = 0; i < this.kClusters; i++)
            {
                distort[i] = this.clusters[i].Distortion(this.cvectors[i].ToArray());
            }
        }

        private void getPointsInCluster()
        {
            HVPoint hpi;
            int cindex = 0;
            
            for (int i = 0; i < this.dataPoints.Count; i++)
            {
                if (!this.dataPoints[i].isNullPoint)
                {
                    hpi = (HVPoint)this.dataPoints[i];
                    for (int y = 0; y < hpi.totalYrs; y++)
                    {
                        for (int m = 0; m < hpi.dataVector.GetLength(1); m++)
                        {
                            cindex = hpi.clusterIndex[y, m];
                            this.pointsInCluster[cindex]++;
                            
                            this.cvectors[cindex].Add(hpi.dataVector[y, m]);
                        }
                    }
                }
            }
        }
        private void computeSilhouetteCFs()
        {
            
            HVPoint hpi;
            double[] silHTotals = new double[this.kClusters];
            int cindex = 0;
            for (int i = 0; i < this.dataPoints.Count; i++)
            {
                if (!this.dataPoints[i].isNullPoint)
                {
                    hpi = (HVPoint)this.dataPoints[i];
                    hpi.getSilhouetteCF(this.dataPoints,this.clusters);
                    for(int y=0;y<hpi.totalYrs;y++)
                    {
                        for(int m=0;m<12;m++)
                        {
                            cindex = hpi.clusterIndex[y, m];
                            silHTotals[cindex] += hpi.silhouetteCF[y, m];
                        }
                    }
                }
            }
            for (int i = 0; i < silHTotals.Length; i++)
            {
                this.silhouetteCF[i] = silHTotals[i] / this.pointsInCluster[i];
            }


        }
        private void defineWieghtedClusters(int nClusters)
        {
            Accord.Math.Random.Generator.Seed = 0;

            // Create a new K-Means algorithm with 3 clusters 
            KMeans kmeans = new KMeans(k: nClusters)
            {

                Distance = new WeightedSquareEuclidean(new double[] { 1, 0.33, 0.7 })//{ 1.4, 1.1, 0.6 }
            };
            // Compute the algorithm, retrieving an integer array
            // containing the labels for each of the observations
            this.clusters = kmeans.Learn(observations);
            this.clusterCentroids = this.clusters.Centroids;
        }

        private void defineClusters(int nClusters)
        {
            Accord.Math.Random.Generator.Seed = 0;
            KMeans kmeans = new KMeans(k: nClusters);
            
            // Compute the algorithm, retrieving an integer array
            // containing the labels for each of the observations
            this.clusters = kmeans.Learn(observations);
            //double goodness = this.clusters.Distortion(observations);
            this.clusterCentroids = this.clusters.Centroids;
            
        }
        private void classify()
        {
            HVPoint hp;
            for (int i = 0; i < this.dataPoints.Count; i++)
            {
                if (!this.dataPoints[i].isNullPoint)
                {
                    hp = (HVPoint)this.dataPoints[i];
                     hp.getClusterIndex(this.clusters);
                }
            }
        }
        private void createHulls(bool averaged)
        {
            //sort the points into lists for each cluster
            HVPoint hp;
            List<List<double[]>> pts = new List<List<double[]>>();
            for(int i=0;i<this.clusters.Count;i++)
            {
                List<double[]> cp = new List<double[]>();
                pts.Add(cp);
            }
            int cIndex =0;
            double x, y, z;
            for (int i = 0; i < this.dataPoints.Count; i++)
            {
                if (!this.dataPoints[i].isNullPoint)
                {
                    hp = (HVPoint)this.dataPoints[i];
                    for(int yr=0;yr<hp.totalYrs;yr++)
                    {
                        if (averaged)
                        {
                            cIndex = hp.clusterIndex[0, 0];
                            x = hp.dataVector[0, 0][0];
                            y = hp.dataVector[0, 0][1];
                            z = hp.dataVector[0, 0][2];
                            pts[cIndex].Add(new double[] { x, y, z });
                        }
                        else
                        {
                            for (int m = 0; m < 12; m++)
                            {

                                cIndex = hp.clusterIndex[yr, m];
                                x = hp.dataVector[yr, m][0];
                                y = hp.dataVector[yr, m][1];
                                z = hp.dataVector[yr, m][2];
                                pts[cIndex].Add(new double[] { x, y, z });
                            }
                        }
                    }
                    
                }
            }
            //
            StreamWriter sw = new StreamWriter(this.outputpath+"\\mesh.txt");
            for (int i=0;i<pts.Count;i++)
            {
                
                //points have n-dimensions we need n+1 points minimum
                //so if we have 3d data we need 4 points for a hull
                double[][] vertices;
                if (pts[i].Count == 0)
                {
                    //after the cluster number and move to the next cluster
                    sw.WriteLine(i.ToString());
                    continue;
                }
                if (pts[i].Count < 4)
                {
                    vertices = microCluster(pts[i], 4);
                }
                else
                {
                    vertices = new double[pts[i].Count][];
                    for (var p = 0; p < pts[i].Count; p++)
                    {
                        vertices[p] = pts[i][p];
                    }
                }
                var convexHull = ConvexHull.Create(vertices);
                foreach (var face in convexHull.Faces)
                {
                    sw.WriteLine(Math.Round(face.Vertices[0].Position[0],2).ToString() + "," + Math.Round(face.Vertices[0].Position[1], 2).ToString() + "," + Math.Round(face.Vertices[0].Position[2],2).ToString());
                    sw.WriteLine(Math.Round(face.Vertices[1].Position[0], 2).ToString() + "," + Math.Round(face.Vertices[1].Position[1], 2).ToString() + "," + Math.Round(face.Vertices[1].Position[2],2).ToString());
                    sw.WriteLine(Math.Round(face.Vertices[2].Position[0], 2).ToString() + "," + Math.Round(face.Vertices[2].Position[1], 2).ToString() + "," + Math.Round(face.Vertices[2].Position[2],2).ToString());
                }
                //after the vertices write the cluster number
                sw.WriteLine(i.ToString());
            }
            sw.Close();
        }
        private double[][] microCluster(List<double[]> pts,int minpts)
        {
            int newpoints = minpts - pts.Count;
            List<double[]> newset = new List<double[]>();
            
            double x = 0; double y = 0;double z=0;
            for(int i = 0; i < pts.Count; i++)
            {
                newset.Add(pts[i]);
                x += pts[i][0];
                y += pts[i][1];
                z += pts[i][2];
            }
            x = x / pts.Count;
            y = y / pts.Count;
            z = z / pts.Count;
            Random r = new Random();
            for (int i=0;i<newpoints;i++)
            {
                newset.Add(new double[] {x+r.NextDouble()/100, y+ r.NextDouble() / 100, z + r.NextDouble() / 100 });
            }
            return newset.ToArray();
        }
        private double [] getMaxMin(int type)
        {
            double[] maxMin = new double[] { 1000, -1000 };
            
            HVPoint hp;
            double d = 0;
            for (int i = 0; i < this.dataPoints.Count; i++)
            {
                if (!this.dataPoints[i].isNullPoint)
                {
                    hp = (HVPoint)this.dataPoints[i];
                    for (int y = 0; y < hp.totalYrs; y++)
                    {
                        for (int m = 0; m < 12; m++)
                        {
                            //get the max mins
                            d = hp.getDataByType(type, y, m);
                            if (d < maxMin[0]) maxMin[0]=d;
                            if (d > maxMin[1]) maxMin[1]=d;
                        }
                    }
                }
            }
            return maxMin;
        }
        private void setKDataPointsSingleDimAverage(int type)
        {

            HVPoint hp;
            var maxmin = getMaxMin(type);
            
            for (int i = 0; i < this.dataPoints.Count; i++)
            {
                if (!this.dataPoints[i].isNullPoint)
                {
                    hp = (HVPoint)this.dataPoints[i];
                    int count = 0;
                    double d = 0;
                    double dTotal = 0;
                    for (int y = 0; y < hp.totalYrs; y++)
                    {
                        for (int m = 0; m < 12; m++)
                        {
                            //normalise
                            d = hp.getDataByType(type, y, m);
                            d = (d - maxmin[0]) / (maxmin[1] - maxmin[0]);
                            dTotal += d;
                            count++;
                        }
                    }
                    preObs.Add(new double[] { dTotal/count });
                    pointYearMonth.Add(new int[] { i, 0, 0 });
                }
            }
            //build observations
            this.observations = new double[preObs.Count][];
            for (int p = 0; p < preObs.Count; p++)
            {
                observations[p] = preObs[p];
            }
        }
        private void setKDataPointsMultiDimAverage()
        {

            HVPoint hp;
            

            for (int i = 0; i < this.dataPoints.Count; i++)
            {
                if (!this.dataPoints[i].isNullPoint)
                {
                    hp = (HVPoint)this.dataPoints[i];
                    int count = 0;
                    double t = 0;
                    double r = 0;
                    double w = 0;
                    double tTotal = 0;
                    double rTotal = 0;
                    double wTotal = 0;
                    for (int y = 0; y < hp.totalYrs; y++)
                    {
                        for (int m = 0; m < 12; m++)
                        {
                            //normalise
                            t = hp.temp[y, m];
                            r = hp.rh[y, m];
                            w = hp.windSpd[y, m];

                            t = (t - tempMaxMin[0]) / (tempMaxMin[1] - tempMaxMin[0]);
                            r = (r - rHumidityMaxMin[0]) / (rHumidityMaxMin[1] - rHumidityMaxMin[0]);
                            w = (w - windMaxMin[0]) / (windMaxMin[1] - windMaxMin[0]);
                            tTotal += t;
                            rTotal +=r;
                            wTotal += w;
                            count++;
                        }
                    }
                    preObs.Add(new double[] { tTotal / count,rTotal/count,wTotal/count });
                    pointYearMonth.Add(new int[] { i, 0, 0 });
                }
            }
            //build observations
            this.observations = new double[preObs.Count][];
            for (int p = 0; p < preObs.Count; p++)
            {
                observations[p] = preObs[p];
            }
        }
        private void setKDataPointsSingleDim(int type)
        {
            
            HVPoint hp;
            var maxmin = getMaxMin(type);

            double d = 0;
            for (int i = 0; i < this.dataPoints.Count; i++)
            {
                if (!this.dataPoints[i].isNullPoint)
                {
                    hp = (HVPoint)this.dataPoints[i];
                    
                        for (int y = 0; y < hp.totalYrs; y++)
                        {
                            for (int m = 0; m < 12; m++)
                            {
                                //normalise
                                d = hp.getDataByType(type, y, m);
                                d = (d - maxmin[0]) / (maxmin[1] - maxmin[0]);
                                preObs.Add(new double[] { d });
                                pointYearMonth.Add(new int[] { i, y, m });

                            }
                        }
                    
                }
            }
            //build observations
            this.observations = new double[preObs.Count][];
            for (int p = 0; p < preObs.Count; p++)
            {
                observations[p] = preObs[p];
            }
        }
        private void setPrincipleComponent()
        {
            var method = PrincipalComponentMethod.Center;
            this.pca = new PrincipalComponentAnalysis(method);
            pca.NumberOfInputs = 3;
            pca.NumberOfOutputs = 3;
            pca.Learn(this.observations);
        }
        private void pcaTransform()
        {
           this.observations = pca.Transform(this.observations);
            
        }

        private double[][] principleComponent()
        {
            // Step 2. Subtract the mean
            var method = PrincipalComponentMethod.Standardize;
            // Step 3. Calculate the covariance matrix
            this.pca = new PrincipalComponentAnalysis(method);
            pca.NumberOfInputs = 3;
            pca.NumberOfOutputs = 3;
            pca.Learn(this.observations);
            double[][] actual = pca.Transform(this.observations);
            return actual;
        }
        private void findMaxMinData()
        {
            this.windMaxMin = new double[] { 1000, -1000 };
            this.rHumidityMaxMin = new double[] { 1000, -1000 };
            this.tempMaxMin = new double[] { 1000, -1000 };
            HVPoint hp;
            double t = 0;
            double r = 0;
            double w = 0;
            for (int i = 0; i < this.dataPoints.Count; i++)
            {
                if (!this.dataPoints[i].isNullPoint)
                {
                    hp = (HVPoint)this.dataPoints[i];
                    for (int y = 0; y < hp.totalYrs; y++)
                    {
                        for (int m = 0; m < 12; m++)
                        {
                            t = hp.temp[y, m];
                            r = hp.rh[y, m];
                            w = hp.windSpd[y, m];
                            
                            //get the max mins
                            if (t < tempMaxMin[0]) tempMaxMin[0] = t;
                            if (t > tempMaxMin[1]) tempMaxMin[1] = t;

                            if (r < rHumidityMaxMin[0]) rHumidityMaxMin[0] = r;
                            if (r > rHumidityMaxMin[1]) rHumidityMaxMin[1] = r;

                            if (w < windMaxMin[0]) windMaxMin[0] = w;
                            if (w > windMaxMin[1]) windMaxMin[1] = w;
                        }
                    }
                }
            }
            
        }
        private void setKDataPoints()
        {
            HVPoint hp;
            
            double t = 0;
            double r = 0;
            double w = 0;
            for (int i = 0; i < this.dataPoints.Count; i++)
            {
                if (!this.dataPoints[i].isNullPoint)
                {
                    hp = (HVPoint)this.dataPoints[i];
                    for (int y = 0; y < hp.totalYrs; y++)
                    {
                        for (int m = 0; m < 12; m++)
                        {
                            t = hp.temp[y, m];
                            r = hp.rh[y, m];
                            w = hp.windSpd[y, m];

                            //normalise
                            t = (t - tempMaxMin[0]) / (tempMaxMin[1] - tempMaxMin[0]);
                            r = (r - rHumidityMaxMin[0]) / (rHumidityMaxMin[1] - rHumidityMaxMin[0]);
                            w = (w - windMaxMin[0]) / (windMaxMin[1] - windMaxMin[0]);

                            if (this.logScale)
                            {
                                //make sure no logs of 0
                                t = Math.Log10((t+0.01) );
                                r = Math.Log10((r+0.01)) ;
                                w = Math.Log10((w+0.01) ) ;
                                 

                            }

                                preObs.Add(new double[] { t, r, w });
                                pointYearMonth.Add(new int[] { i, y, m });
                          
                        }
                    }
                }
            }
            //build observations
            this.observations = new double[preObs.Count][];
            for (int p = 0; p < preObs.Count; p++)
            {
                observations[p] = preObs[p];
            }

        }
        private double reverseLog(double v)
        {
            double p = 0;
          
                p = (Math.Pow(10, v)-0.01) ;

            return p;
        }
        public void scatterPlot()
        {
            
            double[] t = new double[this.observations.Length];
            double[] r = new double[this.observations.Length];
            double[] w = new double[this.observations.Length];
            for (int i = 0; i < this.observations.Length; i++)
            {
                t[i] = this.observations[i][0];
                r[i] = this.observations[i][1];
                w[i] = this.observations[i][2];
            }

            graph(t, r, "temperature", "relative humidity");
            graph(t, w, "temperature", "wind");
            graph(r, w, "relative humidity", "wind");

        }
        private void graph(double[] x, double [] y, string xaxis, string yaxis)
        {
            ScatterplotView spv = new ScatterplotView();
            spv.Graph.Width = 1200;
            spv.Graph.Height = 800;
            spv.Graph.GraphPane.XAxis.Title.Text = xaxis;
            spv.Graph.GraphPane.YAxis.Title.Text = yaxis;
            spv.Graph.GraphPane.Title.Text = xaxis+" - "+yaxis;
            spv.Graph.GraphPane.Rect = new RectangleF(0, 0, 1200, 800);
            spv.Dock = DockStyle.Fill;
            var scatter = spv.Graph.GraphPane.AddCurve("", x, y, Color.Black, ZedGraph.SymbolType.Circle);
            scatter.Line.IsVisible = false;
            scatter.Symbol.Size = 1;

           
            spv.Graph.GraphPane.AxisChange();
            //save the file of the chart on disk (PNG format)
            spv.Graph.GraphPane.GetImage().Save(this.outputpath + "\\" + spv.Graph.GraphPane.Title.Text+".jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
           
            spv.Dispose();
        }
        private void ciCheck()
        {
            double[][] cents;
            if (this.doPCA) cents = this.pca.Revert(this.clusterCentroids);
            else cents = this.clusterCentroids;
            double[] centsUTCI = new double[cents.Length];
            double[][] centsData = new double[cents.Length][];
            
            double t = 0;
            double r = 0;
            double w = 0;
            List<List<double>> clusterUTCI = new List<List<double>>();
            List<List<double>> clusterIDEAMCI = new List<List<double>>();
            List<List<double>> clusterAPPTEMP = new List<List<double>>();
            int c = 0;
            foreach (double[] p in cents)
            {
                clusterUTCI.Add(new List<double>());
                clusterIDEAMCI.Add(new List<double>());
                clusterAPPTEMP.Add(new List<double>());
                t = p[0];
                r = p[1];
                w = p[2];
                
                    //revers the log10
                    if (this.logScale)
                    {
                        t = reverseLog(t);
                        r = reverseLog( r);
                        w = reverseLog( w);
                    }
                    //unnormalise
                    t = t * (tempMaxMin[1] - tempMaxMin[0]) + tempMaxMin[0];
                    r = r * (rHumidityMaxMin[1] - rHumidityMaxMin[0] + rHumidityMaxMin[0]);
                    w = w * (windMaxMin[1] - windMaxMin[0]) + windMaxMin[0];
                
                centsUTCI[c] = UTCI.CalcUTCI(t, w, t, r);
                centsData[c] = new double[] {t,r,w };
                c++;
            }
            int index = 0;
            HVPoint hp = new HVPoint();
            double ideamic = 0;
            double apptemp = 0;
            for (int i = 0; i < this.dataPoints.Count; i++)
            {
                if (!this.dataPoints[i].isNullPoint)
                {
                    
                    hp = (HVPoint)this.dataPoints[i];
                    for (int y=0;y<hp.totalYrs;y++)
                    {
                        for(int m=0;m<12;m++)
                        {
                            index = hp.clusterIndex[y, m];

                            //|centroid utci - point utci|
                            //get the utci cluster cell deviation
                            deltaUTCITotals[index] += Math.Abs(centsUTCI[index] - hp.utci[y, m]);

                            //|centroid ideamci - point ideamci|
                            //the cluster defined ideam IC
                            ideamic = UTCI.ideamIC(hp.altitude, centsData[index][0], centsData[index][1], centsData[index][2]);
                            //get the ideam cluster cell deviation
                            deltaIDEAMCITotals[index] += Math.Abs(ideamic - hp.ideamIC[y, m]);

                            //|centroid apptemp - point apptemp|
                            //the cluster defined apptemp
                            apptemp = UTCI.appTemp(centsData[index][0], UTCI.vapourPressFromRH(centsData[index][1], centsData[index][0]), centsData[index][2]);
                            //get the apptemp cluster cell deviation
                            deltaAPPTEMPTotals[index] += Math.Abs(apptemp - hp.appTemp[y, m]);

                            //collect the cluster ICs
                            clusterAPPTEMP[index].Add(hp.appTemp[y, m]);
                            clusterUTCI[index].Add(hp.utci[y, m]);
                            clusterIDEAMCI[index].Add(hp.ideamIC[y, m]);
                        }
                    }
                }
            }
            //StreamWriter sw = new StreamWriter(this.outputpath + "\\clusterMeasures.txt");
            //sw.WriteLine("cluster,utci av abs dev,utci standard deviation,ideam ci av abs dev,ideam ci standard deviation,distortion,silhouette");
            for (int p = 0; p < this.kClusters; p++)
            {
                //average the total absolutes UTCI difference
                this.clusterUTCI_avAbsDev[p] = deltaUTCITotals[p] / this.pointsInCluster[p];
                this.clusterUTCISD[p] = clusterUTCI[p].ToArray().StandardDeviation(true);

                //average the total absolutes ideam difference
                this.clusterIDEAMCI_avAbsDev[p] = deltaIDEAMCITotals[p] / this.pointsInCluster[p];
                this.clusterIDEAMCISD[p] = clusterIDEAMCI[p].ToArray().StandardDeviation(true);

                this.clusterAPPTEMP_avAbsDev[p] = deltaAPPTEMPTotals[p] / this.pointsInCluster[p];
                this.clusterAPPTEMPSD[p] = clusterAPPTEMP[p].ToArray().StandardDeviation(true); 

                //if (this.pointsInCluster[p] == 0)
                //{
                //    sw.WriteLine(p.ToString() + ",cluster not found");
                //}
                //else
                //{
                //    sw.WriteLine(p.ToString() + "," + Math.Round(avAbsDevUTCI, 3).ToString() + ","
                //        + Math.Round(utciSD, 3) + "," + Math.Round(avAbsDevIDEAMCI, 3).ToString() + ","
                //        + Math.Round(ideamICSD, 3) + "," + Math.Round(this.distort[p], 3) + "," + Math.Round(this.silhouetteCF[p], 3));
                //}
            }


            //sw.Close();
        }
        private void defineClusterSingleDim(int type)
        {
            //reverse from pca
            double[][] cents;
            if (this.doPCA) cents = this.pca.Revert(this.clusterCentroids);
            else cents = this.clusterCentroids;
            var maxmin = getMaxMin(type);
            double d = 0;
            foreach (double[] p in cents)
            {
                d = p[0];

                //revers the log10
                if (this.logScale)
                {
                    d = reverseLog(d);
                    
                }
                //unnormalise
                d = d * (maxmin[1] - maxmin[0]) + maxmin[0];
                string name = getTypeName(type);
                
                this.clusterLabels.Add(name+": "+Math.Round(d,2).ToString());
            }
        }
        public string getTypeName(int type)
        {
            string name = "";
            if (type == 0) { name = "UTCI"; }
            if (type == 1) { name = "APP_TEMP"; }
            if (type == 2) { name = "IDEAMIC"; }
            return name;
        }
        private void defineClusterLbls()
        {
            //reverse from pca
            double[][] cents;
            if(this.doPCA) cents = this.pca.Revert(this.clusterCentroids);
            else cents = this.clusterCentroids;

            double t = 0;
            double r = 0;
            double w = 0;
            double utci = 0;
            foreach (double[] p in cents)
            {
                t = p[0];
                r = p[1];
                w = p[2];
                //revers the log10
                if (this.logScale)
                {
                    t = reverseLog(t);
                    r = reverseLog(r);
                    w = reverseLog(w);
                }
                //unnormalise
                
                    t = t * (tempMaxMin[1] - tempMaxMin[0]) + tempMaxMin[0];
                    r = r * (rHumidityMaxMin[1] - rHumidityMaxMin[0] + rHumidityMaxMin[0]);
                    w = w * (windMaxMin[1] - windMaxMin[0]) + windMaxMin[0];

                utci = UTCI.CalcUTCI(t, w, t, r); 
                this.clusterLabels.Add("UTCI: " + Math.Round(utci, 2).ToString() +"(temp: " +Math.Round(t,2).ToString()+" rh: "+ Math.Round(r, 2).ToString() + " w: " + Math.Round(w, 2).ToString() +")");
            }
        }
        private void writeClusterClasses()
        {
            MapTools mt = new MapTools();
            StreamWriter sw = new StreamWriter(this.outputpath+"\\clusterclasses.txt");
            int bf = 0;
            string wind = "";
            double t = 0;
            double r = 0;
            double w = 0;
            foreach (double[] p in this.clusterCentroids) {
                //un normalise the data
                t = p[0]* (tempMaxMin[1] - tempMaxMin[0])+ tempMaxMin[0];
                r = p[1] * (rHumidityMaxMin[1] - rHumidityMaxMin[0])+ rHumidityMaxMin[0];
                w = p[2] * (windMaxMin[1] - windMaxMin[0])+windMaxMin[0];
                bf = mt.setBeaufort(w);
                switch(bf)
                {
                    case 0:
                        wind = "calm";
                        break;
                    case 1:
                        wind = "light air";
                        break;
                    case 2:
                        wind = "light breeze";
                        break;
                    case 3:
                        wind = "gentle breeze";
                        break;
                    case 4:
                        wind = "moderate breeze";
                        break;
                    case 5:
                        wind = "fresh breeze";
                        break;
                    default:
                        wind = "strong breeze";
                        break;
                }
                string lbl = "T" + Math.Round(t, 2).ToString() + "_RH:" + Math.Round(r, 2).ToString() + "_W:" + wind;
                this.clusterLabels.Add(lbl);
                sw.Write(lbl+",");
                    }
            sw.Close();
        }
        private void writeCentroids()
        {
            StreamWriter sw = new StreamWriter(this.outputpath + "\\clusterCentroids.txt");
            for (int c = 0; c < this.clusterCentroids.Length; c++)
            {
                sw.WriteLine(c.ToString() + "," + this.clusterCentroids[c][0].ToString());// + "," + this.clusterCentroids[c][1].ToString() + "," + this.clusterCentroids[c][2].ToString());
            }
            sw.Close();
        }

        private void writeClusterGeometry()
        {
            HVPoint hp = new HVPoint();
            StreamWriter sw = new StreamWriter(this.outputpath + "\\clusterGeo.txt");
            
            for (int i = 0; i < this.dataPoints.Count; i++)
            {
                if (!this.dataPoints[i].isNullPoint)
                {
                    hp = (HVPoint)this.dataPoints[i];
                    for (int y = 0; y < hp.totalYrs; y++)
                    {
                        for (int m = 0; m < 12; m++)
                        {
                            sw.WriteLine(hp.clusterIndex[y, m].ToString() + "," + Math.Round(hp.dataVector[y, m][0], 2).ToString());// + "," + Math.Round(hp.dataVector[y,m][1], 2).ToString() + "," + Math.Round(hp.dataVector[y,m][2], 2).ToString());
                        }
                     }
                }
            }
            sw.Close();
        }
        private Vector3d colorToUnitVector(Color col)
        {
            double x = col.R / 255.0;
            double y = col.G / 255.0;
            double z = col.B / 255.0;
            Vector3d color = new Vector3d(x, y, z);
            return color;
        }
        private Color vectorToColor(Vector3d col)
        {
            double x = col.X * 255;
            double y = col.Y * 255;
            double z = col.Z * 255;
            Color color = Color.FromArgb((int)x, (int)y, (int)z);
            return color;
        }
        
        private void setTriScale()
        {
            //       AT
            //       /\
            //      /  \
            //    vpT----WS
            
            double[] vpTLogScale = new double[] {10, 40};
            double[] wsLogScale = new double[] { 0.01, 30 };
            double[] atLogScale = new double[] { 3, 34 };
            this.hvScale.setScales(vpTLogScale, true, wsLogScale,true, atLogScale, false);
            this.hvScale.setColors(Color.White, Color.Blue, Color.DeepPink);
        }
        private void setHVPoints(ClimateClassification wc, List<LocationPoint> gridPoints)
        {
            for (int i = 0; i < gridPoints.Count; i++)
            {
                if (!wc.dataPoints[i].isNullPoint)
                {
                    HVPoint hvp = new HVPoint();
                    WeatherPoint wp = (WeatherPoint)wc.dataPoints[i];

                    hvp.setUpClassification(wp, wp.temp.GetLength(0),gridPoints[i].altitude);
                    //hvp.classify(this.hvScale);
                    //hvp.setColors(this.noColor, this.tempColor, this.windColor, this.humidColor);
                    //hvp.setRanges(this.tempMaxMin,this.windMaxMin,this.rHumidityMaxMin);
                    //hvp.continuousClassifcation();
                    //hvp.discreteClassification();
                    this.dataPoints.Add(hvp);
                }
            }
        }
        private void setHudsonVelascoFields()
        {
            string[] f = {"clusterIndex" };

            List<string> fields = new List<string>();
            fields.AddRange(f);
            this.setDataFieldSingle("clusterIndex", "", (this.kClusters - 1).ToString(), "0", "K means clustering", this.clusterLabels.ToArray(), true);
         

        string[] c = { "temp","rh","windSpd","appTemp","utci","ideamIC","ideamICClass","altitude","beaufortScale" };
            List<string> fieldsCompact = new List<string>();
            fieldsCompact.AddRange(c);
            this.setDataFieldsCompact(fieldsCompact);
            //setVectorColors();
            //double[] t = new double[] { 5, 35};
            //double[] w = new double[] { 0, 13.8};
            //double[] rh = new double[] { 15, 96 };

            //int[] ti = new int[] { (int)t[0], (int)t[1] };
            //int[] wi = new int[] { (int)w[0], (int)w[1] };
            //int[] rhi = new int[] { (int)rh[0], (int)rh[1] };
            //setMaxMins(t, w, rh);
            //generateHVClasses(ti,wi,rhi);
            //generateHVColors(ti, wi, rhi);
        }
        private void setMaxMins(double[] t, double[] w, double[] rh)
        {
            this.windMaxMin = w;
            this.rHumidityMaxMin = rh;
            this.tempMaxMin = t;
        }
        private void setVectorColors()
        {
            Color white = Color.LightGray;
            Color blue = Color.Blue;
            Color red = Color.Red;
            Color green = Color.Black;
            this.noColor = colorToUnitVector(white);
            this.tempColor = colorToUnitVector(red);
            this.windColor = colorToUnitVector(green);
            this.humidColor = colorToUnitVector(blue);
        }
        private string[] generateHVColors(int[] t, int[] w, int[] rh)
        {
            List<string> rgbs = new List<string>();
            string rgb= "";
            HVPoint hvp = new HVPoint();
            //hvp.setRanges(t.Select(Convert.ToDouble).ToArray(), w.Select(Convert.ToDouble).ToArray(), rh.Select(Convert.ToDouble).ToArray());
            Vector3d col;
            for (int i = t[0]; i <= t[1]; i += 5)
            {
                for (int j = rh[0]; j <= rh[1]; j += 10)
                {
                    for (int p = w[0]; p <= w[1]; p += 2)
                    {
                        col = hvp.colorFromParams(i, p, j);
                        rgb = "rgb(" + (int)(col.X*255) + "," + (int)(col.Y * 255) + "," + (int)(col.Z * 255) + ")";
                        rgbs.Add(rgb);
                    }
                }
            }
            string[] s = rgbs.ToArray();
            return s;
        }
        private string[] generateHVClasses(int[] t, int[] w, int[] rh)
        {
            List<string> names = new List<string>();
            
            string n = "";
            string wind = "";
            int bf = 0;
            MapTools mt = new MapTools();
            for (int i = t[0]; i <= t[1]; i += 5)
            {
                for (int j = rh[0]; j <= rh[1]; j += 10)
                {
                    for (int p = w[0]; p <= w[1]; p += 2)
                    {
                        bf = mt.setBeaufort(p);
                        if (bf == 0 || bf == 1) wind = "calm-light breeze";
                        if (bf == 2 || bf == 3) wind = "light-gentle breeze";
                        if (bf == 4 || bf == 5) wind = "gentle-fresh breeze";
                        if (bf > 5) wind = "strong breeze";
                        n = "T" + i.ToString() + "_RH" + j.ToString() + "_W" + wind;
                        names.Add(n);
                    }
                }
            }

            string[] s = names.ToArray();
            return s;
        }
        private void writeHVPointsColors()
        {
            StreamWriter sw = new StreamWriter("colombiaHVPointsContinuous.csv");
            StreamWriter swData = new StreamWriter("colombiaHVPointDiscrete.csv");
            swData.WriteLine("rh,wspd,appT,vp,temp,vpT");
            Point3d p;
            Vector3d v;
            Vector3d dv;
            HVPoint hp;
            for (int i = 0; i < this.dataPoints.Count; i++)
            {
                if (!this.dataPoints[i].isNullPoint)
                {
                    hp = (HVPoint)this.dataPoints[i];

                    for (int m = 0; m < 12; m++)
                    {
                        p = hp.pointInTrScale[0, m];
                        v = hp.HVcontinousColor[0, m];
                        dv = hp.HVdiscreteColor[0, m];

                        sw.WriteLine(hp.weatherP.temp[0, m] + "," + hp.weatherP.windSpd[0, m] + "," + hp.weatherP.rh[0, m] + "," + v.X.ToString() + "," + v.Y.ToString() + "," + v.Z.ToString());
                        swData.WriteLine(hp.weatherP.temp[0, m] + "," + hp.weatherP.windSpd[0, m] + "," + hp.weatherP.rh[0, m] + "," + dv.X.ToString() + "," + dv.Y.ToString() + "," + dv.Z.ToString()+","+hp.HVclassDescriptor[0,m]);
                    }
                }
                
            }
            sw.Close();
            swData.Close();
        }

    }
}
