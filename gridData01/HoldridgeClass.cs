using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Rhino.Geometry;
using System.Drawing;

namespace gridData01
{
    class HoldridgeClass : ClimateClassification
    {
        //SEE The Holdridge life zones of the contignous United States in relation to ecosystem mapping A.E.Lugo1 , S.L.Brown2 , R.Dodson3 , T.S.Smith4 and H.H.Shugart4

        TriVariateLogScale holdridgeScale = new TriVariateLogScale();
        List<HoldridgePoint> hZones = new List<HoldridgePoint>();
        string[] classes = {"Tropical desert","Tropical desert scrub","Tropical thorn woodland","Tropical very dry forest","Tropical dry forest","Tropical moist forest","Tropical wet forest","Tropical rain forest",
            "Subtropical desert","Subtropical desert scrub","Subtropical thorn woodland","Subtropical dry forest","Subtropical moist forest","Subtropical wet forest","Subtropical rain forest",
            "Warm temperate desert","Warm temperate desert scrub","Warm temperate thorn scrub","Warm temperate dry forest","Warm temperate moist forest","Warm temperate wet forest","Warm temperate rain forest",
            "Cool temperate desert","Cool temperate desert scrub","Cool temperate steppe","Cool temperate moist forest","Cool temperate wet forest","Cool temperate rain forest",
            "Boreal desert","Boreal dry scrub","Boreal moist forest","Boreal wet forest","Boreal rain forest",
            "Subpolar dry tundra","Subpolar moist tundra","Subpolar wet tundra","Subpolar rain tundra",
            "Polar desert","Polar desert","Polar desert",
            "Ice","Ice",
            "Ice"};
        public HoldridgeClass(WeatherData data, string climatedata)
        {
            setTriScale();
            
            genHoldridgeLifeZones();
            writeZonesPointsColors();
            setHoldridge();
            runHoldridge(data.getClassification(climatedata), data.gridPoints);
            writeHPointsColors();
        }
        private void genHoldridgeLifeZones()
        {
            //      COLD
            //       /\
            //      /  \
            //   PET----PRECIP 
            //create the 43 predefined zones
            Vector3d v1 = new Vector3d(this.holdridgeScale.C-this.holdridgeScale.A);//vector from PET to cold
            Vector3d v2 = new Vector3d(this.holdridgeScale.C-this.holdridgeScale.B);//vector from Rain to cold
            int rows = 8;
            int nPts = rows;
            v1 = v1 / rows;
            v2 = v2 / rows;
            Vector3d halfV1 = new Vector3d(0,v1.Y/3,0);
            
            Point3d p1 = this.holdridgeScale.A;
            Point3d p2 = this.holdridgeScale.B;
            Point3d p3 = new Point3d();
            Vector3d rowV = new Vector3d();
            Point3d zoneP = new Point3d();
            double dist = 0;
            HoldridgePoint lifeZone;
            double[] abc;
            for (int i = 0; i < rows; i++)
            {
                if (i > 0)
                {
                    p1 = p1 + v1;
                    p2 = p2 + v2;
                }
                rowV = (p2 - p1);
                dist = rowV.Length / nPts;
                rowV.Unitize();
                rowV = rowV * dist;
                Vector3d halfR = new Vector3d(rowV);
                halfR = halfR / 2;

                if (i == 1)
                {
                    //double row for the suptropical / warm temperate range
                    for (int l = 0; l < 2; l++)
                    {
                        for (int j = 0; j < nPts; j++)
                        {
                            if (j == 0) p3 = p1 + halfR;
                            else p3 = p3 + rowV;
                            
                            if (l == 0)
                            {
                                zoneP = (p3 - halfV1);
                            }
                            if (l == 1) zoneP = (p3 + halfV1);
                            abc = this.holdridgeScale.getABCParamsFromPoint(zoneP);
                            lifeZone = new HoldridgePoint();
                            lifeZone.setPointAsZone(zoneP, abc);
                           
                            hZones.Add(lifeZone);
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < nPts; j++)
                    {
                        if (j == 0) p3 = p1 + halfR;
                        else p3 = p3 + rowV;
                        zoneP = (p3);
                        abc = this.holdridgeScale.getABCParamsFromPoint(zoneP);
                        lifeZone = new HoldridgePoint();
                        lifeZone.setPointAsZone(zoneP, abc);
                        
                        hZones.Add(lifeZone);
                    }
                }
                nPts--;

            }
            setClasses();
        }
        private void setClasses()
        {
            for (int i = 0; i < this.hZones.Count; i++)
            {
                this.hZones[i].setZoneClass(this.classes[i],i);
                this.hZones[i].color[0] = this.holdridgeScale.getVectorColor(this.hZones[i].potentialEvapRatio[0], this.hZones[i].precip[0], this.hZones[i].bioTemp[0]);
            }
        }
        private void setHoldridge()
        {
            string[] f = { "holdridgeClass", "holdridgeClassNum", "precip", "bioTemp", "potentialEvapRatio"};
            
            List<string> fields = new List<string>();
            fields.AddRange(f);
            this.setDataFields(fields);

            string[] c = { "holdridgeClass" };
            List<string> fieldsCompact = new List<string>();
            fieldsCompact.AddRange(c);
            this.setDataFieldsCompact(fieldsCompact);

        }
        private void writeZonesPointsColors()
        {
            StreamWriter sw = new StreamWriter("zones.csv");
            Point3d p;
            Vector3d v;
            for(int i=0;i<this.hZones.Count;i++)
            {
                p = this.hZones[i].pointInTrScale[0];
                v = this.hZones[i].color[0];
                sw.WriteLine(p.X.ToString() + "," + p.Y.ToString() + "," + p.Z.ToString() + "," + v.X.ToString() + "," + v.Y.ToString() + "," + v.Z.ToString());
            }
            sw.Close();
        }
        private void writeHPointsColors()
        {
            StreamWriter sw = new StreamWriter("colombiaHPoints.csv");
            Point3d p;
            Vector3d v;
            HoldridgePoint hp;
            for (int i = 0; i < this.dataPoints.Count; i++)
            {
                hp = (HoldridgePoint)this.dataPoints[i];
                p = hp.pointInTrScale[0];
                v = hp.color[0];
                sw.WriteLine(p.X.ToString() + "," + p.Y.ToString() + "," + p.Z.ToString() + "," + v.X.ToString() + "," + v.Y.ToString() + "," + v.Z.ToString());
            }
            sw.Close();
        }
        private void setTriScale()
        {
            double[] petLogScale = new double[] { 0.125,32 };
            double[] precipLogScale = new double[] { 62.5,16000 };
            double[] biotempLogScale = new double[] { 0.001,36 };
            this.holdridgeScale.setScales(petLogScale,false, precipLogScale,false, biotempLogScale,true);
            this.holdridgeScale.setColors(Color.Yellow,Color.Turquoise,Color.Blue);
        }
        private void runHoldridge(ClimateClassification wc, List<LocationPoint> gridPoints)
        {
            for (int i = 0; i < gridPoints.Count; i++)
            {
                HoldridgePoint hp = new HoldridgePoint();
                WeatherPoint wp = (WeatherPoint)wc.dataPoints[i];

                hp.setUpClassification(wp, wp.temp.GetLength(0));
                hp.classify(this.hZones,this.holdridgeScale);
                this.dataPoints.Add(hp);
            }
        }

    }
   
} 

