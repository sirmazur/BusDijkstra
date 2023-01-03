using System.Collections.Specialized;
using System.Linq;
namespace ConsoleApp10
{
    class program
    {
        /*class RailConnection
        {
            public int start { get; set; }
            public int finish { get; set; }
            public RailConnection(int start, int finish)
            {
                this.start=start;
                this.finish=finish;
            }
        }
        class BusConnection
        {
            int start, finish;
            public BusConnection(int start, int finish)
            {
                this.start=start;
                this.finish=finish;
            }
        }
        */
        static void Bus_Pseudo_Dijkstra(ref Connections connections_tree, int start_city, int city_count, int rail_price, int bus_price)
        {
            List<int> stops = new List<int>();
            int[,] tab = new int[2, city_count];
            List<int> destinations = new List<int>();
            List<int> start_stations = connections_tree.Get_Starting_Stations_List();
            for (int z = 0; z<start_stations.Count; z++)
            {
                destinations=(List<int>)connections_tree.GetConnections_Rail(start_stations[z]);
                for (int i = 0; i<2; i++)
                {

                    for (int j = 0; j<start_stations.Count(); j++)
                    {
                        if (i==0)
                        {

                            if (j==z)
                            {
                                tab[i, j]=0;
                                tab[i+1, j]=-1;
                            }
                            else
                            {
                                if (destinations.Contains(start_stations[j]))
                                {
                                    tab[i, j]=bus_price;
                                    tab[i+1, j]=-1;
                                    stops.Add(j);
                                }

                            }
                        }
                        else
                        if (tab[i, j]!=-1)
                        {
                            for (int n = 0; n<stops.Count(); n++)
                            {
                                destinations=(List<int>)connections_tree.GetConnections_Rail(start_stations[stops[n]]);


                                if (destinations.Contains(start_stations[j])&&tab[i, j]!=-1)
                                {

                                    if (connections_tree.GetConnections_Bus(start_stations[j]).Contains(start_stations[z])==false)
                                    {
                                        connections_tree.AddConnection_Bus(start_stations[j], start_stations[z]);
                                    }
                                    if (connections_tree.GetConnections_Bus(start_stations[z]).Contains(start_stations[j])==false)
                                    {
                                        connections_tree.AddConnection_Bus(start_stations[z], start_stations[j]);
                                    }
                                }

                            }
                        }
                    }

                }
            }




            /*List<int> quick = new List<int>();
            for (int i = 0; i<start_stations.Count(); i++)
            {
                Console.WriteLine(start_stations[i]+":\n");
                quick=(List<int>)connections_tree.GetConnections_Rail(start_stations[i]);
                for (int j = 0; j<quick.Count(); j++)
                    Console.WriteLine(quick[j]);
            }
            for (int i = 0; i<start_stations.Count(); i++)
            {
                Console.WriteLine(start_stations[i]+":\n");
                quick=(List<int>)connections_tree.GetConnections_Rail(start_stations[i]);
                quick = quick.Concat((List<int>)connections_tree.GetConnections_Bus(start_stations[i])).ToList();
                for (int j = 0; j<quick.Count(); j++)
                    Console.WriteLine(quick[j]);
            }*/
        }
        static void Commution_Full_Dijkstra(ref Connections connections_tree, int start_city, int city_count, int rail_price, int bus_price)
        {
            int current = start_city, min = 0, minindex = 0;
            int[,] tab = new int[city_count+1, city_count];
            List<int> destinations_rail = new List<int>();
            List<int> destinations_bus = new List<int>();
            List<int> start_stations = connections_tree.Get_Starting_Stations_List();
            int[] stations_completed = new int[city_count];
            List<int> been_there = new List<int>();

            destinations_rail=(List<int>)connections_tree.GetConnections_Rail(start_city);
            destinations_bus=(List<int>)connections_tree.GetConnections_Bus(start_city);


            if (bus_price<rail_price)
            {
                for (int i = 0; i<start_stations.Count; i++)
                {
                    if (start_stations[i]==start_city)
                    {
                        tab[0, i]=0;
                        tab[1, i]=-1;
                        stations_completed[i]=0;
                        been_there.Add(i);
                    }
                    else
                        if (destinations_rail.Contains(start_stations[i]))
                    {
                        tab[0, i]=rail_price;
                    }
                    if (destinations_bus.Contains(start_stations[i]))
                    {
                        tab[0, i]=bus_price;
                    }


                }
            }
            else
                for (int i = 0; i<start_stations.Count; i++)
                {
                    if (start_stations[i]==start_city)
                    {
                        tab[0, i]=0;
                        tab[1, i]=-1;
                        stations_completed[i]=0;
                        been_there.Add(i);
                    }
                    else
                    if (destinations_bus.Contains(start_stations[i]))
                    {
                        tab[0, i]=bus_price;
                    }
                    if (destinations_rail.Contains(start_stations[i]))
                    {
                        tab[0, i]=rail_price;
                    }
                }
            for (int i = 0; i<start_stations.Count; i++)
            {
                if (tab[0, i]>0 && min==0)
                {
                    min=tab[0, i];
                    minindex=i;
                }
                else
                    if (tab[0, i]>0 && tab[0, i]<min)
                {
                    min=tab[0, i];
                    minindex=i;
                }

            }
            current=minindex;
            min=0;
            been_there.Add(minindex);
            tab[1, current]=-1;
            stations_completed[current]=tab[0, current];
            for (int i = 1; i<start_stations.Count; i++)
            {
                //Console.WriteLine(":"+tab[i, 0]+" "+tab[i, 1]+" "+tab[i, 2]+" "+tab[i, 3]+" "+tab[i, 4]+":");
                destinations_rail=(List<int>)connections_tree.GetConnections_Rail(current);
                destinations_bus=(List<int>)connections_tree.GetConnections_Bus(current);
                for (int j = 0; j<start_stations.Count; j++)
                { 
                    if (tab[i, j]==-1)
                    {
                        tab[i+1, j]=-1;
                    }
                    else
                        if (tab[i, j]!=-1)
                    {
                        if (bus_price<rail_price)
                        {
                            if (destinations_bus.Contains(start_stations[j]))
                            {

                                if (tab[i-1, j]!=0)
                                    tab[i, j]= Math.Min(tab[i-1, current]+ bus_price, tab[i-1, j]);
                                else
                                    tab[i, j]=tab[i-1, current]+bus_price;
                                //Console.WriteLine(tab[i, j]+":"+i+":"+ j+":");

                            }
                            else if (destinations_rail.Contains(start_stations[j]))
                            {
                                if (tab[i-1, j]!=0)
                                    tab[i, j]= Math.Min(tab[i-1, current]+ rail_price, tab[i-1, j]);
                                else
                                    tab[i, j]=tab[i-1, current]+rail_price;
                            }
                        }
                        else
                        {

                            if (destinations_rail.Contains(start_stations[j]))
                            {
                                if (tab[i-1, j]!=0)
                                    tab[i, j]= Math.Min(tab[i-1, current]+ rail_price, tab[i - 1, j]);
                                else
                                    tab[i, j]=tab[i-1, current]+rail_price;
                            }
                            else
                            if (destinations_bus.Contains(start_stations[j]))
                            {
                                if (tab[i-1, j]!=0)
                                    tab[i, j]= Math.Min(tab[i-1, current]+ bus_price, tab[i - 1, j]);
                                else
                                    tab[i, j]=tab[i, current]+bus_price;
                            }
                        }
                    }
                }
                for (int z = 0; z<start_stations.Count; z++)
                {
                    if (tab[i, z]>0 && min==0 && been_there.Contains(z)==false)
                    {
                        min=tab[i, z];
                        minindex=z;
                    }
                    else
                    if (tab[i, z]>0 && tab[i, z]<min && been_there.Contains(z)==false)
                    {
                        min=tab[i, z];
                        minindex=z;
                    }
                }
                //Console.WriteLine("minindex:"+minindex+"min:"+min);
                if (min!=0)
                {
                    been_there.Add(minindex);
                    stations_completed[minindex]=tab[i, minindex];
                    current=minindex;
                    min=0;
                    tab[i+1, current]=-1;
                }
                else
                {
                    for (int c = 0; c<start_stations.Count; c++)
                    {
                        Console.WriteLine(stations_completed[c]);
                    }
                    break;
                }
                

            } 
        }



            static void Main(string[] args)
            {
                int city_count, connections_count, start_city, rail_price, bus_price, city_a, city_b;
                city_count=Convert.ToInt32(Console.ReadLine());
                connections_count=Convert.ToInt32(Console.ReadLine());
                Connections connections_tree = new Connections();
                start_city=Convert.ToInt32(Console.ReadLine());
                rail_price=Convert.ToInt32(Console.ReadLine());
                bus_price=Convert.ToInt32(Console.ReadLine());
                for (int i = 0; i<connections_count; i++)
                {
                    city_a=Convert.ToInt32(Console.ReadLine());
                    city_b=Convert.ToInt32(Console.ReadLine());
                    connections_tree.AddConnection_Rail(city_a, city_b);
                    connections_tree.AddConnection_Rail(city_b, city_a);

                }
                Bus_Pseudo_Dijkstra(ref connections_tree, start_city, city_count, rail_price, bus_price);
                Commution_Full_Dijkstra(ref connections_tree, start_city, city_count, rail_price, bus_price);
            }
        
    } 
}
