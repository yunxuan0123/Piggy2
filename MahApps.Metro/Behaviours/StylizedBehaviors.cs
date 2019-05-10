using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Media.Animation;

namespace MahApps.Metro.Behaviours
{
	public class StylizedBehaviors
	{
		private readonly static DependencyProperty OriginalBehaviorProperty;

		public readonly static DependencyProperty BehaviorsProperty;

		static StylizedBehaviors()
		{
			Class6.yDnXvgqzyB5jw();
			StylizedBehaviors.OriginalBehaviorProperty = DependencyProperty.RegisterAttached("OriginalBehaviorInternal", typeof(Behavior), typeof(StylizedBehaviors), new UIPropertyMetadata(null));
			StylizedBehaviors.BehaviorsProperty = DependencyProperty.RegisterAttached("Behaviors", typeof(StylizedBehaviorCollection), typeof(StylizedBehaviors), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(StylizedBehaviors.OnPropertyChanged)));
		}

		public StylizedBehaviors()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		[Category("MahApps.Metro")]
		public static StylizedBehaviorCollection GetBehaviors(DependencyObject uie)
		{
			return (StylizedBehaviorCollection)uie.GetValue(StylizedBehaviors.BehaviorsProperty);
		}

		private static int GetIndexOf(BehaviorCollection itemBehaviors, Behavior behavior)
		{
			int num = -1;
			Behavior originalBehavior = StylizedBehaviors.GetOriginalBehavior(behavior);
			int num1 = 0;
			while (true)
			{
				if (num1 < itemBehaviors.Count)
				{
					Behavior item = itemBehaviors[num1];
					if (item != behavior)
					{
						if (item != originalBehavior)
						{
							Behavior originalBehavior1 = StylizedBehaviors.GetOriginalBehavior(item);
							if (originalBehavior1 == behavior)
							{
								break;
							}
							if (originalBehavior1 != originalBehavior)
							{
								num1++;
								continue;
							}
							else
							{
								break;
							}
						}
					}
					num = num1;
					return num;
				}
				else
				{
					return num;
				}
			}
			num = num1;
			return num;
		}

		private static Behavior GetOriginalBehavior(DependencyObject obj)
		{
			return obj.GetValue(StylizedBehaviors.OriginalBehaviorProperty) as Behavior;
		}

		private static void OnPropertyChanged(DependencyObject dpo, DependencyPropertyChangedEventArgs e)
		{
			UIElement uIElement = dpo as UIElement;
			if (uIElement == null)
			{
				return;
			}
			BehaviorCollection behaviors = Interaction.GetBehaviors(uIElement);
			StylizedBehaviorCollection newValue = e.NewValue as StylizedBehaviorCollection;
			StylizedBehaviorCollection oldValue = e.OldValue as StylizedBehaviorCollection;
			if (newValue == oldValue)
			{
				return;
			}
			if (oldValue != null)
			{
				foreach (Behavior behavior in oldValue)
				{
					int indexOf = StylizedBehaviors.GetIndexOf(behaviors, behavior);
					if (indexOf < 0)
					{
						continue;
					}
					behaviors.RemoveAt(indexOf);
				}
			}
			if (newValue != null)
			{
				foreach (Behavior behavior1 in newValue)
				{
					if (StylizedBehaviors.GetIndexOf(behaviors, behavior1) >= 0)
					{
						continue;
					}
					Behavior behavior2 = (Behavior)behavior1.Clone();
					StylizedBehaviors.SetOriginalBehavior(behavior2, behavior1);
					behaviors.Add(behavior2);
				}
			}
		}

		public static void SetBehaviors(DependencyObject uie, StylizedBehaviorCollection value)
		{
			uie.SetValue(StylizedBehaviors.BehaviorsProperty, value);
		}

		private static void SetOriginalBehavior(DependencyObject obj, Behavior value)
		{
			obj.SetValue(StylizedBehaviors.OriginalBehaviorProperty, value);
		}
	}
}