using BusTicket.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BusTicket.Controllers
{
    public class BusBookingController : ApiController
    {
        finalprojectdbEntities dalobj = new finalprojectdbEntities();
        ResponseData res = new ResponseData();
        [Route("api/BusBooking/CityDetails")]
        public ResponseData Get()
        {
            List<T_CityDetail> cdetails = dalobj.T_CityDetail.ToList();
            res.Data = cdetails;
            return res;
        }

        [Route("api/BusBooking/GetBuses")]
        [HttpPost]
       
        public ResponseData GetBuses([FromBody]T_RouteDetail v)
        {
            List<T_BusDetails> buslist = dalobj.T_BusDetails.ToList();
            List<T_JourneyDetails> journeylist = dalobj.T_JourneyDetails.ToList();
            List<T_RouteDetail> routelist = dalobj.T_RouteDetail.ToList();
            List<T_AvlSeats> avllist = dalobj.T_AvlSeats.ToList();
            //Int32 i = 1; 
            var innerJoinQuery =
                                from bus in buslist
                                join journy in journeylist on bus.BusId equals journy.BusId
                                join route in routelist on journy.RouteId equals route.RouteId
                                join avl in avllist on bus.BusId equals avl.BusId 
                                where route.FromId == v.FromId && route.ToId == v.ToId && avl.Available > 0
                                select new { bus.BusCapacity, bus.BusType, journy.JourneyFare,route.RouteId ,route.ToId, route.FromId,
                                    bus.BusId,journy.JourneyId,journy.ArrivalTime,journy.DepartureTime,bus.BusRegNo,avl.Available};
            // dalobj.Database.SqlQuery( )
            //int query1 = from r1 in routelist

            //             where r1.ToId.Equals("Karad") & r1.FromId.Equals("Pune")
            //             select r1.RouteId;
            //var query2 =
            //    from b in buslist
            //    join j in journeylist on b.BusId equals j.BusId
            //    join r in routelist on j.RouteId equals r.RouteId
            //    where r.RouteId =query1[0].RouteI
            //    select new { b.BusType, b.BusCapacity, j.JourneyFare, j.DepartureTime, j.ArrivalTime, r.RouteDistance, r.FromId, r.ToId };
            res.Data = innerJoinQuery;
            if (res.Data != null)
                res.Status="Success";
            return res;
        }


        // POST: api/BusBooking
        [HttpPost]
        public ResponseData getdate([FromBody]Dateclass d)
        {
            //string val = "01-23-2020";
            string pattern = "MM-dd-yyyy";
            DateTime parsedDate;
            DateTime.TryParseExact(d.getdate, pattern, null,
                                   DateTimeStyles.None, out parsedDate);
            //DateTime d = DateTime.Parse(value.ToString());

            DayOfWeek day = parsedDate.DayOfWeek;
            //Console.WriteLine(day);
            res.Data = day;
            return res;
        }
        //[HttpPost]
        //[Route("api/bus/CheckAvailable")]
        //public ResponseData CheckAvailable([FromBody] T_AvlSeats u)
        //{
        //    List<T_AvlSeats> avllist = dalobj.T_AvlSeats.ToList();
        //    var query = from a in avllist
        //                where a.BusId == u.BusId && a.Available > u.Available
        //                select a.Available;
        //    if(query!=null)
        //    {
        //        res.Status = "Available";

        //    }   
        //    else
        //    {
        //        res.Status = "Not Available";
        //    }
        //    return res;       
        //}
        //[HttpPut]
        //[Route("api/BusBooking/Putpayment/id")]
        public ResponseData Put(int id,[FromBody] T_ReservationDetails r)
        {
           // DateTime thisDay = 
            r.ReserveDate = DateTime.Today;
            var result = dalobj.T_ReservationDetails.Add(r);
            dalobj.SaveChanges();
            if(result!=null)
            {
                res.Status = "Success";
            }
            else
            {
                res.Status = "Failed";
            }
            //List<T_JourneyDetails> jlist = dalobj.T_JourneyDetails.ToList();
            //var query = from j in jlist
            //            where j.JourneyId == r.JourneyId
            //            select j;
            SqlConnection con = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=finalprojectdb;Integrated Security=True;Pooling=False;MultipleActiveResultSets=True;Application Name=EntityFramework");
            con.Open();
            SqlCommand cmd = new SqlCommand("UpdateAvailable",con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@param2",id));
            cmd.Parameters.Add(new SqlParameter("@param1", r.NoOfSeat));
            cmd.ExecuteReader();
            return res;
        }
        //// PUT: api/BusBooking/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        // DELETE: api/BusBooking/5
        public void Delete(int id)
        {
        }
    }
}
