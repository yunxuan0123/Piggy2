using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Aries.Model
{
	public abstract class ObservableObject : INotifyPropertyChanged
	{
		protected ObservableObject()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChangedEventHandler = this.PropertyChanged;
			if (propertyChangedEventHandler != null)
			{
				propertyChangedEventHandler(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public virtual void RaisePropertyChanged(string propertyName)
		{
			this.OnPropertyChanged(propertyName);
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}