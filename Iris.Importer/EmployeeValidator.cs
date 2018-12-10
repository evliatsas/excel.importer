using MongoDB.Driver;
using System.Linq;
using System.Text.RegularExpressions;

namespace Iris.Importer
{
    public class EmployeeValidator
    {
        private IMongoDatabase _db;
        public EmployeeValidator()
        {
            var cs = string.Format("mongodb://{0}:{1}@{2}", Properties.Settings.Default.username, Properties.Settings.Default.pwd, Properties.Settings.Default.server);
            var client = new MongoDB.Driver.MongoClient(cs);
            _db = client.GetDatabase("Common");
        }

        public ValidationMessage CheckLastName(string value)
        {
            if(string.IsNullOrEmpty(value))
            {
                return new ValidationMessage(ValidationType.Error, "No Last Name");
            }
            else
            {
                return new ValidationMessage();
            }
        }
        public ValidationMessage CheckFirstName(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return new ValidationMessage(ValidationType.Error, "No First Name");
            }
            else
            {
                return new ValidationMessage();
            }
        }
        public ValidationMessage CheckFatherFirstName(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return new ValidationMessage(ValidationType.Warning, "No Father First Name");
            }
            else
            {
                return new ValidationMessage();
            }
        }
        public ValidationMessage CheckGender(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return new ValidationMessage(ValidationType.Warning, "No Gender specified");
            }
            else
            {
                return new ValidationMessage();
            }
        }
        public ValidationMessage CheckAddress(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return new ValidationMessage(ValidationType.Warning, "No Address specified");
            }
            else
            {
                return new ValidationMessage();
            }
        }
        public ValidationMessage CheckCity(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return new ValidationMessage(ValidationType.Warning, "No City specified");
            }
            else
            {
                return new ValidationMessage();
            }
        }
        public ValidationMessage CheckTK(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return new ValidationMessage(ValidationType.Warning, "No T.K. specified");
            }
            else if(Regex.IsMatch(value, @"^[\d- ]*$"))
            {
                return new ValidationMessage();
            }
            else
            {
                return new ValidationMessage(ValidationType.Error, "Invalid T.K.");
            }
        }
        public ValidationMessage CheckPhone(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return new ValidationMessage(ValidationType.Warning, "No Phone specified");
            }
            else if (Regex.IsMatch(value, @"^[\d- ]*$"))
            {
                return new ValidationMessage();
            }
            else
            {
                return new ValidationMessage(ValidationType.Error, "Invalid Phone No");
            }
        }
        public ValidationMessage CheckMobilePhone(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return new ValidationMessage(ValidationType.Warning, "No Mobile Phone specified");
            }
            else if (Regex.IsMatch(value, @"^[\d- ]*$"))
            {
                return new ValidationMessage();
            }
            else
            {
                return new ValidationMessage(ValidationType.Error, "Invalid Mobile Phone No");
            }
        }
        public ValidationMessage CheckEmail(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return new ValidationMessage(ValidationType.Error, "No email specified");
            }

            try
            {
                var email = new System.Net.Mail.MailAddress(value);

                //check if email already assigned to another employee
                var collection = _db.GetCollection<Employee>("Employees");
                var filter = Builders<Employee>.Filter.Eq<string>("ContactInfo.EMail", email.Address);
                var query = collection.Find(filter);
                var exists = query.Any();
                if (exists)
                {
                    return new ValidationMessage(ValidationType.Error, "email address already exists");
                }
                else
                {
                    return new ValidationMessage();
                }
            }
            catch
            {
                return new ValidationMessage(ValidationType.Error, "Invalid email address");
            }
        }
        public ValidationMessage CheckCategory(string value, out EmployeeCategory category)
        {
            category = EmployeeCategory.Other;
            if (string.IsNullOrEmpty(value))
            {
                return new ValidationMessage(ValidationType.Warning, "No category specified, set to default");
            }
            else
            {
                switch (value)
                {
                    case "ΠΕ":
                        category = EmployeeCategory.PE;
                        break;
                    case "ΔΕ":
                        category = EmployeeCategory.DE;
                        break;
                    case "ΤΕ":
                        category = EmployeeCategory.TE;
                        break;
                    case "ΥΕ":
                        category = EmployeeCategory.YE;
                        break;
                    default:
                        return new ValidationMessage(ValidationType.Warning, "Invalid category, set to default");
                }

                return new ValidationMessage();
            }
        }
        public ValidationMessage CheckRank(string value, out Lookup<int> rank)
        {
            rank = new Lookup<int>() { Id = -1, Description = "" };
            if (string.IsNullOrEmpty(value))
            {
                return new ValidationMessage(ValidationType.Warning, "No rank specified, set to default");
            }
            else
            {
                switch (value)
                {
                    case "Α'":
                        rank = new Lookup<int>() { Id = 1, Description = value };
                        break;
                    case "Β'":
                        rank = new Lookup<int>() { Id = 2, Description = value };
                        break;
                    case "Γ'":
                        rank = new Lookup<int>() { Id = 3, Description = value };
                        break;
                    case "Δ'":
                        rank = new Lookup<int>() { Id = 4, Description = value };
                        break;
                    case "Ε'":
                        rank = new Lookup<int>() { Id = 5, Description = value };
                        break;
                    case "Α":
                        rank = new Lookup<int>() { Id = 1, Description = "Α'" };
                        break;
                    case "Β":
                        rank = new Lookup<int>() { Id = 2, Description = "Β'" };
                        break;
                    case "Γ":
                        rank = new Lookup<int>() { Id = 3, Description = "Γ'" };
                        break;
                    case "Δ":
                        rank = new Lookup<int>() { Id = 4, Description = "Δ'" };
                        break;
                    case "Ε":
                        rank = new Lookup<int>() { Id = 5, Description = "Ε'" };
                        break;
                    default:
                        return new ValidationMessage(ValidationType.Warning, "Invalid rank, set to default");
                }

                return new ValidationMessage();
            }
        }
        public ValidationMessage CheckSpeciality(string value, out Lookup<int> speciality)
        {
            speciality = new Lookup<int>() { Id = -1, Description = "" };
            if (string.IsNullOrEmpty(value))
            {
                return new ValidationMessage(ValidationType.Warning, "No speciality specified, set to default");
            }
            else
            {

                //check if speciality already exists
                var collection = _db.GetCollection<Speciality>("Specialities");
                var filter = Builders<Speciality>.Filter.Eq<string>("Description", value);
                var query = collection.Find(filter);
                var existing = query.FirstOrDefault();

                if (existing == null)
                    return new ValidationMessage(ValidationType.Warning, "Specified speciality not found, set to default");
                else
                {
                    speciality = existing.ToLookup();
                    return new ValidationMessage();
                }
            }
        }
        public ValidationMessage CheckOccupationType(string value, out Lookup<int> oType)
        {
            oType = new Lookup<int>() { Id = 1, Description = "ΜΟΝΙΜΟΣ" };
            if (string.IsNullOrEmpty(value))
            {
                return new ValidationMessage(ValidationType.Warning, "No occupation type specified, set to default");
            }
            else
            {
                switch (value)
                {
                    case "ΜΟΝΙΜΟΣ":
                        oType = new Lookup<int>() { Id = 1, Description = "ΜΟΝΙΜΟΣ" };
                        break;
                    case "ΜΟΝΙΜΗ":
                        oType = new Lookup<int>() { Id = 1, Description = "ΜΟΝΙΜΟΣ" };
                        break;
                    case "ΙΔΑΧ":
                        oType = new Lookup<int>() { Id = 2, Description = value };
                        break;
                    case "ΙΔΟΧ":
                        oType = new Lookup<int>() { Id = 3, Description = value };
                        break;
                    case "ΜΕ ΕΜΜΙΣΘΗ ΕΝΤΟΛΗ":
                        oType = new Lookup<int>() { Id = 4, Description = value };
                        break;
                    case "ΜΕΤΑΚΛΗΤΟΣ":
                        oType = new Lookup<int>() { Id = 5, Description = value };
                        break;
                    default:
                        return new ValidationMessage(ValidationType.Warning, "Invalid occupation type, set to default");
                }

                return new ValidationMessage();
            }
        }
        public ValidationMessage CheckPosition(string value, string header, out Lookup<int> position)
        {
            position = new Lookup<int>() { Id = -1, Description = "" };
            if (string.IsNullOrEmpty(value))
            {
                return new ValidationMessage(ValidationType.Error, "No position specified");
            }
            else
            {

                //check if speciality already exists
                var collection = _db.GetCollection<Position>("Positions");
                var filter = Builders<Position>.Filter.Eq<string>("Name", value);
                var query = collection.Find(filter);
                var existing = query.FirstOrDefault();

                if (existing == null)
                    return new ValidationMessage(ValidationType.Error, $"{header} not found");
                else
                {
                    position = new Lookup<int>() { Id = existing.Hstr_Id, Description = existing.Name };
                    return new ValidationMessage();
                }
            }
        }
        public ValidationMessage CheckDuty(string value, out Lookup<int> duty)
        {
            duty = new Lookup<int>() { Id = 11, Description = "Εισηγητής" };
            if (string.IsNullOrEmpty(value))
            {
                return new ValidationMessage(ValidationType.Warning, "No duty specified, set to default");
            }
            else
            {

                //check if speciality already exists
                var collection = _db.GetCollection<Duty>("Duties");
                var filter = Builders<Duty>.Filter.Eq<string>("Description", value);
                var query = collection.Find(filter);
                var existing = query.FirstOrDefault();

                if (existing == null)
                    return new ValidationMessage(ValidationType.Error, "Specified duty not found");
                else
                {
                    duty = existing.ToLookup();
                    return new ValidationMessage();
                }
            }
        }
    }
}
