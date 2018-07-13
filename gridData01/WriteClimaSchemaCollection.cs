using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Rhino.Geometry;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace gridData01
{
    class WriteClimaSchemaCollection
    {
        private WeatherData dataset;
        private List<string> whatdata;
        private bool indented=false;
        private bool compact = false;
        StreamWriter sw;

        public WriteClimaSchemaCollection(WeatherData data,List<string> what, string where,bool indent,bool comp)
        {
            this.dataset = data;
            this.whatdata = what;
            this.indented = indent;
            this.compact = comp;
            write(where);
        }
        private void write(string file)
        {

            this.sw = new StreamWriter(file);

            using (JsonTextWriter writer = new JsonTextWriter(this.sw))
            {
                if (indented) writer.Formatting = Formatting.Indented;
                else writer.Formatting = Formatting.None;

                writeClimaJsonCollection(writer);

            }
            
        }


        private void writeClimaJsonCollection(JsonTextWriter writer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("collectionType");
            //writer.WriteValue("monthly Colombian weather data sourced from http://www.cgiar-csi.org/data/uea-cru-ts-v3-10-01-historic-climate-database and http://www.cgiar-csi.org/data/srtm-90m-digital-elevation-database-v4-1");
            writer.WriteValue(this.dataset.description);
            writer.WritePropertyName("classificiationTypes");
            writer.WriteStartArray();
            foreach(string cc in this.whatdata) writer.WriteValue(cc);
            writer.WriteEndArray();
            writer.WritePropertyName("startYear");
            writer.WriteValue(this.dataset.srtYr);
            writer.WritePropertyName("endYear");
            writer.WriteValue(this.dataset.endYr);
            writer.WritePropertyName("dataFields");
            writer.WriteStartArray();
            for (int i = 0; i < this.whatdata.Count; i++)
            {
                bool writeFields = true;
                //write the fields if the data is not a mix of averages and full sets
                if(!mixedOutput()) writeFields = true;
                //don't write if the data is mixed and this data is an average
                if (mixedOutput() && this.whatdata[i].Contains("Average")) writeFields = false; 

                if(writeFields)
                {
                    writer.WriteStartObject();
                    writer.WritePropertyName(this.whatdata[i]);
                    writer.WriteStartArray();
                    ClimateClassification cc = this.dataset.getClassification(this.whatdata[i]);
                    //need a method to not repeat fields for averages
                    List<DataField> dfs = new List<DataField>();
                    if (this.compact) dfs = cc.dataFieldsCompact;
                    else dfs = cc.dataFields;
                    for (int j = 0; j < dfs.Count; j++)
                    {
                        writeDataField(dfs[j], writer);

                    }

                    writer.WriteEndArray();
                    writer.WriteEndObject();
                }
            }
            

            writer.WriteEndArray();

            writer.WritePropertyName("gridPoints");
            writer.WriteStartArray();

            
            for (int j = 0; j < this.dataset.gridPoints.Count; j++)
            {
                
                LocationPoint gp = this.dataset.gridPoints[j];
                if (gp.isNullPoint)
                {
                    continue;
                }
                writer.WriteStartObject();
                writer.WritePropertyName("location");
                writer.WriteStartObject();
                writer.WritePropertyName("latitude");
                writer.WriteValue(gp.latitude);
                writer.WritePropertyName("longitude");
                writer.WriteValue(gp.longitude);
                writer.WriteEndObject();

                writer.WritePropertyName("alt");
                writer.WriteValue(gp.altitude);
                //for each point we write all the data
                for (int i = 0; i < this.whatdata.Count; i++)
                {
                   
                        writer.WritePropertyName(this.whatdata[i]);
                        writer.WriteStartArray();
                    int totalyrs = this.dataset.endYr - this.dataset.srtYr;
                    if (this.whatdata[i].Contains("Average")) totalyrs = 0;

                        for (int y = 0; y <= totalyrs; y++)
                        {
                        this.sw.Write(writeClimaJson( y, j, this.whatdata[i]));
                        if(y!=totalyrs) this.sw.Write(",");
                        }
                        writer.WriteEndArray();
                    
                }
                
                writer.WriteEndObject();
            }
        
            writer.WriteEndArray();
            writer.WriteEndObject();
        }
        private bool mixedOutput()
        {
            bool oneAv = false;
            bool oneMulti = false;
            foreach(string c in whatdata)
            {
                if (c.Contains("Average")) oneAv = true;
                if (!c.Contains("Average")) oneMulti = true;
            }
            if (oneAv && oneMulti) return true;
            else return false;
        }

        private string writeClimaJson( int yr, int p, string whatdata)
        {
            StringWriter localsw = new StringWriter();
            JsonTextWriter jsonwriter = new JsonTextWriter(localsw);
            var cc = this.dataset.getClassification(whatdata);
            var point = cc.dataPoints[p];
            
            jsonwriter.WriteStartArray();
            List<DataField> dfs = new List<DataField>();
            if (this.compact) dfs = cc.dataFieldsCompact;
            else dfs = cc.dataFields;
            for (int i = 0; i < dfs.Count; i++)
               {
                writeClassArray(dfs[i].name,ref jsonwriter, point, yr);
                }
            
            jsonwriter.WriteEndArray();
            return localsw.ToString();
        }
        private void writeClassArray(string propname, ref JsonTextWriter jsonwriter, DataPoint dp, int y)
        {
            Type t = dp.GetType();
            var p = t.GetProperty(propname);
            
            object ob = p.GetValue(dp, null);

            switch(p.PropertyType.Name)
            {
                case "Int32[,]":
                    var s = (Int32[,])ob;
                    jsonwriter.WriteStartArray();
                    for (int m = 0; m < 12; m++)
                    {
                        Int32 sv = s[y, m];
                        jsonwriter.WriteValue(s[y, m]);
                    }
                    jsonwriter.WriteEndArray();
                    break;
                case "Double[,]":
                    var d = (Double[,])ob;
                    jsonwriter.WriteStartArray();
                    for (int m = 0; m < 12; m++)
                    {
                        Double dv = d[y, m];
                        jsonwriter.WriteValue(d[y, m]);
                    }
                    jsonwriter.WriteEndArray();
                    break;
                case "Vector3d[,]":
                    var col = (Vector3d[,])ob;
                    jsonwriter.WriteStartArray();
                    for (int m = 0; m < 12; m++)
                    {
                        Vector3d v3d = col[y, m];
                        jsonwriter.WriteStartObject();
                        jsonwriter.WritePropertyName("r");
                        jsonwriter.WriteValue((int)(v3d.X * 255));
                        jsonwriter.WritePropertyName("g");
                        jsonwriter.WriteValue((int)(v3d.Y * 255));
                        jsonwriter.WritePropertyName("b");
                        jsonwriter.WriteValue((int)(v3d.Z * 255));
                        jsonwriter.WriteEndObject();
                    }
                    jsonwriter.WriteEndArray();
                    break;
                case "String[,]":
                    var klass = (string[,])ob;
                    jsonwriter.WriteStartArray();
                    for (int m = 0; m < 12; m++)
                    {
                        jsonwriter.WriteValue(klass[y, m]);
                    }
                    jsonwriter.WriteEndArray();
                    break;
                case "String[]":
                    var k = (string[])ob;
                    jsonwriter.WriteValue(k[y]);
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

        private void writeDataField(DataField df, JsonWriter writer)
        {

            
            writer.WriteStartObject();
            writer.WritePropertyName("name");
            writer.WriteValue(df.name);
            writer.WritePropertyName("units");
            
            writer.WriteValue(df.unit);
            writer.WritePropertyName("description");
            writer.WriteValue(df.descrip);
            writer.WritePropertyName("min");
            writer.WriteValue(df.min);
            writer.WritePropertyName("max");
            writer.WriteValue(df.max);
            writer.WritePropertyName("labels");
            if (df.labels == null) writer.WriteValue(df.labels);
            else
            {
                writer.WriteStartArray();
                foreach (string label in df.labels) writer.WriteValue(label);
                writer.WriteEndArray();
            }
            writer.WritePropertyName("discreteScale");
            writer.WriteValue(df.discreteScale);
            writer.WriteEndObject();

        }
        
        private void writeWeatherArray(string val, JsonWriter writer, WeatherPoint gp, int y)
        {
            object ob = gp.GetType().GetProperty(val).GetValue(gp, null);
            var s = (Int32[,])ob;
            for (int m = 0; m < 12; m++)
            {
                writer.WriteValue(s[y, m]);
            }

        }
    }
}
