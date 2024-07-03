using static Global;
using EmployeesWebApp.Services;
using api.DTO;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly EmployeeService _employeeService;

        public HomeController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public async Task<IActionResult> Index(string name, int pagenumber = 1, int pagesize = 5)
        {
            var employees = await _employeeService.GetEmployeesAsync(name, pagenumber, pagesize);
            var total = await _employeeService.GetTotal();

            if (employees == null || !employees.Any())
            {
                employees = [];
            }

            ViewData["CurrentPage"] = pagenumber;
            ViewData["TotalPages"] = (int)Math.Ceiling((double)total / pagesize);
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateStaffDto employee)
        {
            if (ModelState.IsValid)
            {
                var success = await _employeeService.CreateEmployeeAsync(employee); // Ensure this calls the correct service method
                if (success)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Unable to create employee.");
            }
            return View(employee);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            // Map the employee data to UpdateStaffDto
            var editDto = new UpdateStaffDto
            {
                Fname = employee.Fname,
                Lname = employee.Lname,
                Address = employee.Address,
                Contact = employee.Contact,
                Pay = employee.Pay,
                Company = employee.Company,
                Summary = employee.Summary
            };
            ViewData["EmployeeId"] = employee.Id;
            return View(editDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateStaffDto editDto)
        {
            if (ModelState.IsValid)
            {
                var success = await _employeeService.UpdateEmployeeAsync(id, editDto); // Ensure this calls the correct service method
                if (success)
                {
                    return RedirectToAction(nameof(Details), new {id});
                }
                ModelState.AddModelError("", "Unable to update employee.");
            }
            return View(editDto);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id); // Ensure this calls the correct service method
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
            var success = await _employeeService.DeleteEmployeeAsync(id); // Ensure this calls the correct service method
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id); // Ensure this calls the correct service method
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }
    }
}

