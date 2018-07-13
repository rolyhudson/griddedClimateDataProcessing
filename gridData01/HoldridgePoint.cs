using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;
namespace gridData01
{
    class HoldridgePoint : DataPoint
    {
        public string[] holdridgeClass { get; set; }
        public int[] holdridgeClassNum { get; set; }
        public int[] precip { get; set; }
        public double[] bioTemp { get; set; }//measured as the mean of all temperatures above freezing, with all temperatures below freezing and above 30 °C adjusted to 0 °C
        public double[] potentialEvapRatio { get; set; }//potential evapotranspiration ratio (PET) to mean total annual precipitation.
        private int totalYrs;
        public Vector3d[] color { get; set; }
        public Point3d[] pointInTrScale;
        public HoldridgePoint()
        {
        }
        public void setPointAsZone(Point3d p,double[] abc)
        {
            this.totalYrs = 1;
            this.precip = new int[this.totalYrs];
            this.bioTemp = new double[this.totalYrs];
            this.potentialEvapRatio = new double[this.totalYrs];
            this.holdridgeClass = new string[this.totalYrs];
            this.holdridgeClassNum = new int[this.totalYrs];
            this.color = new Vector3d[this.totalYrs];
            this.pointInTrScale = new Point3d[this.totalYrs];

            this.pointInTrScale[0] = p;
            this.potentialEvapRatio[0] = abc[0];
            this.precip[0] = (int)abc[1];
            this.bioTemp[0] = abc[2];
            

        }
        public void setZoneClass(string c,int n)
        {
            this.holdridgeClass[0]=c;
            this.holdridgeClassNum[0]=n;
        }
        public void setUpClassification(WeatherPoint wp, int yrs)
        {
            this.totalYrs = yrs;
            this.precip = new int[this.totalYrs];
            this.bioTemp = new double[this.totalYrs];
            this.potentialEvapRatio = new double[this.totalYrs];
            this.holdridgeClass = new string[this.totalYrs];
            this.holdridgeClassNum = new int[this.totalYrs];
            this.color = new Vector3d[this.totalYrs];
            this.pointInTrScale = new Point3d[this.totalYrs];
            calcBioTemp(wp);
            calcPrecipPETR(wp);
        }
        
        private void calcBioTemp(WeatherPoint wp)
        {

            for (int i = 0; i < this.totalYrs; i++)
            {
                double total = 0;
                for (int j = 0; j < 12; j++)
                {
                    if (wp.temp[i, j] > 300 || wp.temp[i, j] < 0) total = total + 0;
                    else total = total + wp.temp[i, j];
                }
                this.bioTemp[i] = Math.Round((total / 12),2);
            }
        }
        private void calcPrecipPETR(WeatherPoint wp)
        {

            for (int i = 0; i < this.totalYrs; i++)
            {
                double total = 0;
                for (int j = 0; j < 12; j++)
                {
                    total = total + wp.precip[i, j];
                }
                //potential evapotranspiration ratio=(Tbio ∗ 58.93)/Pann
                this.precip[i] = (int)(total / 12);
                this.potentialEvapRatio[i] = Math.Round(this.bioTemp[i] * 58.93 / this.precip[i],2);

            }
        }
        public void classify( List<HoldridgePoint> zones, TriVariateLogScale triscale)
        {
            for (int i = 0; i < this.totalYrs; i++)
            {
                Point3d p = triscale.getPointFromABCParams(this.potentialEvapRatio[i], this.precip[i], this.bioTemp[i]);
                this.color[i] = triscale.getVectorColor(this.potentialEvapRatio[i], this.precip[i], this.bioTemp[i]);
                this.pointInTrScale[i] = p;
                double dist = 1000000000;
                int index = 0;
                for (int j = 0; j < zones.Count; j++)
                {
                    if (p.DistanceTo(zones[j].pointInTrScale[0]) < dist)
                    {
                        index = j;
                        dist = p.DistanceTo(zones[j].pointInTrScale[0]);
                    }
                }
                this.holdridgeClass[i] = zones[index].holdridgeClass[0];
                this.holdridgeClassNum[i] = zones[index].holdridgeClassNum[0];
            }
        }
    }
}
