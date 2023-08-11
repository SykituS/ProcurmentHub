using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;
using ProcurementHub.Model.CustomModels;

namespace ProcurementHub.Controls
{
    public static class TeamRestaurantsControl
    {
        public static Popup GeneratePopupForItemManagement(TeamRestaurantsModel model, IRelayCommand editRestaurantCommand, IRelayCommand restaurantsItemsCommand)
        {
            var popup = new Popup()
            {
                Color = Colors.Transparent,
                Content = new Frame()
                {
                    BackgroundColor = Colors.White,
                    BorderColor = Colors.Transparent,
                    CornerRadius = 25,
                    HeightRequest = 150,
                    Content = new VerticalStackLayout()
                    {
                        Padding = new Thickness(15, 5, 15, 5),
                        BackgroundColor = Colors.White,
                        Spacing = 5,
                        Children =
                        {
                            new Button() { Text = "Edit Restaurant", CornerRadius = 15, FontSize = 14, FontAttributes = FontAttributes.Bold, Command = editRestaurantCommand, CommandParameter = model, BackgroundColor = Color.FromArgb("#0d529c"), BorderColor = Color.FromArgb("#0d529c"), TextColor = Colors.White},
                            new Button() { Text = "Restaurant items", CornerRadius = 15, FontSize = 14, FontAttributes = FontAttributes.Bold, Command = restaurantsItemsCommand, CommandParameter = model, BackgroundColor = Color.FromArgb("#0d529c"), BorderColor = Color.FromArgb("#0d529c"), TextColor = Colors.White},
                        }
                    }
                }
            };

            return popup;
        }
    }
}
