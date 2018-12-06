﻿using System;
using CommonTypes;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace CommonTypes
{
    /// <summary>
    /// Class representing a "View" on a server or client
    /// A view is a list of the current alive servers 
    /// </summary>
    [Serializable]
    public class View
    {
        private List<String> Servers { get; set; }

        public int ID { get; set; }

        public int Count { get => Servers.Count;  }


        List<ITuple> State = new List<ITuple>();


        public View()
        {
            Servers = new List<string>();
        }

        public View(View view)
        {
            Servers = new List<string>(view.DeepUrlsCopy());
            ID = view.ID;
        }

        public View(List<String> serverList)
        {
            Servers = new List<string>();
            // Appends serverList do the servers
            Servers.AddRange(serverList);
        }

        public View(List<String> serverList, int id)
        {
            Servers = new List<string>();
            // Appends serverList do the servers
            Servers.AddRange(serverList);
            ID = id;
        }

        public void Lock()
        {
            Monitor.Enter(Servers);
        }

        public void UnLock()
        {
            Monitor.Exit(Servers);
        }
        public List<String> DeepUrlsCopy()
        {
            lock(Servers){
                List<String> serversUrl = new List<string>();
                foreach (string s in Servers)
                {
                    serversUrl.Add(string.Copy(s));
                }
                return serversUrl;
            }
        }


        public List<String> GetUrls()
        {
            lock (Servers)
            {
                return Servers;
            }
        }

        public List<ITSpaceServer> GetProxys(string url)
        {
            lock (Servers)
            {
                List<ITSpaceServer> servers = new List<ITSpaceServer>();
            
                foreach (string serverUrl in Servers)
                {
                    if(!serverUrl.Equals(url))
                        servers.Add((ITSpaceServer)Activator.GetObject(typeof(ITSpaceServer), serverUrl));
                }
                return servers;
            }
        }


        public void Remove(String url)
        {
            lock (Servers)
            {
                Servers.Remove(url);
            }
        }



        override public string ToString()
        {
            lock (Servers)
            {
                string content = "";
                foreach (String server in Servers)
                {
                    content += " <" + server + " id:" + ID + "> ";
                }

                return content;
            }
        }

        /// <summary>
        /// Adds a server to the list of servers in this view
        /// Does not allow repeated elements to be added
        /// </summary>
        /// <param name="server"></param>
        public void Add(String server)
        {
            lock (Servers)
            {
                if (!Servers.Contains(server))
                    Servers.Add(server);
            }
        }

        public bool Contains(String server)
        {
            lock (Servers)
            {
                return Servers.Contains(server);
            }
        }

        /// <summary>
        /// Unordered comparison between the content of the 2 views
        /// </summary>
        /// <param name="obj"> the argument list to be compared to</param>
        /// <returns>true if both views have same servers</returns>
        public override bool Equals(object obj)
        {
            List<string> item = obj as List<String>;
            lock (Servers)
            {
                if (item == null || item.Count != this.Servers.Count)
                {
                    return false;
                }

                foreach (String server in item)
                {
                    if (!Servers.Contains(server))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// Auto generated by IDE; requested as a warning due to overriding .Equals()
        /// </summary>
        /// <returns>Hash Code for this object</returns>
        public override int GetHashCode()
        {
            return 1835854603 + EqualityComparer<List<string>>.Default.GetHashCode(Servers);
        }
    }
}
