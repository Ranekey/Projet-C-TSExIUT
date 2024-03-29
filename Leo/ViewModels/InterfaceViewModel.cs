using Projet_IUTxTSE.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Projet_IUTxTSE.ViewModels
{
    public class InterfaceViewModel : ViewModelBase
    {
        public readonly ObservableCollection<Note> _notes;
    }
}
