using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Training.Api.Entity;
using Training.Api.Model;

namespace Training.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly ILogger<EmployeesController> _logger;
        private readonly AppDbContext _appDbContext;

        public EmployeesController(ILogger<EmployeesController> logger, AppDbContext appDbContext)
        {
            _logger = logger;
            _appDbContext = appDbContext;
        }

        /// <summary>
        /// Retrieve all emplyee
        /// </summary>
        /// <returns></returns>
        [HttpGet("")]
        public IEnumerable<Employee> GetEmployees()
        {
            var user = User.Identity.Name;
            return _appDbContext.Employees.ToList();
        }

        [HttpGet("{employeeId}")]
        public Employee GetEmployee(long employeeId)
        {
            return _appDbContext.Employees.Where(x => x.EmployeeId == employeeId).FirstOrDefault();
        }

        /// <summary>
        /// Crete new employee
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("")]
        public IEnumerable<Employee> SaveEmployees([FromBody] EmployeeBindingModel model)
        {

            var passwords = new List<string> { "123", "abc", "xyz", "test" };
            var roles = new List<string> { "admin", "developer", "accountant", "manager" };
            Employee newEmployee = new Employee
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Password = passwords[new Random().Next(passwords.Count - 1)],
                Role = roles[new Random().Next(roles.Count - 1)],
                IsActive = true,
                CreatedBy = "Trainer",
                CreatedDate = DateTime.UtcNow
            };

            //  C++ way
            //Employee newEmployee1 = new Employee();
            //newEmployee1.FirstName = model.FirstName;
            //newEmployee1.LastName = model.LastName;
            //newEmployee1.IsActive = true;
            //newEmployee1.CreatedBy = "Trainer";
            //newEmployee1.CreatedDate = DateTime.UtcNow;



            _appDbContext.Employees.Add(newEmployee);
            _appDbContext.SaveChanges();

            return _appDbContext.Employees.ToList();
        }

        /// <summary>
        /// Modify employee
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{employeeId}")]
        public IEnumerable<Employee> UdpateEmployees(long employeeId, [FromBody] EmployeeBindingModel model)
        {
            var emp = _appDbContext.Employees.Where(x => x.EmployeeId == employeeId).FirstOrDefault();

            if (emp == null)
            {
                return _appDbContext.Employees.ToList();
            }

            emp.FirstName = model.FirstName;
            emp.LastName = model.LastName;
            _appDbContext.SaveChanges();

            return _appDbContext.Employees.ToList();
        }

        /// <summary>
        /// Delete employee
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpDelete("{employeeId}")]
        public IEnumerable<Employee> DeleteEmployee(long employeeId)
        {
            var emp = _appDbContext.Employees.Where(x => x.EmployeeId == employeeId).FirstOrDefault();

            if (emp == null)
            {
                return _appDbContext.Employees.ToList();
            }

            _appDbContext.Remove(emp);
            _appDbContext.SaveChanges();

            return _appDbContext.Employees.ToList();
        }
    }
}
