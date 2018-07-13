using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gridData01
{
    class AverageData
    {
        public WeatherClass datasetAv = new WeatherClass();
        private WeatherClass dataset = new WeatherClass();
        private double sYr;
        private double eYr;
        public AverageData(WeatherData data)
        {
            this.dataset = (WeatherClass)data.getClassification("climateData");
            this.sYr = data.srtYr;
            this.eYr = data.endYr;
            
            averageWeather();
        }
        private void averageWeather()
        {
            this.datasetAv.extendedDescription = "Averaged data year, monthly averages for "+this.sYr + " to " + this.eYr;
            this.datasetAv.description = "climateDataAverages";
            this.datasetAv.dataFields = this.dataset.dataFields;
            int totalYrs = 1;
            double t;
            double v;
            double min;
            double max;
            double range;
            double precip;
            double ws;
            for (int i = 0; i < this.dataset.dataPoints.Count; i++)
            {
                
                
                    WeatherPoint gp = new WeatherPoint(1);
                if (dataset.dataPoints[i].isNullPoint) gp.isNullPoint = true;
                    for (int month = 0; month < 12; month++)
                    {
                        t = 0;
                        v = 0;
                        min = 0;
                        max = 0;
                        range = 0;
                        precip = 0;
                        ws = 0;
                        WeatherPoint wp = (WeatherPoint)this.dataset.dataPoints[i];
                        for (int yr = 0; yr < totalYrs; yr++)
                        {
                            //need to account for null values

                            t += wp.temp[yr, month];
                            v += wp.vp[yr, month];
                            min += wp.tmin[yr, month];
                            max += wp.tmax[yr, month];
                            range += wp.trange[yr, month];
                            precip += wp.precip[yr, month];
                            ws += wp.windSpd[yr, month];

                        }

                        gp.temp[0, month] = Math.Round((t / totalYrs), 1);
                        gp.vp[0, month] = Math.Round((v / totalYrs), 1);
                        gp.tmin[0, month] = Math.Round((min / totalYrs), 1);
                        gp.tmax[0, month] = Math.Round((max / totalYrs), 1);
                        gp.trange[0, month] = Math.Round((range / totalYrs), 1);
                        gp.precip[0, month] = Math.Round((precip / totalYrs), 1);
                        gp.windSpd[0, month] = Math.Round((ws / totalYrs), 2);
                        gp.calcRH(0, month);
                    }
                    this.datasetAv.dataPoints.Add(gp);
                
            }
           
        }
    }
}
