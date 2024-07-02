using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Models;

namespace EmployeesWebApp.Views.Home
{
    public class IndexModel : PageModel
    {
        private readonly api.Data.StaffDB _context;

        public IndexModel(api.Data.StaffDB context)
        {
            _context = context;
        }

        public IList<Employee> Employee { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Employee = await _context.Employees.ToListAsync();
        }
    }
}
