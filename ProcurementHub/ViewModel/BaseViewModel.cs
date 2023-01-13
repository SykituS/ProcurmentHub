namespace ProcurementHub.ViewModel
{
    [INotifyPropertyChanged]
    public partial class BaseViewModel
    {
        [ObservableProperty]
        bool isBusy;

        [ObservableProperty]
        string title;

        public bool IsNotBusy => !isBusy;
    }
}
