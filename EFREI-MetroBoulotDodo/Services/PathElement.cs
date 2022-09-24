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
        private int _temps;  // the name field
        public int Temps    // the Name property
        {
            get => _temps;
            set => _temps = value;
        }
        private string _nomAntecedant;  // the name field
        public string NomAntecedant    // the Name property
        {
            get => _nomAntecedant;
            set => _nomAntecedant = value;
        }

        public PathElement(int temps, string nomAntecedant)
        {
            this._temps = temps;
            this._nomAntecedant = nomAntecedant;
        }

    }
}
