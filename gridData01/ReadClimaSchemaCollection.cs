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
    class ReadClimaSchemaCollection
    {
        public WeatherData dataset = new WeatherData();
        private string filepath;
        public bool result;
        public string error;

        
        public ReadClimaSchemaCollection(string path)
        {
            this.filepath = path;
            //readBigData(this.filepath);
            try {
                readData();

                dataset.description = "Gridded climate data " + dataset.srtYr.ToString() + " to " + dataset.endYr.ToString();
                this.result = true;
            }
            catch
            {
                this.result = false;
                this.error = "There was a problem with the climate data";
            }
            
        }
        private void readBigData(string path)
        {
            JsonSerializer serializer = new JsonSerializer();
            WeatherClass wc;
            using (FileStream s = File.Open(path, FileMode.Open))
            using (StreamReader sr = new StreamReader(s))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                while (reader.Read())
                {
                    // deserialize only when there's "{" character in the stream
                    if (reader.TokenType == JsonToken.StartObject)
                    {
                        
                    }
                }
            }
        }
        private void readData()
        {
            StreamReader reader = new StreamReader(this.filepath);
            JObject data = new JObject();
            String value = reader.ReadToEnd();

            WeatherClass wc = new WeatherClass();
            using (JsonTextReader jsr = new JsonTextReader(new StringReader(value)))
            {
                data = (JObject)JToken.ReadFrom(jsr);
                wc.description = "climateData";
                
                //climaetData
                this.dataset.setTimePeriod((int)data.SelectToken("startYear"), (int)data.SelectToken("endYear"));
                var df = data.SelectToken("dataFields");
                if (df is JArray)//there is one datafield set for each classfication type
                {
                    foreach (JObject content in df.Children<JObject>())
                    {
                        foreach (JProperty prop in content.Properties())
                        {
                            var fields = content.SelectToken(prop.Name);
                            List<string> f = new List<string>();
                            foreach (var item in fields.Children())
                            {
                                f.Add((string)item.SelectToken("name"));
                            }
                            wc.setDataFields(f);
                            wc.setDataFieldsCompact(f);
                        }
                    }
                    
                }

                var gp = data.SelectToken("gridPoints");
                //wc.setDataCollection(gp.Count(), totalyrs);
                if (gp is JArray)
                {
                    List<WeatherPoint> wps = new List<WeatherPoint>();
                    
                    double lat=0;
                    double lon = 0;
                    double alt = 0;
                    foreach (var item in gp.Children())
                    {
                        lat = (double)item.SelectToken("location.latitude");
                        lon = (double)item.SelectToken("location.longitude");
                        alt = (int)item.SelectToken("alt");

                        this.dataset.gridPoints.Add(new LocationPoint(lat,lon,alt));
                        WeatherPoint wp = new WeatherPoint(this.dataset.years);
                        var cd = item.SelectToken("climateData");
                        if(cd is JArray)
                        {
                           
                            for (int i = 0; i < this.dataset.years; i++)
                            {
                                var t = cd.SelectToken("[" + i + "]");//grab one year of data
                                for (int j = 0; j < wc.dataFields.Count; j++)
                                {
                                    var d = t.SelectToken("[" + j + "]");//grab one datafield array
                                    double[] items = d.Select(jv => Convert.ToDouble(jv)).ToArray();
                                   wp.writeDataToGridPoint(items, wc.dataFields[j].name, i);
                                }
                            }
                        }
                        wps.Add(wp);
                        
                    }
                    wc.setData(wps);
                    
                    this.dataset.classifications.Add(wc);
                    this.dataset.flagNullLocations();
                }
                

            }
        }
    }
}
