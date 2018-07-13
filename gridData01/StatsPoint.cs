using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Statistics;

namespace gridData01
{
    class StatsPoint : DataPoint
    {
        //variation for this point through the year so one per year

        //% difference form the annual mean

        //variation for this point compared to all other points in the data set 2d array as we hav one sd per month per year
        //value is the same for all points

        //variation from the average data at month year across all points


        public double[]  tempAnnualStdDevPt { get; set; }
        public double[]  tempAnnualStdDevData { get; set; }
        public double[]  tempAnnualAbsChgData { get; set; }
        public double[,] tempMonthlyAbsChgPt { get; set; }
        public double[,] tempMonthlyStdDevData { get; set; }
        public double[,] tempMonthlyAbsChgData { get; set; }

        public double[] vpAnnualStdDevPt { get; set; }
        public double[] vpAnnualStdDevData { get; set; }
        public double[] vpAnnualAbsChgData { get; set; }
        public double[,] vpMonthlyAbsChgPt { get; set; }
        public double[,] vpMonthlyStdDevData { get; set; }
        public double[,] vpMonthlyAbsChgData { get; set; }

        public double[] rhAnnualStdDevPt { get; set; }
        public double[] rhAnnualStdDevData { get; set; }
        public double[] rhAnnualAbsChgData { get; set; }
        public double[,] rhMonthlyAbsChgPt { get; set; }
        public double[,] rhMonthlyStdDevData { get; set; }
        public double[,] rhMonthlyAbsChgData { get; set; }

        public double[] tminAnnualStdDevPt { get; set; }
        public double[] tminAnnualStdDevData { get; set; }
        public double[] tminAnnualAbsChgData { get; set; }
        public double[,] tminMonthlyAbsChgPt { get; set; }
        public double[,] tminMonthlyStdDevData { get; set; }
        public double[,] tminMonthlyAbsChgData { get; set; }

        public double[] tmaxAnnualStdDevPt { get; set; }
        public double[] tmaxAnnualStdDevData { get; set; }
        public double[] tmaxAnnualAbsChgData { get; set; }
        public double[,] tmaxMonthlyAbsChgPt { get; set; }
        public double[,] tmaxMonthlyStdDevData { get; set; }
        public double[,] tmaxMonthlyAbsChgData { get; set; }

        public double[] trangeAnnualStdDevPt { get; set; }
        public double[] trangeAnnualStdDevData { get; set; }
        public double[] trangeAnnualAbsChgData { get; set; }
        public double[,] trangeMonthlyAbsChgPt { get; set; }
        public double[,] trangeMonthlyStdDevData { get; set; }
        public double[,] trangeMonthlyAbsChgData { get; set; }

        public double[] precipAnnualStdDevPt { get; set; }
        public double[] precipAnnualStdDevData { get; set; }
        public double[] precipAnnualAbsChgData { get; set; }
        public double[,] precipMonthlyAbsChgPt { get; set; }
        public double[,] precipMonthlyStdDevData { get; set; }
        public double[,] precipMonthlyAbsChgData { get; set; }

