using Core.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.MicrosoftTranslator
{
    public static class Translate
    {
        public static string RunSample(TranslateDto data)
        {
            if (string.IsNullOrWhiteSpace(data.Key))
            {
                throw new ArgumentException("The subscription key has not been provided.");
            }

            var authTokenSource = new AzureAuthToken(data.Key.Trim());
            string authToken;
            try
            {
                authToken = authTokenSource.GetAccessToken();
            }
            catch (HttpRequestException)
            {
                if (authTokenSource.RequestStatusCode == HttpStatusCode.Unauthorized)
                    return "Request to token service is not authorized (401). Check that the Azure subscription key is valid.";
                if (authTokenSource.RequestStatusCode == HttpStatusCode.Forbidden)
                    return "Request to token service is not authorized (403). For accounts in the free-tier, check that the account quota is not exceeded.";
                throw;
            }

            return Run(authToken, data);
        }

        private static string Run(string authToken, TranslateDto data)
        {
            string uri = "https://api.microsofttranslator.com/v2/Http.svc/Translate?text=" + HttpUtility.UrlEncode(data.Text) + "&from=" + data.From + "&to=" + data.To;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpWebRequest.Headers.Add("Authorization", authToken);
            using (WebResponse response = httpWebRequest.GetResponse())
            using (Stream stream = response.GetResponseStream())
            {
                DataContractSerializer dcs = new DataContractSerializer(Type.GetType("System.String"));
                string translation = (string)dcs.ReadObject(stream);

                return translation;
            }
        }
    }
}
