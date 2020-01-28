using BusTicket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BusTicket.Controllers
{
    public class RoleController : ApiController
    {
        finalprojectdbEntities dalobj = new finalprojectdbEntities();
        // GET: api/Role
        public IEnumerable<T_Roles> Get()
        {
            return dalobj.T_Roles.ToList();
        } 

        // GET: api/Role/5
        public T_Roles Get(int id)
        {
            T_Roles u = dalobj.T_Roles.Find(id);
            return u;
        }

        // POST: api/Role
        public void Post([FromBody]T_Roles value)
        {
            dalobj.T_Roles.Add(value);
            dalobj.SaveChanges();
        }
        
        // PUT: api/Role/5
        public void Put(int id, [FromBody]T_Roles value)
        {
            T_Roles r = dalobj.T_Roles.Find(id);
            r.RoleName = value.RoleName;
            dalobj.SaveChanges();
        }

        // DELETE: api/Role/5
        public void Delete(int id)
        {
            T_Roles r = dalobj.T_Roles.Find(id);
            dalobj.T_Roles.Remove(r);
            dalobj.SaveChanges();
        }
    }
}
