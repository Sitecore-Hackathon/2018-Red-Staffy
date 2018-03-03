using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;

namespace Hackathon.RedStaffy.XConnectBI.Repositories
{
    public class PowerBiRepository:IPowerBiRepository
    {
        public string GetToken()
        {
            //Get an authentication access token
            var accessToken = GetTokenAsync();
            return accessToken.Result;
        }

        private async Task<string> GetTokenAsync()
        {
            string clientID = "b6f9d4ec-2541-4077-b152-65eeb3bfd9c6";
            string clientSecret = "U0t30/CbIIuxUErQcFnWQZP/MKylVNeVJgt7vO8rcpM=";
            string resourceUri = "https://analysis.windows.net/powerbi/api";
            string authorityUri = "https://login.windows.net/common/oauth2/authorize";
            
            ClientCredential credential = new ClientCredential(clientID, clientSecret);
            AuthenticationContext context = new AuthenticationContext(authorityUri);
            AuthenticationResult result = await context.AcquireTokenAsync(resourceUri, credential);
            return result.AccessToken;
        }

        public void CreateDataset(string accessToken)
        {
            string powerBIDatasetsApiUrl = "https://api.powerbi.com/v1.0/myorg/datasets";
         
            HttpWebRequest request = WebRequest.Create(powerBIDatasetsApiUrl) as System.Net.HttpWebRequest;
            request.KeepAlive = true;
            request.Method = "POST";
            request.ContentLength = 0;
            request.ContentType = "application/json";

            //Add token to the request header
            request.Headers.Add("Authorization", String.Format("Bearer {0}", accessToken));

            //Create dataset JSON for POST request
            string datasetJson = "{\"name\": \"SalesMarketing\", \"tables\": " +
                                 "[{\"name\": \"Product\", \"columns\": " +
                                 "[{ \"name\": \"ProductID\", \"dataType\": \"Int64\"}, " +
                                 "{ \"name\": \"Name\", \"dataType\": \"string\"}, " +
                                 "{ \"name\": \"Category\", \"dataType\": \"string\"}," +
                                 "{ \"name\": \"IsCompete\", \"dataType\": \"bool\"}," +
                                 "{ \"name\": \"ManufacturedOn\", \"dataType\": \"DateTime\"}" +
                                 "]}]}";

            //POST web request
            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(datasetJson);
            request.ContentLength = byteArray.Length;

            //Write JSON byte[] into a Stream
            using (Stream writer = request.GetRequestStream())
            {
                writer.Write(byteArray, 0, byteArray.Length);

                var response = (HttpWebResponse)request.GetResponse();
            }
        }
        
        public string GetDataset(string accessToken)
        {
            string powerBIDatasetsApiUrl = "https://api.powerbi.com/beta/07e62381-54f0-43a1-928b-2f30c549475a/datasets";
            HttpWebRequest request = System.Net.WebRequest.Create(powerBIDatasetsApiUrl) as System.Net.HttpWebRequest;
            request.KeepAlive = true;
            request.Method = "GET";
            request.ContentLength = 0;
            request.ContentType = "application/json";

            //Add token to the request header
            request.Headers.Add("Authorization", String.Format("Bearer {0}", accessToken));

            string datasetId = string.Empty;
            //Get HttpWebResponse from GET request
            using (HttpWebResponse httpResponse = request.GetResponse() as System.Net.HttpWebResponse)
            {
                //Get StreamReader that holds the response stream
                using (StreamReader reader = new System.IO.StreamReader(httpResponse.GetResponseStream()))
                {
                    string responseContent = reader.ReadToEnd();

                    //TODO: Install NuGet Newtonsoft.Json package: Install-Package Newtonsoft.Json
                    //and add using Newtonsoft.Json
                    var results = JsonConvert.DeserializeObject<dynamic>(responseContent);

                    //Get the first id
                    datasetId = results["value"][0]["id"];
                    return datasetId;
                }
            }
        }

        public void AddRows(string datasetId, string tableName, string accessToken)
        {
            string powerBIApiAddRowsUrl = String.Format("https://api.powerbi.com/v1.0/myorg/datasets/{0}/tables/{1}/rows", datasetId, tableName);

            //POST web request to add rows.
            //To add rows to a dataset in a group, use the Groups uri: https://api.powerbi.com/v1.0/myorg/groups/{group_id}/datasets/{dataset_id}/tables/{table_name}/rows
            //Change request method to "POST"
            HttpWebRequest request = System.Net.WebRequest.Create(powerBIApiAddRowsUrl) as System.Net.HttpWebRequest;
            request.KeepAlive = true;
            request.Method = "POST";
            request.ContentLength = 0;
            request.ContentType = "application/json";

            //Add token to the request header
            request.Headers.Add("Authorization", String.Format("Bearer {0}", accessToken));

            //JSON content for product row
            string rowsJson = "{\"rows\":" +
                              "[{\"ProductID\":1,\"Name\":\"Adjustable Race\",\"Category\":\"Components\",\"IsCompete\":true,\"ManufacturedOn\":\"07/30/2014\"}," +
                              "{\"ProductID\":2,\"Name\":\"LL Crankarm\",\"Category\":\"Components\",\"IsCompete\":true,\"ManufacturedOn\":\"07/30/2014\"}," +
                              "{\"ProductID\":3,\"Name\":\"HL Mountain Frame - Silver\",\"Category\":\"Bikes\",\"IsCompete\":true,\"ManufacturedOn\":\"07/30/2014\"}]}";

            //POST web request
            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(rowsJson);
            request.ContentLength = byteArray.Length;

            //Write JSON byte[] into a Stream
            using (Stream writer = request.GetRequestStream())
            {
                writer.Write(byteArray, 0, byteArray.Length);

                var response = (HttpWebResponse)request.GetResponse();
            }
        }

    }
}