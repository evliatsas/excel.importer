namespace Iris.Importer
{
    public class ContactInfo
    {
        /// <summary>
        /// The Mailing Adress
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// The City Name
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// Post Office Region Code (TK or ZipCode)
        /// </summary>
        public string TK { get; set; }
        /// <summary>
        /// Landline Phone Number
        /// </summary>
        public string PhoneNo { get; set; }
        /// <summary>
        /// Mobile Phone Number
        /// </summary>
        public string MobilePhoneNo { get; set; }
        /// <summary>
        /// The eMail contact Address
        /// </summary>
        public string EMail { get; set; }
    }
}
