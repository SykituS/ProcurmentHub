using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using ProcurementHub.ViewModel.TeamsViewModels.TeamRestaurantsViewModels;
using Map = Microsoft.Maui.Controls.Maps.Map;

namespace ProcurementHub.View.Teams.TeamRestaurants;

public partial class TeamRestaurantsAddEditPage : ContentPage
{
	public TeamRestaurantsAddEditPage(TeamRestaurantsAddEditViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;

        map.Pins.Add(new Pin()
        {
            Location = new Location(51.748580, 19.405720),
            Label = "McDonald",
            Address = "Location 123",
            Type = PinType.Place,
        });
        map.IsTrafficEnabled = true;
        map.MoveToRegion(MapSpan.FromCenterAndRadius(new Location(51.759048, 19.458599), Distance.FromKilometers(10)));

    }

    void OnMapClicked(object sender, MapClickedEventArgs e)
    {
        System.Diagnostics.Debug.WriteLine($"MapClick: {e.Location.Latitude}, {e.Location.Longitude}");
        
        map.Pins.Add(new Pin()
        {
            Location = new Location(e.Location.Latitude, e.Location.Longitude),
            Label = "Restaurant",
            Address = "",
            Type = PinType.Generic,
        });
    }
}