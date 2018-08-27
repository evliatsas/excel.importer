using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.Importer
{
    public class Position
    {
        /// <summary>
        /// The HAF Structure ID (HRM PK Reference) of the Position
        /// </summary>
        public int Hstr_Id { get; set; }
        public int ParentId { get; set; }
        /// <summary>
        /// The short Name of the Position
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The full Description of the Position
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// True if the Position is a Unit
        /// </summary>
        public bool IsUnit { get; set; }
        /// <summary>
        /// The HSTR Unit of the Position
        /// </summary>
        public Lookup<int> Unit { get; set; }
        /// <summary>
        /// The HSTR Level of the Position
        /// </summary>
        public Lookup<int> Level { get; set; }
        /// <summary>
        /// True if the Position is a Root Position
        /// </summary>
        public bool IsRoot { get; set; }
        /// <summary>
        /// The HSTR Root (HQ or Unit) of the Position
        /// </summary>
        public Lookup<int> Root { get; set; }
        /// <summary>
        /// Hierarchy Index of the Position
        /// </summary>
        public int OrderIndex { get; set; }
        /// <summary>
        /// True if the Position Has MIS Capabilities
        /// </summary>
        public bool HasMis { get; set; }


        public Position()
        {
        }
    }
}
