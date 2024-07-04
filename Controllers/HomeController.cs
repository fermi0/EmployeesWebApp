using static Global;
using EmployeesWebApp.Services;
using api.DTO;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly EmployeeService _employeeService;

        public HomeController(EmployeeService employeeService) // DI for service call that integrates API, which is manipulated here
        {
            _employeeService = employeeService;
        }

        public async Task<IActionResult> Index(string name, int pagenumber = 1, int pagesize = 5) // index have the parameters such as ?name=foo
        {
            var employees = await _employeeService.GetEmployeesAsync(name, pagenumber, pagesize); // calls service
            var total = await _employeeService.GetTotal(); // necessary for pagination to work

            if (employees == null || !employees.Any())
            {
                employees = []; // pass default values in the instance
            }

            // actual parameters that is implemented in Views
            ViewData["CurrentPage"] = pagenumber;
            ViewData["TotalPages"] = (int)Math.Ceiling((double)total / pagesize); // calculated by diving the number of items displayed from total
            ViewData["SearchTerm"] = name;
            ViewData["PageSize"] = pagesize;

            return View(employees);
        }

        // GET: Home/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // use of cookies
        public async Task<IActionResult> Create(CreateStaffDto employee)
        {
            if (ModelState.IsValid)
            {
                var success = await _employeeService.CreateEmployeeAsync(employee); // calls from service
                if (success)
                {
                    return RedirectToAction(nameof(Index)); // after creating go back to the home page
                }
                ModelState.AddModelError("", "Unable to create employee.");
            }
            return View(employee);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id) // request for the id of employee to edit as display
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            // Map the employee data to UpdateStaffDto
            var editDto = new UpdateStaffDto // creates the instance of UpdateStaffDto from API
            {
                Fname = employee.Fname,
                Lname = employee.Lname,
                Address = employee.Address,
                Contact = employee.Contact,
                Pay = employee.Pay,
                Company = employee.Company,
                Summary = employee.Summary
            };
            ViewData["EmployeeId"] = employee.Id; // calls GetEmployeeByIdAsync from service that use Employee model, Id is taken from there
            return View(editDto); // we dont need id for post since it's auto generated in Employee model
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateStaffDto editDto)
        {
            if (ModelState.IsValid)
            {
                var success = await _employeeService.UpdateEmployeeAsync(id, editDto); // calls service that calls Update() from API
                if (success)
                {
                    return RedirectToAction(nameof(Details), new {id}); // after updating request the id that was edited taken from Details()
                }
                ModelState.AddModelError("", "Unable to update employee.");
            }
            return View(editDto);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _employeeService.DeleteEmployeeAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }
    }
}

