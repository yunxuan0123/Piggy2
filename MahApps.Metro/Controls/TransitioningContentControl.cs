using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace MahApps.Metro.Controls
{
	public class TransitioningContentControl : ContentControl
	{
		internal const string PresentationGroup = "PresentationStates";

		internal const string NormalState = "Normal";

		internal const string PreviousContentPresentationSitePartName = "PreviousContentPresentationSite";

		internal const string CurrentContentPresentationSitePartName = "CurrentContentPresentationSite";

		private bool _allowIsTransitioningWrite;

		private Storyboard _currentTransition;

		public const TransitionType DefaultTransitionState = TransitionType.Default;

		public readonly static DependencyProperty IsTransitioningProperty;

		public readonly static DependencyProperty TransitionProperty;

		public readonly static DependencyProperty RestartTransitionOnContentChangeProperty;

		public readonly static DependencyProperty CustomVisualStatesProperty;

		public readonly static DependencyProperty CustomVisualStatesNameProperty;

		private ContentPresenter CurrentContentPresentationSite
		{
			get;
			set;
		}

		private Storyboard CurrentTransition
		{
			get
			{
				return this._currentTransition;
			}
			set
			{
				if (this._currentTransition != null)
				{
					this._currentTransition.Completed -= new EventHandler(this.OnTransitionCompleted);
				}
				this._currentTransition = value;
				if (this._currentTransition != null)
				{
					this._currentTransition.Completed += new EventHandler(this.OnTransitionCompleted);
				}
			}
		}

		public ObservableCollection<VisualState> CustomVisualStates
		{
			get
			{
				return (ObservableCollection<VisualState>)base.GetValue(TransitioningContentControl.CustomVisualStatesProperty);
			}
			set
			{
				base.SetValue(TransitioningContentControl.CustomVisualStatesProperty, value);
			}
		}

		public string CustomVisualStatesName
		{
			get
			{
				return (string)base.GetValue(TransitioningContentControl.CustomVisualStatesNameProperty);
			}
			set
			{
				base.SetValue(TransitioningContentControl.CustomVisualStatesNameProperty, value);
			}
		}

		public bool IsTransitioning
		{
			get
			{
				return (bool)base.GetValue(TransitioningContentControl.IsTransitioningProperty);
			}
			private set
			{
				this._allowIsTransitioningWrite = true;
				base.SetValue(TransitioningContentControl.IsTransitioningProperty, value);
				this._allowIsTransitioningWrite = false;
			}
		}

		private ContentPresenter PreviousContentPresentationSite
		{
			get;
			set;
		}

		public bool RestartTransitionOnContentChange
		{
			get
			{
				return (bool)base.GetValue(TransitioningContentControl.RestartTransitionOnContentChangeProperty);
			}
			set
			{
				base.SetValue(TransitioningContentControl.RestartTransitionOnContentChangeProperty, value);
			}
		}

		public TransitionType Transition
		{
			get
			{
				return (TransitionType)base.GetValue(TransitioningContentControl.TransitionProperty);
			}
			set
			{
				base.SetValue(TransitioningContentControl.TransitionProperty, value);
			}
		}

		static TransitioningContentControl()
		{
			Class6.yDnXvgqzyB5jw();
			TransitioningContentControl.IsTransitioningProperty = DependencyProperty.Register("IsTransitioning", typeof(bool), typeof(TransitioningContentControl), new PropertyMetadata(new PropertyChangedCallback(TransitioningContentControl.OnIsTransitioningPropertyChanged)));
			TransitioningContentControl.TransitionProperty = DependencyProperty.Register("Transition", typeof(TransitionType), typeof(TransitioningContentControl), new FrameworkPropertyMetadata((object)TransitionType.Default, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(TransitioningContentControl.OnTransitionPropertyChanged)));
			TransitioningContentControl.RestartTransitionOnContentChangeProperty = DependencyProperty.Register("RestartTransitionOnContentChange", typeof(bool), typeof(TransitioningContentControl), new PropertyMetadata(false, new PropertyChangedCallback(TransitioningContentControl.OnRestartTransitionOnContentChangePropertyChanged)));
			TransitioningContentControl.CustomVisualStatesProperty = DependencyProperty.Register("CustomVisualStates", typeof(ObservableCollection<VisualState>), typeof(TransitioningContentControl), new PropertyMetadata(null));
			TransitioningContentControl.CustomVisualStatesNameProperty = DependencyProperty.Register("CustomVisualStatesName", typeof(string), typeof(TransitioningContentControl), new PropertyMetadata("CustomTransition"));
		}

		public TransitioningContentControl()
		{
			Class6.yDnXvgqzyB5jw();
			base();
			this.CustomVisualStates = new ObservableCollection<VisualState>();
			base.DefaultStyleKey = typeof(TransitioningContentControl);
		}

		public void AbortTransition()
		{
			VisualStateManager.GoToState(this, "Normal", false);
			this.IsTransitioning = false;
			if (this.PreviousContentPresentationSite != null)
			{
				this.PreviousContentPresentationSite.Content = null;
			}
		}

		private Storyboard GetStoryboard(TransitionType newTransition)
		{
			VisualStateGroup visualStateGroup = MahApps.Metro.Controls.VisualStates.TryGetVisualStateGroup(this, "PresentationStates");
			Storyboard storyboard = null;
			if (visualStateGroup != null)
			{
				string transitionName = this.GetTransitionName(newTransition);
				storyboard = (
					from state in visualStateGroup.States.OfType<VisualState>()
					where state.Name == transitionName
					select state.Storyboard).FirstOrDefault<Storyboard>();
			}
			return storyboard;
		}

		private string GetTransitionName(TransitionType transition)
		{
			switch (transition)
			{
				case TransitionType.Normal:
				{
					return "Normal";
				}
				case TransitionType.Up:
				{
					return "UpTransition";
				}
				case TransitionType.Down:
				{
					return "DownTransition";
				}
				case TransitionType.Right:
				{
					return "RightTransition";
				}
				case TransitionType.RightReplace:
				{
					return "RightReplaceTransition";
				}
				case TransitionType.Left:
				{
					return "LeftTransition";
				}
				case TransitionType.LeftReplace:
				{
					return "LeftReplaceTransition";
				}
				case TransitionType.Custom:
				{
					return this.CustomVisualStatesName;
				}
				default:
				{
					return "DefaultTransition";
				}
			}
		}

		public override void OnApplyTemplate()
		{
			if (this.IsTransitioning)
			{
				this.AbortTransition();
			}
			if (this.CustomVisualStates != null && this.CustomVisualStates.Any<VisualState>())
			{
				VisualStateGroup visualStateGroup = MahApps.Metro.Controls.VisualStates.TryGetVisualStateGroup(this, "PresentationStates");
				if (visualStateGroup != null)
				{
					foreach (VisualState customVisualState in this.CustomVisualStates)
					{
						visualStateGroup.States.Add(customVisualState);
					}
				}
			}
			base.OnApplyTemplate();
			this.PreviousContentPresentationSite = base.GetTemplateChild("PreviousContentPresentationSite") as ContentPresenter;
			this.CurrentContentPresentationSite = base.GetTemplateChild("CurrentContentPresentationSite") as ContentPresenter;
			if (this.CurrentContentPresentationSite != null)
			{
				if (base.ContentTemplateSelector != null)
				{
					this.CurrentContentPresentationSite.ContentTemplate = base.ContentTemplateSelector.SelectTemplate(base.Content, this);
				}
				this.CurrentContentPresentationSite.Content = base.Content;
			}
			Storyboard storyboard = this.GetStoryboard(this.Transition);
			this.CurrentTransition = storyboard;
			if (storyboard == null)
			{
				TransitionType transition = this.Transition;
				this.Transition = TransitionType.Default;
				throw new ArgumentException(string.Format("'{0}' Transition could not be found!", transition), "Transition");
			}
			VisualStateManager.GoToState(this, "Normal", false);
		}

		protected override void OnContentChanged(object oldContent, object newContent)
		{
			base.OnContentChanged(oldContent, newContent);
			this.StartTransition(oldContent, newContent);
		}

		private static void OnIsTransitioningPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TransitioningContentControl oldValue = (TransitioningContentControl)d;
			if (!oldValue._allowIsTransitioningWrite)
			{
				oldValue.IsTransitioning = (bool)e.OldValue;
				throw new InvalidOperationException();
			}
		}

		protected virtual void OnRestartTransitionOnContentChangeChanged(bool oldValue, bool newValue)
		{
		}

		private static void OnRestartTransitionOnContentChangePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((TransitioningContentControl)d).OnRestartTransitionOnContentChangeChanged((bool)e.OldValue, (bool)e.NewValue);
		}

		private void OnTransitionCompleted(object sender, EventArgs e)
		{
			this.AbortTransition();
			RoutedEventHandler routedEventHandler = this.TransitionCompleted;
			if (routedEventHandler != null)
			{
				routedEventHandler(this, new RoutedEventArgs());
			}
		}

		private static void OnTransitionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TransitioningContentControl transitioningContentControl = (TransitioningContentControl)d;
			TransitionType oldValue = (TransitionType)e.OldValue;
			TransitionType newValue = (TransitionType)e.NewValue;
			if (transitioningContentControl.IsTransitioning)
			{
				transitioningContentControl.AbortTransition();
			}
			Storyboard storyboard = transitioningContentControl.GetStoryboard(newValue);
			if (storyboard != null)
			{
				transitioningContentControl.CurrentTransition = storyboard;
				return;
			}
			if (MahApps.Metro.Controls.VisualStates.TryGetVisualStateGroup(transitioningContentControl, "PresentationStates") != null)
			{
				transitioningContentControl.SetValue(TransitioningContentControl.TransitionProperty, oldValue);
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Temporary removed exception message", new object[] { newValue }));
			}
			transitioningContentControl.CurrentTransition = null;
		}

		public void ReloadTransition()
		{
			if (this.CurrentContentPresentationSite != null && this.PreviousContentPresentationSite != null)
			{
				if (this.RestartTransitionOnContentChange)
				{
					this.CurrentTransition.Completed -= new EventHandler(this.OnTransitionCompleted);
				}
				if (!this.IsTransitioning || this.RestartTransitionOnContentChange)
				{
					if (this.RestartTransitionOnContentChange)
					{
						this.CurrentTransition.Completed += new EventHandler(this.OnTransitionCompleted);
					}
					this.IsTransitioning = true;
					VisualStateManager.GoToState(this, "Normal", false);
					VisualStateManager.GoToState(this, this.GetTransitionName(this.Transition), true);
				}
			}
		}

		private void StartTransition(object oldContent, object newContent)
		{
			if (this.CurrentContentPresentationSite != null && this.PreviousContentPresentationSite != null)
			{
				if (this.RestartTransitionOnContentChange)
				{
					this.CurrentTransition.Completed -= new EventHandler(this.OnTransitionCompleted);
				}
				if (base.ContentTemplateSelector != null)
				{
					this.PreviousContentPresentationSite.ContentTemplate = base.ContentTemplateSelector.SelectTemplate(oldContent, this);
					this.CurrentContentPresentationSite.ContentTemplate = base.ContentTemplateSelector.SelectTemplate(newContent, this);
				}
				this.CurrentContentPresentationSite.Content = newContent;
				this.PreviousContentPresentationSite.Content = oldContent;
				if (!this.IsTransitioning || this.RestartTransitionOnContentChange)
				{
					if (this.RestartTransitionOnContentChange)
					{
						this.CurrentTransition.Completed += new EventHandler(this.OnTransitionCompleted);
					}
					this.IsTransitioning = true;
					VisualStateManager.GoToState(this, "Normal", false);
					VisualStateManager.GoToState(this, this.GetTransitionName(this.Transition), true);
				}
			}
		}

		public event RoutedEventHandler TransitionCompleted;
	}
}