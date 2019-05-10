using System;

namespace System.Windows.Interactivity
{
	internal sealed class NameResolvedEventArgs : EventArgs
	{
		private object oldObject;

		private object newObject;

		public object NewObject
		{
			get
			{
				return this.newObject;
			}
		}

		public object OldObject
		{
			get
			{
				return this.oldObject;
			}
		}

		public NameResolvedEventArgs(object oldObject, object newObject)
		{
			Class6.yDnXvgqzyB5jw();
			base();
			this.oldObject = oldObject;
			this.newObject = newObject;
		}
	}
}