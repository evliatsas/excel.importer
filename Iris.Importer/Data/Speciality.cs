using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.Importer
{
    [BsonIgnoreExtraElements]
    public class Speciality
    {
        /// <summary>
        /// The Id of the Speciality in HRM
        /// </summary>
        public int HrmId { get; set; }
        /// <summary>
        /// Speciality Description
        /// </summary>
        public string Description { get; set; }

        public Speciality() { }
        public Speciality(Lookup<int> speciality)
        {
            this.HrmId = speciality.Id;
            this.Description = speciality.Description;
        }

        public Lookup<int> ToLookup()
        {
            var result = new Lookup<int>()
            {
                Id = this.HrmId,
                Description = this.Description
            };
            return result;
        }
    }
}
