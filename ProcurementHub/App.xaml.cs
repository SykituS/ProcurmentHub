using GrpcShared.Models;

namespace ProcurementHub;

public partial class App : Application
{
    public static Users LoggedUserInApplication;
	public App()
	{
		InitializeComponent();
        Current.UserAppTheme = AppTheme.Light;
		MainPage = new AppShell();
	}
}
