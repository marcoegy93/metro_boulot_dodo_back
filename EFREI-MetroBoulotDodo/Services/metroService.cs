using Microsoft.AspNetCore.Server.Kestrel.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace MetroBoulotDodo.Services
{
    public class metroService
    {
        private readonly string fileMetro = @"./Data/metro.txt";
        private readonly string filePos = @"./Data/pospoints.txt";
        private IDictionary<int, Station> Stations = new Dictionary<int, Station>();

        public metroService()
        {
            //faire initialisation au lancement de l'app et non pas au lancement dune requete
            readFileMetro();
            getShortestPath(26, 24);
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
                int num = 0;
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
                            if (line.Split(';').Length == 3)
                            {
                                i = 0;
                                name = "";
                                foreach (var part in line.Split(';')[0].Split(' '))
                                {
                                    if (i == 1)
                                    {
                                        int.TryParse(line.Split(' ')[1], out num);
                                    }
                                    if (i > 1)
                                    {
                                        if (i > 2)
                                            name += " ";
                                        name += part;
                                    }
                                    i++;
                                }

                                if (string.Compare("True", line.Split(';')[2].Split(' ')[0]) == 0)
                                    Stations.Add(num, new Station(num, name, line.Split(';')[1], true, line.Split(';')[2].Split(' ')[1]));
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
            }
            using (StreamReader ReaderObject = new StreamReader(filePos))
            {
                string line;
                string namesta;
                string memoire = "";
                bool premiere;
                bool trouve;
                while ((line = ReaderObject.ReadLine()) != null)
                {
                    namesta = "";
                    trouve = false;
                    premiere = true;
                    foreach (string s in line.Split(';')[2].Split('@'))
                    {

                        namesta += s + " ";
                        premiere = false;
                    }
                    if (string.Compare(namesta, memoire) == 0)
                    {
                        continue;
                    }
                    else
                    {
                        memoire = namesta;
                    }
                    foreach (KeyValuePair<int, Station> sta in Stations)
                    {
                        if (string.Compare(namesta, sta.Value.getname()) == 0)
                        {

                            sta.Value.setco(line.Split(';')[0], line.Split(';')[1]);
                            trouve = true;
                        }
                        else if (trouve)
                            break;

                    }
                }
                //foreach (KeyValuePair<int, Station> sta in Stations)
                // sta.Value.stringtest();
            }
        }

        public string retourarbre()
        {
            string retour = "";
            foreach (KeyValuePair<int, Station> sta in Stations)
                retour += sta.Value.affichearretes();
            return retour;
        }

        public string Crearbre()
        {
            string retour = "";
            foreach (KeyValuePair<int, Station> sta in Stations)
                retour += sta.Value.affichestation();
            return retour;
        }

        public bool isConnexe()
        {
            bool retour = true;
            Stations[0].isConnexe();
            foreach (KeyValuePair<int, Station> sta in Stations)
            {
                if (!sta.Value.getConn())
                {
                    retour = false;
                    break;
                }
            }
            return retour;
        }

        public string getDijkstra(int idDebut, int idFin)
        {
            List<PathElement> Dijkstra = getShortestPath(idDebut, idFin);
            PathElement e;
            string retour = "";
            for(int i = 0;i< Dijkstra.Count; i++)
            {
                e = Dijkstra[Dijkstra.Count - 1 - i];
                if(e.NumAntecedant!=null)
                    retour += Stations[e.NumAntecedant.Value].affichearrete(e.Station); 
            }
            retour += Dijkstra[0].Temps;
            return retour;
        }

        // Store the new children directly in the distances map and sort the map afterwards, then add the first element of the map to path that isnt already a part of it
        private List<PathElement> getShortestPath(int idDebut, int idFin)
        {
            Station debut = Stations[idDebut];
            Station fin = Stations[idFin];
            IDictionary<int, PathElement> temps = new Dictionary<int, PathElement>();
            temps.Add(debut.getNumero(), new PathElement(debut, 0, null));
            IDictionary<int, Station> traites = new Dictionary<int, Station>();
            List<int> ids = new List<int>();
            ids.Add(idFin);
            //List<Station> traites = new List<Station>();
            traites.Add(debut.getNumero(), debut);
            while (!(traites.Any(item => ids.Contains(item.Key)))) // do this until we find an item with an id inside of a list of ids
            {

                List<Arrete> connectes = new List<Arrete>();
                foreach (Station station in traites.Values)
                {
                    foreach (Arrete arrete in station.getConnectes())
                    {
                        connectes.Add(arrete);
                    }

                }

                metroService.quickSort(connectes, 0, connectes.Count - 1);
                bool firstIt = true;
                int shortestTime = 0;
                Station nextTraitee = null;
                foreach (Arrete arrete in connectes)
                {
                    if (!(traites.ContainsKey(arrete.getDir().getNumero())))//Changement de ligne pas possible.  Here we say 'No trait pas les aretes qui vont a un station deja traitees. Mais '
                    {
                       
                        //Mettre a jour la distance
                        int numStationChoisi = arrete.getDir().getNumero();
                        int totaleTime = arrete.getTemps() + temps[arrete.getStationOrigine().getNumero()].Temps;
                        if (temps.ContainsKey(numStationChoisi)) {
                            if (totaleTime < temps[numStationChoisi].Temps) {
                                temps[numStationChoisi].Temps = totaleTime;
                            }
                        } else {
                            temps.Add(numStationChoisi, new PathElement(arrete.getDir(), totaleTime, arrete.getStationOrigine().getNumero()));
                        }
                        if (firstIt) {
                            firstIt = false;
                            shortestTime = totaleTime;
                            nextTraitee = arrete.getDir();
                        } else {
                            if (totaleTime < shortestTime) {
                                shortestTime = totaleTime;
                                nextTraitee = arrete.getDir();
                            }
                        }
                    }
                }
                if(nextTraitee != null) traites.Add(nextTraitee.getNumero(), nextTraitee);
            }
            List<PathElement> path = new List<PathElement>();
            PathElement tempElement = temps[fin.getNumero()];
            path.Add(tempElement);
            while (tempElement.NumAntecedant != null)
            {
                tempElement = temps[tempElement.NumAntecedant.Value];
                path.Add(tempElement);
            }


            return path;
        }


        //Possible use of quick sort but finally, no need.
        private static void quickSort(List<Arrete> list, int premier, int dernier)
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

        public List<Arrete> Fusion(List<Arrete> a, List<Arrete> b)
        {
            List<Arrete> list = new List<Arrete>();
            int l1 = 0, l2 = 0;
            int id = 0;
            while( l1<a.Count && l2 < b.Count)
            {
                if(a[l1].getTemps() <= b[l2].getTemps())
                {
                    list.Add(a[l1]);
                    l1++;
                }
                else
                {
                    list.Add(b[l2]);
                    l2++;  
                }
            }
            for(; l1 < a.Count;l1++)
            {
                list.Add(a[l1]);
            }
            for (; l2 < b.Count; l2++)
            {
                list.Add(b[l2]);
            }
            return list;
        } 

        public List<Arrete> trilist(List<Arrete> a)
        {
            a.Sort((x, y) => x.getTemps().CompareTo(y.getTemps()));
            return a;
        }
        public string getACPM()
        {
            Arrete memory;
            int temps =0;
            int id;
            bool stop = false;
            string retour = "";
            List<Arrete> list = new List<Arrete>();
            List<Arrete> query;
            list.AddRange(Stations[0].getConnectes());
            Stations[0].setACPM();
            trilist(list);
            while (list.Count > 0)
            {
                while (list.First().getDir().getACPM())
                {


                    list.Remove(list.First());                    
                    if (list.Count()==0)
                    {
                        stop = true;
                        System.Diagnostics.Debug.WriteLine("stop");
                        break;
                    }

                }
                if (stop) break;
                temps += list.First().getTemps();
                memory = list.First();
                list.Remove(list.First());
                retour += memory.toString();
                memory.getDir().setACPM();
                query = new List<Arrete>();
                query.AddRange(memory.getDir().getConnectes());
                query = trilist(query);
                list = Fusion(list, query);
            }
            retour += temps;
            foreach(KeyValuePair<int, Station> sta in Stations)
            {
                sta.Value.resACPM();
                System.Diagnostics.Debug.WriteLine(sta.Value.getACPM()) ;
            }

            return retour;
        }
        public string getACPMGare()
        {
            Arrete memory;
            int temps = 0;
            int id;
            bool stop = false;
            string retour = "";
            List<Arrete> list = new List<Arrete>();
            List<Arrete> query;
            list.AddRange(Stations[0].getConnectes());
            Stations[0].setACPM();
            trilist(list);
            while (list.Count > 0)
            {
                while (list.First().getDir().getACPM())
                {


                    list.Remove(list.First());
                    if (list.Count() == 0)
                    {
                        stop = true;
                        System.Diagnostics.Debug.WriteLine("stop");
                        break;
                    }

                }
                if (stop) break;
                temps += list.First().getTemps();
                memory = list.First();
                list.Remove(list.First());
                retour += memory.toString();
                memory.getDir().setACPM();
                query = new List<Arrete>();
                query.AddRange(memory.getDir().getConnectes());
                for(int i = 0; memory.getDir().getNumero() + i < Stations.Count; i++)
                {
                    if (String.Compare(memory.getDir().getname(), Stations[memory.getDir().getNumero() + i].getname()) == 0)
                    {
                        Stations[memory.getDir().getNumero() + i].setACPM();
                        query.AddRange(Stations[memory.getDir().getNumero() + i].getConnectes());
                    }
                    else break;
                }
                for (int i = 0; memory.getDir().getNumero() - i > 0; i++)
                {
                    if (String.Compare(memory.getDir().getname(), Stations[memory.getDir().getNumero() - i].getname()) == 0)
                    {
                        Stations[memory.getDir().getNumero() - i].setACPM();
                        query.AddRange(Stations[memory.getDir().getNumero() - i].getConnectes());
                    }
                    else break;
                }
                query = trilist(query);
                list = Fusion(list, query);
            }
            retour += temps;
            foreach (KeyValuePair<int, Station> sta in Stations)
            {
                sta.Value.resACPM();
                System.Diagnostics.Debug.WriteLine(sta.Value.getACPM());
            }

            return retour;
        }
    }
}