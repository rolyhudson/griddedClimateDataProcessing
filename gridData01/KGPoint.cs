using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gridData01
{
    class KGPoint : DataPoint
    {
        //one classfier parameter per year http://koeppen-geiger.vu-wien.ac.at/pdf/Paper_2006.pdf
        public string[] kgClass { get; set; }
        public string[] kgClassDescriptive { get; set; }
        public int[] kgClassNum { get; set; }
        public double[] Tann { get; set; }
        public double[] Tmax { get; set; }
        public double[] Tmin { get; set; }
        public double[] Pann { get; set; }
        public double[] Pmin { get; set; }
        public double[] Psmin { get; set; }
        public double[] Psmax { get; set; }
        public double[] Pwmin { get; set; }
        public double[] Pwmax { get; set; }
        public double[] Pth { get; set; }
        public int[] Tmon { get; set; }//num months with >=10
        private int totalYrs;
        
        public KGPoint()
        {
            
        }
        public void setUpKGClassification(WeatherPoint gp,LocationPoint lp, int yrs)
        {
            this.totalYrs = yrs;
            this.Tann = new double[this.totalYrs];
            this.Tmax = new double[this.totalYrs];
            this.Tmin = new double[this.totalYrs];
            this.Pann = new double[this.totalYrs];
            this.Pmin = new double[this.totalYrs];
            this.Psmin = new double[this.totalYrs];
            this.Psmax = new double[this.totalYrs];
            this.Pwmin = new double[this.totalYrs];
            this.Pwmax = new double[this.totalYrs];
            this.Pth = new double[this.totalYrs];
            this.Tmon = new int[this.totalYrs];
            this.kgClassNum = new int[this.totalYrs];
            this.kgClass = new string[this.totalYrs];
            this.kgClassDescriptive = new string[this.totalYrs];
            setTempValues(gp);
            setPrecipValues(gp,lp);
            climateClassifier();
            setClassiferNum();
            setClassifierText();
        }
        private void setClassifierText()
        {
            for (int i = 0; i < this.kgClassDescriptive.Length; i++)
            {
                string main = "";
                string precip = "";
                string temp = "";
                foreach(char c in this.kgClass[i])
                {
                    switch(c)
                    {
                        case 'A':
                            main = "equatorial ";
                            break;
                        case 'B':
                            main = "arid ";
                            break;
                        case 'C':
                            main = "warm temperate ";
                            break;
                        case 'D':
                            main = "snow ";
                            break;
                        case 'E':
                            main = "polar ";
                            break;
                        case 'W':
                            precip = "desert ";
                            break;
                        case 'S':
                            precip = "steppe ";
                            break;
                        case 'f':
                            precip = "fully humid ";
                            break;
                        case 's':
                            precip = "summer dry ";
                            break;
                        case 'w':
                            precip = "winter dry ";
                            break;
                        case 'm':
                            precip = "monsoonal ";
                            break;

                        case 'h':
                            temp = "hot arid ";
                            break;
                        case 'k':
                            temp = "cold arid ";
                            break;
                        case 'a':
                            temp = "hot summer ";
                            break;
                        case 'b':
                            temp = "warm summer ";
                            break;
                        case 'c':
                            temp = "cool summer ";
                            break;
                        case 'd':
                            temp = "extremely continental ";
                            break;
                        case 'F':
                            temp = "polar frost ";
                            break;
                        case 'T':
                            temp = "polar tundra ";
                            break;
                    }
                }
                this.kgClassDescriptive[i] = main + precip + temp;
            }
        }
        private void setClassiferNum()
        {
            for (int i = 0; i < this.kgClassNum.Length; i++)
            {
                switch(this.kgClass[i])
                {
                    case "Af":
                        this.kgClassNum[i] = 0;
                        break;
                    case "Am":
                        this.kgClassNum[i] = 1;
                        break;
                    case "As":
                        this.kgClassNum[i] = 2;
                        break;
                    case "Aw":
                        this.kgClassNum[i] = 3;
                        break;
                    case "BWk":
                        this.kgClassNum[i] = 4;
                        break;
                    case "BWh":
                        this.kgClassNum[i] = 5;
                        break;
                    case "BSk":
                        this.kgClassNum[i] = 6;
                        break;
                    case "BSh":
                        this.kgClassNum[i] = 7;
                        break;
                    case "Cfa":
                        this.kgClassNum[i] = 8;
                        break;
                    case "Cfb":
                        this.kgClassNum[i] = 9;
                        break;
                    case "Cfc":
                        this.kgClassNum[i] = 10;
                        break;
                    case "Csa":
                        this.kgClassNum[i] = 11;
                        break;
                    case "Csb":
                        this.kgClassNum[i] = 12;
                        break;
                    case "Csc":
                        this.kgClassNum[i] = 13;
                        break;
                    case "Cwa":
                        this.kgClassNum[i] = 14;
                        break;
                    case "Cwb":
                        this.kgClassNum[i] = 15;
                        break;
                    case "Cwc":
                        this.kgClassNum[i] = 16;
                        break;

                    case "Dfa":
                        this.kgClassNum[i] = 17;
                        break;
                    case "Dfb":
                        this.kgClassNum[i] = 18;
                        break;
                    case "Dfc":
                        this.kgClassNum[i] = 19;
                        break;
                    case "Dfd":
                        this.kgClassNum[i] = 20;
                        break;
                    case "Dsa":
                        this.kgClassNum[i] = 21;
                        break;
                    case "Dsb":
                        this.kgClassNum[i] = 22;
                        break;
                    case "Dsc":
                        this.kgClassNum[i] = 23;
                        break;
                    case "Dsd":
                        this.kgClassNum[i] = 24;
                        break;
                    case "Dwa":
                        this.kgClassNum[i] = 25;
                        break;
                    case "Dwb":
                        this.kgClassNum[i] = 26;
                        break;
                    case "Dwc":
                        this.kgClassNum[i] = 27;
                        break;
                    case "Dwd":
                        this.kgClassNum[i] = 28;
                        break;

                    case "EF":
                        this.kgClassNum[i] = 29;
                        break;
                    case "ET":
                        this.kgClassNum[i] = 30;
                        break;
                }
            }
        }
        private void thirdLetterCandD(int y)
        {
            if (this.Tmax[y] >= 22) this.kgClass[y] = this.kgClass[y] + "a";
            else
            {
                if(this.Tmon[y]>=4) this.kgClass[y] = this.kgClass[y] + "b";
                else
                {
                    if(this.Tmin[y]>-38) this.kgClass[y] = this.kgClass[y] + "c";
                    else
                    {
                        if (this.Tmin[y] <= -38) this.kgClass[y] = this.kgClass[y] + "d";
                    }
                }
            }
        }
        private void climateClassifier()
        {
            //remember the factor of 10 in the original data
            for (int i = 0; i < this.totalYrs; i++)
            {
                if (this.Tmin[i]>= 18) //A equatorial
                {
                    if (this.Pmin[i]  >= 60) this.kgClass[i] = "Af";
                    if (this.Pann[i] >= 25 * (100 - this.Pmin[i])) this.kgClass[i] = "Am";
                    if (this.Psmin[i] < 60) this.kgClass[i] = "As";
                    if (this.Pwmin[i] < 60) this.kgClass[i] = "Aw";
                }
                if(this.Pann[i]<10*this.Pth[i])//B Arid
                {
                    if (this.Pann[i] > 5 * this.Pth[i]) this.kgClass[i] = "BS";
                    if (this.Pann[i] <= 5 * this.Pth[i]) this.kgClass[i] = "BW";
                    if (this.Tann[i] >= 18) this.kgClass[i] = this.kgClass[i] + "h";
                    if (this.Tann[i] < 18) this.kgClass[i] = this.kgClass[i] + "k";

                }
                if(this.Tmin[i]>-3&&this.Tmin[i]<18)//C warm temperate
                {
                    if (this.Psmin[i]<this.Pwmin[i]&&this.Pwmax[i]>3*this.Psmin[i]&&this.Psmin[i]<40) this.kgClass[i] = "Cs";
                    if (this.Pwmin[i]<this.Psmin[i]&&this.Psmax[i]>10*this.Pwmin[i]) this.kgClass[i] = "Cw";
                    else this.kgClass[i] = "Cf";
                    thirdLetterCandD(i);
                    
                }
                if(this.Tmin[i]<=-3)//D snow
                {
                    if (this.Psmin[i] < this.Pwmin[i] && this.Pwmax[i] > 3 * this.Psmin[i] && this.Psmin[i] < 40) this.kgClass[i] = "Ds";
                    if (this.Pwmin[i] < this.Psmin[i] && this.Psmax[i] > 10 * this.Pwmin[i]) this.kgClass[i] = "Dw";
                    else this.kgClass[i] = "Df";

                    thirdLetterCandD(i);
                }
                if(this.Tmax[i]<10)//E Polar
                {
                    if (this.Tmax[i] >= 0 && this.Tmax[i]  < 10) this.kgClass[i] = "ET";
                    if (this.Tmax[i]< 0) this.kgClass[i] = "EF";
                }
            }
            //
        }
        private void setTempValues(WeatherPoint gp)
        {
            
            for(int i=0;i< this.totalYrs; i++)
            {
                double max = -1000;
                double min = 1000;
                double total = 0;
                int over10 = 0;
                for (int j=0;j<12;j++)
                {
                    if (gp.temp[i, j] > max) max = gp.temp[i, j];
                    if (gp.temp[i, j] < min) min = gp.temp[i, j];
                    if (gp.temp[i, j] > 10) over10++;
                    total += gp.temp[i, j];
                }
                this.Tmon[i] = over10;
                this.Tmax[i] = max;
                this.Tmin[i] = min;
                this.Tann[i] = Convert.ToInt16(total / 12);
            }
        }
        private void setPrecipValues(WeatherPoint gp,LocationPoint lp)
        {
            
            for (int i = 0; i < this.totalYrs; i++)
            {
                double smax = -1000;
                double smin = 1000000;
                double wmax = -1000;
                double wmin = 1000000;
                double min = 1000000;
                double total = 0;
                double stotal = 0;
                double wtotal = 0;
                for (int j = 0; j < 12; j++)
                {
                    if (gp.precip[i, j] < min) min = gp.precip[i, j];
                    total += gp.precip[i, j];
                }
                
                if (lp.altitude >= 0)// north hemi
                {
                    //summer n hemi
                    for (int j = 3; j < 9; j++)//april to sept
                    {
                        if (gp.precip[i, j] > smax) smax = gp.precip[i, j];
                        if (gp.precip[i, j] < smin) smin = gp.precip[i, j];
                        stotal += gp.precip[i, j];
                    }
                    //winter n hemi
                    for (int j = 0; j < 12; j++)
                    {
                        if (j < 3 || j > 8)//skip summer months
                        {
                            if (gp.precip[i, j] > wmax) wmax = gp.precip[i, j];
                            if (gp.precip[i, j] < wmin) wmin = gp.precip[i, j];
                            wtotal += gp.precip[i, j];
                        }
                    }
                }
                else // south hemi
                {
                    //winter south hemi
                    for (int j = 3; j < 9; j++)//april to sept
                    {
                        if (gp.precip[i, j] > wmax) wmax = gp.precip[i, j];
                        if (gp.precip[i, j] < wmin) wmin = gp.precip[i, j];
                        wtotal += gp.precip[i, j];

                    }
                    //summer south hemi
                    for (int j = 0; j < 12; j++)
                    {
                        if (j < 3 || j > 8)//skip winter months
                        {
                            if (gp.precip[i, j] > smax) smax = gp.precip[i, j];
                            if (gp.precip[i, j] < smin) smin = gp.precip[i, j];
                            stotal += gp.precip[i, j];
                        }
                    }
                }
                this.Pmin[i] = min;
                this.Pann[i] = total;
                this.Psmax[i] = smax;
                this.Psmin[i] = smin;
                this.Pwmax[i] = wmax;
                this.Pwmin[i] = wmin;
                //see World Map of the Köppen-Geiger climate classification updated http://koeppen-geiger.vu-wien.ac.at/pdf/Paper_2006.pdf
                //for this dryness threshold formula
                if (total > 0)
                {
                    if (wtotal / total > 0.67)
                    {
                        this.Pth[i] = 2 * this.Tann[i];
                    }
                    else
                    {
                        if (stotal / total > 0.67)
                        {
                            this.Pth[i] = 2 * this.Tann[i] + 28;
                        }
                        else
                        {
                            this.Pth[i] = 2 * this.Tann[i] + 14;
                        }
                    }
                }
            }
        }
    } 
}
