namespace ProcurementHub.Controls;

public partial class FlyoutHeaderControl : StackLayout
{
	public FlyoutHeaderControl()
	{
		InitializeComponent();
        if (App.LoggedUserInApplication != null)
        {
            lblUserName.Text = App.LoggedUserInApplication.UserName;
            lblName.Text = App.LoggedUserInApplication.Person.GetFullName();
        }
	}
}