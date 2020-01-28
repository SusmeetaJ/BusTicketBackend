using BusTicket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BusTicket.Controllers
{
    public class UserController : ApiController
    {
        finalprojectdbEntities dalobj = new finalprojectdbEntities();
         
        UserController()
        {
            dalobj.Configuration.ProxyCreationEnabled = false;
        }


        
        // GET: api/User
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/User/5
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        [Route("api/Login")]
        public ResponseData Login([FromBody]T_Users u)
        {
            ResponseData res = new ResponseData();
            string enc = null;
            MySecurityLib.Security.Encrypt(u.Password,out enc);

            if(u.EmailId != null && u.Password != null)
            {
                var userList = dalobj.T_Users.ToList();
                T_Users validuser = (from user in userList
                                     where user.EmailId == u.EmailId && user.Password ==enc
                                     select user).SingleOrDefault();
                if(validuser != null)
                {
                    // res.Data = new { EmailId=validuser.EmailId, Password=validuser.Password, Role=validuser.T_Roles.RoleName };
                    res.Data = validuser;
                    res.Status = "Success";
                    res.Error = null;
                    return res;
                }
                else
                {
                    res.Error = null;
                    res.Status = "Incorrect Credentials";
                    return res;
                }
            } 
            else
            {
                res.Error = null;
                res.Status = "Username and Password fields are empty";
                return res;
            }  
        }
        // POST: api/User
       // [HttpPost]
        [Route("api/User")]
        public void Post([FromBody]T_Users value)
        {
            dalobj.T_Users.Add(value);
            dalobj.SaveChanges();
        }

        // PUT: api/User/5
        public void Put(int id, [FromBody]string value)
        {
        }

        
         // DELETE: api/User/5
        public void Delete(int id)
        {
        }
        [HttpPost]
        public ResponseData Register([FromBody]T_Users value)
        {
             
            ResponseData responce = new ResponseData();
            value.RoleId = 2;//by default User/Customer 
            var pass = value.Password;
            string encryptedpass=null;
            MySecurityLib.Security.Encrypt(pass,out encryptedpass);
            value.Password = encryptedpass;
            var result = dalobj.T_Users.Add(value);
            dalobj.SaveChanges();
            if (result != null)
            {
                responce.Data = result;
                responce.Error = null;
                responce.Status = "Success";
                return responce;
            }
            else
            {
                responce.Data = null;
                responce.Error = null;
                responce.Status = "Failded";
                return responce;
            }
        }


    }
}
