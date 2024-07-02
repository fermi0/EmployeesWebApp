using api.Filters;
using api.DTO;
using static Global;

namespace EmployeesWebApp.Services
{
    public class EmployeeService
    {
        private readonly HttpClient _httpClient;

        public EmployeeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(api_url);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<IEnumerable<EmployeesDto>> GetEmployeesAsync(string name, int pagenumber, int pagesize)
        {
            var query = new EmployeeSearchQuery
            {
                Name = name,
                PageNumber = pagenumber,
                PageSize = pagesize
            };

            var queryString = $"?name={query.Name}&pagenumber={query.PageNumber}&pagesize={query.PageSize}";
            HttpResponseMessage response = await _httpClient.GetAsync($"api/employees{queryString}");
            response.EnsureSuccessStatusCode();
            IEnumerable<EmployeesDto>? employees = await response.Content.ReadFromJsonAsync<IEnumerable<EmployeesDto>>();
            return employees ?? [];
        }

        public async Task<EmployeesDto?> GetEmployeeByIdAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/employees/{id}");
            response.EnsureSuccessStatusCode();
            var employee = await response.Content.ReadFromJsonAsync<EmployeesDto>();
            return employee;
        }

        public async Task<bool> CreateEmployeeAsync(EmployeesDto employee)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/employees", employee);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateEmployeeAsync(EmployeesDto employee)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/employees/{employee.Id}", employee);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"api/employees/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<int> GetTotal()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("api/employees/all");
            response.EnsureSuccessStatusCode();
            IEnumerable<EmployeesDto>? employees = await response.Content.ReadFromJsonAsync<IEnumerable<EmployeesDto>>();
            return employees?.Count() ?? 00;
        }
    }
}
