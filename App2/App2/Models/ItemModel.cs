using System;
using System.ComponentModel;
using SQLite;

namespace App2.Models
{
    public class ItemModel : INotifyPropertyChanged
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        private bool _isDetialsTab;
        [Ignore]
        public bool IsDetailsTab 
        {
            get => _isDetialsTab;
            set
            {
                if (_isDetialsTab != value)
                {
                    _isDetialsTab = value;
                    PropertyChanged.Invoke(this,new PropertyChangedEventArgs(nameof(IsDetailsTab)));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }  
}