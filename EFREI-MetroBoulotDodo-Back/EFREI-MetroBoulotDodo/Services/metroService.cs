using System;
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
        private readonly string filePos = @"./Data/pospoints.txt";
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
                    System.Diagnostics.Debug.WriteLine("*" + namesta + "*");
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
                retour += sta.Value.affichearrete();
            return retour;
        }

        public string isConnexe()
        {
            string retour = "Connexe";
            Stations[0].isConnexe();
            foreach(KeyValuePair<int, Station> sta in Stations)
            {
                if (!sta.Value.getConn())
                {
                    retour = "Non Connexe";
                    break;
                }
            }
            return retour;
        }


    }
}
