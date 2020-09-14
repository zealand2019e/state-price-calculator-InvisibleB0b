using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace StatePriceCalculator
{
    class Server
    {

        public static void Start()
        {

            try
            {
                TcpListener server = null;

                IPAddress localAddress = null;
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        Console.WriteLine(ip.ToString());

                        localAddress = IPAddress.Parse(ip.ToString());

                    }
                }

                int port = 3001;


                server = new TcpListener(IPAddress.Loopback, port);

                server.Start();

                Console.WriteLine("Waiting for a connection........");

                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Connected!");

                DoClient(client);


                server.Stop();
                Console.WriteLine("server stopped");

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


        }

        public static void DoClient(TcpClient socket)
        {
            Stream ns = socket.GetStream();
            StreamReader sr = new StreamReader(ns);
            StreamWriter sw = new StreamWriter(ns);
            sw.AutoFlush = true;

            sw.WriteLine("You are connected!!!");


            while (true)
            {



                string message = sr.ReadLine();

                if (message.ToLower().Contains("luk"))
                {
                    break;
                }



                Console.WriteLine("Received message : " + message);



                double first = Convert.ToDouble(message.Split(" ")[0]);

                int secound = Convert.ToInt32(message.Split(" ")[1]);

                string third = message.Split(" ")[2].ToUpper();

                switch (third)
                {
                    case "DKK":
                        sw.WriteLine($"result : {first * secound * 1.25}");
                        break;
                    case "USA":
                        sw.WriteLine($"result : {first * secound * 1.05} ");
                        break;
                    case "UK":
                        sw.WriteLine($"result : {first * secound * 1.10}");
                        break;
                    case "SPA":
                        sw.WriteLine($"result : {first * secound * 1.15}");
                        break;
                    default:
                        sw.WriteLine("Not a function");
                        break;
                }




            }
            sw.WriteLine("Luk");

            ns.Close();

            Console.WriteLine("net stream closed");

            socket.Close();
            Console.WriteLine("client closed");
        }
    }
}
