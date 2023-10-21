using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;


namespace Webcam_proj
{
    class Program
    {
        private static IPEndPoint consumerEndPoint;
        private static UdpClient udpClient = new UdpClient();
        
        static void Main(string[] args)
        {
            var consumerIp = ConfigurationManager.AppSettings.Get("consumerIp");
            var consumerPort = int.Parse(ConfigurationManager.AppSettings.Get("consumerPort"));
            consumerEndPoint = new IPEndPoint(IPAddress.Parse(consumerIp), consumerPort);

            Console.WriteLine($"consumer: {consumerEndPoint}");

            FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            VideoCaptureDevice videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
            videoSource.NewFrame += VideoSource_NewFrame;
            videoSource.Start();

            Console.ReadLine();
        }

        private static void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            var bmp = new Bitmap(eventArgs.Frame, 800, 600);

            try
            {
                using (var ms = new MemoryStream())
                {
                    bmp.Save(ms, ImageFormat.Jpeg);
                    var bytes = ms.ToArray();
                    udpClient.Send(bytes, bytes.Length, consumerEndPoint);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
