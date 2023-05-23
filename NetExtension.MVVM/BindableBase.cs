using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetExtension.MVVM
{
    public abstract class BindableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public bool SetProperty<T>(ref T property, T value, string propertyName) { 
        
            if(EqualityComparer<T>.Default.Equals(property, value)) return false;

            property = value;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            return true;
        
        }

        public bool SetProperty<T>(ref T property, T value, string propertyName,Action<T> onPropertyChanged)
        {

            if (EqualityComparer<T>.Default.Equals(property, value)) return false;

            property = value;

            onPropertyChanged?.Invoke(property);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            return true;

        }
    }
}
