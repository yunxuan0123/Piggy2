using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;

namespace System.Windows.Interactivity
{
	internal sealed class NameResolver
	{
		private string name;

		private FrameworkElement nameScopeReferenceElement;

		private FrameworkElement ActualNameScopeReferenceElement
		{
			get
			{
				if (this.NameScopeReferenceElement == null || !Interaction.IsElementLoaded(this.NameScopeReferenceElement))
				{
					return null;
				}
				return this.GetActualNameScopeReference(this.NameScopeReferenceElement);
			}
		}

		private bool HasAttempedResolve
		{
			get;
			set;
		}

		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				DependencyObject obj = this.Object;
				this.name = value;
				this.UpdateObjectFromName(obj);
			}
		}

		public FrameworkElement NameScopeReferenceElement
		{
			get
			{
				return this.nameScopeReferenceElement;
			}
			set
			{
				FrameworkElement nameScopeReferenceElement = this.NameScopeReferenceElement;
				this.nameScopeReferenceElement = value;
				this.OnNameScopeReferenceElementChanged(nameScopeReferenceElement);
			}
		}

		public DependencyObject Object
		{
			get
			{
				if (string.IsNullOrEmpty(this.Name) && this.HasAttempedResolve)
				{
					return this.NameScopeReferenceElement;
				}
				return this.ResolvedObject;
			}
		}

		private bool PendingReferenceElementLoad
		{
			get;
			set;
		}

		private DependencyObject ResolvedObject
		{
			get;
			set;
		}

		public NameResolver()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		private FrameworkElement GetActualNameScopeReference(FrameworkElement initialReferenceElement)
		{
			FrameworkElement parent = initialReferenceElement;
			if (this.IsNameScope(initialReferenceElement))
			{
				parent = initialReferenceElement.Parent as FrameworkElement ?? parent;
			}
			return parent;
		}

		private bool IsNameScope(FrameworkElement frameworkElement)
		{
			FrameworkElement parent = frameworkElement.Parent as FrameworkElement;
			if (parent == null)
			{
				return false;
			}
			return parent.FindName(this.Name) != null;
		}

		private void OnNameScopeReferenceElementChanged(FrameworkElement oldNameScopeReference)
		{
			if (this.PendingReferenceElementLoad)
			{
				oldNameScopeReference.Loaded -= new RoutedEventHandler(this.OnNameScopeReferenceLoaded);
				this.PendingReferenceElementLoad = false;
			}
			this.HasAttempedResolve = false;
			this.UpdateObjectFromName(this.Object);
		}

		private void OnNameScopeReferenceLoaded(object sender, RoutedEventArgs e)
		{
			this.PendingReferenceElementLoad = false;
			this.NameScopeReferenceElement.Loaded -= new RoutedEventHandler(this.OnNameScopeReferenceLoaded);
			this.UpdateObjectFromName(this.Object);
		}

		private void OnObjectChanged(DependencyObject oldTarget, DependencyObject newTarget)
		{
			if (this.ResolvedElementChanged != null)
			{
				this.ResolvedElementChanged(this, new NameResolvedEventArgs(oldTarget, newTarget));
			}
		}

		private void UpdateObjectFromName(DependencyObject oldObject)
		{
			DependencyObject dependencyObject = null;
			this.ResolvedObject = null;
			if (this.NameScopeReferenceElement != null)
			{
				if (!Interaction.IsElementLoaded(this.NameScopeReferenceElement))
				{
					this.NameScopeReferenceElement.Loaded += new RoutedEventHandler(this.OnNameScopeReferenceLoaded);
					this.PendingReferenceElementLoad = true;
					return;
				}
				if (!string.IsNullOrEmpty(this.Name))
				{
					FrameworkElement actualNameScopeReferenceElement = this.ActualNameScopeReferenceElement;
					if (actualNameScopeReferenceElement != null)
					{
						dependencyObject = actualNameScopeReferenceElement.FindName(this.Name) as DependencyObject;
					}
				}
			}
			this.HasAttempedResolve = true;
			this.ResolvedObject = dependencyObject;
			if (oldObject != this.Object)
			{
				this.OnObjectChanged(oldObject, this.Object);
			}
		}

		public event EventHandler<NameResolvedEventArgs> ResolvedElementChanged;
	}
}