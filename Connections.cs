using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp10
{
    internal class Connections
    {
        private class Entry
        {
            public int Start;
            public List<int> Rail_Destinations;
            public List<int> Bus_Destinations;

            public Entry(int start)
            {
                Start = start;
                Rail_Destinations = new List<int>();
                Bus_Destinations = new List<int>();
            }
        }

        private RedBlackTree<int, Entry> tree;

        public Connections()
        {
            tree = new RedBlackTree<int, Entry>();
        }

        
        public void AddConnection_Rail(int start, int finish)
        {


            if (tree.Contains(start)==true)
            { Entry entry = tree.Get(start); entry.Rail_Destinations.Add(finish); }
            else
            {
                Entry entry = new Entry(start);
                tree.Put(start, entry);
                entry.Rail_Destinations.Add(finish);
            }

        }
        public void AddConnection_Bus(int start, int finish)
        {


            if (tree.Contains(start)==true)
            { Entry entry = tree.Get(start); entry.Bus_Destinations.Add(finish); }
            else
            {
                Entry entry = new Entry(start);
                tree.Put(start, entry);
                entry.Bus_Destinations.Add(finish);
            }

        }
        public IEnumerable<int> GetConnections_Rail(int start)
        {
            try
            {
                Entry entry = tree.Get(start);
                return entry != null ? entry.Rail_Destinations : Enumerable.Empty<int>();
            }
            catch (KeyNotFoundException e)
            {
                Console.Clear();
                return Enumerable.Empty<int>();
            }

        }
        public IEnumerable<int> GetConnections_Bus(int start)
        {
            try
            {
                Entry entry = tree.Get(start);
                return entry != null ? entry.Bus_Destinations : Enumerable.Empty<int>();
            }
            catch (KeyNotFoundException e)
            {
                Console.Clear();
                return Enumerable.Empty<int>();
            }

        }

        public IEnumerable<int> Get_Starting_Stations()
        {
            return tree.Keys();
        }
        public List<int> Get_Starting_Stations_List()
        {
            List<int> list = new List<int>();
            foreach (int Start in Get_Starting_Stations())
            {
                list.Add(Start);
            }
            return list;
        }
    }
}

