using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Globe3DLight.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        private bool _isDirty;
        private ViewModelBase _owner = null;
        private string _name = string.Empty;

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual ViewModelBase Owner
        {
            get => _owner;
            set => RaiseAndSetIfChanged(ref _owner, value);
        }
  
        public virtual string Name
        {
            get => _name;
            set => RaiseAndSetIfChanged(ref _name, value);
        }

        public virtual bool IsDirty() => _isDirty;

        public virtual void MarkAsDirty() => _isDirty = true;

        public virtual void Invalidate() => _isDirty = false;
                   
        public virtual object Copy(IDictionary<object, object> shared) => throw new NotImplementedException();

        public void RaisePropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);
         
        public void RaiseAndSetIfChanged<T>(ref T field, T value, [CallerMemberName] string propertyName = default)
        {
            if (!Equals(field, value))
            {
                field = value;
                _isDirty = true;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));             
            }         
        }

        public bool Update<T>(ref T field, T value, [CallerMemberName] string propertyName = default)
        {
            if (!Equals(field, value))
            {
                field = value;
                _isDirty = true;
                RaisePropertyChanged(new PropertyChangedEventArgs(propertyName));
                return true;
            }
            return false;
        }
    }
}
