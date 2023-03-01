using GrpcShared;

namespace ProcurementHub.ViewModel
{
    [INotifyPropertyChanged]
    public partial class BaseViewModel
    {
        public Greeter.GreeterClient _greeterClient;

        public BaseViewModel(Greeter.GreeterClient greeterClient)
        {
            _greeterClient = greeterClient;
        }

        [ObservableProperty]
        bool isBusy;

        [ObservableProperty]
        string title;

        public bool IsNotBusy => !isBusy;
    }
}
