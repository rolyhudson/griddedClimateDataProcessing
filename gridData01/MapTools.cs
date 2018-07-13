using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace gridData01
{
    public class MapTools
    {
        public MapTools()
        {

        }
        public static bool isPointInPolygon(double[] point, List<double[]> vs)
        {
            // ray-casting algorithm based on
            // http://www.ecse.rpi.edu/Homepages/wrf/Research/Short_Notes/pnpoly.html

            double x = point[0], y = point[1];

            bool inside = false;
            for (int i = 0, j = vs.Count - 1; i < vs.Count; j = i++)
            {
                double xi = vs[i][0], yi = vs[i][1];
                double xj = vs[j][0], yj = vs[j][1];

                bool intersect = ((yi > y) != (yj > y))
                    && (x < (xj - xi) * (y - yi) / (yj - yi) + xi);
                if (intersect) inside = !inside;
            }

            return inside;
        }
        public static double saturationPress(double temp)
        {
            //convert temp C to kelvin
            double kelvin = temp + 273.15;
            double[] c = new double[7];
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

            for (var i = 0; i < 6; i++)
            {
                pascals += c[i] * Math.Pow(kelvin, i - 1);
            }
            pascals += c[6] * Math.Log(kelvin);
            pascals = Math.Exp(pascals);
            return pascals;
        }
        public int setBeaufort(double wind)
        {
            if (wind < 0.3) return 0;
            if (wind >= 0.3 && wind < 1.5) return 1;
            if (wind >= 1.5 && wind < 3.3) return 2;
            if (wind >= 3.3 && wind < 5.5) return 3;
            if (wind >= 5.5 && wind < 7.9) return 4;
            if (wind >= 7.9 && wind < 10.7) return 5;
            if (wind >= 10.7 && wind < 13.8) return 6;
            if (wind >= 13.8 && wind < 17.1) return 7;
            if (wind >= 17.1 && wind < 20.7) return 8;
            if (wind >= 20.7 && wind < 24.4) return 9;
            if (wind >= 24.4 && wind < 28.4) return 10;
            if (wind >= 28.4 && wind < 32.6) return 11;
            if (wind >= 3.6) return 12;
            return 0;
        }
    }
}
