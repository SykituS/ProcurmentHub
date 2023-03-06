using GrpcShared;

namespace ProcurementHub.ViewModel
{
    [INotifyPropertyChanged]
    public partial class BaseViewModel
    {
        public Procurement.ProcurementClient ProcurementClient;
        
        public BaseViewModel(Procurement.ProcurementClient procurementClient)
        {
            ProcurementClient = procurementClient;
        }

        [ObservableProperty]
        bool isBusy;

        [ObservableProperty]
        string title;

        public bool IsNotBusy => !isBusy;
    }
}
