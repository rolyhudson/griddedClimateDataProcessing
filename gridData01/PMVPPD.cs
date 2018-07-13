using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gridData01
{
    class PMVPPD
    {
        
        public double pmv;
        public double ppd;
        public double tcla;//surface temperature of the clothing
        public double hc;
        public double xn;
        public PMVPPD(double ta, double rh)
        {
            //coverted from BASIC program in BS EN ISO 7730 - 2005
            // ta;//air temp
            // tr;//mean radiant temp
            // vel;//relative air velocity
            // rh; //relative humidty
            // met;//Metabolic rate
            // Clo;//Clothing 
            // wme;//External work, normally around 0
            double vel = 0.1;
            double met =1.2;//sedentary activity
            double clo=1;
            double wme = 0;
            double tr = ta; //assuming radiant temp is dry bulb temp  

            double fcl;// clothing area factor
            double pa = rh * 10 * saturatedVapPress(ta);//water vapour pressure Pa

            double icl = 0.155 * clo;//thermal insulation of the clothing in m 2 K/W
            double m = met * 58.15;// metabolic rate in W/m 2
            double w = wme * 58.15;//external work in W/m 2
            double mw = m - w;//internal heat production in the human body
            if (icl <= 0.078) fcl = 1 + (1.29 * icl);
            else fcl = 1.05 + (0.645 * icl);
            double hcf = 12.1 * Math.Sqrt(vel);//heat transfer coeff by frced convection
            double taa = ta + 273;//air temp kelvin
            double tra = tr + 273;//mean radiant temp in kelvin

            //calc surf temp by iteration
            surfTemp(taa, icl, fcl, ta, mw, tra, hcf);

            //heat loss components
            double hl1 = 3.05 * 0.001 * (5733 - 6.99 * mw - pa);//heat loss diff. through skin
            double hl2;//heat loss by sweating (comfort)
            if (mw > 58.15) hl2 = 0.42 * (mw - 58.15);
            else hl2 = 0;
            double hl3 = 1.7 * 0.00001 * m * (5867 - pa);//latent respiration heat loss
            double hl4 = 0.0014 * m * (34 - ta);//dry respiration heat loss
            double hl5 = 3.96 * fcl * (Math.Pow(this.xn, 4) - Math.Pow(tra / 100, 4));// heat loss by radiation
            double hl6 = fcl * this.hc * (this.tcla - ta);// heat loss by convection missing from the BASIC code!
                                                         //calc pmv and ppd
            double ts = 0.303 * Math.Exp(-0.036 * m) + 0.028;//thermal sensation trans coeff
            this.pmv = ts * (mw - hl1 - hl2 - hl3 - hl4 - hl5 - hl6);//predicted mean vote
            this.ppd = 100 - 95 * Math.Exp(-0.03353 * Math.Pow(pmv, 4) - 0.2179 * Math.Pow(pmv, 2));// predicted percentage dissat.
            
		
    }
    private double saturatedVapPress(double t)
    {
        return Math.Exp(16.6536 - 4030.183 / (t + 235));

    }
    private void surfTemp(double taa, double icl, double fcl, double ta, double mw, double tra, double hcf)
    {
        double tcla = taa + (35.5 - ta) / (3.5 * icl + 0.1);//first guess surf temp clothing 

        double p1 = icl * fcl;
        double p2 = p1 * 3.96;
        double p3 = p1 * 100;
        double p4 = p1 * taa;
        double p5 = 308.7 - 0.028 * mw + p2 * Math.Pow(tra / 100, 4);
        double xn = tcla / 100;
        double xf = tcla / 50;
        double n = 0;
        double eps = 0.00015;//stop criteria
        double hcn = 0;
        double hc=0;
        while (Math.Abs(xn - xf) > eps)
        {
            xf = (xf + xn) / 2;
            hcn = 2.38 * Math.Pow(Math.Abs(100 * xf - taa), 0.25);// heat transf. coeff. by natural convection
            if (hcf > hcn) hc = hcf;
            else hc = hcn;
            xn = (p5 + p4 * hc - p2 * Math.Pow(xf, 4)) / (100 + p3 * hc);
            n++;
            if (n > 150)
            {
                //('iterations exceeded');
               
            }
        }
        
        this.tcla = 100 * xn - 273;//surface temperature of the clothing
        this.hc = hc;
        this.xn = xn;
        
    }

    
public double saturationPress(double temp)
        {
            //convert temp C to kelvin
            double kelvin = temp + 273.15;
            double [] c = new double[7];
            double pascals = 0;
            if (kelvin < 273.15)
            {
                c[0] = -5674.5359;
                c[1] = 6.3925247;
                c[2] = -0.9677843 * Math.Pow(10, -2);
                c[3] = 0.62215701 * Math.Pow(10, -6);
                c[4] = 0.20747825 * Math.Pow(10, -8);
                c[5] = -0.9484024 * Math.Pow(10, -12);
                c[6] = 4.1635019;
            }
            else
            {
                c[0] = -5800.2206;
                c[1] = 1.3914993;
                c[2] = -0.048640239;
                c[3] = 0.41764768 * Math.Pow(10, -4);
                c[4] = -0.14452093 * Math.Pow(10, -7);
                c[5] = 0;
                c[6] = 6.5459673;
            }

            for (int i = 0; i < 6; i++)
            {
                pascals += c[i] * Math.Pow(kelvin, i - 1);
            }
            pascals += c[6] * Math.Log(kelvin);
            pascals = Math.Exp(pascals);
            return pascals;
        }
        public double relHumidfromHumidRatioTemp(double t, double hr, double atmosP)
        {
            double partPress = partPressure(hr, atmosP);
            double satPress = saturationPress(t);
            double rh = relHumid(partPress, satPress);
            return rh;
        }
        public double partPress(double relHumid, double saturationPress)
        {
            //relhumid 0>1
            return relHumid * saturationPress / 100;

        }
        public double relHumid(double partPress, double saturationPress)
        {
            //relhumid 0>1
            return partPress * 100 / saturationPress;

        }

        public double humidtyRatio(double atmosP, double pw)
        {//pw is partial pressure of water vapour
            double w = (0.62198 * pw) / (atmosP - pw);
            return w;
        }
        public double partPressure(double humidtyRatio, double atmosP)
        {
            double pp = (50000 * atmosP * humidtyRatio) / (50000 * humidtyRatio + 31099);
            return pp;
        }
        
        public double getStandardPressure(double altitude)   // input is m Returns result in Pascals
        {
            // Below 51 km: Practical Meteorology by Roland Stull, pg 12
            // Above 51 km: http://www.braeunig.us/space/atmmodel.htm
            // Validation data: https://www.avs.org/AVS/files/c7/c7edaedb-95b2-438f-adfb-36de54f87b9e.pdf

            altitude = altitude / 1000.0;  // Convert m to km
            double geopot_height = getGeopotential(altitude);

            double t = getStandardTemperature(geopot_height);

            if (geopot_height <= 11)
                return 101325 * Math.Pow(288.15 / t, -5.255877);
            else if (geopot_height <= 20)
                return 22632.06 * Math.Exp(-0.1577 * (geopot_height - 11));
            else if (geopot_height <= 32)
                return 5474.889 * Math.Pow(216.65 / t, 34.16319);
            else if (geopot_height <= 47)
                return 868.0187 * Math.Pow(228.65 / t, 12.2011);
            else if (geopot_height <= 51)
                return 110.9063 * Math.Exp(-0.1262 * (geopot_height - 47));
            else if (geopot_height <= 71)
                return 66.93887 * Math.Pow(270.65 / t, -12.2011);
            else if (geopot_height <= 84.85)
                return 3.956420 * Math.Pow(214.65 / t, -17.0816);

            //throw std::out_of_range("altitude must be less than 86 km.");
            return -1;
        }

        // geopot_height = earth_radius * altitude / (earth_radius + altitude) /// All in km
        // Temperature is in Kelvin = 273.15 + Celsius
        public double  getStandardTemperature(double geopot_height)
        {
            // Standard atmospheric pressure
            // Below 51 km: Practical Meteorology by Roland Stull, pg 12
            // Above 51 km: http://www.braeunig.us/space/atmmodel.htm

            if (geopot_height <= 11)          // Troposphere
                return 288.15 - (6.5 * geopot_height);
            else if (geopot_height <= 20)     // Stratosphere starts
                return 216.65;
            else if (geopot_height <= 32)
                return 196.65 + geopot_height;
            else if (geopot_height <= 47)
                return 228.65 + 2.8 * (geopot_height - 32);
            else if (geopot_height <= 51)     // Mesosphere starts
                return 270.65;
            else if (geopot_height <= 71)
                return 270.65 - 2.8 * (geopot_height - 51);
            else if (geopot_height <= 84.85)
                return 214.65 - 2 * (geopot_height - 71);
            // Thermosphere has high kinetic temperature (500 C to 2000 C) but temperature
            // as measured by a thermometer would be very low because of almost vacuum.

            //throw std::out_of_range("geopot_height must be less than 84.85 km.")
            return -1;
        }

        public double getGeopotential(double altitude_km)
        {
            double EARTH_RADIUS = 6356.766; // km

            return EARTH_RADIUS * altitude_km / (EARTH_RADIUS + altitude_km);
        }
    }
}
