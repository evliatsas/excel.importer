using System.Collections.Generic;

namespace Iris.Importer
{
    public class AssignedDuty
    {
        /// <summary>
        /// The HRM Duty assigned to the person
        /// </summary>
        public Lookup<int> Duty { get; set; }
        /// <summary>
        /// The HRM position in reference to the duty
        /// </summary>
        public Lookup<int> Position { get; set; }
    }

    public class DutyDistinctComparer : IEqualityComparer<AssignedDuty>
    {
        public bool Equals(AssignedDuty x, AssignedDuty y)
        {
            return x.Duty.Id == y.Duty.Id && x.Position.Id == y.Position.Id;
        }

        public int GetHashCode(AssignedDuty x)
        {
            return x.Duty.Id.GetHashCode() ^ x.Position.Id.GetHashCode();
        }
    }
}
