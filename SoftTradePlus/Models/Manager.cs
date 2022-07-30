using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SoftTradePlus.Models
{
    public class Manager : INotifyPropertyChanged
    {
        private int _id;
        private string _name;
        public int Id { 
            get { return _id; } 
            set { _id = value; OnPropertyChanged("Id"); }
        }
        public string Name { 
            get { return _name; } 
            set { _name = value; OnPropertyChanged("Name"); }
        }
        public List<Client>? Clients { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
