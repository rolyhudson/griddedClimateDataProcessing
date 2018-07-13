using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gridData01
{
    class ClimateClassification
    {
        public string[] fields;//
        public List<DataField> dataFields = new List<DataField>();
        public List<DataField> dataFieldsCompact = new List<DataField>();
        public string description;
        public string extendedDescription;
        public List<DataPoint> dataPoints = new List<DataPoint>();
        public void setDataFields(List<string> f)
        {
            for (int i = 0; i < f.Count; i++)
            {
                DataField df = new DataField();
                df.autoSet(f[i]);
                this.dataFields.Add(df);
            }
        }
        public void setDataFieldSingle(string n, string u, string maxi, string mini, string des, string[] lbls, bool discrete)
        {
            DataField df = new DataField();
            df.specialSet(n, u, maxi,mini, des, lbls, discrete);
            this.dataFields.Add(df);
        }
        public void setDataFieldsCompact(List<string> f)
        {
            for (int i = 0; i < f.Count; i++)
            {
                DataField df = new DataField();
                df.autoSet(f[i]);
                this.dataFieldsCompact.Add(df);
            }
        }
        public void setFields(List<string> f)
        {
            this.fields = new string[f.Count];
            for (int i = 0; i < f.Count; i++)
            {
                this.fields[i] = f[i];
            }

        }
    }

    
}
