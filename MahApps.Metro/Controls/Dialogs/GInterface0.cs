using System;
using System.Threading.Tasks;

namespace MahApps.Metro.Controls.Dialogs
{
	public interface GInterface0
	{
		Task<TDialog> GetCurrentDialogAsync<TDialog>(object context)
		where TDialog : BaseMetroDialog;

		Task HideMetroDialogAsync(object context, BaseMetroDialog dialog, MetroDialogSettings settings = null);

		Task<string> ShowInputAsync(object context, string title, string message, MetroDialogSettings settings = null);

		Task<LoginDialogData> ShowLoginAsync(object context, string title, string message, LoginDialogSettings settings = null);

		Task<MessageDialogResult> ShowMessageAsync(object context, string title, string message, MessageDialogStyle style = 0, MetroDialogSettings settings = null);

		Task ShowMetroDialogAsync(object context, BaseMetroDialog dialog, MetroDialogSettings settings = null);

		Task<ProgressDialogController> ShowProgressAsync(object context, string title, string message, bool isCancelable = false, MetroDialogSettings settings = null);
	}
}