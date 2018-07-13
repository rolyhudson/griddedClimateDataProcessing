using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace gridData01
{
    class GetFTPData
    {
        public void download()
        {
            string folderY = "";
            string folderM = "";
            string file = "";
            string mAsString = "";
            string address = "ftp://ftp.remss.com/ccmp/v02.0/";
            string fullAddress = "";
            string dlFolder = @"C:\Users\r.hudson\Documents\WORK\piloto\griddedDataSet\climateData\wind\";
            WebClient request = new WebClient();
            request.Credentials = new NetworkCredential("roland-hudson@unipiloto.edu.co", "roland-hudson@unipiloto.edu.co");
            for(int i = 2000;i<2010;i++)
            {
                folderY = "Y" + i.ToString();

                for(int j=1;j<13; j++)
                {
                    if(j<10) mAsString = "0" + j.ToString();
                    else mAsString = j.ToString();

                    folderM = "M" + mAsString;
                    file = "CCMP_Wind_Analysis_" + i.ToString() + mAsString + "_V02.0_L3.5_RSS.nc";
                    fullAddress = address + folderY + "/" + folderM + "/" + file;
                    byte[] fileData = request.DownloadData(fullAddress);

                    FileStream fileDL = File.Create(dlFolder + file);
                    fileDL.Write(fileData, 0, fileData.Length);
                    fileDL.Close();

                }
            }
        }
    }
}
