using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExotelSdk.Models
{
    public class ExotelCallResponse
    {
        public string Sid { get; set; }
        public string ParentCallSid { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string AccountSid { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public string PhoneNumberSid { get; set; }
        public string Status { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? Duration { get; set; }
        public double? Price { get; set; }
        public string Direction { get; set; }
        public string AnsweredBy { get; set; }
        public string ForwardedFrom { get; set; }
        public string CallerName { get; set; }
        public string Uri { get; set; }
        public string RecordingUrl { get; set; }
    }
}
