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
            Station start = Stations[26];
            Station end = Stations[24];
            getShortestPath(start, end);
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
        private List<PathElement> getShortestPath(Station debut, Station fin)
        {

            IDictionary<string, PathElement> temps = new Dictionary<string, PathElement>();
            temps.Add(debut.getname(), new PathElement(0, null));
            List<Station> traites = new List<Station>();
            traites.Add(debut);
            while (!(traites.Any(item=> item.getname() == fin.getname()))){

                List<Arrete> connectes = new List<Arrete>();
                foreach(Station station in traites) {
                    foreach (Arrete arrete in station.getConnectes())
                    {
                        connectes.Add(arrete);
                    }
                        
                }

                metroService.quickSort(connectes, 0, connectes.Count - 1);
                bool firstNonTraites = true;
                foreach(Arrete arrete in connectes)
                {
                    if (!(traites.Any(item => item.getname() == arrete.getDir().getname())))
                    {
                        if(firstNonTraites)
                        {
                            traites.Add(arrete.getDir());
                        }
                        //Mettre a jour la distance
                        string nomStationChoisi = arrete.getDir().getname();
                        if (temps.ContainsKey(nomStationChoisi)) {
                            if ((arrete.getTemps() + temps[arrete.getStationOrigine().getname()].Temps)  < temps[nomStationChoisi].Temps ) {
                                temps[nomStationChoisi].Temps = arrete.getTemps();
                            }
                        } else {
                            temps.Add(nomStationChoisi, new PathElement(arrete.getTemps() + temps[arrete.getStationOrigine().getname()].Temps, arrete.getStationOrigine().getname()));
                        }
                    }
                }

            }
            List<PathElement> path = new List<PathElement>();
            PathElement tempElement = temps[fin.getname()];
            path.Add(tempElement);
            while (tempElement.NomAntecedant != null) {
                path.Add(tempElement);
                tempElement = temps[tempElement.NomAntecedant];
            }

            
            return path;
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
                if (list[j].getTemps() <= list[dernier].getTemps())
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
