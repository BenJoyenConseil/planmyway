using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pmw_lib
{
  public  class Utilisateur
    {
        private string _Nom;

        public string Nom
        {
            get { return _Nom; }
            set { _Nom = value; }
        }
        private string _Prenom;

        public string Prenom
        {
            get { return _Prenom; }
            set { _Prenom = value; }
        }
    }
}
