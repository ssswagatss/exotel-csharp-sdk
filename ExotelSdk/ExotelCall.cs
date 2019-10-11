using ExotelSdk.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ExotelSdk
{
    public class ExotelCall
    {
        private readonly string _sId;
        private readonly string _apiKey;
        private readonly string _apiToken;
        private readonly string _baseUrl;

        private const string _callEndPoint = "Calls/connect.json";
        private const string _smsEndPoint = "Sms/send.json";

        public ExotelCall(string sid, string apiKey, string apiToken)
        {
            _sId = sid;
            _apiKey = apiKey;
            _apiToken = apiToken;
            _baseUrl = $"https://api.exotel.com/v1/Accounts/{this._sId}/";
        }

        /// <summary>
        /// This API will connect two numbers. It connects From Number first. Once the person at the From end picks up the phone, it will connect to the number provided as To. You can choose which number should be connected first by adding that number in the From field. An HTTP POST request is made to
        /// </summary>
        /// <param name="from">The phone number that will be called first. Preferably in E.164 format. If not set, our system will try to match it with a country and make a call. If landline number, prefix it with STD code; Ex: 080XXXX2400</param>
        /// <param name="to">Your customer's phone number. If landline number, prefix it with STD code; Ex: 080XXXX2400</param>
        /// <param name="callerId">This is your ExoPhone/Exotel Virtual Number</param>
        /// <param name="timeLimit">The time limit (in seconds) that you want this call to last. The call will be cut after this time</param>
        /// <param name="timeOut">The time (in seconds) to ring the called parties (both first and second call leg)</param>
        /// <param name="statusCallback">An HTTP POST request will be made to this URL depending on what events are subscribed using ‘StatusCallbackEvents’.</param>
        /// <param name="isRecord">Record the conversation of your call. The RecordingUrl will be sent to the StatusCallback URL if this is set to 'true' and the call conversation happens. Can be:
        ///true (default) - Call conversation will be recorded.
        ///false - Call conversation will not be recorded.
        ///</param>
        /// <returns></returns>
        public async Task<ExotelResponse> ConnectTwoNumbersAsync(string from, string to, string callerId, int? timeLimit = null,
                                              int? timeOut = null, string statusCallback = null, bool isRecord = true)
        {
            var response = new ExotelResponse();
            Dictionary<string, string> postValues = new Dictionary<string, string>();
            postValues.Add("From", from);
            postValues.Add("To", to);
            postValues.Add("CallerId", callerId);

            //Bind the optional parameters
            if (timeLimit.HasValue)
                postValues.Add("TimeLimit", timeLimit.Value.ToString());
            if (timeOut.HasValue)
                postValues.Add("TimeOut", timeOut.Value.ToString());
            if (!string.IsNullOrEmpty(statusCallback))
                postValues.Add("StatusCallback", statusCallback);
            if (!isRecord)
                postValues.Add("Record", "false");


            var formContent = ConvertToFormUrlEncodedContent(postValues);
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_apiKey}:{_apiToken}")));
                var postResponse = await client.PostAsync($"{_baseUrl}/{_callEndPoint}", formContent);
                var responsesrtr = await postResponse.Content.ReadAsStringAsync();
                JObject rss = JObject.Parse(responsesrtr);
                if (postResponse.IsSuccessStatusCode)
                {
                    response.IsSuccess = true;
                    if (rss.ContainsKey("Call"))
                        response.Call = JsonConvert.DeserializeObject<ExotelCallResponse>(rss["Call"].ToString());
                }
                else
                {
                    response.IsSuccess = false;
                    if (rss.ContainsKey("RestException"))
                        response.RestException = JsonConvert.DeserializeObject<ExotelRestException>(rss["RestException"].ToString());

                }
            }
            return response;
        }
        private FormUrlEncodedContent ConvertToFormUrlEncodedContent(Dictionary<string, string> postValues)
        {
            return new FormUrlEncodedContent(postValues.Select(x => new KeyValuePair<string, string>(x.Key, x.Value)).ToArray());
        }
    }
}
