namespace Training.Api.Entity
{
    /// <summary>
    /// 
    /// </summary> <summary>
    /// 
    /// </summary>
    public class Employee
    {
        public long EmployeeId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool? IsActive { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

    }
}
