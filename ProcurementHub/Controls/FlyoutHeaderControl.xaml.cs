namespace ProcurementHub.Controls;

public partial class FlyoutHeaderControl : StackLayout
{
	public FlyoutHeaderControl()
	{
		InitializeComponent();
        if (App.User != null)
        {
            lblUserName.Text = App.User.UserName;
            lblName.Text = App.User.Person.GetFullName();
        }
	}
}