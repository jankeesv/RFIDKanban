using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Model
{
    public class RfidRegistration
    {
        public string RfidUid { get; set; }
        public string StationId { get; set; }
        public DateTime registrationDateTime { get; set; }
    }
}