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
        //private int _manager_id;
        //private int _status_id;
        public int Id { get; set; }
        public string Name { 
            get { return _name; }
            set { _name = value; OnPropertyChanged("Name"); }        
        }
        //public int Manager_id {
        //    get { return _manager_id; } 
        //    set { _manager_id = value; OnPropertyChanged("Manager_id"); }
        //}
        public Manager Manager { get; set; }
        //public List<ClientProduct> ClientProduct { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();

        //public int Status_id { get { return _status_id; } 
        //    set { 
        //        _status_id = value;
        //        OnPropertyChanged("Status_id"); } }
        public ClientStatus ClientStatus { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}
