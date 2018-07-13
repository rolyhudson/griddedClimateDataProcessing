using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gridData01
{
    class KGClass:ClimateClassification
    {
        public KGClass(WeatherData data,string climatedata)
        {
            
            setKg();
            
            runKG(data.getClassification(climatedata),data.gridPoints);
        }
        
        private void setKg()
        {
            string[] f = { "kgClass", "kgClassNum", "kgClassDescriptive","Tann", "Tmin", "Tmax", "Tmon", "Pann", "Pmin", "Psmin", "Psmax", "Pwmin", "Pwmax", "Pth" };
            List<string> fields = new List<string>();
            fields.AddRange(f);
            this.setDataFields(fields);

            List<string> fieldsCompact = new List<string>();
            string[] c = { "kgClassDescriptive" };
            fieldsCompact.AddRange(c);
            this.setDataFieldsCompact(fieldsCompact);

        }
        private void runKG(ClimateClassification cc,List<LocationPoint> gridPoints)
        {
            for (int i = 0; i < gridPoints.Count; i++)
            {
                KGPoint kgp = new KGPoint();
                WeatherPoint wp = (WeatherPoint)cc.dataPoints[i];
                
                kgp.setUpKGClassification(wp,gridPoints[i], wp.temp.GetLength(0));
                this.dataPoints.Add(kgp);
            }
        }
    }
}
