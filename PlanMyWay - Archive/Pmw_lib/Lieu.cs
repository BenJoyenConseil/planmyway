using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pmw_lib
{
   public class Lieu
    {
        private string _adresse;

        public string Adresse
        {
            get { return _adresse; }
            set { _adresse = value; }
        }
        private int _codePostal;

        public int CodePostal
        {
            get { return _codePostal; }
            set { _codePostal = value; }
        }
        private string _Ville;

        public string Ville
        {
            get { return _Ville; }
            set { _Ville = value; }
        }
        private string _Pays;

        public string Pays
        {
            get { return _Pays; }
            set { _Pays = value; }
        }
    }
}
