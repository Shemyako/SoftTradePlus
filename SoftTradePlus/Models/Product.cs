using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SoftTradePlus.Models
{
    public class Product : INotifyPropertyChanged
    {
        private int _id;
        private string _name;
        private float _price;
        private bool _is_sub;
        private DateTime? _sub_end;

        public List<Client> Clients { get; set; } = new List<Client>();
        public int Id { 
            get { return _id; } 
            set { _id = value; OnPropertyChanged("Id"); }
        }
        public string Name { 
            get { return _name; } 
            set { _name = value; OnPropertyChanged("Name"); }
        }
        public float Price { 
            get { return _price; } 
            set { _price = value; OnPropertyChanged("Price"); }
        }
        public bool Is_sub { 
            get { return _is_sub; } 
            set { _is_sub = value; OnPropertyChanged("Is_sub"); }
        }
        public DateTime? Sub_end {
            get { return _sub_end; } 
            set { _sub_end = value; OnPropertyChanged("Sub_end"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
