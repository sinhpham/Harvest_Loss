using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq.Expressions;

namespace Harvest_Loss
{
    public abstract class NotifyingClass : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(storage, value))
                return false;

            storage = value;
            this.OnPropertyChanged(propertyName);

            return true;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            var eventHandler = PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void OnPropertyChanged<T>(Expression<Func<T>> selectorExpression)
        {
            if (selectorExpression == null)
            {
                throw new ArgumentNullException("selectorExpression");
            }
            var body = selectorExpression.Body as MemberExpression;
            if (body == null)
                throw new ArgumentException("The body must be a member expression");
            OnPropertyChanged(body.Member.Name);
        }
    }
}

