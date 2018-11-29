﻿using CommonTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessCreationService
{
    public class ProcessCreationService : MarshalByRefObject

       
    {
        //List of clients
        Dictionary<string, Process> Clients = new Dictionary<string, Process>();

        //List of servers
        Dictionary<string, Process> Servers = new Dictionary<string, Process>();

        public ProcessCreationService()
        {

        }

        public void StartServer(string id,string url, int mindelay, int maxdelay,string serverid2, string algorithm)
        {

            //Initialize a process startinfo with the server.exe file
            ProcessStartInfo info = new ProcessStartInfo(AuxFunctions.GetProjPath() + "\\Server\\bin\\Debug\\Server.exe");

            //info.UseShellExecute = false;

            info.CreateNoWindow = false;

            //add the arguments to the info
            info.Arguments = url + " " + mindelay + " " + maxdelay + " " + serverid2 +  " " + algorithm;

            //Start the process
            Process P = Process.Start(info);

            //Add process to the process servers list
            Servers.Add(id, P);

        }

        public void StartClient(string script, string id, string algorithm)
        {
            //Initialize a process startinfo with the client.exe file
            ProcessStartInfo info = new ProcessStartInfo(AuxFunctions.GetProjPath() + "\\Client\\bin\\Debug\\Client.exe");

            //info.UseShellExecute = false;

            info.CreateNoWindow = false;

            //add the arguments to the info
            info.Arguments = script + " " + id + " " + algorithm;

            //Start the process
            Process P = Process.Start(info);

            Clients.Add(id, P);


        }

        public void Crash(string id)
        {
            foreach(KeyValuePair<string, Process> ServerProcess in Servers)
            {
                if (id == ServerProcess.Key)
                {
                    ServerProcess.Value.Kill();
                }
            }
        }
    }
}
