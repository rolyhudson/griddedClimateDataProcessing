using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gridData01
{
    class WeatherClass:ClimateClassification
    {
        private int totalNullPoints;
        public WeatherClass()
        {

        }
        
        public void setDataCollection(int n,int yrs)
        {
            // nis number of datapoints yrs number of years
            this.dataPoints = new List<DataPoint>();
            for(int i=0;i< n;i++)
            {
                this.dataPoints.Add(new WeatherPoint(yrs));
            }
        }
        public void setData(List<WeatherPoint> wps)
        {
            this.totalNullPoints = 0;
            this.dataPoints = new List<DataPoint>();
            for (int i = 0; i < wps.Count; i++)
            {
                for(int j=0;j<this.dataFields.Count;j++)
                {
                    wps[i].checkInRange(this.dataFields[j].name, Convert.ToDouble(this.dataFields[j].min), Convert.ToDouble(this.dataFields[j].max));
                }
                wps[i].checkPointForNulls();
                if (wps[i].isNullPoint) this.totalNullPoints++;
                this.dataPoints.Add(wps[i]);
            }
        }
       
    }
}
