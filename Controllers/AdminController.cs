using Hotelmanagementsystem.App_Start;
using Hotelmanagementsystem.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Hotelmanagementsystem.Controllers
{
    [SessionExpire]
    public class AdminController : Controller
    {
        AdminDB empDB = new AdminDB();
        string Con = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
        // GET: Admin

        [HttpGet]
        [DisableSession]       
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [DisableSession]
        public ActionResult Login(Adminmodel objData)
        {
            DataTable objDatatable = new DataTable();
            //if (objData.vregorloginflag.ToString() == "L")
            //{
            try
            {
                string sQuery = "Select * from Users where vusername= @vusername and vpassword=@vpassword";
                using (SqlConnection Sqlcon = new SqlConnection(Con))
                {
                    using (SqlCommand Sqlcom = new SqlCommand(sQuery, Sqlcon))
                    {
                        Sqlcom.Parameters.AddWithValue("@vusername", objData.vusername);
                        Sqlcom.Parameters.AddWithValue("@vpassword", objData.vpassword);
                        Sqlcon.Open();
                        SqlDataAdapter objDataAdapter = new SqlDataAdapter(Sqlcom);
                        objDataAdapter.Fill(objDatatable);
                        //int n = Convert.ToInt32(Sqlcom.ExecuteScalar());
                        //Sqlcon.Close();
                        if (objDatatable.Rows.Count <= 0 || objDatatable == null)
                        {
                            return RedirectToAction("Login", "Admin");
                        }

                        Session["nid"] = objDatatable.Rows[0]["nid"].ToString();
                        Session["vmobnumber"] = objDatatable.Rows[0]["vmobnumber"].ToString();

                        if (objDatatable.Rows.Count > 0)
                        {
                            if (objDatatable.Rows[0]["vusertype"].ToString() == "Admin")
                            {
                                return RedirectToAction("Registration", "Admin");
                            }
                            if (objDatatable.Rows[0]["vusertype"].ToString() == "Employee")
                            {
                                return RedirectToAction("Addcustomer", "Employee");
                            }
                            
                        }
                    }
                }
            }

            catch (Exception)
            {

                throw;
            }
            //}

            return View();
        }

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [DisableSession]
        public JsonResult List()
        {
            return Json(empDB.ListAll(), JsonRequestBehavior.AllowGet);
        }

        [DisableSession]
        public JsonResult Add(Adminmodel emp)
        {
            return Json(empDB.Add(emp), JsonRequestBehavior.AllowGet);
        }

        [DisableSession]
        public JsonResult GetbyID(int ID)
        {
            var Employee = empDB.ListAll().Find(x => x.nid.Equals(ID));
            return Json(Employee, JsonRequestBehavior.AllowGet);
        }

        [DisableSession]
        public JsonResult Update(Adminmodel emp)
        {
            return Json(empDB.Update(emp), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete(int ID)
        {
            return Json(empDB.Delete(ID), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View("ChangePassword");
        }

        [HttpPost]
        public ActionResult ChangePassword(Adminmodel objChangePasswordModel)
        {
            try
            {
                string nid = Session["nid"].ToString();
                string vmobnumber = Session["vmobnumber"].ToString();

                AdminDB objLoginManager = new AdminDB();
                bool bResult = objLoginManager.ChangePassword(objChangePasswordModel, nid, vmobnumber);

                if (bResult == true)
                {
                    FormsAuthentication.SignOut();
                    Session.Abandon();
                    return RedirectToAction("Login", "Admin");
                }

                ViewBag.Error = "Not able to udpate try again";
            }
            catch (Exception)
            {
                ViewBag.Error = "Failed to change password.";
            }
            return View("ChangePassword");
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Login", "Admin");
        }

    }
}