﻿using System;
using CommonTypes;
using System.Collections;
using System.Collections.Generic;

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

        List<ITuple> State = new List<ITuple>();

        public View()
        {
            Servers = new List<string>();
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


        public List<String> GetUrls()
        {
            return Servers;
        }

        public void Remove(String url)
        {
            Servers.Remove(url);
        }



        override public string ToString()
        {
            string content = "";
            foreach(String server in Servers)
            {
                content +=" <" +server +" id:"+ ID +"> ";
            }
            return content;
        }

        /// <summary>
        /// Adds a server to the list of servers in this view
        /// Does not allow repeated elements to be added
        /// </summary>
        /// <param name="server"></param>
        public void Add(String server)
        {
            if(!Servers.Contains(server))
                Servers.Add(server);
        }

        public bool Contains(String server)
        {
            return Servers.Contains(server);
        }

        /// <summary>
        /// Unordered comparison between the content of the 2 views
        /// </summary>
        /// <param name="obj"> the argument list to be compared to</param>
        /// <returns>true if both views have same servers</returns>
        public override bool Equals(object obj)
        {
            List<string> item = obj as List<String>;

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
