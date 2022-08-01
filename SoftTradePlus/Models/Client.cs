using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SoftTradePlus.Models
{

    public class Client : INotifyPropertyChanged
    {

        private string _name;
        private Manager _manager;
        private ClientStatus _clientStatus;

        public int Id { get; set; }

        public string Name { 
            get { return _name; }
            set { _name = value; OnPropertyChanged("Name"); }        
        }

        public Manager Manager { 
            get { return _manager; }
            set { _manager = value; OnPropertyChanged("Manager"); }
        }

        public List<Product> Products { get; set; } = new List<Product>();

        public ClientStatus ClientStatus { 
            get { return _clientStatus; }
            set { _clientStatus = value; OnPropertyChanged("ClientStatus"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}
