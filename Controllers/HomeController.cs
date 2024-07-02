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
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeesDto employee)
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

        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id); // Ensure this calls the correct service method
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmployeesDto employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var success = await _employeeService.UpdateEmployeeAsync(employee); // Ensure this calls the correct service method
                if (success)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Unable to update employee.");
            }
            return View(employee);
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

