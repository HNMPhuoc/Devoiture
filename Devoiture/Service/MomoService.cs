using Devoiture.Models.Momo;
using Devoiture.Models.Order;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System.Security.Cryptography;
using System.Text;

namespace Devoiture.Service
{
    public class MomoService : IMomoService
    {
        private readonly IOptions<MomoOptionModel> _options;
        public MomoService(IOptions<MomoOptionModel> options)
        {
            _options = options;
        }
        public async Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(OrderInfoModel model)
        {
            model.OrderId = DateTime.UtcNow.Ticks.ToString();
            model.OrderInfo = "Khách hàng: " + model.FullName + ". Nội dung: " + model.OrderInfo;
            var extraDataObject = new { mayc = model.Mayc, FullName = model.FullName };
            var extraDataJson = JsonConvert.SerializeObject(extraDataObject);
            var extraDataBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(extraDataJson));
            var rawData =
                $"partnerCode={_options.Value.PartnerCode}" +
                $"&accessKey={_options.Value.AccessKey}" +
                $"&requestId={model.OrderId}&amount={((int)model.Amount)}" +
                $"&orderId={model.OrderId}&orderInfo={model.OrderInfo}" +
                $"&returnUrl={_options.Value.ReturnUrl}" +
                $"&notifyUrl={_options.Value.NotifyUrl}&extraData={extraDataBase64}";

            var signature = ComputeHmacSha256(rawData, _options.Value.SecretKey);

            var client = new RestClient(new Uri(_options.Value.MomoApiUrl));
            var request = new RestRequest() { Method = Method.Post };

            request.AddHeader("Content-Type", "application/json; charset=UTF-8");

            // Create an object representing the request data
            var requestData = new
            {
                accessKey = _options.Value.AccessKey,
                partnerCode = _options.Value.PartnerCode,
                requestType = _options.Value.RequestType,
                notifyUrl = _options.Value.NotifyUrl,
                returnUrl = _options.Value.ReturnUrl,
                orderId = model.OrderId,
                amount = ((int)model.Amount).ToString(),
                orderInfo = model.OrderInfo,
                requestId = model.OrderId,
                extraData = extraDataBase64,
                signature = signature,
            };

            string Json = JsonConvert.SerializeObject(requestData);
            request.AddParameter("application/json", Json, ParameterType.RequestBody);

            var response = await client.ExecuteAsync(request);

            return JsonConvert.DeserializeObject<MomoCreatePaymentResponseModel>(response.Content);
        }
        public MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection)
        {
            try
            {
                var amount = int.Parse(collection.First(s => s.Key == "amount").Value);
                var orderInfo = collection.First(s => s.Key == "orderInfo").Value;
                var orderId = collection.First(s => s.Key == "orderId").Value;
                var extraDataBase64 = collection.First(s => s.Key == "extraData").Value;

                // Decode base64 and parse JSON
                var extraDataJson = Encoding.UTF8.GetString(Convert.FromBase64String(extraDataBase64));
                var extraData = JsonConvert.DeserializeObject<Dictionary<string, string>>(extraDataJson);

                // Extract mayc and fullname from extraData
                extraData.TryGetValue("mayc", out var maycValue);
                extraData.TryGetValue("FullName", out var fullname);

                // Parse mayc if available
                int.TryParse(maycValue, out var mayc);

                return new MomoExecuteResponseModel()
                {
                    Amount = amount,
                    OrderId = orderId,
                    OrderInfo = orderInfo,
                    FullName = fullname,
                    Mayc = mayc
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                Console.WriteLine("Stack Trace: " + ex.StackTrace);
                return null;
            }
        }
        private string ComputeHmacSha256(string message, string secretKey)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);
            var messageBytes = Encoding.UTF8.GetBytes(message);

            byte[] hashBytes;

            using (var hmac = new HMACSHA256(keyBytes))
            {
                hashBytes = hmac.ComputeHash(messageBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
