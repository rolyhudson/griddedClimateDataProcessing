using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gridData01
{
    class DataField
    {
        public string name;
        public string unit;
        public string descrip;
       
        public string min;
        public string max;
        public string[] labels;
        public bool discreteScale;
        public DataField()
        {

        }
        //private string[] readLabelsFromFile(string path)
        //{

        //}
        public void specialSet(string n, string u, string maxi, string mini, string des, string[] lbls,bool discrete)
        {
            name = n;
            unit = u;
            max = maxi;
            min = mini;
            descrip = des;
            labels = lbls;
            discreteScale = discrete;
        }
        public void autoSet(string val)
        {
            this.name = val;
            discreteScale = false;
            switch (val)
            {
                case "Tmin":
                    unit = "C";
                    descrip = "monthly mean temperatures of the coldest month";
                    max = "40";
                    min = "-15";
                    break;
                case "Tann":
                    unit = "C";
                    descrip = "annual mean near-surface (2 m) temperature";
                    max = "40";
                    min = "-15";
                    break;
                case "Tmax":
                    unit = "C";
                    descrip = "monthly mean temperatures of the warmest month";
                    max = "40";
                    min = "-15";
                    break;
                case "Tmon":
                    unit = "";
                    descrip = "number of months with >= 10c average temp";
                    max = "12";
                    min = "1";
                    discreteScale = true;
                    break;
                case "Pann":
                    unit = "mm";
                    descrip = "accumulated annual precipitation";
                    max = "10000";
                    min = "0";
                    break;
                case "Psmin":
                    unit = "mm";
                    descrip = " lowest monthly precipitation values for the summer half - years on the hemisphere considered";
                    max = "6000";
                    min = "0";
                    break;
                case "Pmin":
                    unit = "mm";
                    descrip = " precipitation of the dryest month";
                    max = "6000";
                    min = "0";
                    break;
                case "Psmax":
                    unit = "mm";
                    descrip = " highest monthly precipitation values for the summer half - years on the hemisphere considered";
                    max = "6000";
                    min = "0";
                    break;
                case "Pwmin":
                    unit = "mm";
                    descrip = " lowest monthly precipitation values for the winter half - years on the hemisphere considered";
                    max = "6000";
                    min = "0";
                    break;
                case "Pwmax":
                    unit = "mm";
                    descrip = " highest monthly precipitation values for the winter half - years on the hemisphere considered";
                    max = "6000";
                    min = "0";
                    break;
                case "Pth":
                    unit = "mm";
                    descrip = "dryness threshold dependent on annual mean near-surface (2 m) temperature";
                    max = "100";
                    min = "0";
                    break;
                case "kgClass":
                    unit = "";
                    descrip = "Koppen Geiger Climate Classfication code";
                    max = "na";
                    min = "na";
                    labels = new string[] { "Af","Am","As","Aw","BWk","BWh","BSk","BSh","Cfa","Cfb","Cfc","Csa","Csb","Csc","Cwa",
                                            "Cwb","Cwc","Dfa","Dfb","Dfc","Dfd","Dsa","Dsb","Dsc","Dsd","Dwa","Dwb","Dwc","Dwd","EF","ET"};
                    break;
                case "kgClassNum":
                    unit = "";
                    descrip = "Koppen Geiger Climate Classfication numeric";
                    max = "30";
                    min = "0";
                    discreteScale = true;
                    break;
                case "kgClassDescriptive":
                    unit = "";
                    descrip = "Koppen Geiger Climate Classfication description";
                    max = "na";
                    min = "na";
                    labels = new string[] { "equatorial fully humid (Af)", "equatorial monsoonal (Am)", "equatorial summer dry (As)",
                        "equatorial winter dry (Aw)", "arid desert cold arid (BWk)", "arid desert hot arid (BWh)", "arid steppe cold arid (BSk)",
                        "arid steppe hot arid (BSh)", "warm temperate fully humid hot summer (Cfa)", "warm temperate fully humid warm summer (Cfb)",
                        "warm temperate fully humid cool summer (Cfc)", "warm temperate summer dry hot summer (Csa)",
                        "warm temperate summer dry warm summer (Csb)", "warm temperate summer dry cool summer (Csc)",
                        "warm temperate winter dry hot summer (Cwa)", "warm temperate winter dry warm summer (Cwb)",
                        "warm temperate winter dry cool summer (Cwc)", "snow fully humid hot summer (Dfa)", "snow fully humid warm summer (Dfb)",
                        "snow fully humid cool summer (Dfc)", "snow fully humid extremely continental (Dfd)", "snow summer dry hot summer (Dsa)",
                        "snow summer dry warm summer (Dsb)", "snow summer dry cool summer (Dsc)", "snow summer dry extremely continental (Dsd)",
                        "snow winter dry hot summer (Dwa)", "snow winter dry warm summer (Dwb)", "snow winter dry cool summer (Dwc)",
                        "snow winter dry extremely continental (Dwd)", "polar polar frost (EF)", "polar polar tundra (ET)" };
                    break;
                case "temp":
                    unit = "C";
                    descrip = "monthly mean temperature";
                    max = "40";
                    min = "-15";
                    break;
                case "rh":
                    unit = "%";
                    descrip = "monthly mean relative humdity";
                    max = "100";
                    min = "0";
                    break;
                case "vp":
                    unit = "hPa";
                    descrip = "monthly mean vapour pressure";
                    max = "30";
                    min = "1";
                    break;
                case "tmax":
                    unit = "C";
                    descrip = "monthly mean maximum temperature";
                    max = "40";
                    min = "-15";
                    break;
                case "tmin":
                    unit = "C";
                    descrip = "monthly mean minimum temperature";
                    max = "40";
                    min = "-15";
                    break;
                case "trange":
                    unit = "C";
                    descrip = "monthly mean temperature range";
                    max = "20";
                    min = "2";
                    discreteScale = true;
                    break;
                case "precip":
                    unit = "mm";
                    descrip = "monthly mean precipitation";
                    max = "10000";
                    min = "0";
                    break;
                case "windSpd":
                    unit = "ms-1";
                    descrip = "monthly mean wind speed";
                    max = "20";
                    min = "0";
                    break;
                case "appTemp":
                    unit = "C";
                    descrip = "apparent temperature"; // see Robert G. Steadman. 1994
                    max = "40";
                    min = "-15";
                    break;
                   
                case "utci":
                    unit = "C";
                    descrip = "Universal Thermal Climate Index"; 
                    max = "40";
                    min = "-15";
                    break;
                case "ideamIC":
                    unit = "";
                    descrip = "IDEAM comfort index";
                    max = "20";
                    min = "0";
                    discreteScale = true;
                    break; 
                case "ideamICClass":
                    unit = "";
                    descrip = "IDEAM comfort class"; 
                    max = "6";
                    min = "0";
                    labels = new string[] { "Muy caluroso", "Caluroso", "Cálido", "Agradable", "Algo frío", "Frío","Muy frío" };
                    discreteScale = true;
                    break; 
                case "altitude":
                    unit = "m";
                    descrip = "altitude";
                    max = "4000";
                    min = "0";
                    break;
                case "beaufortScale":
                    unit = "";
                    descrip = "beaufort wind force scale"; // see Robert G. Steadman. 1994
                    max = "12";
                    min = "0";
                    labels = new string[] { "calm", "light air", "light breeze", "gentle breeze", "moderate breeze", "fresh breeze",
                        "strong breeze","high wind","gale","strong/severe gale","storm","violent storm","huricane force" };
                    discreteScale = true;
                    break;
                case "HVclassDescriptor":
                    unit = "";
                    descrip = "hudsonVelascoClass";
                    max = "na";
                    min = "na";
                    break;
                case "HVcontinousColor":
                    unit = "";
                    descrip = "continous colouring of data points";
                    max = "na";
                    min = "na";
                    break;
                case "HVdiscreteColor":
                    unit = "";
                    descrip = "discrete colouring of data points";
                    max = "na";
                    min = "na";
                    break;
                case "clusterIndex":
                    unit = "";
                    descrip = "K means clustering";
                    max = "10";
                    min = "0";
                    discreteScale = true;
                    break;
                case "clusterClass":
                    unit = "";
                    descrip = "K means clustering class description";
                    max = "na";
                    min = "na";
                    labels = new string[] { "T25.94_RH84.84_Wlight-gentle breeze", "T25.32_RH89.84_Wlight-gentle breeze", "T27.32_RH73.85_Wlight-gentle breeze", "T26.75_RH81.85_Wgentle-fresh breeze", "T17.79_RH85.71_Wlight-gentle breeze", "T15.99_RH77.42_Wlight-gentle breeze", "T28.21_RH65.86_Wgentle-fresh breeze", "T25.4_RH87.73_Wgentle-fresh breeze", "T26.06_RH79.66_Wlight-gentle breeze", "T17_RH94.61_Wlight-gentle breeze" };
                    break;
                case "bioTemp":
                    unit = "C";
                    descrip = "mean of temperatures above 0 and less than 30";
                    max = "36";
                    min = "0";
                    break;
                case "potentialEvapRatio":
                    unit = "";
                    descrip = "potential evapotranspiration ratio (PET) to mean precipitation";
                    max = "32";
                    min = "0.125";
                    break;
                case "holdridgeClass":
                    unit = "";
                    descrip = "Description of the zone shown in the typical 3 axis system";
                    max = "na";
                    min = "na";
                    labels = new string[]  {"Tropical desert","Tropical desert scrub","Tropical thorn woodland","Tropical very dry forest","Tropical dry forest","Tropical moist forest","Tropical wet forest","Tropical rain forest",
            "Subtropical desert","Subtropical desert scrub","Subtropical thorn woodland","Subtropical dry forest","Subtropical moist forest","Subtropical wet forest","Subtropical rain forest",
            "Warm temperate desert","Warm temperate desert scrub","Warm temperate thorn scrub","Warm temperate dry forest","Warm temperate moist forest","Warm temperate wet forest","Warm temperate rain forest",
            "Cool temperate desert","Cool temperate desert scrub","Cool temperate steppe","Cool temperate moist forest","Cool temperate wet forest","Cool temperate rain forest",
            "Boreal desert","Boreal dry scrub","Boreal moist forest","Boreal wet forest","Boreal rain forest",
            "Subpolar dry tundra","Subpolar moist tundra","Subpolar wet tundra","Subpolar rain tundra",
            "Polar desert","Polar desert","Polar desert",
            "Ice","Ice",
            "Ice"};
                    break;
                case "holdridgeClassNum":
                    unit = "";
                    descrip = "Classfication numeric refering to the 44 zones shown in the typical 3 axis system";
                    max = "43";
                    min = "0";
                    discreteScale = true;
                    break;
                  
                case "tempAnnualStdDevPt":
                    unit = "C";
                    descrip = "Mean temperature standard deviation for single data point for a year";
                    max = "10";
                    min = "0";
                    discreteScale = true;
                    break;
                case "tempAnnualStdDevData":
                    unit = "C";
                    descrip = "Mean temperature standard deviation for all data points for a year";
                    max = "10";
                    min = "0";
                    discreteScale = true;
                    break;
                case "tempAnnualAbsChgData":
                    unit = "C";
                    descrip = "Mean temperature absolute change for all data points for a year";
                    max = "15";
                    min = "-15";
                    break;
                case "tempMonthlyAbsChgPt":
                    unit = "C";
                    descrip = "Mean temperature absolute change for a single data point at selected year and month";
                    max = "15";
                    min = "-15";
                    break;
                case "tempMonthlyStdDevData":
                    unit = "C";
                    descrip = "Mean temperature standard deviation for all data points at selected year and month";
                    max = "10";
                    min = "0";
                    discreteScale = true;
                    break;
                case "tempMonthlyAbsChgData":
                    unit = "C";
                    descrip = "Mean temperature absolute change for all data points at selected year and month";
                    max = "15";
                    min = "-15";
                    break;

                case "vpAnnualStdDevPt":
                    unit = "hPa";
                    descrip = "Mean vapour pressure standard deviation for a single data point for a year";
                    max = "10";
                    min = "1";
                    discreteScale = true;
                    break;
                case "vpAnnualStdDevData":
                    unit = "hPa";
                    descrip = "Mean vapour pressure standard deviation for all data points for a year";
                    max = "10";
                    min = "1";
                    discreteScale = true;
                    break;
                case "vpAnnualAbsChgData":
                    unit = "hPa";
                    descrip = "Mean vapour pressure absolute change for all data points for a year";
                    max = "14";
                    min = "-14";
                    discreteScale = true;
                    break;
                case "vpMonthlyAbsChgPt":
                    unit = "hPa";
                    descrip = "Mean vapour pressure absolute change for a single data point at selected year and month";
                    max = "14";
                    min = "-14";
                    discreteScale = true;
                    break;
                case "vpMonthlyStdDevData":
                    unit = "hPa";
                    descrip = "Mean vapour pressure standard deviation for all data points at selected year and month";
                    max = "10";
                    min = "1";
                    discreteScale = true;
                    break;
                case "vpMonthlyAbsChgData":
                    unit = "hPa";
                    descrip = "Mean vapour pressure absolute change for all data points at selected year and month";
                    max = "14";
                    min = "-14";
                    discreteScale = true;
                    break;
                

                case "rhAnnualStdDevPt":
                    unit = "%";
                    descrip = "Mean relative humidity standard deviation for a single data point for a year";
                    max = "40";
                    min = "0";
                    break;
                case "rhAnnualStdDevData":
                    unit = "%";
                    descrip = "Mean relative humidity standard deviation for a all data points for a year";
                    max = "40";
                    min = "0";
                    break;
                case "rhAnnualAbsChgData":
                    unit = "%";
                    descrip = "Mean relative humidity absolute change for all data points for a year";
                    max = "20";
                    min = "-20";
                    break;
                case "rhMonthlyAbsChgPt":
                    unit = "%";
                    descrip = "Mean relative humidity absolute change for a single data point at selected year and month";
                    max = "20";
                    min = "-20";
                    break;
                case "rhMonthlyStdDevData":
                    unit = "%";
                    descrip = "Mean relative humidity standard deviation for all data points at selected year and month";
                    max = "40";
                    min = "0";
                    break;
                case "rhMonthlyAbsChgData":
                    unit = "%";
                    descrip = "Mean relative humidity absolute change for all data points at selected year and month";
                    max = "20";
                    min = "-20";
                    break;

                case "tminAnnualStdDevPt":
                    unit = "C";
                    descrip = "Mean minimum temperature standard deviation for a single data point for a year";
                    max = "10";
                    min = "0";
                    discreteScale = true;
                    break;
                case "tminAnnualStdDevData":
                    unit = "C";
                    descrip = "Mean minimum temperature standard deviation for all data points for a year";
                    max = "10";
                    min = "0";
                    discreteScale = true;
                    break;
                case "tminAnnualAbsChgData":
                    unit = "C";
                    descrip = "Mean minimum temperature absolute change all data points for a year";
                    max = "10";
                    min = "-10";
                    break;
                case "tminMonthlyAbsChgPt":
                    unit = "C";
                    descrip = "Mean minimum temperature absolute change for a single data point at selected year and month";
                    max = "10";
                    min = "-10";
                    break;
                case "tminMonthlyStdDevData":
                    unit = "C";
                    descrip = "Mean minimum temperature standard deviation for all data points at selected year and month";
                    max = "10";
                    min = "0";
                    discreteScale = true;
                    break;
                case "tminMonthlyAbsChgData":
                    unit = "C";
                    descrip = "Mean minimum temperature absolute change for all data points at selected year and month";
                    max = "10";
                    min = "-10";
                    break;

                case "tmaxAnnualStdDevPt":
                    unit = "C";
                    descrip = "Mean maximum temperature standard deviation for a single data point for a year";
                    max = "10";
                    min = "0";
                    discreteScale = true;
                    break;
                case "tmaxAnnualStdDevData":
                    unit = "C";
                    descrip = "Mean maximum temperature standard deviation for all data points for a year";
                    max = "10";
                    min = "0";
                    discreteScale = true;
                    break;
                case "tmaxAnnualAbsChgData":
                    unit = "C";
                    descrip = "Mean maximum temperature absolute change for all data points for a year";
                    max = "10";
                    min = "-10";
                    break;
                case "tmaxMonthlyAbsChgPt":
                    unit = "C";
                    descrip = "Mean maximum temperature absolute change for all data points at selected year and month";
                    max = "10";
                    min = "-10";
                    break;
                case "tmaxMonthlyStdDevData":
                    unit = "C";
                    descrip = "Mean maximum temperature standard deviation for all data points at selected year and month";
                    max = "10";
                    min = "0";
                    discreteScale = true;
                    break;
                case "tmaxMonthlyAbsChgData":
                    unit = "C";
                    descrip = "Mean maximum temperature absolute change for all data points at selected year and month";
                    max = "10";
                    min = "-10";
                    break;

                case "trangeAnnualStdDevPt":
                    unit = "C";
                    descrip = "Mean temperature range standard deviation for a single data point for a year";
                    max = "10";
                    min = "0";
                    discreteScale = true;
                    break;
                case "trangeAnnualStdDevData":
                    unit = "C";
                    descrip = "Mean temperature range standard deviation for all data points for a year";
                    max = "10";
                    min = "0";
                    discreteScale = true;
                    break;
                case "trangeAnnualAbsChgData":
                    unit = "C";
                    descrip = "Mean temperature range absolute change for all data points for a year";
                    max = "10";
                    min = "10";
                    break;
                case "trangeMonthlyAbsChgPt":
                    unit = "C";
                    descrip = "Mean temperature range absolute change for a single data point at selected year and month";
                    max = "10";
                    min = "-10";
                    break;
                case "trangeMonthlyStdDevData":
                    unit = "C";
                    descrip = "Mean temperature range standard deviation for all data points at selected year and month";
                    max = "10";
                    min = "0";
                    discreteScale = true;
                    break;
                case "trangeMonthlyAbsChgData":
                    unit = "C";
                    descrip = "Mean temperature range absolute change for all data points at selected year and month";
                    max = "10";
                    min = "-10";
                    break;

                case "precipAnnualStdDevPt":
                    unit = "mm";
                    descrip = "Mean precipitation standard deviation for a single data point for a year";
                    max = "3000";
                    min = "0";
                  
                    break;
                case "precipAnnualStdDevData":
                    unit = "mm";
                    descrip = "Mean precipitation standard deviation for all data points for a year";
                    max = "3000";
                    min = "0";

                    break;
                case "precipAnnualAbsChgData":
                    unit = "mm";
                    descrip = "Mean precipitation absolute change for all data points for a year";
                    max = "1800";
                    min = "-1800";
                    break;
                case "precipMonthlyAbsChgPt":
                    unit = "mm";
                    descrip = "Mean precipitation absolute change for a single data point for a year";
                    max = "1800";
                    min = "-1800";
                    break;
                case "precipMonthlyStdDevData":
                    unit = "mm";
                    descrip = "Mean precipitation standard deviation for all data points at selected year and month";
                    max = "3000";
                    min = "0";
                    break;
                case "precipMonthlyAbsChgData":
                    unit = "mm";
                    descrip = "Mean precipitation absolute change for all data points at selected year and month";
                    max = "4800";
                    min = "-4800";
                    break;
                



            }
        }
    }
}
