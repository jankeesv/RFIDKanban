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

                //If tag is not registered throw an exception
                if(registeredRfidTag == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                var station = context.RFIDStations.Where(s => s.HostName == hostName).FirstOrDefault();

                //If station is not found throw an exception
                if (station == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                //Check if the combination of station and tag already is registered
                string participantName;
                string hostNameDoing = string.Format("{0}_doing", hostName);
                string hostNameDone = string.Format("{0}_done", hostName);

                var stationTagRegistrations = context.RFIDRegistrations.Where(s => (s.HostName == hostNameDoing || s.HostName == hostNameDone) && s.TagName == registeredRfidTag.TagName).ToList();
                if (stationTagRegistrations.Count == 0)
                {
                    //Registration combination doesn't exist so state is doing
                    participantName = station.ParticipantName + " (Doing)";
                    hostName = hostName + "_doing";
                }
                else if(stationTagRegistrations.Count == 1)
                {
                    //Registration combination already exist so state is done
                    participantName = station.ParticipantName + " (Done)";
                    hostName = hostName + "_done";
                }
                else
                {
                    //Registration combinations already exist for doing and done so third try is not possible
                    throw new HttpResponseException(HttpStatusCode.Unauthorized);
                }

                string exerciseName = "test";

                //Create a new registration based on the station name and rfid tag
                context.RFIDRegistrations.Add(new RFIDRegistrations { ID = Guid.NewGuid(), TagName = registeredRfidTag.TagName, TagType = registeredRfidTag.TagType, ParticipantName = participantName, HostName = hostName, RegistrationDateTime = DateTime.Now, ExerciseName = exerciseName });
                context.SaveChanges();

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
