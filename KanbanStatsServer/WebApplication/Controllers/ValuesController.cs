using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]dynamic value)
        {
            string rfidUid = value.rfidUid.Value;
            string hostName = value.hostName.Value;

            using (var context = new RFIDKanbanEntities())
            {
                //Check if RFID exists before creating a registration
                var registeredRfidTag = context.RFIDTag.Where(r => r.RFIDUID == rfidUid).FirstOrDefault();

                if(registeredRfidTag == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                //Register a new RFID tag
                //context.RFIDTag.Add(new RFIDTag { ID = Guid.NewGuid(), Name = "Drop ", RFIDUID = rfidUid, Type = "Drop" });
                //context.SaveChanges();
            }
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
