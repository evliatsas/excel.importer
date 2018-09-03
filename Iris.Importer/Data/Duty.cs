using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.Importer
{
    [BsonIgnoreExtraElements]
    public class Duty
    {
        /// <summary>
        /// The Id of the Duty in HRM
        /// </summary>
        public int HrmId { get; set; }
        /// <summary>
        /// Duty Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Indicate that the Duty is Included in Approval
        /// </summary>
        public bool IsPrimary { get; set; }

        public Duty() { }
        public Duty(Lookup<int> duty)
        {
            this.HrmId = duty.Id;
            this.Description = duty.Description;
        }

        public Duty(Lookup<int> duty, decimal isPrimary)
        {
            this.HrmId = duty.Id;
            this.Description = duty.Description;
            this.IsPrimary = (isPrimary == 1);
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
