using GrpcShared.Models;

namespace ProcurementHub;

public partial class App : Application
{
    public static Users User;
	public App(AppShellViewModel viewModel)
	{
		InitializeComponent();

		MainPage = new AppShell(viewModel);
	}
}
