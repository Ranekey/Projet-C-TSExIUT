using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_IUTxTSE.Models
{
    public class Note
    {
        public string ecole {  get; }
        public string matiere { get; }
        public string contenu { get; }

        public Note(string ecole, string matiere, string contenu)
        {
            this.ecole = ecole;
            this.matiere = matiere;
            this.contenu = contenu;
        }

    }
}
