using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MetroBoulotDodo.Services
{
    public class Arrete
    {
        private Station dir;
        private Station stationOrigine;
        private int temps;

        public Station getDir()
        {
            return dir;
        }

        public int getTemps()
        {
            return temps;
        }

        public string toString()
        {
            System.Diagnostics.Debug.WriteLine("a");
            return stationOrigine.getNumero() + ";" + stationOrigine.getname() + ";" + stationOrigine.getcoo() + ";" + dir.getNumero() + ";" + dir.getname() + ";" + dir.getcoo() + ";" + dir.getligne() + "\n";
        }

        public Station getStationOrigine() { return stationOrigine; }

        public Arrete(Station dir, Station stationOrigine, int temps)
        {
            this.dir = dir;
            this.temps = temps;
            this.stationOrigine = stationOrigine;
        }

        public void stringtest()
        {
            System.Diagnostics.Debug.WriteLine(
                                  "Station: " + dir.getname()
                                  + " temps: " + temps
                                   ) ;
        }






    }
}