        private int years;
        public StatsPoint()
        {

        }
        public StatsPoint(int yrs)
        {
            this.years = yrs;
            this.tempAnnualStdDevPt = new double[yrs];
            this.tempAnnualStdDevData = new double[yrs];
            this.tempAnnualAbsChgData = new double[yrs];
            this.tempMonthlyAbsChgPt = new double[yrs, 12];
            this.tempMonthlyStdDevData = new double[yrs, 12];
            this.tempMonthlyAbsChgData = new double[yrs, 12];

            this.vpAnnualStdDevPt = new double[yrs];
            this.vpAnnualStdDevData = new double[yrs];
            this.vpAnnualAbsChgData = new double[yrs];
            this.vpMonthlyAbsChgPt = new double[yrs, 12];
            this.vpMonthlyStdDevData = new double[yrs, 12];
            this.vpMonthlyAbsChgData = new double[yrs, 12];

            this.rhAnnualStdDevPt = new double[yrs];
            this.rhAnnualStdDevData = new double[yrs];
            this.rhAnnualAbsChgData = new double[yrs];
            this.rhMonthlyAbsChgPt = new double[yrs, 12];
            this.rhMonthlyStdDevData = new double[yrs, 12];
            this.rhMonthlyAbsChgData = new double[yrs, 12];

            this.tminAnnualStdDevPt = new double[yrs];
            this.tminAnnualStdDevData = new double[yrs];
            this.tminAnnualAbsChgData = new double[yrs];
            this.tminMonthlyAbsChgPt = new double[yrs, 12];
            this.tminMonthlyStdDevData = new double[yrs, 12];
            this.tminMonthlyAbsChgData = new double[yrs, 12];

            this.tmaxAnnualStdDevPt = new double[yrs];
            this.tmaxAnnualStdDevData = new double[yrs];
            this.tmaxAnnualAbsChgData = new double[yrs];
            this.tmaxMonthlyAbsChgPt = new double[yrs, 12];
            this.tmaxMonthlyStdDevData = new double[yrs, 12];
            this.tmaxMonthlyAbsChgData = new double[yrs, 12];

            this.trangeAnnualStdDevPt = new double[yrs];
            this.trangeAnnualStdDevData = new double[yrs];
            this.trangeAnnualAbsChgData = new double[yrs];
            this.trangeMonthlyAbsChgPt = new double[yrs, 12];
            this.trangeMonthlyStdDevData = new double[yrs, 12];
            this.trangeMonthlyAbsChgData = new double[yrs, 12];

            this.precipAnnualStdDevPt = new double[yrs];
            this.precipAnnualStdDevData = new double[yrs];
            this.precipAnnualAbsChgData = new double[yrs];
            this.precipMonthlyAbsChgPt = new double[yrs, 12];
            this.precipMonthlyStdDevData = new double[yrs, 12];
            this.precipMonthlyAbsChgData = new double[yrs, 12];
        }
        public void statSelect(WeatherPoint wp,string propname)
        {
            Type t = wp.GetType();
            var p = t.GetProperty(propname);
            object ob = p.GetValue(wp, null);
            var d = (Double[,])ob;
            double[] stdev = new double[this.years];
            double[,] relchg = new double[this.years, 12];
            calcPointAnnualStats(d, ref stdev, ref relchg);
            switch (propname)
            {
                
                case "temp":
                    this.tempAnnualStdDevPt = stdev;
                    this.tempMonthlyAbsChgPt = relchg;
                    break;
                case "rh":
                    this.rhAnnualStdDevPt = stdev;
                    this.rhMonthlyAbsChgPt = relchg;
                    break;
                case "vp":
                    this.vpAnnualStdDevPt = stdev;
                    this.vpMonthlyAbsChgPt = relchg;
                    break;
                case "tmin":
                    this.tminAnnualStdDevPt = stdev;
                    this.tminMonthlyAbsChgPt = relchg;
                    break;
                case "tmax":
                    this.tmaxAnnualStdDevPt = stdev;
                    this.tmaxMonthlyAbsChgPt = relchg;
                    break;
                case "trange":
                    this.trangeAnnualStdDevPt = stdev;
                    this.trangeMonthlyAbsChgPt = relchg;
                    break;
                case "precip":
                    this.precipAnnualStdDevPt = stdev;
                    this.precipMonthlyAbsChgPt = relchg;
                    break;
            }
        }
        private void calcPointAnnualStats(double[,] wData,ref double[] annualStdDevPt,ref double[,] annualRelChgPt)
        {
            for (int i = 0; i <wData.GetLength(0); i++)
            {
                List<double> samples = new List<double>();
                double total = 0;
                int count = 0;
                for (int j = 0; j < 12; j++)
                {
                    //check for the missing data -999
                    if (wData[i, j] > -40)
                    {
                        samples.Add(wData[i, j]);
                        total += wData[i, j];
                        count++;
                    }
                }
                double mean = total / count;
                //SD =0 if no data ??
                if (count == 0) annualStdDevPt[i] = 0;
                else annualStdDevPt[i] = Math.Round(Statistics.StandardDeviation(samples), 3);
                for (int j = 0; j < 12; j++)
                {
                    if (wData[i, j] > -40)
                    {
                        annualRelChgPt[i, j] = Math.Round(wData[i, j]-mean, 3);
                    }
                    else
                    {
                        annualRelChgPt[i, j] = -50;
                    }
                    

                }
            }
        }

    }
}
