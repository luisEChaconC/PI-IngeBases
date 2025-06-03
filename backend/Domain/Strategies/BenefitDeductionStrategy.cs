using backend.Repositories;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace backend.Domain.Strategies
{
    public class BenefitDeductionStrategy : IDeductionCalculationStrategy
    {
        private readonly APIRepository _apiRepository;
        private readonly HttpClient _httpClient;

        public BenefitDeductionStrategy(APIRepository apiRepository, HttpClient httpClient)
        {
            _apiRepository = apiRepository;
            _httpClient = httpClient;
        }

        public decimal CalculateDeduction(decimal grossSalary, Benefit? benefit = null)
        {
            if (benefit == null) return 0;

            if(benefit.Name != "Joya")
            {
                switch (benefit.Type)
                {
                    case "FixedAmount":
                        return benefit.FixedAmount ?? 0;
                    case "FixedPercentage":
                        return Math.Round(grossSalary * ((benefit.FixedPercentage ?? 0) / 100m), 2);
                    case "API":
                        return CallBenefitApi(benefit).GetAwaiter().GetResult();
                    default:
                        return 0;
                }
            } else
            {
                return CallBenefitApi(benefit).GetAwaiter().GetResult();
            }
           
        }
       
       // SI O SÍ NECESITO EL ID DE EMPLEADO
       private async Task<decimal> CallBenefitApi(Benefit benefit)
        {
            // Obtener la API según el nombre 
            var apiName = benefit.Name == "Joya"
                ? "Life Insurance"
                : benefit.LinkAPI;
            if (string.IsNullOrEmpty(apiName)) return 0;

            

            var api = _apiRepository.GetAPIs().FirstOrDefault(a => a.Name == apiName);
            if (api == null) return 0;
            Console.WriteLine($"Token usado: {api.Token}");

            var parameters = _apiRepository.GetParametersByAPI(api.Id);

            var paramValues = new Dictionary<string, string>();

            foreach (var param in parameters)
            {
                var values = _apiRepository.GetParameterValues(param.Id)
                    .Where(v => v.EmployeeId == Guid.Parse("21311152-4752-4DD0-8565-895718FE0485")) // Cambiar luego
                    .FirstOrDefault();

                string value = values?.ValueType switch
                {
                    "String" => values.StringValue,
                    "Int" => values.IntValue?.ToString(),
                    "Date" => values.DateValue?.ToString("yyyy-MM-dd"),
                    _ => null
                };

                paramValues[param.Name] = value ?? "";
            }

            var url = api.URL;
            var queryParams = new List<string>();
            foreach (var param in parameters)
            {
  

                var value = paramValues[param.Name];
                Console.WriteLine($"param: {param.Name}, value: {value}");
                if (param.Name.ToLower() == "gender" && api.Name.ToLower() == "life insurance")
                {
                    queryParams.Add($"sex={Uri.EscapeDataString(value)}");
                }
                else
                {
                    queryParams.Add($"{param.Name}={Uri.EscapeDataString(value)}");
                }
            }
            if (queryParams.Count > 0)
                url += "?" + string.Join("&", queryParams);

            var request = new HttpRequestMessage(
                api.EndpointMethod == "POST" ? HttpMethod.Post : HttpMethod.Get,
                url
            );

            if (!string.IsNullOrEmpty(api.Token) && !string.IsNullOrEmpty(api.SecurityKeyName))
            {
                if (api.SecurityKeyName.Equals("Authorization", StringComparison.OrdinalIgnoreCase))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", api.Token);
                }
                else
                {
                    request.Headers.Add(api.SecurityKeyName, api.Token);
                }
            }
                        

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error {response.StatusCode}: {errorContent}");
                throw new HttpRequestException($"Response status code does not indicate success: {(int)response.StatusCode} ({response.ReasonPhrase})");
            }
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("monthlyCost", out var costProp))
                return costProp.GetDecimal();

            return 0;
        }

    }
}