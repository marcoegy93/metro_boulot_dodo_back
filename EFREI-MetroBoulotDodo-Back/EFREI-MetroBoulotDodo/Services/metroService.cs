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
                                System.Diagnostics.Debug.WriteLine(
                                   "num_sommet: "  + line.Split(' ')[1]
                                   + " nom_sommet: " + line.Split(' ')[2]
                                   + " numéro_ligne: " + line.Split(';')[1]
                                   + " si_terminus: " + line.Split(';')[2].Split(' ')[0]
                                   + " branchement: " + line.Split(';')[2].Split(' ')[1]
                                    );
                            }
                        }

                        if (line[0] == 'E')
                        {
                            // Les arcs ! 
                            System.Diagnostics.Debug.WriteLine(
                               line.Split(' ')[1] + " "
                               + line.Split(' ')[2] + " "
                               + line.Split(' ')[3] + " "
                                );
                        }


                    }
                }
            }

        }


        
    }
}
