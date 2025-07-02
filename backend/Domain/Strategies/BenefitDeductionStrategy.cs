using backend.Repositories;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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

        public BenefitDeductionStrategy()
        {
        }

        public decimal CalculateDeduction(decimal grossSalary, string contractType, string gender, Benefit? benefit = null, Guid? employeeId = null, Guid? paymentDetailsId = null)
        {
            if (benefit == null) return 0;

            switch (benefit.Type)
            {
                case "FixedAmount":
                    return benefit.FixedAmount ?? 0;
                case "FixedPercentage":
                    return Math.Round(grossSalary * ((benefit.FixedPercentage ?? 0) / 100m), 2);
                case "API":
                    return CallBenefitApi(gender, benefit, employeeId, grossSalary).GetAwaiter().GetResult();
                default:
                    return 0;
            }
        
        }
       
        private async Task<decimal> CallBenefitApi(string gender, Benefit benefit, Guid? employeeId = null, decimal grossSalary = 0)
        {
            var api = _apiRepository.GetAPIs().FirstOrDefault(a => a.Name == benefit.Name);
            if (api == null) return 0;

            Console.WriteLine($"Token usado: {api.Token}");

            var parameters = _apiRepository.GetParametersByAPI(api.Id);
            var paramValues = BuildParameterValues(parameters, api, gender, employeeId, grossSalary);

            var request = BuildHttpRequest(api, parameters, paramValues);

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error {response.StatusCode}: {errorContent}");
                throw new HttpRequestException($"Response status code does not indicate success: {(int)response.StatusCode} ({response.ReasonPhrase})");
            }

            var json = await response.Content.ReadAsStringAsync();
            return ParseApiResponse(api, json);
        }

        private Dictionary<string, object> BuildParameterValues(IEnumerable<ApiParameterModel> parameters, ApiModel api, string gender, Guid? employeeId, decimal grossSalary)
        {
            var paramValues = new Dictionary<string, object>();
            foreach (var param in parameters)
            {
                object value = null;
                if (param.Type == "SystemDefined")
                {
                    if (param.Name.Equals("gender", StringComparison.OrdinalIgnoreCase) || param.Name.Equals("genero", StringComparison.OrdinalIgnoreCase))
                    {
                        if (api.Name == "Life Insurance")
                        {
                            value = (gender == "M") ? "male" : "female";
                            paramValues["Gender"] = value;
                            continue;
                        }
                        else
                        {
                            value = (gender == "M") ? "masculino" : "femenino";
                            paramValues["Genero"] = value;
                            continue;
                        }
                    }
                    else if (param.Name.Equals("salary", StringComparison.OrdinalIgnoreCase))
                    {
                        value = grossSalary;
                    }
                }
                else
                {
                    var values = _apiRepository.GetParameterValues(param.Id)
                        .FirstOrDefault(v => v.EmployeeId == employeeId);

                    value = values?.ValueType switch
                    {
                        "string" => values.StringValue,
                        "int" => values.IntValue,
                        "date" => values.DateValue?.ToString("yyyy-MM-dd"),
                        _ => null
                    };
                }
                paramValues[param.Name] = value ?? "";
            }
            return paramValues;
        }

        private HttpRequestMessage BuildHttpRequest(ApiModel api, IEnumerable<ApiParameterModel> parameters, Dictionary<string, object> paramValues)
        {
            var url = api.URL;
            var request = new HttpRequestMessage(
                api.EndpointMethod == "POST" ? HttpMethod.Post : HttpMethod.Get,
                url
            );

            // Token
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

            if (api.EndpointMethod == "POST")
            {
                var formattedParams = FormatPostParameters(api, paramValues);
                var jsonBody = JsonSerializer.Serialize(formattedParams);
                Console.WriteLine("JSON enviado en POST:");
                Console.WriteLine(jsonBody);
                request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            }
            else // GET
            {
                var queryParams = BuildGetQueryParams(api, parameters, paramValues);
                if (queryParams.Count > 0)
                    request.RequestUri = new Uri(url + "?" + string.Join("&", queryParams));
            }

            return request;
        }

        private Dictionary<string, object> FormatPostParameters(ApiModel api, Dictionary<string, object> paramValues)
        {
            if (api.Name == "Medicare")
            {
                var apiParamMapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    { "Dependents", "cantidadDependientes" },
                    { "Genero", "genero" },
                    { "Date of birth", "fechaNacimiento" }
                };

                var formattedParams = new Dictionary<string, object>();
                foreach (var kvp in paramValues)
                {
                    var apiParamName = apiParamMapping.ContainsKey(kvp.Key) ? apiParamMapping[kvp.Key] : kvp.Key;
                    formattedParams[apiParamName] = kvp.Value;
                }
                return formattedParams;
            }
            else if (api.Name == "Association")
            {
                Console.WriteLine("ParamValues para Association:");
                foreach (var kvp in paramValues)
                {
                    Console.WriteLine($"- {kvp.Key}: {kvp.Value}");
                }

                var associationName = paramValues.FirstOrDefault(kvp =>
                    kvp.Key.Equals("AssociationName", StringComparison.OrdinalIgnoreCase) ||
                    kvp.Key.Equals("Association name", StringComparison.OrdinalIgnoreCase)
                ).Value ?? "default";

                var employeeSalary = paramValues.FirstOrDefault(kvp =>
                    kvp.Key.Equals("Salary", StringComparison.OrdinalIgnoreCase)
                ).Value ?? 0;

                var formattedParams = new Dictionary<string, object>
                {
                    { "AssociationName", associationName },
                    { "EmployeeSalary", employeeSalary }
                };

                Console.WriteLine("JSON final para Association:");
                Console.WriteLine(JsonSerializer.Serialize(formattedParams));
                return formattedParams;
            }
            else
            {
                return paramValues;
            }
        }

        private List<string> BuildGetQueryParams(ApiModel api, IEnumerable<ApiParameterModel> parameters, Dictionary<string, object> paramValues)
        {
            var queryParams = new List<string>();
            foreach (var param in parameters)
            {
                var value = paramValues[param.Name]?.ToString();
                Console.WriteLine($"param: {param.Name}, value: {value}");

                if (param.Name.Equals("gender", StringComparison.OrdinalIgnoreCase) && api.Name.ToLower() == "life insurance")
                {
                    queryParams.Add($"sex={Uri.EscapeDataString(value)}");
                }
                else
                {
                    queryParams.Add($"{param.Name}={Uri.EscapeDataString(value)}");
                }
            }
            return queryParams;
        }

        private decimal ParseApiResponse(ApiModel api, string json)
        {
            var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (root.ValueKind == JsonValueKind.Object)
            {

                if (api.Name == "Association" && root.TryGetProperty("amountToCharge", out var amountProp))
                {
                    return amountProp.GetDecimal();
                }

                return root.TryGetProperty("monthlyCost", out var costProp) ? costProp.GetDecimal() : 0;
            }
            else if (root.ValueKind == JsonValueKind.Number)
            {
                return root.GetDecimal();
            }
            else
            {
                Console.WriteLine($"Tipo de respuesta inesperado: {root.ValueKind}");
                return 0;
            }
        }



    }
}