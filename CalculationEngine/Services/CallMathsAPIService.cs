using CalculationEngine.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CalculationEngine.Services
{
    public interface ICallMathsAPIService
    {
        Task<ExpressionResponse> CalculateResult(string[] expressions, int? precision = 0);
    }
    public class CallMathsAPIService : ICallMathsAPIService
    {
        public async Task<ExpressionResponse> CalculateResult(string[] expressions, int? precision = 0)
        {
            try
            {
                HttpResponseMessage response = null;
                ExpressionRequest reqBody = new ExpressionRequest() { expr = expressions, precision = precision.HasValue ? precision.Value : 0 };
                var bodyJson = JsonConvert.SerializeObject(reqBody);

                response = await PostHttpRequest(bodyJson);

                if (response != null)
                {
                    response.EnsureSuccessStatusCode();
                    var responseResult = response.Content.ReadAsStringAsync();
                    var calculatedResponse = JsonConvert.DeserializeObject<ExpressionResponse>(responseResult.Result.ToString());
                    return calculatedResponse;
                }
                return null;
            }
            catch(Exception ex)
            {
                return new ExpressionResponse() { result = null, error = $"Error occurred while calling API to evaluate expression - {ex.Message}" };
            }
        }
        private async Task<HttpResponseMessage> PostHttpRequest(string bodyJson)
        {
            try
            {
                HttpResponseMessage response;
                var baseURI = "http://api.mathjs.org/v4/";
                HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Post, new Uri(baseURI));
                var client = new HttpClient();
                httpRequest.Content = new StringContent(bodyJson);
                httpRequest.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = await client.SendAsync(httpRequest);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    
}
