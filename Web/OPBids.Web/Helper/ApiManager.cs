using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using OPBids.Entities.Common;
using OPBids.Entities.Base;
using OPBids.Entities.View.Shared;
using OPBids.Entities.View.Setting;
using OPBids.Common;

namespace OPBids.Web.Helper
{
    public class ApiManager<T>
    {
        public T Invoke(string baseUrl, string api, BaseVM payload)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var request = new HttpRequestMessage(HttpMethod.Post, api) { Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json") };

                var result = client.SendAsync(request).Result;
                var resultContent = result.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<T>(resultContent);
            }
        }
        public T Invoke(string baseUrl, string api, BaseVM[] payload)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var request = new HttpRequestMessage(HttpMethod.Post, api) { Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json") };

                var result = client.SendAsync(request).Result;
                var resultContent = result.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<T>(resultContent);
            }
        }
        public Status InvokeNoData(string baseUrl, string api, BaseVM payload)
        {
            using (var client = new HttpClient())
            {
                Status _status = new Status();

                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var request = new HttpRequestMessage(HttpMethod.Post, api) { Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json") };

                var result = client.SendAsync(request).Result;
                if (!result.IsSuccessStatusCode)
                {
                    _status.code = 1;
                    _status.description = result.ReasonPhrase;
                }

                return _status;
            }
        }

        public T InvokeActivityLog(string baseUrl, string api, ActivityLogModel model)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var request = new HttpRequestMessage(HttpMethod.Post, api) { Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json") };

                var result = client.SendAsync(request).Result;
                var resultContent = result.Content.ReadAsStringAsync().Result;
                try
                {
                    return JsonConvert.DeserializeObject<T>(resultContent);
                }
                catch(Exception ex)
                {
                    return JsonConvert.DeserializeObject<T>("");
                }
            }
        }

        public T InvokeGetLog(string baseUrl, string api, Payload payload)
        {



            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var request = new HttpRequestMessage(HttpMethod.Post, api) { Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json") };

                var result = client.SendAsync(request).Result;
                var resultContent = result.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<T>(resultContent);
            }
        }

        public T InvokeUpdateUserInfo(string baseUrl, string api, AccessUsersVM model)
        {
            
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var request = new HttpRequestMessage(HttpMethod.Post, api) { Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json") };

                var result = client.SendAsync(request).Result;
                var resultContent = result.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<T>(resultContent);
            }
        }
    }
}