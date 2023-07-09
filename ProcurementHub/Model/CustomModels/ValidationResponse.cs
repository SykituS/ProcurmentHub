namespace ProcurementHub.Model.CustomModels
{
    public class ValidationResponseWithResult<T> : ValidationResponse where T : class
    {
        public T ResultValues { get; set; }
    }

    public class ValidationResponse
    {
        public bool Successful { get; set; }
        public string Information { get; set; }
    }
}
