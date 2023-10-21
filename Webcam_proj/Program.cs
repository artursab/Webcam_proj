using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video.DirectShow;


namespace Webcam_proj
{
    class Program
    {
        private static IPEndPoint consumerEndPoint;
        
        static void Main(string[] args)
        {
            var consumerIp = ConfigurationManager.AppSettings.Get("consumerIp");
            var consumerPort = int.Parse(ConfigurationManager.AppSettings.Get("consumerPort"));
            consumerEndPoint = new IPEndPoint(IPAddress.Parse(consumerIp), consumerPort);

            Console.WriteLine($"consumer: {consumerEndPoint}");

            FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            VideoCaptureDevice videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
        }
    }
}
