using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SandBox
{
    public class Notification : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(property, value))
                return false;

            property = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}