using ExotelSdk.Models;
using System;
using System.Net;

namespace ExotelSdk.Models
{
    public class ExotelResponse
    {
        public bool IsSuccess { get; set; }
        public ExotelRestException RestException { get; set; }
        public ExotelCallResponse Call { get; set; }
    }
}
