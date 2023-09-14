using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GrpcShared;

namespace ProcurementHub.Services
{
    public class ApiServices : BaseServices
    {
        public ApiServices(Procurement.ProcurementClient procurementClient) : base(procurementClient)
        {
        }

        public async Task ConvertAddressToGeoLocation()
        {
            var url =
                "https://api.geoapify.com/v2/places";
            var client = new HttpClient();

            var categories = "commercial.food_and_drink,catering";
            var filter = "circle:19.364098557728767,51.74177486544875,5000";
            var limit = "100";
            var key = "93ae08b64c714f9fafc967867f8ca43f";

            var path = $"{url}?categories={categories}&filter={filter}&limit={limit}&apiKey={key}";
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.GetAsync(path);
        }
    }
}
