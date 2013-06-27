using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace YouChatClient
{
    class Program
    {
        static string name = "";
        static int port = 9000;
        static IPAddress ip;
        static Socket sck;
        static Thread rec;

        static void recV()
        {
            while (true)
            {
                Thread.Sleep(500);
                byte[] buffer = new byte[255];
                int rec = sck.Receive(buffer, 0, buffer.Length, 0);
                Array.Resize(ref buffer, rec);
                Console.WriteLine(Encoding.Default.GetString(buffer));
            }
        }
        static void Main(string[] args)
        {
            rec = new Thread(recV);
            Console.WriteLine("Please Enter your Name");
            name = Console.ReadLine();
            Console.WriteLine("Please Enter Your Server IP");
            ip= IPAddress.Parse(Console.ReadLine());
            Console.WriteLine("Input your  Port ");
           
            string inputport = Console.ReadLine();
            try { port = Convert.ToInt32(inputport); }
            catch { port = 9000; }
            sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sck.Connect(new IPEndPoint(ip, port));
            rec.Start();
            byte[] commsg = Encoding.Default.GetBytes("<" + name + " :> Connected" );
            sck.Send(commsg, 0, commsg.Length, 0);
            while (sck.Connected)
            {
                byte[] sdata = Encoding.Default.GetBytes("<" + name + " :> " + Console.ReadLine());
                sck.Send(sdata, 0, sdata.Length, 0);
            }

           
           
        }
    }
}
