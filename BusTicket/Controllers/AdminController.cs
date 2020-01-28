using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BusTicket.Models;
namespace BusTicket.Controllers
{
    public class AdminController : ApiController
    {
        finalprojectdbEntities dalobj = new finalprojectdbEntities();
        ResponseData res = new ResponseData();
        // GET: api/Admin
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        //[HttpGet]
       // [Route("api/AdminController/")]

        public ResponseData Get(int id)
        {
            var result = dalobj.T_RouteDetail.Find(id);
             if(result!=null)
            {
                res.Data = result;
                res.Status = "Found";
            }
             else
            {
                res.Data = null;
                res.Status = "Not Found";
            }
            return res;
        }
        [Route("api/AdminController/GetRoutes")]
        [HttpGet]
        public ResponseData GetRoutes()
        {
            var result = dalobj.T_RouteDetail.ToList();
            if (result != null)
                res.Data = result;
            return res;
        }
      
        [Route("api/AdminController/AddRoute")]
        [HttpPost]
        public ResponseData AddRoute([FromBody] T_RouteDetail route)
        {
            var result = dalobj.T_RouteDetail.Add(route);
            dalobj.SaveChanges();
            if(result!=null)
            {
                res.Status = "Success";
                res.Data = null;
            }
            
            return res;
        }

        // PUT: api/Admin/5
        public ResponseData Put(int id, [FromBody]T_RouteDetail r)
        {
            var routetobeupdated = dalobj.T_RouteDetail.Find(id);
            routetobeupdated.FromId = r.FromId;
            routetobeupdated.ToId = r.ToId;
            routetobeupdated.RouteDistance = r.RouteDistance;
            dalobj.SaveChanges();
            if (routetobeupdated != null)
            {
                res.Status = "Updated";
            }
            else
                res.Status = "Some error";
            return res;
        }

        // DELETE: api/Admin/5
        public void Delete(int id)
        {
        }
        [HttpPost]
        [Route("api/Admin/RegisterAdmin")]
        public ResponseData RegisterAdmin([FromBody] T_Users a) 
        {
            a.RoleId = 1;
            var pass = a.Password;
            string encryptedpass = null;
            MySecurityLib.Security.Encrypt(pass, out encryptedpass);
            a.Password = encryptedpass;
            var result = dalobj.T_Users.Add(a);
            dalobj.SaveChanges();
            if(result!=null)
            {
                res.Status = "Success";
                res.Error = null;
                res.Data = null;
            }
            else
            {
                res.Status = "Failed";
                res.Error = null;
                res.Data = null;
            }
            return res;
        }


        [HttpGet]
        [Route("api/AdminController/GetReservDetails")]
        public ResponseData GetReservDetails()
        {
            ResponseData res = new ResponseData();
            List<T_BusDetails> buslist = dalobj.T_BusDetails.ToList();
            List<T_JourneyDetails> journeylist = dalobj.T_JourneyDetails.ToList();
            List<T_RouteDetail> routelist = dalobj.T_RouteDetail.ToList();
            List<T_ReservationDetails> reservlist = dalobj.T_ReservationDetails.ToList();
            List<T_Users> userlist = dalobj.T_Users.ToList();

            var innerquery = from reserv in reservlist
                             join user in userlist on reserv.UserId equals user.UserId
                             join journey in journeylist on reserv.JourneyId equals journey.JourneyId
                             join route in routelist on journey.RouteId equals route.RouteId
                             join bus in buslist on journey.BusId equals bus.BusId
                             select new
                             {
                                 bus.BusId,
                                 journey.JourneyId,
                                 route.FromId,
                                 route.ToId,
                                 journey.DepartureTime,
                                 journey.ArrivalTime,
                                 journey.JourneyFare,
                                 bus.BusType,
                                 reserv.NoOfSeat,
                                 reserv.ReserveDate,
                                   route.RouteId,
                                   user.Name
                             };

            res.Data = innerquery;

            if (res.Data != null)
            {
                res.Status = "Success";

            }
            else
            {
                res.Status = "failed";
            }
            return res;

        }
        [Route("api/AdminController/AddBus")]
        [HttpPost]
        public ResponseData AddBus([FromBody] T_BusDetails busdata)
        {
            var result = dalobj.T_BusDetails.Add(busdata);
            dalobj.SaveChanges();
            if (result != null)
            {
                res.Status = "success";
                res.Data = null;
            }
            return res;

        }
        [Route("api/AdminController/GetBuses")]
        [HttpGet]
        public ResponseData GetBuses()
        {
            var result = dalobj.T_BusDetails.ToList();
            if (result != null)
                res.Data = result;
            return res;
        }


        [HttpGet]
        [Route("api/AdminController/GetBusById/{id}")]
        public ResponseData GetBusById(int id)
        {
            var result = dalobj.T_BusDetails.Find(id);

            if (result != null)
            {
                res.Data = result;
                res.Status = "success";
            }
            else
            {
                res.Data = null;
                res.Status = "Failed";
            }
            return res;

        }

        [HttpPut]
        [Route("api/AdminController/UpdateBus/{id}")]
        public ResponseData UpdateBus(int id, [FromBody]T_BusDetails b)
        {
            var Bustobeupdated = dalobj.T_BusDetails.Find(id);
            Bustobeupdated.BusId = b.BusId;
            Bustobeupdated.BusRegNo = b.BusRegNo;
            Bustobeupdated.BusType = b.BusType;
            Bustobeupdated.BusCapacity = b.BusCapacity;
            dalobj.SaveChanges();
            if (Bustobeupdated != null)
            {
                res.Status = "success";
            }
            else
            {
                res.Status = "Failed";
            }
            return res;
        }

        [HttpGet]
        [Route("api/AdminController/GetJourneyDetails")]
        public ResponseData GetJourneyDetails()
        {
            ResponseData res = new ResponseData();
            List<T_AvlSeats> Availlist = dalobj.T_AvlSeats.ToList();
            List<T_BusDetails> buslist = dalobj.T_BusDetails.ToList();
            List<T_JourneyDetails> journeylist = dalobj.T_JourneyDetails.ToList();
            List<T_RouteDetail> routelist = dalobj.T_RouteDetail.ToList();

            var innerJoinQuery = from journey in journeylist
                                 join bus in buslist on journey.BusId equals bus.BusId
                                 join route in routelist on journey.RouteId equals route.RouteId
                                 join avl in Availlist on bus.BusId equals avl.BusId
                                 select new
                                 {
                                     journey.BusId,
                                     journey.JourneyId,
                                     route.FromId,
                                     route.ToId,
                                     bus.BusCapacity,
                                     avl.Available,
                                     journey.DepartureTime,
                                     journey.ArrivalTime,
                                     journey.JourneyFare,
                                     bus.BusType,
                                     route.RouteId
                                 };



            res.Data = innerJoinQuery;

            if (res.Data != null)
            {
                res.Status = "success";

            }
            else
            {
                res.Status = "failed";
            }
            return res;
        }

    }
}
