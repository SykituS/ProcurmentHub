using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Font = Microsoft.Maui.Font;

namespace ProcurementHub.Controls
{
    public static class SnackBarControl
    {
        /// <summary>
        /// Create snack bar
        /// </summary>
        /// <param name="text">Text to show for user</param>
        /// <param name="action">Actions</param>
        /// <param name="duration">Duration time for snack bar</param>
        /// <returns></returns>
        public static async Task<ISnackbar> CreateSnackBar(string text, Action action = null, TimeSpan? duration = null)
        {
            duration ??= TimeSpan.FromSeconds(3);

            var cancellationTokenSource = new CancellationTokenSource();

            var snackBarOptions = new SnackbarOptions()
            {
                BackgroundColor = Colors.AliceBlue,
                TextColor = Colors.Black,
                CornerRadius = new CornerRadius(15),
                Font = Font.SystemFontOfSize(14),
            };

            var snackBar = Snackbar.Make(text, action, duration: duration, visualOptions: snackBarOptions);
            await snackBar.Show(cancellationTokenSource.Token);
            return snackBar;
        }
    }
}
