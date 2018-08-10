using System;
using System.Collections.Generic;

namespace Iris.Importer
{
    public class Employee 
    {
        #region Properties

        /// <summary>
        /// First Name
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Last Name
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Fathers First Name
        /// </summary>
        public string FatherFirstName { get; set; }
        /// <summary>
        /// The Persons Gender
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// The Persons Mail and Phone Contact Info
        /// </summary>
        public virtual ContactInfo ContactInfo { get; set; }
        /// <summary>
        /// Photo of the HAF Employee
        /// </summary>
        public byte[] Photo { get; set; }
        /// <summary>
        /// Unique Identifier used for Reference with the HRM API
        /// </summary>
        public int EmployeeId { get; set; }
        /// <summary>
        /// Military Identification No
        /// </summary>
        public string SN { get; set; }
        /// <summary>
        /// The Id and Description of current Rank
        /// </summary>
        public Lookup<int> Rank { get; set; }
        /// <summary>
        /// The Id and Description of current Speciality
        /// </summary>
        public Lookup<int> Speciality { get; set; }
        public Lookup<int> OccupationType { get; set; }
        /// <summary>
        /// The Category of the Employee
        /// </summary>
        public EmployeeCategory Category { get; set; }
        /// <summary>
        /// The Personnel Active Duties
        /// </summary>
        public IEnumerable<AssignedDuty> Duties { get; set; }
        /// <summary>
        /// Full description of the HAF structure position of the entity
        /// </summary>
        public Lookup<int> Position { get; set; }
        /// <summary>
        /// Full description of the HAF structure unit of the entity
        /// </summary>
        public Lookup<int> Unit { get; set; }
        /// <summary>
        /// The Employees Title
        /// </summary>
        public string Title
        {
            get
            {
                return string.Format(
                    "{0} {1} ({2} {3}, {4})",
                    LastName,
                    FirstName,
                    EnumerationExtensions.GetDescription<EmployeeCategory>(Enum.GetName(typeof(EmployeeCategory), Category)),
                    Speciality == null ? "" : Speciality.Description,
                    Rank == null ? "" : Rank.Description);
            }
        }

        #endregion

        #region Constructor

        public Employee()
        {            
            Duties = new List<AssignedDuty>();
        }

        #endregion
    }

    public class EmployeeDistinctComparer : IEqualityComparer<Employee>
    {
        public bool Equals(Employee x, Employee y)
        {
            return x.EmployeeId == y.EmployeeId && x.Category == y.Category;
        }

        public int GetHashCode(Employee x)
        {
            return x.EmployeeId.GetHashCode() ^ x.Category.GetHashCode();
        }
    }
}
