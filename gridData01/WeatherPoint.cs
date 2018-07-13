using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace gridData01
{
    class WeatherPoint:DataPoint
    {
        public double[,] temp { get; set; }
        public double[,] vp { get; set; }
        public double[,] rh { get; set; }
        public double[,] tmin { get; set; }
        public double[,] tmax { get; set; }
        public double[,] trange { get; set; }
        public double[,] precip { get; set; }
        public double[,] windSpd { get; set; }
        public int years;
        public WeatherPoint()
        {
        }
        
        public WeatherPoint(int yrs)
        {
            this.years = yrs;
            this.temp = new double[yrs, 12];
            this.vp = new double[yrs, 12];
            this.rh = new double[yrs, 12];
            this.tmin = new double[yrs, 12];
            this.tmax = new double[yrs, 12];
            this.trange = new double[yrs, 12];
            this.precip = new double[yrs, 12];
            this.windSpd = new double[yrs, 12];

        }
        public double[,] getProp(WeatherPoint wp, string propname)
        {
            Type t = wp.GetType();
            var p = t.GetProperty(propname);

            object ob = p.GetValue(wp, null);
            double[,] d = (Double[,])ob;

            return d;
        }
        public void writeDataToGridPoint(double[] data, string field, int y)
        {
            switch (field)
            {
                case "temp":
                    for (int i = 0; i < 12; i++)
                    {
                        this.temp[y, i] = data[i];
                    }
                    break;
                case "vp":
                    for (int i = 0; i < 12; i++)
                    {
                        this.vp[y, i] = data[i];
                    }
                    break;
                case "rh":
                    for (int i = 0; i < 12; i++)
                    {
                        this.rh[y, i] = data[i];
                    }
                    break;
                case "tmin":
                    for (int i = 0; i < 12; i++)
                    {
                        this.tmin[y, i] = data[i];
                    }
                    break;
                case "tmax":
                    for (int i = 0; i < 12; i++)
                    {
                        this.tmax[y, i] = data[i];
                    }
                    break;
                case "trange":
                    for (int i = 0; i < 12; i++)
                    {
                        this.trange[y, i] = data[i];
                    }
                    break;
                case "precip":
                    for (int i = 0; i < 12; i++)
                    {
                        this.precip[y, i] = data[i];
                    }
                    break;
                case "windSpd":
                    for (int i = 0; i < 12; i++)
                    {
                        this.windSpd[y, i] = data[i];
                    }
                    break;
            }
        }
        public void checkInRange(string propname,double min,double max)
        {
            Type t = this.GetType();
            var p = t.GetProperty(propname);
            object ob = p.GetValue(this, null);
            var d = (Double[,])ob;
            int outsideRange = 0;
            int total = 0;
            for (int y = 0; y < this.years; y++)
            {
                for (int m = 0; m < 12; m++)
                {
                    Double dv = d[y, m];
                    if (dv < min || dv > max) outsideRange++;
                    total++;
                }
            }
            double percentNull = outsideRange * 1.0 / total * 1.0;
            if (percentNull > 0.9)
            {
                this.isNullPoint = true;
            }
        }
        public void checkPointForNulls()
        {
            int total = 0;
            int nulls = 0;
            for(int y=0;y<this.years;y++)
            {
                for(int m=0;m<12;m++)
                {
                    total+=6;

                    if (this.temp[y, m] == -999 || this.temp[y, m] == -99.9) nulls++;
                       
                    if (this.vp[y, m]==-999 || this.vp[y, m] == -99.9) nulls++;

                    if(this.tmin[y, m]==-999 || this.tmin[y, m] == -99.9) nulls++;
                    if(this.tmax[y, m]==-999 || this.tmax[y, m] == -99.9) nulls++;
                    if(this.trange[y, m]==-999 || this.trange[y, m] == -99.9) nulls++;
                    if(this.precip[y, m]==-999 || this.precip[y, m] == -99.9) nulls++;
                    if (this.windSpd[y, m] == -9999.0 || this.windSpd[y, m] == -99.9) nulls++;
                }
            }
            double percentNull = nulls * 1.0 / total * 1.0;
            if (percentNull > 0.9)
            {
                this.isNullPoint = true;
            }
        }
        public void calcRH(int yr, int month)
        {
            double sp = MapTools.saturationPress(this.temp[yr, month]);
            this.rh[yr, month] = Math.Round(this.vp[yr, month] / sp * 10000,1);

        }
        public void calcRH()
        {
            for(int yr=0;yr<this.temp.GetLength(0);yr++)
            {
                for (int month = 0; month < 12; month ++)
                {
                    double sp = MapTools.saturationPress(this.temp[yr, month]);
                    this.rh[yr, month] = Math.Round(this.vp[yr, month] / sp * 10000,1);

                    //check vp from rh 
                    //= rh / 100 × 6.105 × exp ( 17.27 × Ta / ( 237.7 + Ta ) )
                    double vp = this.rh[yr, month] / 100 * 6.105 * Math.Exp(17.27 * this.temp[yr, month] / (237.7 + this.temp[yr, month]));
                }
            }

        }
    }
}
