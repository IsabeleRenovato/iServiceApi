using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace iServiceServices.Services
{
    public class FtpServices
    {
        private string ftpServer = "149.100.142.179";
        private string ftpUsername = "root_ftp";
        private string ftpPassword = "27_53]KK{Gy(";

        public string UploadFile(IFormFile file, string remoteDir, string remoteFileName)
        {
            using (var imageStream = new MemoryStream())
            {
                file.CopyTo(imageStream);
                Image image = Image.FromStream(imageStream);
                MemoryStream pngStream = new MemoryStream();
                image.Save(pngStream, ImageFormat.Png);
                pngStream.Position = 0;

                string url = $"ftp://{ftpServer}/var/www/html/images/{remoteDir}/{remoteFileName}";
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
                request.UsePassive = true;
                request.UseBinary = true;
                request.KeepAlive = false;

                using (Stream reqStream = request.GetRequestStream())
                {
                    pngStream.CopyTo(reqStream);
                }

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    return $"http://srv452480.hstgr.cloud/images/{remoteDir}/{remoteFileName}";
                }
            }
        }
    }
}
