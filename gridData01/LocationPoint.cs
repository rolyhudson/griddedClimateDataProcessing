using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gridData01
{
    class LocationPoint:DataPoint
    {
        public string placename;
        public double latitude;
        public double longitude;
        public double altitude;
        public bool isNull;

        public LocationPoint(double lat,double lon,double alt)
        {
            this.latitude = lat;
            this.longitude = lon;
            this.altitude = alt;
        }
        public LocationPoint(double lat, double lon)
        {
            this.latitude = lat;
            this.longitude = lon;
        }
    }
}
