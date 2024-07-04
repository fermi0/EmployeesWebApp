// uses api references
using api.Filters;
using api.Models;
using api.DTO;
using static Global;

namespace EmployeesWebApp.Services
{
    public class EmployeeService
    {
        private readonly HttpClient _httpClient; // to interact with client

        public EmployeeService(HttpClient httpClient) // DI for api call
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(api_url); // Calls the API url from Global
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            // makes sure the request and response have json mediatype
        }

        // request GetEmployeeQ() from the API
        public async Task<IEnumerable<EmployeesDto>> GetEmployeesAsync(string name, int pagenumber, int pagesize)
        {
            var query = new EmployeeSearchQuery // creates an instance using EmployeeSearchQuery from Filters in API as a blueprint
            {
                Name = name,
                PageNumber = pagenumber,
                PageSize = pagesize
            };

            var queryString = $"?name={query.Name}&pagenumber={query.PageNumber}&pagesize={query.PageSize}"; // the instantiated parameters are applied here
            HttpResponseMessage response = await _httpClient.GetAsync($"api/employees{queryString}"); // GetAsync is a HttpClient method
            response.EnsureSuccessStatusCode(); // ensures the previous line succeed before executing next line
            var employees = await response.Content.ReadFromJsonAsync<IEnumerable<EmployeesDto>>(); // reads the list from json response
            return employees ?? []; // use of null-coalescing for default value in case of null
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int id) // taken from Employee model so salary is also displayed
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/employees/{id}");
            if (response.IsSuccessStatusCode)
            {
                var employee = await response.Content.ReadFromJsonAsync<Employee>(); // json data related to the id which is passed in API's GetId()
                return employee;
            }
            return null;
        }

        public async Task<bool> CreateEmployeeAsync(CreateStaffDto employee) // implementing good coding practices by separating DTO & models
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/employees", employee); // filled formed are stored as key:value in json
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateEmployeeAsync(int id, UpdateStaffDto editDto)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/employees/{id}", editDto); // Doesnt create but update the existing
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"api/employees/{id}");
            return response.IsSuccessStatusCode;
        }

        // this method is necessary for pagination to work. API needs to throw all items to count and divide by pagesize
        public async Task<int> GetTotal()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("api/employees/all"); // this calls GetAll() from API
            response.EnsureSuccessStatusCode();
            IEnumerable<EmployeesDto>? employees = await response.Content.ReadFromJsonAsync<IEnumerable<EmployeesDto>>();
            return employees?.Count() ?? 00; // only returns number
        }
    }
}
