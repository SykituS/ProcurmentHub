using CommunityToolkit.Maui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcurementHub.Controls
{
    public static class OrderWaitControl
    {
        public static Popup GeneratePopupForOrderDeliveryConfirmation(IRelayCommand finishOrderCommand)
        {
            var popup = new Popup()
            {
                Color = Colors.Transparent,
                Content = new Frame()
                {
                    BackgroundColor = Colors.White,
                    BorderColor = Colors.Transparent,
                    CornerRadius = 25,
                    Content = new VerticalStackLayout()
                    {
                        Padding = new Thickness(15, 5, 15, 5),
                        HeightRequest = 200,
                        BackgroundColor = Colors.White,
                        Spacing = 5,
                        Children =
                        {
                            new Label() { Text = "Order will be closed!", HorizontalTextAlignment = TextAlignment.Center, FontSize = 24 },
                            new Label() { Text = "Did you delivered order?", HorizontalTextAlignment = TextAlignment.Center, FontSize = 12 },
                            new Button() { Text = "Yes", CornerRadius = 15, FontSize = 14, FontAttributes = FontAttributes.Bold, Command = finishOrderCommand, CommandParameter = true, BackgroundColor = Color.FromArgb("#0d529c"), BorderColor = Color.FromArgb("#0d529c"), TextColor = Colors.White},
                            new Button() { Text = "No", CornerRadius = 15, FontSize = 14, FontAttributes = FontAttributes.Bold, Command = finishOrderCommand, CommandParameter = false, BackgroundColor = Color.FromArgb("#0d529c"), BorderColor = Color.FromArgb("#0d529c"), TextColor = Colors.White},
                        }
                    }
                }
            };

            return popup;
        }
    }
}
