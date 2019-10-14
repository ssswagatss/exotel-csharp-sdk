using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExotelSdk.Models
{
    public class ExotelSmsResponse
    {
        public string Sid { get; set; }
        public string AccountSid { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public DateTime? DateSent { get; set; }
        public string Body { get; set; }
        public string Direction { get; set; }
        public string Uri { get; set; }
        public string ApiVersion { get; set; }
        public double? Price { get; set; }
        public string Status { get; set; }
        public string DetailedStatusCode { get; set; }
        public string DetailedStatus { get; set; }
    }
}
