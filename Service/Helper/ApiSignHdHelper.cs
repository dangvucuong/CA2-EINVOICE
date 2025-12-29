using System.ServiceModel;
using Model.Static;
using ApiSign;

namespace Service.Helper
{
    public static class ApiSignHdHelper
    {
        public static wsCA2EinvoiceSoapClient GetClient()
        {
            var epAddress = new System.ServiceModel.EndpointAddress(AppSettings.ApiSignHd.Endpoint);
            var binding = new BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly);
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
            var client = new wsCA2EinvoiceSoapClient(binding, epAddress);
            return client;
        }
       
    }
}