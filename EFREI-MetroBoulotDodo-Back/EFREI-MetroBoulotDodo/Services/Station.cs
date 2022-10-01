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
        private ArrayList connectes = new ArrayList();
        private string coord;
        private bool Connexe;


        public Station(int num, string name, string ligne, bool terminus, string embranchement)
        {
            this.num = num;
            this.name = name;
            this.ligne = ligne;
            this.terminus = terminus;
            this.embranchement = embranchement;
            this.Connexe = false;
        }

        public void stringtest()
        {
            System.Diagnostics.Debug.WriteLine(
                                  "num_sommet: " + num
                                  + " nom_sommet: " + name
                                  + " numéro_ligne: " + ligne
                                  + " si_terminus: " + terminus
                                  + " branchement: " + embranchement
                                  + "coord: " + coord
                                   );
            foreach (Arrete a in connectes)
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

        public void setco(string x, string y)
        {

            this.coord = x + "/" + y;
        }

        public string affichearrete()
        {
            string retour = "";
            foreach (Arrete a in connectes)
            {
                if (string.Compare(this.name, a.GetStation().getname()) != 0)
                {
                    retour +=this.num+";"+ this.name + ";" + this.coord + ";" + a.GetStation().getname() +";" + a.GetStation().getname() + ";" + a.GetStation().getcoo() + ";" + this.ligne + "\n";
                }
            }
            return retour;
        }

        public int getnum()
        {
            return num;
        }

        public string getcoo()
        {
            return coord;
        }

        public void isConnexe()
        {
            if (!Connexe)
            {
                Connexe = true;
                foreach (Arrete a in connectes)
                {
                    a.GetStation().isConnexe();
                }
            }
        }

        public bool getConn()
        {
            return Connexe;
        }




    }
}
