using Microsoft.AspNetCore.Server.Kestrel.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MetroBoulotDodo.Services
{
    public class metroService
    {
        private readonly string fileMetro = @"./Data/metro.txt";
        private IDictionary<int, Station> Stations = new Dictionary<int, Station>();

        public metroService()
        {
            //faire initialisation au lancement de l'app et non pas au lancement dune requete
            readFileMetro();

        }


        private void readFileMetro()
        {
            using (StreamReader ReaderObject = new StreamReader(fileMetro))
            {
                string line;
                Station station;
                Station b;
                int x;
                int y;
                int t;
                bool premiere = true;
                int num =0;
                string name;
                int i;

                while ((line = ReaderObject.ReadLine()) != null)
                {
                    if (line != "")
                    {
                        if (line[0] == 'V')
                        {
                            // Les sommets !
                           // System.Diagnostics.Debug.WriteLine(line);
                           if(line.Split(';').Length == 3)
                            {
                                i = 0;
                                name = "";
                                foreach (var part in line.Split(';')[0].Split(' '))
                                {
                                    if(i==1)
                                    {
                                        int.TryParse(line.Split(' ')[1], out num);
                                    }
                                    if(i>1)
                                    {
                                        if(i>2)
                                            name += " ";
                                        name += part;
                                    }
                                    i++;
                                }
                                
                                if (string.Compare("True", line.Split(';')[2].Split(' ')[0]) == 0)
                                    Stations.Add(num,new Station(num, name, line.Split(';')[1], true, line.Split(';')[2].Split(' ')[1]));
                                else
                                    Stations.Add(num, new Station(num, name, line.Split(';')[1], false, line.Split(';')[2].Split(' ')[1]));
                                
                            }
                        }

                        if (line[0] == 'E')
                        {
                            if (premiere)
                                premiere = false;
                            else
                            {
                                // Les arcs ! 
                                int.TryParse(line.Split(' ')[1], out x);
                                int.TryParse(line.Split(' ')[3], out t);
                                int.TryParse(line.Split(' ')[2], out y);
                                station = Stations[x];
                                b = Stations[y];
                                station.ajoutdir(b, t);
                                b.ajoutdir(station, t);
                            }
                        }


                    }
                }
                foreach (KeyValuePair<int, Station> sta in Stations)
                    sta.Value.stringtest();
            }
        }
        private List<Station> getDijkstra(Station debut, Station fin)
        {
            IDictionary<int, int> distances = new Dictionary<int, int>();
            List<Station> path = new List<Station>();
            path.Add(debut);

        }
        // Store the new children directly in the distances map and sort the map afterwards, then add the first element of the map to path that isnt already a part of it
        private List<Station> getShortestPath(Station debut, Station fin)
        {
            IDictionary<int, int> distances = new Dictionary<int, int>();
            List<Station> path = new List<Station>();
            path.Add(debut);
            List<Station> traites = new List<Station>();
            traites.Add(debut);
            while (!(path.Contains(fin))){ 
                List<Arrete> connectes = new List<Arrete>();
                foreach(Station station in traites) {
                    foreach (Arrete arrete in station.getConnectes())
                    {
                        connectes.Add(arrete);
                    }
                        
                }

                metroService.quickSort(connectes, 0, connectes.Count - 1);
                Arrete arreteChoisi;//= connectes[0];
                traites.Add(arreteChoisi.getDir());
                //
                bool firstNonTraites = true;
                foreach(Arrete arrete in connectes)
                {
                    if (!(traites.Contains(arrete.getDir())))
                    {
                        if(firstNonTraites) arreteChoisi = arrete;
                        //Mettre a jour la distance

                    }
                }
                //path.Add(arreteChoisi.getDir());
                //distances.Add(arreteChoisi.getDir().getNumero(), arreteChoisi.getTemps());
            }


            //getShortestPath();
            //Hashmap containing pairs of stops and distances
            //For each child, store the debut stop and the distance the child has from it
            //For each child, sore the child with the shortest distance and later recursivly use this fonctionon it
            //If child with shortest path is the end node, stop the algorithm
            return null;
        }

       
        //Possible use of quick sort but finally, no need.
        private static void quickSort(List<Arrete> list,int premier, int dernier)
        {
            if (premier < dernier)
            {
                int pivot = (dernier - premier) / 2 + premier;
                pivot = repartir(list, premier, dernier, pivot);
                quickSort(list, premier, pivot - 1);
                quickSort(list, pivot + 1, dernier);
            }
            //foreach (Station station in list) ;// ...sort the list
            ////if station in path of already completed stations, dont add to list
            //return list;
        }

        private static int repartir(List<Arrete> list, int premier, int dernier, int pivot)
        {
            swap(list, dernier, pivot);
            int i = premier;
            for (int j = premier; j < dernier; ++j)
            {
                if (list[j].getTemps() >= list[dernier].getTemps())
                {
                    swap(list, j, i);
                    i++;
                }
            }
            swap(list, dernier, i);
            return i;
        }

        private static void swap(List<Arrete> list, int dernier, int pivot)
        {
            Arrete tmp = list[pivot];
            list[pivot] = list[dernier];
            list[dernier] = tmp;
        }
    }
}
