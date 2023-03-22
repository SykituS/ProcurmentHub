using GrpcShared.Models;

namespace ProcurementHub;

public partial class App : Application
{
    public static Users LoggedUserInApplication;
	public App(AppShellViewModel viewModel)
	{
		InitializeComponent();

		MainPage = new AppShell(viewModel);
	}
}
