using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySecurityLib;
namespace MyUnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            String result = Security.GetOTP("1", 5);
            Assert.AreNotEqual(string.Empty,result);
        }
        [TestMethod]
        public void TestMethod2()
        {
            string pwd = "sunbeam";
            String emcData = null;
            bool result = Security.Encrypt(pwd, out emcData);
            Assert.AreEqual(true, result);
        }
        [TestMethod]
        public void TestMethod3()
        {
            string pwd = "Qw + qozhUc64  ";
            String emcData = null;
            bool result = Security.Decrypt(pwd, out emcData);
            Assert.AreEqual(true, result);
        }
    }
}
