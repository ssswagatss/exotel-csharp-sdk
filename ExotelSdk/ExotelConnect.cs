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
    public class ExotelConnect
    {
        private readonly string _sId;
        private readonly string _apiKey;
        private readonly string _apiToken;
        private readonly string _baseUrl;
        private static HttpClient _httpClient;


        private const string _callEndPoint = "Calls/connect.json";
        private const string _smsEndPoint = "Sms/send.json";
        private const string _callDetailsEndPoint = "Calls";
        private const string _smsDetailsEndPoint = "Sms";

        /// <summary>
        /// Initializes a new instance of the <see cref="ExotelConnect"/> class.
        /// </summary>
        /// <param name="sid">Account sid.</param>
        /// <param name="apiKey">Your API key.</param>
        /// <param name="apiToken">Your API token.</param>
        public ExotelConnect(string sid, string apiKey, string apiToken)
        {
            _sId = sid;
            _apiKey = apiKey;
            _apiToken = apiToken;
            _baseUrl = $"https://api.exotel.com/v1/Accounts/{this._sId}/";
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_apiKey}:{_apiToken}")));
        }

        #region Exotel Call Apis
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
            var postResponse = await _httpClient.PostAsync($"{_baseUrl}/{_callEndPoint}", formContent);
            return await MapExotelCallResponse(postResponse);
        }
        /// <summary>
        /// Gets the call details.
        /// </summary>
        /// <param name="callReferenceId">Call Reference Id</param>
        /// <returns></returns>
        public async Task<ExotelResponse> GetCallDetails(string callReferenceId)
        {
            var getResponse = await _httpClient.GetAsync($"{_baseUrl}/{_callDetailsEndPoint}/{callReferenceId}.json");
            return await MapExotelCallResponse(getResponse);
        }
        #endregion

        #region Exotel SMS Apis
        /// <summary>
        /// This API will send an SMS to the specified To number.
        /// </summary>
        /// <param name="from">Specify one of your ExoPhone
        ///For transactional SMSes, the SenderID(When you buy an ExoPhone, you will be asked to enter a 6-letter sender ID from which your SMSes will be sent. For Eg: LM-EXOTEL or LM-WEBDEV etc.) will be the one that corresponds to the ExoPhone
        ///For promotional SMSes, the SenderID will anyways be a generic numeric one(Ex: LM-123456)
        ///</param>
        /// <param name="to">Mobile number to which SMS has to be sent. Preferably in E.164 format. If not set, our system will try to match it with a country and route the SMS</param>
        /// <param name="messageBody">Content of your SMS; Max Length of the body cannot exceed 2000 characters</param>
        /// <param name="encodingType">Message type of SMS; "plain" or "unicode"</param>
        /// <param name="priority">Priority of the SMS; "normal" or "high". Business critical operations like sending verification codes, confirming appointments etc which require immediate SMS delivery should opt for high priority.</param>
        /// <returns></returns>
        public async Task<ExotelResponse> SendSms(string from, string to, string messageBody, string encodingType = "", string priority = "")
        {
            Dictionary<string, string> postValues = new Dictionary<string, string>();
            postValues.Add("From", from);
            postValues.Add("To", to);
            postValues.Add("Body", messageBody);
            //Bind the optional parameters
            if (!string.IsNullOrEmpty(encodingType))
                postValues.Add("EncodingType", encodingType);

            if (!string.IsNullOrEmpty(priority))
                postValues.Add("Priority", priority);

            var formContent = ConvertToFormUrlEncodedContent(postValues);
            var postResponse = await _httpClient.PostAsync($"{_baseUrl}/{_callEndPoint}", formContent);
            return await MapExotelSmsResponse(postResponse);
        }
        /// <summary>
        /// Gets the SMS details.
        /// </summary>
        /// <param name="smsReferenceId">The SMS reference identifier.</param>
        /// <returns></returns>
        public async Task<ExotelResponse> GetSmsDetails(string smsReferenceId)
        {
            var getResponse = await _httpClient.GetAsync($"{_baseUrl}/{_smsDetailsEndPoint}/{smsReferenceId}.json");
            return await MapExotelSmsResponse(getResponse);
        }
        #endregion

        #region Private Methods
        private async Task<ExotelResponse> MapExotelSmsResponse(HttpResponseMessage responseMessage)
        {
            var response = new ExotelResponse();
            var responsesrtr = await responseMessage.Content.ReadAsStringAsync();
            JObject rss = JObject.Parse(responsesrtr);
            if (responseMessage.IsSuccessStatusCode)
            {
                response.IsSuccess = true;
                if (rss.ContainsKey("SMSMessage"))
                    response.SMSMessage = JsonConvert.DeserializeObject<ExotelSmsResponse>(rss["SMSMessage"].ToString());
            }
            else
            {
                response.IsSuccess = false;
                if (rss.ContainsKey("RestException"))
                    response.RestException = JsonConvert.DeserializeObject<ExotelRestException>(rss["RestException"].ToString());
            }
            return response;
        }
        private async Task<ExotelResponse> MapExotelCallResponse(HttpResponseMessage responseMessage)
        {
            var response = new ExotelResponse();
            var responsesrtr = await responseMessage.Content.ReadAsStringAsync();
            JObject rss = JObject.Parse(responsesrtr);
            if (responseMessage.IsSuccessStatusCode)
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
            return response;
        }
        private FormUrlEncodedContent ConvertToFormUrlEncodedContent(Dictionary<string, string> postValues)
        {
            return new FormUrlEncodedContent(postValues.Select(x => new KeyValuePair<string, string>(x.Key, x.Value)).ToArray());
        }
        #endregion
    }
}
