using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BusTicket.Models;
using System.Net.Mail;

namespace BusTicket.Controllers
{
    public class AccountController : ApiController
    {
        finalprojectdbEntities dalobj = new finalprojectdbEntities();
        // GET: api/Acount
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Acount/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Acount
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Acount/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Acount/5
        public void Delete(int id)
        {
        }

        [HttpPost]
        [Route("api/Account/OTP")]
        public ResponseData OTP([FromBody]dynamic otpDetails)
        {
            string email = otpDetails.EmailId.ToString();


            var userlist = dalobj.T_Users.ToList();
            var User = (from u in userlist
                        where u.EmailId == email
                        select u).SingleOrDefault();
            string o = otpDetails.OTP.ToString();

            var otpd = dalobj.T_OTP_Details.ToList();
            var vOTP = (from v in otpd
                        where v.UserId == User.UserId && v.OTP == o
                        select v).SingleOrDefault();

            if (vOTP != null)
            {
                ResponseData RC = new ResponseData();
                RC.Status = "success";
                RC.Error = null;
                RC.Data = vOTP;
                return RC;
            }
            else
            {
                ResponseData RC = new ResponseData();
                RC.Status = "fail";
                RC.Error = null;
                RC.Data = false;
                return RC;
            }
        }
        [HttpPost]
        [Route("api/Account/IsExist")]
        public ResponseData IsExist([FromBody]T_Users value)
        {
            Email e = new Email();
            var userlist = dalobj.T_Users.ToList();
            var User = (from u in userlist
                        where u.EmailId == value.EmailId
                        select u).SingleOrDefault();
            if (User != null)
            {
                ResponseData RC = new ResponseData();
                string mails = GetOTP();

                T_OTP_Details otp = new T_OTP_Details();
                otp.UserId = User.UserId;
                otp.ValidTill = DateTime.Now;
                otp.GeneratedOn = DateTime.Now;
                otp.OTP = mails;
                dalobj.T_OTP_Details.Add(otp);
                dalobj.SaveChanges();
                e.Body = "OTP is" + mails;
                e.To = User.EmailId;
                e.Subject = "OTP :";
                SendEmail(e);
                RC.Status = "success";
                RC.Error = null;
                RC.Data = mails;
                return RC;
            }
            else
            {
                ResponseData RC = new ResponseData();
                RC.Status = "fail";
                RC.Error = null;
                RC.Data = false;
                return RC;
            }

        }


        [HttpPut]
        [Route("api/Account/UpdatePassword")]
        public ResponseData updatepassword([FromBody]T_Users value)
        {

            var userlist = dalobj.T_Users.ToList();
            var User = (from u in userlist
                        where u.EmailId == value.EmailId
                        select u).SingleOrDefault();

            if (User != null)
            {
                User.Password = value.Password;
                dalobj.SaveChanges();
                ResponseData rc = new ResponseData();
                rc.Status = "success";
                rc.Error = null;
                rc.Data = User;
                return rc;
            }
            else
            {
                ResponseData rc = new ResponseData();
                rc.Status = "fail";
                rc.Error = null;
                rc.Data = null;
                return rc;
            }
        }
        private string GetOTP(string otpType = "1", int len = 5)
        {
            //otptype 1 = alpha numeric
            string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
            string numbers = "1234567890";

            string characters = numbers;
            if (otpType == "1")
            {
                characters += alphabets + small_alphabets + numbers;
            }
            int length = 5;
            string otp = string.Empty;
            for (int i = 0; i < length; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (otp.IndexOf(character) != -1);
                otp += character;
            }
            return otp;
        }
        protected ResponseData SendEmail([FromBody]Email e)
        {
            ResponseData res = new ResponseData();
            string to = e.To;
            string body = e.Body;
            string subject = e.Subject;
            string result = "Message Sent Successfully..!!";
            string senderID = "sush.3996@gmail.com";// use sender’s email id here..
            const string senderPassword = "friendfoe"; // sender password here…
            try
            {
                SmtpClient smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com", // smtp server address here…
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new System.Net.NetworkCredential(senderID, senderPassword),
                    Timeout = 30000,
                };
                MailMessage message = new MailMessage(senderID, to, subject, body);
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                result = "Error sending email.!!!";
                res.Error = ex;
            }
            return res;
        }
    }
}
