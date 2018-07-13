using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gridData01
{
    class WeatherData
    {
        public int srtYr { get; set; }
        public int endYr { get; set; }
        public string description;
        public List<ClimateClassification> classifications = new List<ClimateClassification>();
        public int years;
        public List<LocationPoint> gridPoints = new List<LocationPoint>();
        public int totalNulls;
        public WeatherData()
        {
            

        }
        public void addLocationPoints (List<LocationPoint> lp)
        {
            gridPoints = new List<LocationPoint>();
            for (int i = 0; i < lp.Count; i++)
            {
                this.gridPoints.Add(lp[i]);
            }
        }
        public ClimateClassification getClassification(string descrip)
        {
            ClimateClassification cc = null;
            for (int i = 0; i < this.classifications.Count; i++)
            {
                if (this.classifications[i].description == descrip) cc = this.classifications[i];
            }
            return cc;
        }
        public void flagNullLocations()
        {
            this.totalNulls = 0;
            ClimateClassification cc = getClassification("climateData");
            for(int i=0;i< cc.dataPoints.Count;i++)
            {
                if (cc.dataPoints[i].isNullPoint)
                {
                    this.gridPoints[i].isNullPoint = true;
                    this.totalNulls++;
                }
            }
        }
        public void setTimePeriod(int start,int end)
        {
            this.srtYr = start;
            this.endYr = end;
            this.years = this.endYr - this.srtYr+1;
        }
    }
}
