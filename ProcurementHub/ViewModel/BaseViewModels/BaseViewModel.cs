using GrpcShared;

namespace ProcurementHub.ViewModel.BaseViewModels
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
        bool _isBusy;

        [ObservableProperty]
        string _title;

        public bool IsNotBusy => !_isBusy;
    }
}
