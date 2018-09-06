using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace gridData01
{
    class WriteSparkJSON
    {
        private WeatherData dataset;
        private string folder;
        public WriteSparkJSON(WeatherData ds, String path)
        {
            dataset = ds;
            folder = path;
        }
        public void writeRecords()
        {
            StreamWriter sw = new StreamWriter(folder + "\\records.json");
            JsonTextWriter writer = new JsonTextWriter(sw);
            int dpCode = 0;
            foreach (DataPoint dp in this.dataset.classifications[0].dataPoints)
            {
                
                for(int y = 0; y < dataset.years; y++)
                {
                    for(int m=0;m<12;m++)
                    {
                        writer.WriteStartObject();
                        writer.WritePropertyName("locationCode");
                        writer.WriteValue(dpCode);
                        writer.WritePropertyName("year");
                        writer.WriteValue(y+dataset.srtYr);
                        writer.WritePropertyName("month");
                        writer.WriteValue(m+1);
                        foreach (DataField field in this.dataset.classifications[0].dataFields)
                        {

                            writeClassArray(field.name, ref writer, dp, y, m);

                        }
                        writer.WriteEndObject();
                        writer.WriteRaw("\n");
                    }
                }
                dpCode++;
                
            }
                    
                
            writer.Close();
            sw.Close();
        }
        public void writeGrid()
        {
            StreamWriter sw = new StreamWriter(folder + "\\locations.json");
            JsonTextWriter writer = new JsonTextWriter(sw);
            foreach (LocationPoint gp in this.dataset.gridPoints)
            {
               
                writer.WriteStartObject();
                writer.WritePropertyName("latitude");
                writer.WriteValue(gp.latitude);
                writer.WritePropertyName("longitude");
                writer.WriteValue(gp.longitude);
                writer.WriteEndObject();
                writer.WritePropertyName("alt");
                writer.WriteValue(gp.altitude);
                writer.WriteEndObject();
                writer.WriteRaw("\n");
            }
            writer.Close();
            sw.Close();
        }
        private void writeClassArray(string propname, ref JsonTextWriter jsonwriter, DataPoint dp, int y,int m)
        {
            Type t = dp.GetType();
            var p = t.GetProperty(propname);

            object ob = p.GetValue(dp, null);
            jsonwriter.WritePropertyName(propname);
            switch (p.PropertyType.Name)
            {
                case "Int32[,]":
                    var s = (Int32[,])ob;
                        Int32 sv = s[y, m];
                        jsonwriter.WriteValue(s[y, m]);
                    
                    break;
                case "Double[,]":
                    var d = (Double[,])ob;
                        Double dv = d[y, m];
                        jsonwriter.WriteValue(d[y, m]);
                    break;
                
                
                case "Double[]":
                    var h = (double[])ob;
                    double v = h[y];
                    jsonwriter.WriteValue(h[y]);
                    break;
                case "Int32[]":
                    var b = (Int32[])ob;
                    Int32 bv = b[y];
                    jsonwriter.WriteValue(b[y]);
                    break;
                case "Double":
                    var a = (Double)ob;

                    jsonwriter.WriteValue(a);
                    break;
            }

        }
    }
}
