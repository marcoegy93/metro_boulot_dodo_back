using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MetroBoulotDodo.Services
{
    public class Arrete
    {
        private Station dir;
        private int temps;


        public Arrete(Station dir, int temps)
        {
            this.dir = dir;
            this.temps = temps;
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
