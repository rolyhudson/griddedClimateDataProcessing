using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Statistics;

namespace gridData01
{
    class StatsClass : ClimateClassification
    {
        private int yrs;
        public StatsClass(WeatherData data, string climatedata)
        {
            if(climatedata == "climateDataAverages") this.yrs = 1;
            else this.yrs = data.years;
            setStats();
            runStatsPoint(data.getClassification(climatedata), data.gridPoints);
            runStatsDataYear(data.getClassification(climatedata), data.gridPoints); 
            runStatsDataMonth(data.getClassification(climatedata), data.gridPoints);
        }
        
        private void setStats()
        {
            string[] f = { "tempAnnualStdDevPt","tempAnnualStdDevData", "tempAnnualAbsChgData", "tempMonthlyAbsChgPt","tempMonthlyStdDevData", "tempMonthlyAbsChgData",
                "vpAnnualStdDevPt","vpAnnualStdDevData", "vpAnnualAbsChgData", "vpMonthlyAbsChgPt","vpMonthlyStdDevData", "vpMonthlyAbsChgData",
                "rhAnnualStdDevPt","rhAnnualStdDevData", "rhAnnualAbsChgData", "rhMonthlyAbsChgPt","rhMonthlyStdDevData", "rhMonthlyAbsChgData",
                "tminAnnualStdDevPt","tminAnnualStdDevData", "tminAnnualAbsChgData", "tminMonthlyAbsChgPt","tminMonthlyStdDevData", "tminMonthlyAbsChgData",
                "tmaxAnnualStdDevPt","tmaxAnnualStdDevData", "tmaxAnnualAbsChgData", "tmaxMonthlyAbsChgPt","tmaxMonthlyStdDevData", "tmaxMonthlyAbsChgData",
                "trangeAnnualStdDevPt","trangeAnnualStdDevData", "trangeAnnualAbsChgData", "trangeMonthlyAbsChgPt","trangeMonthlyStdDevData", "trangeMonthlyAbsChgData",
                "precipAnnualStdDevPt","precipAnnualStdDevData", "precipAnnualAbsChgData", "precipMonthlyAbsChgPt","precipMonthlyStdDevData", "precipMonthlyAbsChgData" };
            List<string> fields = new List<string>();
            fields.AddRange(f);
            this.setDataFields(fields);
            
           

        }
        private void runStatsPoint(ClimateClassification wc, List<LocationPoint> gridPoints)
        {
            for (int i = 0; i < gridPoints.Count; i++)
            {
                StatsPoint sp = new StatsPoint(this.yrs);
                WeatherPoint wp = (WeatherPoint)wc.dataPoints[i];
                for(int f =0;f< wc.dataFields.Count;f++)
                {
                    sp.statSelect(wp, wc.dataFields[f].name);
                }
              
                this.dataPoints.Add(sp);
            }
        }
        private void runStatsDataYear(ClimateClassification wc, List<LocationPoint> gridPoints)
        {
            for (int y = 0; y < this.yrs; y++)
            {
                for (int f = 0; f < wc.dataFields.Count; f++)
                {
                    double total = 0;
                    int count = 0;
                    string propname = "";
                    List<double> samples = new List<double>();
                    WeatherPoint wp = new WeatherPoint();
                    double[,] d;
                    //looking at annual values we look at each month and all data points
                    for (int m = 0; m < 12; m++)
                    {

                        propname = wc.dataFields[f].name;
                        //loop the points at current month year create sample set and total
                        for (int i = 0; i < gridPoints.Count; i++)
                        {
                            wp = (WeatherPoint)wc.dataPoints[i];
                            d = wp.getProp(wp, propname);
                            if (d[y, m] > -40)
                            {
                                total += d[y, m];
                                samples.Add(d[y, m]);
                                count++;
                            }
                        }

                    }
                    double sd;
                    //sd = 0 if no data?
                    if (count == 0) sd = 0;
                    else sd = Math.Round(Statistics.StandardDeviation(samples), 3);
                    int pCount = 0;
                    double mean = total / count;
                    //loop the stats points at current month year and add the stdev and rel change
                    foreach (StatsPoint sp in this.dataPoints)
                    {
                        wp = (WeatherPoint)wc.dataPoints[pCount];
                        d = wp.getProp(wp, propname);
                        //get the mean for the point
                        samples = new List<double>();
                        for (int i = 0; i < 12; i++) samples.Add(d[y, i]);
                        double am = Statistics.Mean(samples);
                        double relchg;
                        if (am > -40)
                        {
                            relchg = Math.Round(am - mean, 3);
                        }
                        else
                        {
                            relchg = -50;
                        }
                        switch (propname)
                        {

                            case "temp":
                                sp.tempAnnualStdDevData[y] = sd;
                                sp.tempAnnualAbsChgData[y] = relchg;
                                break;
                            case "rh":
                                sp.rhAnnualStdDevData[y] = sd;
                                sp.rhAnnualAbsChgData[y] = relchg;
                                break;
                            case "vp":
                                sp.vpAnnualStdDevData[y] = sd;
                                sp.vpAnnualAbsChgData[y] = relchg;
                                break;
                            case "tmin":
                                sp.tminAnnualStdDevData[y] = sd;
                                sp.tminAnnualAbsChgData[y] = relchg;
                                break;
                            case "tmax":
                                sp.tmaxAnnualStdDevData[y] = sd;
                                sp.tmaxAnnualAbsChgData[y] = relchg;
                                break;
                            case "trange":
                                sp.trangeAnnualStdDevData[y] = sd;
                                sp.trangeAnnualAbsChgData[y] = relchg;
                                break;
                            case "precip":
                                sp.precipAnnualStdDevData[y] = sd;
                                sp.precipAnnualAbsChgData[y] = relchg;
                                break;
                        }
                        pCount++;
                    }
                }
            }
       }
        private void runStatsDataMonth(ClimateClassification wc, List<LocationPoint> gridPoints)
        {
            for(int y =0;y<this.yrs;y++)
            {
                for(int m = 0;m<12;m++)
                {
                    for (int f = 0; f < wc.dataFields.Count; f++)
                    {
                        double total = 0;
                        string propname = wc.dataFields[f].name;
                        List<double> samples = new List<double>();
                        WeatherPoint wp = new WeatherPoint();
                        double[,] d;
                        //loop the points at current month year create sample set and total
                        int count = 0;
                        for (int i = 0; i < gridPoints.Count; i++)
                        {
                            
                            wp = (WeatherPoint)wc.dataPoints[i];
                            d = wp.getProp(wp, propname);
                            if (d[y, m] > -40)
                            {
                                total += d[y, m];
                                samples.Add(d[y, m]);
                                count++;
                            }

                        }

                        double sd;
                        //sd = 0 if no data?
                        if (count == 0) sd = 0;
                        else sd = Math.Round(Statistics.StandardDeviation(samples),3);
                        int pCount = 0;
                        double mean = total / count;
                        //loop the stats points at current month year and add the stdev and rel change
                        foreach (StatsPoint sp in this.dataPoints)
                        {
                            wp = (WeatherPoint)wc.dataPoints[pCount];
                            d = wp.getProp(wp, propname);
                            double relchg;
                            if (d[y, m] > -40)
                            {
                                relchg = Math.Round(d[y, m]-mean, 3);
                            }
                            else
                            {
                                relchg = -50;
                            }
                            switch (propname)
                            {

                                case "temp":
                                    sp.tempMonthlyStdDevData[y, m] = sd;
                                    sp.tempMonthlyAbsChgData[y, m] = relchg;
                                    break;
                                case "rh":
                                    sp.rhMonthlyStdDevData[y, m] = sd;
                                    sp.rhMonthlyAbsChgData[y, m] = relchg;
                                    break;
                                case "vp":
                                    sp.vpMonthlyStdDevData[y, m] = sd;
                                    sp.vpMonthlyAbsChgData[y, m] = relchg;
                                    break;
                                case "tmin":
                                    sp.tminMonthlyStdDevData[y, m] = sd;
                                    sp.tminMonthlyAbsChgData[y, m] = relchg;
                                    break;
                                case "tmax":
                                    sp.tmaxMonthlyStdDevData[y, m] = sd;
                                    sp.tmaxMonthlyAbsChgData[y, m] = relchg;
                                    break;
                                case "trange":
                                    sp.trangeMonthlyStdDevData[y, m] = sd;
                                    sp.trangeMonthlyAbsChgData[y, m] = relchg;
                                    break;
                                case "precip":
                                    sp.precipMonthlyStdDevData[y, m] = sd;
                                    sp.precipMonthlyAbsChgData[y, m] = relchg;
                                    break;
                            }
                            pCount++;
                        }
                    }

                }
            }
            
        }
    }
}
