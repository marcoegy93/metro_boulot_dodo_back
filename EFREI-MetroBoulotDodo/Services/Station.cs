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
        private string coord;
        private bool Connexe;
        private List<Arrete> connectes = new List<Arrete>();


        public Station(int num, string name, string ligne, bool terminus, string embranchement)
        {
            this.num = num;
            this.name = name;
            this.ligne = ligne;
            this.terminus = terminus;
            this.Connexe = false;
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

        public void setco(string x, string y)
        {

            this.coord = x + "/" + y;
        }

        public string getcoo()
        {
            return coord;
        }

        public void ajoutdir(Station dir, int temps)
        {
            connectes.Add(new Arrete(dir, this, temps));
        }

        public string affichearretes()
        {
            string retour = "";
            foreach (Arrete a in connectes)
            {
                if (string.Compare(this.name, a.getDir().getname()) != 0)
                {
                    retour += this.num + ";" + this.name + ";" + this.coord + ";" + a.getDir().getNumero() + ";" + a.getDir().getname() + ";" + a.getDir().getcoo() + ";" + this.ligne + "\n";
                }
            }
            return retour;
        }

        public string affichestation()
        {
            return this.num + ";" + this.name + ";" + this.coord + ";" + this.ligne + "\n";
        }
        public string affichearrete(Station vers)
        {
            string retour = "";
            foreach (Arrete a in connectes)
            {
                if (string.Compare(vers.getname(), a.getDir().getname()) == 0)
                {
                    retour += this.num + ";" + this.name + ";" + this.coord + ";" + a.getDir().getNumero() + ";" + a.getDir().getname() + ";" + a.getDir().getcoo() + ";" + this.ligne + "\n";
                    break;
                }
            }
            return retour;
        }
        public List<Arrete> getConnectes()
        {
            return connectes;
        }
        public int getNumero() { return num; }

        public void isConnexe()
        {
            if (!Connexe)
            {
                Connexe = true;
                foreach (Arrete a in connectes)
                {
                    a.getDir().isConnexe();
                }
            }
        }

        public bool getConn()
        {
            return Connexe;
        }




    }
}
