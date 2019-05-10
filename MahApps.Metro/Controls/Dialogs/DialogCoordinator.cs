using MahApps.Metro.Controls;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace MahApps.Metro.Controls.Dialogs
{
	public class DialogCoordinator : GInterface0
	{
		public readonly static DialogCoordinator Instance;

		static DialogCoordinator()
		{
			Class6.yDnXvgqzyB5jw();
			DialogCoordinator.Instance = new DialogCoordinator();
		}

		public DialogCoordinator()
		{
			Class6.yDnXvgqzyB5jw();
			base();
		}

		public Task<TDialog> GetCurrentDialogAsync<TDialog>(object context)
		where TDialog : BaseMetroDialog
		{
			return DialogCoordinator.GetMetroWindow(context).GetCurrentDialogAsync<TDialog>();
		}

		private static MetroWindow GetMetroWindow(object context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (!DialogParticipation.IsRegistered(context))
			{
				throw new InvalidOperationException("Context is not registered. Consider using DialogParticipation.Register in XAML to bind in the DataContext.");
			}
			MetroWindow window = Window.GetWindow(DialogParticipation.GetAssociation(context)) as MetroWindow;
			if (window == null)
			{
				throw new InvalidOperationException("Control is not inside a MetroWindow.");
			}
			return window;
		}

		public Task HideMetroDialogAsync(object context, BaseMetroDialog dialog, MetroDialogSettings settings = null)
		{
			return DialogCoordinator.GetMetroWindow(context).HideMetroDialogAsync(dialog, settings);
		}

		public Task<string> ShowInputAsync(object context, string title, string message, MetroDialogSettings metroDialogSettings = null)
		{
			MetroWindow metroWindow = DialogCoordinator.GetMetroWindow(context);
			return metroWindow.ShowInputAsync(title, message, metroDialogSettings);
		}

		public Task<LoginDialogData> ShowLoginAsync(object context, string title, string message, LoginDialogSettings settings = null)
		{
			MetroWindow metroWindow = DialogCoordinator.GetMetroWindow(context);
			return metroWindow.ShowLoginAsync(title, message, settings);
		}

		public Task<MessageDialogResult> ShowMessageAsync(object context, string title, string message, MessageDialogStyle style = 0, MetroDialogSettings settings = null)
		{
			MetroWindow metroWindow = DialogCoordinator.GetMetroWindow(context);
			return metroWindow.ShowMessageAsync(title, message, style, settings);
		}

		public Task ShowMetroDialogAsync(object context, BaseMetroDialog dialog, MetroDialogSettings settings = null)
		{
			return DialogCoordinator.GetMetroWindow(context).ShowMetroDialogAsync(dialog, settings);
		}

		public Task<ProgressDialogController> ShowProgressAsync(object context, string title, string message, bool isCancelable = false, MetroDialogSettings settings = null)
		{
			MetroWindow metroWindow = DialogCoordinator.GetMetroWindow(context);
			return metroWindow.ShowProgressAsync(title, message, isCancelable, settings);
		}
	}
}