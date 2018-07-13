using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace gridData01
{
    class DefineGrid
    {
        public List<LocationPoint> points = new List<LocationPoint>();
        private double minLat = 90;
        private double maxLat = -90;
        private double minLon = 180;
        private double maxLon = -180;
        private string filepath;
        public string error;
        private List<List<double[]>> boundaries = new List<List<double[]>>();
        public DefineGrid(string path)
        {
            this.filepath = path;
        }
        public bool defineGrid()
        {
            this.points = new List<LocationPoint>();
            if (getBoundaries())
            {
                pointsInside();
                return true;
            }
            else return false;
        }
        public bool readGrid()
        {

            this.points = new List<LocationPoint>();
            try {
                StreamReader sr = new StreamReader(this.filepath);
                string line = sr.ReadLine();
                int count = 0;
                while (line != null)
                {
                    string[] parts = line.Split(',');
                    LocationPoint lp = new LocationPoint(double.Parse(parts[1]), double.Parse(parts[0]), Convert.ToInt32(parts[2]));
                    this.points.Add(lp);
                    line = sr.ReadLine();
                    count++;
                }
                sr.Close();
                return true;
            }
            catch
            {
                this.error = "Check the grid input file is in the csv format and contains grid inforamtion in the format:\n lon,lat,alt";
                return false;
            }
        }
        private bool getBoundaries()
        {
            //read the boundary json
            StreamReader reader = new StreamReader(this.filepath);
            JObject data = new JObject();
            String value = reader.ReadToEnd();
            bool success = false;
            try {
                using (JsonTextReader jsr = new JsonTextReader(new StringReader(value)))

                {
                    data = (JObject)JToken.ReadFrom(jsr);
                    var features = data.SelectToken("features");
                    foreach (JObject country in features)
                    {
                        //check if its a multipolygon or polygon
                        var polygonType = (string)country.SelectToken("geometry.type");
                        var bounds = country.SelectToken("geometry.coordinates");
                        if (bounds is JArray)
                        {
                            if (polygonType == "MultiPolygon")
                            {
                                foreach (var item in bounds.Children())
                                {
                                    var s = item.First();

                                    boundaries.Add(getPointsFromPolygon(s));
                                }
                            }
                            else
                            {
                                boundaries.Add(getPointsFromPolygon(bounds.First()));
                            }
                        }
                    }
                }
                success = true;
            }
            catch
            {
                this.error = "Check the input is in correct geojson format";
                success = false;
            }
            return success;
        }
        private List<double[]> getPointsFromPolygon(JToken s)
        {
            List<double[]> boundary = new List<double[]>();

            for (int j = 0; j < s.Count(); j++)
            {
                var point = s.SelectToken("[" + j + "]").ToObject<List<double>>();
                updateRange(point);
                double[] p = new double[] { point[0], point[1] };
                boundary.Add(p);
            }
            return boundary;
        }
        private void updateRange(List<double> p)
        {
            double lat = p[1];
            double lon = p[0];
            if (lat > this.maxLat) this.maxLat = lat;
            if (lat < this.minLat) this.minLat = lat;
            if (lon > this.maxLon) this.maxLon = lon;
            if (lon < this.minLon) this.minLon = lon;

        }
        private void pointsInside()
        {
            double cellSize = 0.5;//degrees lat or lon
            double lonRange = maxLon - minLon;
            double latRange = maxLat - minLat;
            int lonCells = (int)Math.Ceiling(lonRange / cellSize);
            int latCells = (int)Math.Ceiling(latRange / cellSize);
            double[] point = new double[2];
            for (int i = 0; i < lonCells; i++)
            {
                for (int j = 0; j < latCells; j++)
                {
                    point[0] = i * cellSize + minLon;
                    point[1] = j * cellSize + minLat;
                    //is it inside the country boundary
                    testPoint(point);
                }
            }
        }
        private void testPoint(double[] point)
        {
            for (int i = 0; i < boundaries.Count; i++)//checks for multi boundary country islands etc
            {
                if (MapTools.isPointInPolygon(point, boundaries[i]))
                {

                    this.points.Add(new LocationPoint(point[1], point[0]));
                }
            }
        }
    }
}
