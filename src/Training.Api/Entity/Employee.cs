namespace Training.Api.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public class Employee
    {
        public long EmployeeId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }

        public bool? IsActive { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

    }
}
