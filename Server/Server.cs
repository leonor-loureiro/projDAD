﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using CommonTypes;


namespace Server
{
    class Server
    {
        public static TcpChannel channel;
        
        static void Main(string[] args)
        {
            string Url = "tcp://localhost:50001/S";
            int MinDelay = 0;
            int MaxDelay = 0;
            int Port;
            string Name;
            List<String> Servers = new List<string>();
            string serverid2 = " ";
            List<ITuple> newState = new List<ITuple>();
            TSpaceServerSMR server = null;
            SMRState serverState = null;
            View newview = new View();

            //Type of algorithm for server
            string algorithm = "x";


            if (args.Length > 0)
            {
                Url = args[0];
                algorithm = args[1];
                // Todo -> receive servers list as args

            }

            Port = getPortFromURL(Url);
            Name = getNameFromURL(Url);

            channel = new TcpChannel(Port);
            ChannelServices.RegisterChannel(channel, true);

            if (args.Length == 4)
            {

                MinDelay = Int32.Parse(args[1]);
                MaxDelay = Int32.Parse(args[2]);
                algorithm = args[3];
            }

            if(args.Length == 5)
            {
                MinDelay = Int32.Parse(args[1]);
                MaxDelay = Int32.Parse(args[2]);
                algorithm = args[4];
                serverid2 = args[3];

                //get the remote object of the other server
                if (algorithm.Equals("s")){
                    server = (TSpaceServerSMR)Activator.GetObject(typeof(TSpaceServerSMR), serverid2);
                    Console.WriteLine("getting state from server");
                    serverState = server.GetSMRState();
                    Console.WriteLine("got the state");
                 } 
                //get the view from the other server
                Console.WriteLine("Asked for view");
                //newview = server.UpdateView();
                //Console.WriteLine(newview.ToString());
                //get the tuples from the other server
                //newState = server.GetTuples();

            }




            if (algorithm == "x") {
                //RemotingConfiguration.RegisterWellKnownServiceType(typeof(TSpaceServerXL), Name, WellKnownObjectMode.Singleton);
                TSpaceServerXL TS = new TSpaceServerXL(Url, MinDelay,MaxDelay);
                RemotingServices.Marshal(TS, Name, typeof(ITSpaceServer));

                //set the tuples of the new server
                TS.SetTuples(newState);

                try
                {
                    TS.UpdateView();
                    
                }catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.GetType().ToString());
                    Console.WriteLine(e.StackTrace);


                }
            }
            if (algorithm == "s")
            {
                TSpaceServerSMR TS = null;
                //checking if there is a previous stat
                if (serverState != null)
                {
                    Console.WriteLine("previous state exists");
                    TS = new TSpaceServerSMR(Url, MinDelay, MaxDelay);
                }
                else
                {
                    Console.WriteLine("no previous state exists");
                    TS = new TSpaceServerSMR(Url, MinDelay, MaxDelay, newview);
                }

                RemotingServices.Marshal(TS, Name, typeof(TSpaceServerSMR));
                //RemotingConfiguration.RegisterWellKnownServiceType(typeof(TSpaceServerSMR), Name, WellKnownObjectMode.Singleton);

                //set the tuples of the new server
                

                try
                {
                    if (serverState != null)
                    {
                        Console.WriteLine("Setting previous state");
                        //TS.SetSMRState(serverState);
                        //TS.UpdateView();
                    }
                    else
                    {
                        Console.WriteLine("no previous state need to update");
                        TS.UpdateView();
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.GetType().ToString());
                    Console.WriteLine(e.StackTrace);


                }

            }



            System.Console.WriteLine("<enter> para sair...");
            System.Console.ReadLine();
        }

        static int getPortFromURL(string url)
        {
            string[] splitUrl = url.Split(':', '/');

            return Int32.Parse(splitUrl[4]);
        }

        static string getNameFromURL(string url)
        {
            string[] splitUrl = url.Split(':', '/');

            return splitUrl[5];
        }
    }
}