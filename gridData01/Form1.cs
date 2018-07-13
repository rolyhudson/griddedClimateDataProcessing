using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Accord.MachineLearning;
using Accord.Statistics; 
using Accord.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace gridData01
{
    public partial class Form1 : Form
    {

        WeatherData dataset = new WeatherData();
        private string filepath;
        private int startYr;
        private int endYr;
        List<string> classificationNames = new List<string>();
        List<List<double[]>> boundaries = new List<List<double[]>>();
        public Form1()
        {
            InitializeComponent();
            updateYears();
            averageDataBtn.Enabled = false;
            kgBtn.Enabled = false;
        }
        private bool getFilePath(string title, string filter, string filetype)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Title = title;
            openFileDialog1.DefaultExt = filetype;
            openFileDialog1.Filter =filter;
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                this.filepath = openFileDialog1.FileName;
                return true;
            }
            else return false;
        }
        private void updateYears()
        {
            this.startYr = Convert.ToInt32(startYrUpDown.Value);

            this.endYr = Convert.ToInt32(endYrUpDown.Value);


            if (this.startYr > this.endYr)
            {
                int hold = this.startYr;
                this.startYr = this.endYr;
                this.endYr = hold;
            }
        }
        private void yearChanged(object sender, EventArgs e)
        {
            updateYears();
        }

        private bool saveFilePath(string title, string filter, string filetype)
        {
            saveFileDialog1.FileName = "";
            saveFileDialog1.Title = title;
            saveFileDialog1.DefaultExt = filetype;
            saveFileDialog1.Filter = filter;
            DialogResult result = saveFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                this.filepath = saveFileDialog1.FileName;
                return true;
            }
            else return false;
        }
        private bool getFolderPath(string description)
        {
            folderBrowserDialog1.Description = description;
            folderBrowserDialog1.SelectedPath = "C:\\Users\\r.hudson\\Documents\\WORK\\piloto\\griddedDataSet";
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                this.filepath = folderBrowserDialog1.SelectedPath;
                return true;
            }
            else return false;
        }
        private void makeGrid(object sender, EventArgs e)
        {
            dataset = new WeatherData();
            if (getFilePath("Select a geojson file", "geojson files (*.json)|*.json|All files (*.*)|*.*", "json"))
            {
                DefineGrid dg = new DefineGrid(this.filepath);

                if (dg.defineGrid())
                {
                    dataset.addLocationPoints(dg.points);
                    statusUpdate();
                }
                else MessageBox.Show(dg.error, "Input error");
            }

        }
        private void elevation(object sender, EventArgs e)
        {
            setElevation();
        }
        private void writeCountryBtn_Click(object sender, EventArgs e)
        {
         
            writePoints();
        }
        private void readAGrid(object sender, EventArgs e)
        {

            if (getFilePath("Select a csv with presaved lon, lat and alt", "csv files (*.csv)|*.csv|All files (*.*)|*.*", "csv"))
            {
                DefineGrid dg = new DefineGrid(this.filepath);
                if (dg.readGrid())
                {
                    dataset.addLocationPoints(dg.points);
                    statusUpdate();
                }
                else MessageBox.Show(dg.error, "Input error");
            }
        }
        private void readWeather(object sender, EventArgs e)
        {
            setWeather();

        }
        private void generateKG(object sender, EventArgs e)
        {
            KGClass kgc = null;
            try
            {
                kgc = new KGClass(this.dataset, "climateData");
                kgc.description = "koppenGeigerData";
                this.dataset.classifications.Add(kgc);
            }
            catch
            {

            }
            try {
                kgc = new KGClass(this.dataset, "climateDataAverages");
                kgc.description = "koppenGeigerDataAverage";
                this.dataset.classifications.Add(kgc);
            }
            catch { }
            statusUpdate();
        }
        
        private void testWeather(object sender, EventArgs e)
        {
            dataset = new WeatherData();
            dataset.srtYr = 1999;
            dataset.endYr = 2009;
            dataset.gridPoints.Add(new LocationPoint(4.687771, -74.075578));//bogota
            dataset.gridPoints.Add(new LocationPoint(6.240419, -75.566587));//medellin
            dataset.gridPoints.Add(new LocationPoint(3.442047, -76.540040));//cali
            setWeather();
            setElevation();
            //writeJson("testjson.json",dataset);
        }
        private void setWeather()
        {
            if (getFolderPath("Select the folder containing subfolders of weather files from http://www.cgiar-csi.org/data/uea-cru-ts-v3-10-01-historic-climate-database"))
            {
                ASCIIgridTools asciigrid = new ASCIIgridTools();
                this.dataset.description = "Gridded climate data " + this.startYr.ToString() + " to " + this.endYr.ToString();
                if (!asciigrid.getWeather(ref this.dataset, this.filepath, this.startYr, this.endYr)) MessageBox.Show(asciigrid.error, "Input error");
                else
                {
                    statusUpdate();
                    enableClassification();
                }
                }
        }
        private void setElevation()
        {
            if (getFolderPath("Select the folder containing DEM files from http://www.cgiar-csi.org/data/srtm-90m-digital-elevation-database-v4-1"))
            {
                ASCIIgridTools asciigrid = new ASCIIgridTools();
                if (!asciigrid.getElevation(ref this.dataset, this.filepath)) MessageBox.Show(asciigrid.error, "Input error");
                else statusUpdate();
            }
        }
        
        private void writePoints()
        {
            if (saveFilePath("Choose where to write the raw point data", "csv files (*.csv)|*.csv|All files (*.*)|*.*", "csv"))
            {
                StreamWriter sw = new StreamWriter(this.filepath);
                for (int i = 0; i < dataset.gridPoints.Count; i++)
                {
                    sw.WriteLine(dataset.gridPoints[i].longitude.ToString() + "," + dataset.gridPoints[i].latitude.ToString() + "," + dataset.gridPoints[i].altitude.ToString());
                }
                sw.Close();
            }
        }
        private void enableClassification()
        {
            averageDataBtn.Enabled = true;
            kgBtn.Enabled = true;
            statsBtn.Enabled = true;
            holdridgeBtn.Enabled = true;
            HVBtn.Enabled = true;
        }
        private void averageData(object sender, EventArgs e)
        {
            AverageData avdata = new AverageData(this.dataset);
            this.dataset.classifications.Add(avdata.datasetAv);
            statusUpdate();
        }
        
        private void writeClimateData(object sender, EventArgs e)
        {
            if (saveFilePath("Choose where to write the unclassfied climate data", "json files (*.json)|*.json|All files (*.*)|*.*", "json"))
            {
                new WriteClimaSchemaCollection(this.dataset, classifcationsToOutput(), this.filepath,indentClimaChk.Checked,this.compactOutputChkbx.Checked);
            }

        }
        private List<string> classifcationsToOutput()
        {
            List<string> output = new List<string>();
            for (int i = 0; i < this.dataset.classifications.Count; i++)
            {
                if (averagesOnlyChk.Checked)
                {
                    if (this.dataset.classifications[i].description.Contains("Average"))
                    {
                        output.Add(this.dataset.classifications[i].description);
                    }
                }
                else
                {
                    if(classOnlyChkBx.Checked)
                    {
                        if (this.dataset.classifications[i].description.Contains("holdridge")|| this.dataset.classifications[i].description.Contains("koppen") || this.dataset.classifications[i].description.Contains("hudson"))
                        {
                            output.Add(this.dataset.classifications[i].description);
                        }
                    }
                    else
                    {
                        output.Add(this.dataset.classifications[i].description);
                    }
                    
                }
               
            }
            return output;
        }
        private void statusUpdate()
        {
            gridPtsLbl.Text = "Total grid points = " + this.dataset.gridPoints.Count;
            yearsLbl.Text = "Years of climate data = " + this.dataset.years;
            classificationLbl.Text = "Data sets generated:"+ getClassificationDescriptions();

        }
        private string getClassificationDescriptions()
        {
            this.classificationNames = new List<string>();
            string c = "";
            if (checkGridExists()) c = c + "\nlocationGrid";
            if (eleDefined()) c = c+ "\nelevationData";
            
            for (int i = 0; i < this.dataset.classifications.Count; i++)
            {
                this.classificationNames.Add(this.dataset.classifications[i].description);
                c = c + "\n" + this.dataset.classifications[i].description;
            }

            return c;
        }
        private bool checkGridExists()
        {
            double total=0;
            for (int i = 0; i < this.dataset.gridPoints.Count; i++)
            {
                total += Math.Abs(this.dataset.gridPoints[i].latitude);
            }
            if (total > 0) return true;
            else return false;
        }
        private bool eleDefined()
        {
            double total = 0;
            for(int i=0;i<this.dataset.gridPoints.Count;i++)
            {
                total += this.dataset.gridPoints[i].altitude;
            }
            if (total > 0) return true;
            else return false;
        }

        private void readclimaBtn_Click(object sender, EventArgs e)
        {
            dataset = new WeatherData();
            if (getFilePath("Select a clima JSON collection file", "json files (*.json)|*.json|All files (*.*)|*.*", "json"))
            {
                ReadClimaSchemaCollection read = new ReadClimaSchemaCollection(this.filepath);
                if (read.result)
                {
                    this.dataset = read.dataset;
                    statusUpdate();
                    enableClassification();
                }
                else MessageBox.Show(read.error, "Input error");
            }
        }
        private void hv1DBtnClick(object sender, EventArgs e)
        {
            HudsonVelascoClass hvc = null;
            ReadClimaSchemaCollection read = new ReadClimaSchemaCollection(@"C:\Users\r.hudson\Documents\WORK\piloto\griddedDataSet\ColombiaInputFiles\2000to2009FULL.json");
            this.dataset = read.dataset;
            AverageData avdata = new AverageData(this.dataset);
            this.dataset.classifications.Add(avdata.datasetAv);
            statusUpdate();
            try
            {
                List<HudsonVelascoClass> allHVCs = new List<HudsonVelascoClass>();
                string path = @"C:\Users\r.hudson\Documents\WORK\piloto\griddedDataSet\ColombianClustering";
                string output = "";
                string typeName = "";
                for (int i = 3; i < 21; i++)
                {
                    for (int t = 0; t < 3; t++)
                    {
                        
                        output = path + "\\" + i.ToString();
                        Directory.CreateDirectory(output);
                        hvc = new HudsonVelascoClass(this.dataset, "climateDataAverages", i, output, pcaChkBx.Checked, normaliseChkBx.Checked, logscaleChkBx.Checked, t,true);
                        hvc.description = "hudsonVelascoAverage";
                        this.dataset.classifications.Add(hvc);
                        typeName = hvc.getTypeName(t);
                        
                        new WriteClimaSchemaCollection(this.dataset, classifcationsToOutput(), output + "\\" +typeName+"AvgClusters.json", indentClimaChk.Checked, this.compactOutputChkbx.Checked);
                        this.dataset.classifications.Remove(hvc);
                    }
                }
                StreamWriter sw = new StreamWriter(path + "\\cluster1DAvgPerformanceStudy.csv");
                sw.WriteLine(",,DISTORTION,SILHOUETTE CF");
                sw.WriteLine("n clusters");
                for (int i = 0; i < allHVCs.Count; i++)
                {
                    sw.WriteLine(allHVCs[i].kClusters);
                    for (int j = 0; j < allHVCs[i].kClusters; j++)
                    {
                        sw.WriteLine("," + j + ","
                            + Math.Round(allHVCs[i].distort[j], 3) + "," + Math.Round(allHVCs[i].silhouetteCF[j], 3));
                    }
                    sw.WriteLine(",means:,"
                        + Math.Round(allHVCs[i].distort.Mean(), 3) + "," + Math.Round(allHVCs[i].silhouetteCF.Mean(), 3));
                }
                sw.Close();
            }
            catch(Exception ex)
            {
                var error = ex;
            }
        }
        private void hvBtnClick(object sender, EventArgs e)
        {
            HudsonVelascoClass hvc = null;
            ReadClimaSchemaCollection read = new ReadClimaSchemaCollection(@"D:\WORK\piloto\griddedDataSet\ColombiaInputFiles\2000to2009FULL.json");
            this.dataset = read.dataset;
            AverageData avdata = new AverageData(this.dataset);
            this.dataset.classifications.Add(avdata.datasetAv);
            statusUpdate();
            try
            {

                List<HudsonVelascoClass> allHVCs = new List<HudsonVelascoClass>();
                string path = @"C:\Users\r.hudson\Documents\WORK\piloto\griddedDataSet\ColombianClustering";
                string output = "";
                for (int i = 3; i < 21; i++)
                {
                    output = path + "\\" + i.ToString();
                    Directory.CreateDirectory(output);
                    hvc = new HudsonVelascoClass(this.dataset, "climateDataAverages", i, output, pcaChkBx.Checked, normaliseChkBx.Checked, logscaleChkBx.Checked);
                    hvc.description = "hudsonVelascoAverage";
                    this.dataset.classifications.Add(hvc);
                    allHVCs.Add(hvc);


                    if(i==3) new WriteClimaSchemaCollection(this.dataset, classifcationsToOutput(), path + "\\baseValues.json", true, true);
                    new WriteClimaSchemaCollection(this.dataset, classifcationsToOutput(), output + "\\" + i.ToString() + "clusters.json", indentClimaChk.Checked, this.compactOutputChkbx.Checked);
                    this.dataset.classifications.Remove(hvc);
                    
                }
                
                //StreamWriter sw = new StreamWriter(path+"\\clusterPerformanceStudy.csv");
                //sw.WriteLine(",,UTCI,,IDEAM CI,,APP_TEMP,,DISTORTION,SILHOUETTE CF");
                //sw.WriteLine("n clusters,,abs dev,SD,abs dev,SD,abs dev,SD,,");
                //for (int i = 0; i < allHVCs.Count; i++)
                //{
                //    sw.WriteLine(allHVCs[i].kClusters);
                //    for(int j=0;j<allHVCs[i].kClusters;j++)
                //    {
                //        sw.WriteLine(","+j+","
                //            + Math.Round(allHVCs[i].clusterUTCI_avAbsDev[j],3) + "," + Math.Round(allHVCs[i].clusterUTCISD[j], 3) + "," 
                //            + Math.Round(allHVCs[i].clusterIDEAMCI_avAbsDev[j], 3) + "," + Math.Round(allHVCs[i].clusterIDEAMCISD[j], 3) + ","
                //            + Math.Round(allHVCs[i].clusterAPPTEMP_avAbsDev[j], 3) + "," + Math.Round(allHVCs[i].clusterAPPTEMPSD[j], 3) + ","
                //            + Math.Round(allHVCs[i].distort[j], 3) + ","+ Math.Round(allHVCs[i].silhouetteCF[j], 3));
                //    }
                //    sw.WriteLine(",means:," 
                //        + Math.Round(allHVCs[i].clusterUTCI_avAbsDev.Mean(),3) + "," + Math.Round(allHVCs[i].clusterUTCISD.Mean(), 3) + "," 
                //        + Math.Round(allHVCs[i].clusterIDEAMCI_avAbsDev.Mean(), 3) + "," + Math.Round(allHVCs[i].clusterIDEAMCISD.Mean(), 3) + ","
                //        + Math.Round(allHVCs[i].clusterAPPTEMP_avAbsDev.Mean(), 3) + "," + Math.Round(allHVCs[i].clusterAPPTEMPSD.Mean(), 3) + ","
                //        + Math.Round(allHVCs[i].distort.Mean(), 3) + "," + Math.Round(allHVCs[i].silhouetteCF.Mean(), 3));
                //}
                //sw.Close();
                
            }
            catch (Exception ex)
            {
                int a = 1;
            }
            
        }
        private void holdridgeBtn_Click(object sender, EventArgs e)
        {
            HoldridgeClass hc = null;
            try
            {
                hc = new HoldridgeClass(this.dataset, "climateData");
                hc.description = "holdridgeData";
                this.dataset.classifications.Add(hc);
            }
            catch { }
            try {
                hc = new HoldridgeClass(this.dataset, "climateDataAverages");
                hc.description = "holdridgeDataAverage";
                this.dataset.classifications.Add(hc);
            }
            catch { }
            statusUpdate();
        }

        private void resetBtn_Click(object sender, EventArgs e)
        {
            this.dataset = new WeatherData();
            statusUpdate();
        }

        private void statsBtn_Click(object sender, EventArgs e)
        {
            StatsClass sc = null;
            try {
                sc= new StatsClass(this.dataset, "climateDataAverages");
                sc.description = "climateStatsAverages";
                this.dataset.classifications.Add(sc);
                
            }
            catch{            }
            try
            {
                sc= new StatsClass(this.dataset, "climateData");
                sc.description = "climateStats";
                this.dataset.classifications.Add(sc);

            }
            catch { }
            statusUpdate();
        }

        private void windBtn_Click(object sender, EventArgs e)
        {
            GetFTPData ftp = new GetFTPData();
            ftp.download();
        }

        private void clusterBtn_Click(object sender, EventArgs e)
        {
            Cluster cluster = new Cluster();
            cluster.test1();
        }
    }

    
    
}
