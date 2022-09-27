using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MetroBoulotDodo.Services
{
    /*public class PathElement
    {
        private int _temps;  // the name field
        public int Temps    // the Name property
        {
            get => _temps;
            set => _temps = value;
        }
        private int _numAntecedant;  // the name field
        public int NumAntecedant    // the Name property
        {
            get => _numAntecedant;
            set => _numAntecedant = value;
        }
        public PathElement(int temps, int numAntecedant)
        {
            this._temps = temps;
            this._numAntecedant = numAntecedant;
        }
    }*/

    public class PathElement
    {
        private Station _station;  // the name field
        public Station Station    // the Name property
        {
            get => _station;
            set => _station = value;
        }

        private int _temps;
        public int Temps
        {
            get => _temps;
            set => _temps = value;
        }
        private string _nomAntecedant;
        public string NomAntecedant
        {
            get => _nomAntecedant;
            set => _nomAntecedant = value;
        }

        public PathElement(Station station, int temps, string nomAntecedant)
        {
            this._station = station;
            this._temps = temps;
            this._nomAntecedant = nomAntecedant;
        }

    }
}