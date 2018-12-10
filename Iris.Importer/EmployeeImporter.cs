using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Iris.Importer
{
    public class EmployeeImporter
    {
        private IMongoDatabase _db;

        public EmployeeImporter()
        {
            var cs = string.Format("mongodb://{0}:{1}@{2}", Properties.Settings.Default.username, Properties.Settings.Default.pwd, Properties.Settings.Default.server);
            var client = new MongoDB.Driver.MongoClient(cs);
            _db = client.GetDatabase("Common");
        }

        public void Import(IEnumerable<Employee> employees)
        {
            var positions = _db.GetCollection<Speciality>("Positions").Find(x => true).ToList();
            var specialities = _db.GetCollection<Speciality>("Specialities").Find(x => true).ToList();
            var duties = _db.GetCollection<Speciality>("Duties").Find(x => true).ToList();
            var siteId = Properties.Settings.Default.siteId;
                        
            SortDefinition<Employee> empMaxIdDef = new SortDefinitionBuilder<Employee>().Descending("EmployeeId");
            var maxEmpId = _db.GetCollection<Employee>("Employees").Find(x => true).Sort(empMaxIdDef).Limit(1).Single();
            var nextEmpId = maxEmpId.EmployeeId;

            var emps = new List<Employee>();
            var pwds = new Dictionary<int, string>();

            try
            {
                foreach (var emp in employees)
                {
                    emp.SiteId = siteId;
                    emp.EmployeeId = ++nextEmpId;

                    if (emps.Any(x => x.ContactInfo.EMail == emp.ContactInfo.EMail)) //add extra duty to existing employee
                    {
                        var existing = emps.First(x => x.ContactInfo.EMail == emp.ContactInfo.EMail);
                        var dList = existing.Duties.ToList();
                        dList.AddRange(emp.Duties);
                        existing.Duties = dList;
                        nextEmpId = emp.EmployeeId;
                    }
                    else
                    {
                        pwds.Add(emp.EmployeeId, emp.Tag.ToString());
                        emp.Tag = null;
                        emps.Add(emp);                        
                    }
                }

                _db.GetCollection<Employee>("Employees").InsertMany(emps);
                //duplicate email
                //var q1 = emps.GroupBy(g=>g.ContactInfo.EMail)
                //    .Where(g => g.Count() > 1)
                //.Select(y => y.Key)
                //.ToList();
                //non valid email
                //checkEmail(emps.Select(x => x.ContactInfo.EMail));

                foreach (var emp in emps)
                {
                    emp.Tag = pwds[emp.EmployeeId];
                    //await DDR.API.Proxies.Auth.CreateUser(emp);
                }

                Console.WriteLine(string.Format("Imported {0} users", emps.Count));
            }
            catch (Exception exc)
            {
                var q = exc;
            }
        }
    }
}
