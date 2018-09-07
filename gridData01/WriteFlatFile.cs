using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rhino.Geometry;

namespace gridData01
{
    class WriteFlatFile
    {
        private WeatherData dataset;
        private string file;
        public WriteFlatFile(WeatherData ds,string outputfile)
        {
            dataset = ds;
            file = outputfile;
        }
        public void writeToCSV()
        {
            StreamWriter sw = new StreamWriter(file);
            int pNum = 0;
            StringBuilder line = new StringBuilder();
            //header

            line.Append("latitude,longitude,elevation,datetime");
            foreach(DataField df in dataset.classifications[0].dataFieldsCompact)
            {
                line.Append(","+df.name);
            }
            sw.WriteLine(line.ToString());
            foreach (DataPoint dp in dataset.classifications[0].dataPoints)
            {
                LocationPoint lp = dataset.gridPoints[pNum];
                for(int y = 0;y<10;y++)
                {
                    for(int m = 0;m<12;m++)
                    {
                        line = new StringBuilder();
                        String datestring = (y + dataset.srtYr)+"-";
                        String month = (m + 1).ToString();
                        if (month.Length == 1) datestring += "0" + month + "-01";
                        else datestring += month + "-01";
                        line.Append(Math.Round(lp.latitude,5) + "," + Math.Round(lp.longitude,5)+"," + Math.Round(lp.altitude, 5) + "," + datestring);
                        for(int i=0;i< dataset.classifications[0].dataFieldsCompact.Count;i++)
                        {
                            //add the value for the year and month
                            line.Append(",");
                            writeClassArray(dataset.classifications[0].dataFieldsCompact[i].name, ref line, dp,y,m);
                        }
                        sw.WriteLine(line.ToString());
                    }
                }
                pNum++;
            }
            sw.Close();
        }
        private void writeClassArray(string propname, ref StringBuilder sb, DataPoint dp, int y,int m)
        {
            Type t = dp.GetType();
            var p = t.GetProperty(propname);

            object ob = p.GetValue(dp, null);

            switch (p.PropertyType.Name)
            {
                case "Int32[,]":
                    var s = (Int32[,])ob;
                    
                        Int32 sv = s[y, m];
                    sb.Append(sv);
                    break;
                case "Double[,]":
                    var d = (Double[,])ob;
                        Double dv = d[y, m];
                    sb.Append(dv);
                    break;
                case "Vector3d[,]":
                    var col = (Vector3d[,])ob;
                        Vector3d v3d = col[y, m];
                    break;
                case "String[,]":
                    var klass = (string[,])ob;
                    sb.Append(klass);
                    break;
                case "String[]":
                    var k = (string[])ob;
                    sb.Append(k);
                    break;
                case "Double[]":
                    var h = (double[])ob;
                    double v = h[y];
                    sb.Append(y);
                    break;
                case "Int32[]":
                    var b = (Int32[])ob;
                    Int32 bv = b[y];
                    sb.Append(bv);
                    break;
                case "Double":
                    var a = (Double)ob;
                    sb.Append(a);

                    break;
            }

        }
    }
}
