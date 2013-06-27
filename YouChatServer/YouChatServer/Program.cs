using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace YouChatServer
{
    class Program
    {
        static Socket sck;
        static Socket acc;
        static int port = 9000;
        static IPAddress ip;
        static Thread rec;
        static string name;
        static string GetIP()
        {
            string strHostName = System.Net.Dns.GetHostName();
            IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);
            IPAddress[] addr = ipEntry.AddressList;
            return addr[addr.Length - 1].ToString();
        }
        static void recV()
        {
            while (true)
            { 
                Thread.Sleep(500);
                byte[] buffer = new byte[255];
                int rec = acc.Receive(buffer, 0, buffer.Length, 0);
                Array.Resize(ref buffer,rec);
                Console.WriteLine(Encoding.Default.GetString(buffer));
            }
        }
        static void Main(string[] args)
        {
            rec = new Thread(recV);
            Console.WriteLine("Your Ip is :"+GetIP());
            Console.WriteLine("Input your host Port ");
          string inputport=  Console.ReadLine();
            Console.WriteLine("Please Enter your Name");
            name= Console.ReadLine();
            try { port = Convert.ToInt32(inputport); }
            catch { port = 9000; }
            ip=IPAddress.Parse(GetIP());
            sck = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            sck.Bind(new IPEndPoint(ip,port));
            sck.Listen(0);
            acc= sck.Accept();
            rec.Start();
            while(true)
            {
                byte[] sdata = Encoding.Default.GetBytes( "<"+name+" :> "+Console.ReadLine());
                acc.Send(sdata,0,sdata.Length,0);
            }




        }
    }
}
