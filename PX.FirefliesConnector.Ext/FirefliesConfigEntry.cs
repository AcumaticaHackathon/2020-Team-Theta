using System;
using System.Collections;
using GraphQL.Client;
using GraphQL.Common.Request;
using PX.Data;

namespace PX.FirefliesConnector.Ext
{
    public class FirefliesConfigEntry : PXGraph<FirefliesConfigEntry>
    {
        public PXSelect<FirefliesConfig> config;
        public PXSave<FirefliesConfig> save;
        public PXCancel<FirefliesConfig> cancel;

        #region Action
        public PXAction<FirefliesConfig> testconnection;

        [PXUIField(DisplayName = "Test Connection", MapEnableRights = PXCacheRights.Update, MapViewRights = PXCacheRights.Select)]
        [PXButton()]
        public virtual IEnumerable testConnection(PXAdapter adapter)
        {
            Persist();
            FirefliesConfig firefliesConfig = config.Current;

            var flAuthRequest = new GraphQLRequest
            {
                Query = @"
	                    {
                          user {
                            user_id
                            email
                            recent_transcript
                          }
                        }"
            };

            var graphQLClient = new GraphQLClient(firefliesConfig.APIEndPoint);
            graphQLClient.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", firefliesConfig.Apikey));
            var graphQLResponse = graphQLClient.PostAsync(flAuthRequest).Result;

            //Dynamic
            var rTranscript = graphQLResponse.Data.user.recent_transcript.Value;
            //Strongly Typed
            User user = graphQLResponse.GetDataFieldAs<User>("user");
            
            //if (firefliesConfig != null && (!string.IsNullOrEmpty(firefliesConfig.APIEndPoint) && !string.IsNullOrEmpty(firefliesConfig.Apikey)))
            //{
            //    var client = new RestClient("https://api.fireflies.ai/graphql");
            //    client.Timeout = -1;
            //    var request = new RestRequest(Method.POST);
            //    string AuthCode = $"Bearer {firefliesConfig.Apikey}";
            //    request.AddHeader("Authorization", AuthCode);
            //    request.AddHeader("Content-Type", "application/json");
            //    request.AddParameter("application/json", "{\"query\":\"{\\r\\n  user {\\r\\n    email\\r\\n    recent_transcript\\r\\n  }\\r\\n}\",\"variables\":{}}",
            //               ParameterType.RequestBody);
            //    IRestResponse response = client.Execute(request);
            //    if (response.StatusCode.ToString().ToUpper() == "OK")
            //    {
            //        config.Ask("Connected Successfully..", MessageButtons.OK);
            //    }
            //    else
            //    {
            //        config.Ask("Failed to Connect..", MessageButtons.OK);
            //    }
            //}
            //else
            //{
            //    config.Ask("Missing Configuration", MessageButtons.OK);
            //}
            return adapter.Get();
        }
        #endregion
    }
}