using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MetroBoulotDodo.Services
{
    public class Station
    {
        private string name;
        private bool terminus;
        private string embranchement;
        private string ligne;
        private int num;
        private List<Arrete> connectes = new List<Arrete>();


        public Station(int num, string name, string ligne, bool terminus, string embranchement)
        {
            this.num = num;
            this.name = name;
            this.ligne = ligne;
            this.terminus = terminus;
            this.embranchement = embranchement;
            this.stringtest();
        }

        public void stringtest()
        {
            System.Diagnostics.Debug.WriteLine(
                                  "num_sommet: " + num
                                  + " nom_sommet: " + name
                                  + " numéro_ligne: " + ligne
                                  + " si_terminus: " + terminus
                                  + " branchement: " + embranchement
                                   );
            foreach(Arrete a in connectes)
            {
                a.stringtest();
            }
        }
        public string getname()
        {
            return name;
        }

        public void ajoutdir(Station dir, int temps)
        {
            connectes.Add(new Arrete(dir, temps));
        }

        public List<Arrete> getConnectes()
        {
            return connectes;
        }
        public int getNumero() { return num; }






    }
}
