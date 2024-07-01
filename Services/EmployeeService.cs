using api.Models;
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

        public async Task<IEnumerable<Employee>> GetEmployeesAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("api/employees");
            response.EnsureSuccessStatusCode();
            var employees = await response.Content.ReadFromJsonAsync<IEnumerable<Employee>>();
            return employees ?? [];
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/employees/{id}");
            response.EnsureSuccessStatusCode();
            var employee = await response.Content.ReadFromJsonAsync<Employee>();
            return employee;
        }

        public async Task<bool> CreateEmployeeAsync(Employee employee)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/employees", employee);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateEmployeeAsync(Employee employee)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/employees/{employee.Id}", employee);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"api/employees/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
