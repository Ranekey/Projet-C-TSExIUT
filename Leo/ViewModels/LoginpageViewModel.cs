using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Projet_IUTxTSE.ViewModels
{
    public class LoginpageViewModel : ViewModelBase
    {
        private string _usernameIUT;
        public string Username_IUT
        {
            get
            {
                return _usernameIUT;
            }
            set
            {
                _usernameIUT = value;
                OnPropertyChanged(nameof(Username_IUT));
            }
        }

        private string _passwordIUT;
        public string Password_IUT
        {
            get
            {
                return _passwordIUT;
            }
            set
            {
                _passwordIUT = value;
                OnPropertyChanged(nameof(Password_IUT));
            }
        }

        private string _usernameTSE;
        public string Username_TSE
        {
            get
            {
                return _usernameTSE;
            }
            set
            {
                _usernameTSE = value;
                OnPropertyChanged(nameof(Username_TSE));
            }
        }

        private string _passwordTSE;
        public string Password_TSE
        {
            get
            {
                return _passwordTSE;
            }
            set
            {
                _passwordTSE = value;
                OnPropertyChanged(nameof(Password_TSE));
            }
        }

        public ICommand ConnecterIUTCommand { get; }

        public ICommand ConnecterTSECommand { get; }

        public LoginpageViewModel()
        {

        }
    }
}
