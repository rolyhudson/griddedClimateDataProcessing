using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace gridData01
{
    class ASCIIgridTools
    {
        public string error;
        public bool getWeather(ref WeatherData dataset,string folder,int start, int end)
        {
            //get several years and each month of the year for each we need temp, vapour pressure and 
            ASCIIgrid worldWeather = new ASCIIgrid();
            dataset.setTimePeriod(start, end);
            
            string[] f = { "temp", "vp", "rh", "tmin", "tmax", "trange", "precip","windSpd" };
            List<string> fields = new List<string>();
            fields.AddRange(f);

            string[] cf = { "temp","rh","precip" };
            List<string> cfields = new List<string>();
            cfields.AddRange(cf);

            WeatherClass weather = new WeatherClass();
            weather.setDataCollection(dataset.gridPoints.Count, dataset.years);
            weather.description = "climateData";
            weather.extendedDescription = "Climate monthly averages for " + dataset.srtYr.ToString() + " to " + dataset.endYr.ToString();
            weather.setDataFields(fields);
            weather.setDataFieldsCompact(cfields);
            List<WeatherPoint> wpoints = weather.dataPoints.Cast<WeatherPoint>().ToList();
            
            int month = 0;
            int year = 0;
            string monthDigits = "";
            try
            {
                for (int i = dataset.srtYr; i <= dataset.endYr; i++)//years
                {
                    for (int j = 1; j <= 12; j++)//months
                    {
                        //get temp  List<Y> listOfY = listOfX.Cast<Y>().ToList()
                        month = j - 1;
                        worldWeather = readASCIIgrid(folder + "\\cru_ts_3_10.1901.2009.raster_ascii.tmp\\tmp\\cru_ts_3_10.1901.2009.tmp_" + i + "_" + j + ".asc");
                        getDataFromGrid(dataset.gridPoints, ref wpoints, worldWeather, "temp", year, month);
                        //get vap press
                        worldWeather = readASCIIgrid(folder + "\\cru_ts_3_10.1901.2009.raster_ascii.vap\\vap\\cru_ts_3_10.1901.2009.vap_" + i + "_" + j + ".asc");
                        getDataFromGrid(dataset.gridPoints, ref wpoints, worldWeather, "vp", year, month);
                        //get min temp
                        worldWeather = readASCIIgrid(folder + "\\cru_ts_3_10.1901.2009.raster_ascii.tmn\\tmn\\cru_ts_3_10.1901.2009.tmn_" + i + "_" + j + ".asc");
                        getDataFromGrid(dataset.gridPoints, ref wpoints, worldWeather, "tmin", year, month);
                        //get max temp
                        worldWeather = readASCIIgrid(folder + "\\cru_ts_3_10.1901.2009.raster_ascii.tmx\\tmx\\cru_ts_3_10.1901.2009.tmx_" + i + "_" + j + ".asc");
                        getDataFromGrid(dataset.gridPoints, ref wpoints, worldWeather, "tmax", year, month);
                        //get temp range
                        worldWeather = readASCIIgrid(folder + "\\cru_ts_3_10.1901.2009.raster_ascii.dtr\\dtr\\cru_ts_3_10.1901.2009.dtr_" + i + "_" + j + ".asc");
                        getDataFromGrid(dataset.gridPoints, ref wpoints, worldWeather, "trange", year, month);
                        //get precipitation
                        worldWeather = readASCIIgrid(folder + "\\cru_ts_3_10_01.1901.2009.raster_ascii.pre\\pre\\cru_ts_3_10_01.1901.2009.pre_" + i + "_" + j + ".asc");
                        getDataFromGrid(dataset.gridPoints, ref wpoints, worldWeather, "pre", year, month);
                        //getWind
                        if (j < 10) monthDigits = "0" + j.ToString();
                        else monthDigits = j.ToString();
                        worldWeather = readASCIIgrid(folder + "\\wind_ascii\\CCMP_Wind_Analysis_" + i + monthDigits + "_V02.0_L3.5_RSS.asc");
                        getDataFromGrid(dataset.gridPoints, ref wpoints, worldWeather, "windSpd", year, month);


                    }
                    year++;
                }
                foreach (WeatherPoint wp in wpoints) wp.calcRH();
                weather.setData(wpoints);
                dataset.classifications.Add(weather);
                dataset.flagNullLocations();
                return true;
            }
            catch(Exception e)
            {
                error = "Problems encountered with the weather data";
                return false;
            }

        }
        public bool getElevation(ref WeatherData dataset,string folder)
        {
            ASCIIgrid elevationData = new ASCIIgrid();
            string targetDirectory = folder;
            try {
                string[] fileEntries = Directory.GetFiles(targetDirectory);
                for (int i = 0; i < fileEntries.Length; i++)
                {
                    elevationData = readASCIIgrid(fileEntries[i]);
                    getDataFromGrid(ref dataset.gridPoints, elevationData, 0, 0);
                }
                return true;
            }
            catch
            {
                error = "Problems encountered with the elevation data";
                return false;
            }

        }
        private void getDataFromGrid(ref List<LocationPoint> gridPoints, ASCIIgrid griddata,  int yr, int month)
        {
            //this is just for extracting elevation data from a DEM given a collection location point with lat and long
            double xll = griddata.xll;// min longitude
            double yll = griddata.yll; //min latitude
            double gridAngle = griddata.cellsize;
            double[] point = new double[2];
            int pointsMatched = 0;
            for (int i = 0; i < gridPoints.Count; i++)
            {
                point[0] = gridPoints[i].longitude;
                point[1] = gridPoints[i].latitude;
                if (MapTools.isPointInPolygon(point, griddata.boundary))
                {
                    pointsMatched++;

                    var rowCol = getRowCol(xll, yll, gridPoints[i].latitude, gridPoints[i].longitude, gridAngle, griddata.data.Count - 1);
                    gridPoints[i].altitude = griddata.data[rowCol[0]][rowCol[1]];
                    
                }
            }

        }
        private void getDataFromGrid(List<LocationPoint> gridPoints, ref List<WeatherPoint> wPoints, ASCIIgrid griddata, string field, int yr, int month)
        {
            double xll = griddata.xll;// min longitude
            double yll = griddata.yll; //min latitude
            double gridAngle = griddata.cellsize;
            double[] point = new double[2];
            int pointsMatched = 0;
            for (int i = 0; i < gridPoints.Count; i++)
            {
                point[0] = gridPoints[i].longitude;
                point[1] = gridPoints[i].latitude;
                if (MapTools.isPointInPolygon(point, griddata.boundary))
                {
                    pointsMatched++;

                    var rowCol = getRowCol(xll, yll, gridPoints[i].latitude, gridPoints[i].longitude, gridAngle, griddata.data.Count - 1);
                    if (field == "temp") wPoints[i].temp[yr, month] = griddata.data[rowCol[0]][rowCol[1]]/10.0;
                    if (field == "vp") wPoints[i].vp[yr, month] = griddata.data[rowCol[0]][rowCol[1]] / 10.0;
                    if (field == "tmin") wPoints[i].tmin[yr, month] = griddata.data[rowCol[0]][rowCol[1]]/ 10.0;
                    if (field == "tmax") wPoints[i].tmax[yr, month] = griddata.data[rowCol[0]][rowCol[1]]/ 10.0;
                    if (field == "trange") wPoints[i].trange[yr, month] = griddata.data[rowCol[0]][rowCol[1]]/ 10.0;
                    if (field == "pre") wPoints[i].precip[yr, month] = griddata.data[rowCol[0]][rowCol[1]];
                    if(field=="windSpd") wPoints[i].windSpd[yr, month] = griddata.data[rowCol[0]][rowCol[1]]/100.0;
                }
            }

        }
        private int[] getRowCol(double worldCxll, double worldCyll, double lat, double lon, double gridsize, int lastRow)
        {
            int[] rowCol = new int[2];
            rowCol[0] = lastRow - (int)Math.Round((lat - worldCyll) / gridsize, 0);
            rowCol[1] = (int)Math.Round((lon - worldCxll) / gridsize, 0);
            return rowCol;
        }
        private ASCIIgrid readASCIIgrid(string file)
        {
            StreamReader sr = new StreamReader(file);
            string line = sr.ReadLine();
            List<List<int>> dataset = new List<List<int>>();
            ASCIIgrid grid = new ASCIIgrid();
            List<int> row = new List<int>();
            int lastSpc = 0;
            int lineCount = 0;
            while (line != null)
            {

                if (lineCount == 6)
                {
                    row = new List<int>();
                    string[] parts = line.Split(' ');
                    for (int i = 0; i < parts.Length; i++)
                    {
                        if (parts[i] != "")
                        {
                            row.Add(int.Parse(parts[i]));
                        }
                    }
                    grid.data.Add(row);
                }
                else
                {
                    lastSpc = line.LastIndexOf(' ');
                    string value = line.Substring(lastSpc);
                    switch (lineCount)
                    {
                        case 0://ncols
                            grid.cellsX = int.Parse(value);
                            break;
                        case 1://nrows
                            grid.cellsY = int.Parse(value);
                            break;
                        case 2://xll
                            grid.xll = Convert.ToDouble(value);
                            break;
                        case 3://yll
                            grid.yll = Convert.ToDouble(value);
                            break;
                        case 4://cellsize
                            grid.cellsize = Convert.ToDouble(value);
                            grid.boundary = defineBoundaryPts(grid);
                            break;
                        case 5://NDATA
                            grid.ndata = double.Parse(value);
                            break;
                    }
                    lineCount++;
                }

                line = sr.ReadLine();
            }
            return grid;
        }
        private List<double[]> defineBoundaryPts(ASCIIgrid grid)
        {
            double width = grid.cellsX * grid.cellsize;
            double height = grid.cellsY * grid.cellsize;

            List<double[]> bound = new List<double[]>();
            double[] lleft = new double[] { grid.xll, grid.yll };
            double[] lright = new double[] { grid.xll + width, grid.yll };
            double[] uright = new double[] { grid.xll + width, grid.yll + height };
            double[] uleft = new double[] { grid.xll, grid.yll + height };
            bound.Add(lleft);
            bound.Add(lright);
            bound.Add(uright);
            bound.Add(uleft);
            return bound;
        }
    }
    public class ASCIIgrid
    {
        public double xll { get; set; }
        public double yll { get; set; }
        public double cellsize { get; set; }
        public int cellsX { get; set; }
        public int cellsY { get; set; }
        public double ndata { get; set; }
        public List<double[]> boundary = new List<double[]>();
        public List<List<int>> data = new List<List<int>>();
    }
}
