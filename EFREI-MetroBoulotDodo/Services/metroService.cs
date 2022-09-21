﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
        // Store the new children directly in the distances map and sort the map afterwards, then add the first element of the map to path that isnt already a part of it
        private ArrayList getShortestPath(Station debut, Station fin)
        {
            IDictionary<int, int> distances = new Dictionary<int, int>();
            
            ArrayList path = new ArrayList();
            path.Add(debut);
            ArrayList sortedChildren = quickSort(debut.getConnecte(), path);
            //sortedChildren[0]
            foreach(Station station in debut.getConnecte())
            {
                
            }
            //Hashmap containing pairs of stops and distances
            //For each child, store the debut stop and the distance the child has from it
            //For each child, sore the child with the shortest distance and later recursivly use this fonctionon it
            //If child with shortest path is the end node, stop the algorithm
            return null;
        }

       
        //Possible use of quick sort but finally, no need.
        private ArrayList quickSort(ArrayList list, ArrayList path)
        {
            foreach (Station station in list) ;// ...sort the list
            //if station in path of already completed stations, dont add to list
            return list;
        }




    }
}
