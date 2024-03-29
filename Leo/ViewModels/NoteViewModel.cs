using Projet_IUTxTSE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_IUTxTSE.ViewModels
{
    public class NoteViewModel : ViewModelBase
    {
        private readonly NoteViewModel _note;
        public string ecole { get; }
        public string matiere { get; }
        public string contenu { get; }

        /*public NoteViewModel(Note note)
        {
            _note = note;
        }*/

    }
}
