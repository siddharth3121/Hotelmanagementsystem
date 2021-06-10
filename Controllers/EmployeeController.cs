using Hotelmanagementsystem.Models;
using System;
using Hotelmanagementsystem.App_Start;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hotelmanagementsystem.Controllers
{
    [SessionExpire]
    public class EmployeeController : Controller
    {
        EmployeeDB empDB = new EmployeeDB();
        string Con = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;
        [HttpGet]
        public ActionResult Addcustomer()
        {
            return View();
        }

        public JsonResult List()
        {
            return Json(empDB.ListAll(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Add(Adminmodel emp)
        {
            return Json(empDB.Add(emp), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetbyID(int ID)
        {
            var Employee = empDB.ListAll().Find(x => x.nid.Equals(ID));
            return Json(Employee, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Update(Adminmodel emp)
        {
            return Json(empDB.Update(emp), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete(int ID)
        {
            return Json(empDB.Delete(ID), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Roomandhall()
        {
            return View();
        }

        public JsonResult Getroomandhall(string roomandhall)
        {
            var lstdata = new List<Roomandhotelmodel>();
            string sQuery = "Select * from Roomandhall where vtype=@roomandhall and vstatus is null";
            SqlConnection sqlcon = new SqlConnection(Con);
            SqlCommand sqlcom = new SqlCommand(sQuery, sqlcon);
            sqlcon.Open();
            sqlcom.Parameters.AddWithValue("@roomandhall", roomandhall);
            var objreader = sqlcom.ExecuteReader();
            while (objreader.Read())
            {
                var objdata = new Roomandhotelmodel();
                objdata.checkroomandhall = objreader["vroomandhall"].ToString();
                lstdata.Add(objdata);
            }
            return Json(lstdata, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Insertroomdetails(Roomandhotelmodel objData)
        {
            DataTable dt = new DataTable();
            if (objData != null)
            {
                try
                {
                    string sQuery = "Select * from Customers where vcustomer = @customername and vmobile = @customermobilenum";
                    SqlConnection SqlConnection = new SqlConnection(Con);
                    SqlCommand SqlCommand = new SqlCommand(sQuery, SqlConnection);
                    SqlConnection.Open();
                    SqlCommand.Parameters.AddWithValue("@customername", objData.customername);
                    SqlCommand.Parameters.AddWithValue("@customermobilenum", objData.customermobilenum);
                    SqlDataAdapter Dataadaptor = new SqlDataAdapter(SqlCommand);

                    Dataadaptor.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        int n = InsertCustDetails(objData);
                    }
                    else
                    {
                        ViewBag.Error = "Customer not Added Please add him/her first.";
                        return RedirectToAction("Roomandhall", "Employee");
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return RedirectToAction("Roomandhall", "Employee");
        }

        public int InsertCustDetails(Roomandhotelmodel objdata)
        {
            int i = 0;
            using (SqlConnection SqlConnection = new SqlConnection(Con))
            {
                SqlConnection.Open();
                SqlCommand Sqlcom = new SqlCommand("Insertroomtakenbycustomer", SqlConnection);
                Sqlcom.CommandType = CommandType.StoredProcedure;
                Sqlcom.Parameters.AddWithValue("@customername", objdata.customername.ToUpper());
                Sqlcom.Parameters.AddWithValue("@customermobilenum", objdata.customermobilenum);
                Sqlcom.Parameters.AddWithValue("@Availableroomandhall", objdata.Availableroomandhall);
                Sqlcom.Parameters.AddWithValue("@checkindate", Convert.ToDateTime(objdata.checkindate));
                Sqlcom.Parameters.AddWithValue("@discount", objdata.discount);
                Sqlcom.Parameters.AddWithValue("@Finalamount", objdata.Finalamount);
                Sqlcom.Parameters.AddWithValue("@roomandhalltype", objdata.roomandhalltype);
                Sqlcom.Parameters.AddWithValue("@SGSTandCGST", objdata.SGSTandCGST);
                Sqlcom.Parameters.AddWithValue("@vstatus", "O");
                Sqlcom.Parameters.AddWithValue("@Total", objdata.Total);
                i = Sqlcom.ExecuteNonQuery();
            }
            return i;
        }

        public ActionResult Roomandhallcheckout()
        {
            return View();
        }

        public ActionResult GetDetails()
        {
            List<Roomandhotelmodel> lstData = new List<Roomandhotelmodel>();
            try
            {
                EmployeeDB objTable = new EmployeeDB();
                lstData = objTable.GetDetails();
            }
            catch (Exception)
            {

                throw;
            }
            return Json(new { data = lstData }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDetails1()
        {
            List<Menuitem> lstData = new List<Menuitem>();
            try
            {
                EmployeeDB objTable = new EmployeeDB();
                lstData = objTable.GetDetails1();
            }
            catch (Exception)
            {

                throw;
            }
            return Json(new { data = lstData }, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        public ActionResult Checkoutroom(string customermobilenum, string customername, string Availableroomandhall)
        {
            DataTable dt = new DataTable();

            string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);

            string sQuery = "Update Roomandhall Set checkouttime = @timestamp where vroomandhall=@Availableroomandhall " +
                            "and vcustomer =@customername and vcustomernumber=@customermobilenum";

            SqlConnection Sqlcon = new SqlConnection(Con);
            SqlCommand Sqlcom = new SqlCommand(sQuery, Sqlcon);
            Sqlcon.Open();
            Sqlcom.Parameters.AddWithValue("@timestamp", timestamp);
            Sqlcom.Parameters.AddWithValue("@Availableroomandhall", Availableroomandhall);
            Sqlcom.Parameters.AddWithValue("@customername", customername);
            Sqlcom.Parameters.AddWithValue("@customermobilenum", customermobilenum);
            int n = Sqlcom.ExecuteNonQuery();

            string sQuery1 = "Select * from Roomandhall where vroomandhall=@Availableroomandhall and " +
                             "vcustomer =@customername and vcustomernumber=@customermobilenum";
            SqlConnection Sqlcon1 = new SqlConnection(Con);
            SqlCommand Sqlcom1 = new SqlCommand(sQuery1, Sqlcon1);
            Sqlcon1.Open();
            Sqlcom1.Parameters.AddWithValue("@Availableroomandhall", Availableroomandhall);
            Sqlcom1.Parameters.AddWithValue("@customername", customername);
            Sqlcom1.Parameters.AddWithValue("@customermobilenum", customermobilenum);

            SqlDataAdapter dataAdapter = new SqlDataAdapter(Sqlcom1);
            dataAdapter.Fill(dt);

            string sQuery2 = "Insert into Temproomandhallrecord Values(@vroomandhall,@vtype,@ntotalamount,@vcustomer," +
                             "@vcustomernumber,@checkintime,@checkouttime,@ndiscount,@nfinalamount)";
            SqlConnection Sqlcon2 = new SqlConnection(Con);
            SqlCommand Sqlcom2 = new SqlCommand(sQuery2, Sqlcon2);
            Sqlcon2.Open();
            Sqlcom2.Parameters.AddWithValue("@vroomandhall", dt.Rows[0]["vroomandhall"].ToString());
            Sqlcom2.Parameters.AddWithValue("@vtype", dt.Rows[0]["vtype"].ToString());
            Sqlcom2.Parameters.AddWithValue("@ntotalamount", dt.Rows[0]["ntotalamount"].ToString());
            Sqlcom2.Parameters.AddWithValue("@vcustomer", dt.Rows[0]["vcustomer"].ToString());
            Sqlcom2.Parameters.AddWithValue("@vcustomernumber", dt.Rows[0]["vcustomernumber"].ToString());
            Sqlcom2.Parameters.AddWithValue("@checkintime", dt.Rows[0]["checkintime"].ToString());
            Sqlcom2.Parameters.AddWithValue("@checkouttime", dt.Rows[0]["checkouttime"].ToString());
            Sqlcom2.Parameters.AddWithValue("@ndiscount", dt.Rows[0]["ndiscount"].ToString());
            Sqlcom2.Parameters.AddWithValue("@nfinalamount", dt.Rows[0]["nfinalamount"].ToString());
            int n1 = Sqlcom2.ExecuteNonQuery();

            string sQuery3 = "Update Roomandhall set ntotalamount = NULL,vstatus = NULL,vcustomer= NULL,vcustomernumber=NULL,checkintime=NULL,checkouttime=NULL,ndiscount=NULL,nfinalamount=NULL " +
                            "where vroomandhall=@Availableroomandhall and vcustomer =@customername and vcustomernumber=@customermobilenum";
            SqlConnection Sqlcon3 = new SqlConnection(Con);
            SqlCommand Sqlcom3 = new SqlCommand(sQuery3, Sqlcon3);
            Sqlcon3.Open();

            Sqlcom3.Parameters.AddWithValue("@Availableroomandhall", Availableroomandhall);
            Sqlcom3.Parameters.AddWithValue("@customername", customername);
            Sqlcom3.Parameters.AddWithValue("@customermobilenum", customermobilenum);
            int n2 = Sqlcom3.ExecuteNonQuery();

            return RedirectToAction("Roomandhallcheckout", "Employee");
        }

        [HttpGet]
        public ActionResult Getroombookedbysearch()
        {
            return View();
        }

        //public ActionResult GetDetails1()
        //{
        //    List<Roomandhotelmodel> lstData = new List<Roomandhotelmodel>();
        //    try
        //    {
        //        EmployeeDB objTable = new EmployeeDB();
        //        lstData = objTable.GetDetails1();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    return Json(new { data = lstData }, JsonRequestBehavior.AllowGet);
        //}

        //    public DataTable Roombookedbetweendates()
        //    {
        //        DataTable dt = new DataTable();
        //        string startDate = DateTime.Now.ToString("YYYY-MM-DD");
        //        string endDate = DateTime.Now.ToString("YYYY-MM-DD");

        //        using (SqlConnection con = new SqlConnection(Con))
        //        {
        //            using (SqlCommand cmd = new SqlCommand("SELECT * FROM Temproomandhallrecord WHERE checkintime between @startDate and @endDate"))
        //            {                 
        //            cmd.Parameters.AddWithValue("@startDate", startDate);
        //            cmd.Parameters.AddWithValue("@endDate", endDate);
        //            cmd.CommandType = CommandType.Text;
        //            cmd.Connection = con;
        //            SqlDataAdapter Da = new SqlDataAdapter(cmd);                   
        //            Da.Fill(dt);
        //            return dt;
        //        }
        //    }
        //}

        public JsonResult Roombookedbetweendates(Roomandhotelmodel objdata1)
        {
            var lstdata = new List<Roomandhotelmodel>();
            string sQuery = "Select * from Temproomandhallrecord where checkintime >= @fromdate or checkintime <= @todate ";
            SqlConnection sqlcon = new SqlConnection(Con);
            SqlCommand sqlcom = new SqlCommand(sQuery, sqlcon);
            sqlcon.Open();
            sqlcom.Parameters.AddWithValue("@fromdate", objdata1.fromdate);
            sqlcom.Parameters.AddWithValue("@todate", objdata1.todate);
            var objReader = sqlcom.ExecuteReader();
            while (objReader.Read())
            {
                var objData = new Roomandhotelmodel();
                objData.customername = objReader["vcustomer"].ToString();
                objData.customermobilenum = objReader["vcustomernumber"].ToString();
                objData.roomandhalltype = objReader["vtype"].ToString();
                objData.Availableroomandhall = objReader["vroomandhall"].ToString();
                objData.checkindate = objReader["checkintime"].ToString();
                objData.checkoutdate = objReader["checkouttime"].ToString();
                objData.Finalamount = objReader["nfinalamount"].ToString();
                lstdata.Add(objData);
            }
            return Json(lstdata, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Menuitem()
        {
            return View();
        }

        public JsonResult Getfoodetails(string Foodtype, string food)
        {
            string sQuery = "";
            var lstdata = new List<Menuitem>();
            if (food != null)
            {
                sQuery = "Select * from Food_menu where Food = @Food";
            }
            else
            {
                sQuery = "Select * from Food_menu where Foodtype = @Foodtype";
            }
           
            SqlConnection sqlcon = new SqlConnection(Con);
            SqlCommand sqlcom = new SqlCommand(sQuery, sqlcon);
            sqlcon.Open();
            if (food != null)
            {
                sqlcom.Parameters.AddWithValue("@food", food);
                //sqlcom.Parameters.AddWithValue("@foodtype", objdata1.foodtype);
            }
            else
            {
                sqlcom.Parameters.AddWithValue("@Foodtype", Foodtype);
            }
                        
            var objreader = sqlcom.ExecuteReader();
            while (objreader.Read())
            {
                var objdata = new Menuitem();

                if (food != null)
                {
                  objdata.amount = objreader["Amount"].ToString();               
                }
                else
                {
                    objdata.availablefood = objreader["Food"].ToString();
                }                  
                lstdata.Add(objdata);
            }
            return Json(lstdata, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult InsertFoodorderdetails(Menuitem objdata1)
        {
            try
            {
                string sQuery = "Insert into Customer_Food Values(@customername,@customermob,@Foodtype,@food,@amount)";
                SqlConnection sqlcon = new SqlConnection(Con);
                SqlCommand sqlcom = new SqlCommand(sQuery, sqlcon);
                sqlcon.Open();
                sqlcom.Parameters.AddWithValue("@customername", objdata1.customername);
                sqlcom.Parameters.AddWithValue("@customermob", objdata1.customermob);
                sqlcom.Parameters.AddWithValue("@Foodtype", objdata1.foodtype);
                sqlcom.Parameters.AddWithValue("@food", objdata1.food);
                sqlcom.Parameters.AddWithValue("@amount", objdata1.amount);
                int n = sqlcom.ExecuteNonQuery();

                
                return RedirectToAction("Menuitem", "Employee");
            }
            catch (Exception ex)
            {

                throw;
            }

        }


        //public void DownloadApplication()
        //{
        //    try
        //    {
        //        HtmlToPdf obj = new HtmlToPdf();
        //        //List<string> lstUploadFile = new List<string>();
        //        //List<string> lstUploadFilePath = new List<string>();
        //        //string sTempPath = string.Format("~/Uploads/GDCE_Jr_Clerk/{0}/{0} - ", Session["REG_ID"].ToString());

        //        //ApplicationManager objDataManager = new ApplicationManager();
        //        //ApplicationModel objData = objDataManager.GetApplicationDetails(Session["REG_ID"].ToString());

        //        //string sTemp = string.Empty;
        //        //sTemp = "HSC Certificate (Original)";
        //        //lstUploadFilePath.Add(Server.MapPath(sTempPath + "HSCCertificate.pdf"));

        //        //if (System.IO.File.Exists(Server.MapPath(sTempPath + "HSCCertificate1.pdf")))
        //        //{
        //        //    sTemp += ", HSC Certificate (Translated)";
        //        //    lstUploadFilePath.Add(Server.MapPath(sTempPath + "HSCCertificate1.pdf"));
        //        //}


        //        //lstUploadFile.Add(sTemp);

        //        //sTemp = "Proof of Birth Certificate (Original)";
        //        //lstUploadFilePath.Add(Server.MapPath(sTempPath + "BirthCertificate.pdf"));

        //        //if (System.IO.File.Exists(Server.MapPath(sTempPath + "BirthCertificate1.pdf")))
        //        //{
        //        //    sTemp += ", Proof of Birth Certificate (Translated)";
        //        //    lstUploadFilePath.Add(Server.MapPath(sTempPath + "BirthCertificate1.pdf"));
        //        //}

        //        //lstUploadFile.Add(sTemp);

        //        //if (objData.Caste.ToUpper() != "UR")
        //        //{
        //        //    sTemp = "Caste Certificate (Original)";
        //        //    lstUploadFilePath.Add(Server.MapPath(sTempPath + "CasteCertificate.pdf"));

        //        //    if (System.IO.File.Exists(Server.MapPath(sTempPath + "CasteCertificate1.pdf")))
        //        //    {
        //        //        sTemp += ", Caste Certificate (Translated)";
        //        //        lstUploadFilePath.Add(Server.MapPath(sTempPath + "CasteCertificate1.pdf"));
        //        //    }

        //        //    lstUploadFile.Add(sTemp);
        //        //}


        //      string  Data += "<html><body>><hr class='style4' style='border-top: 1px dotted red; width: 96%;'>" +
        //              "<table style='width:100%'>" +
        //              "<div style='text-align: center;'><h3>Declaration </h3></div><table style='width:100%'>  " +
        //              "<tr><td width='100%'>I wish to appear for the above GDCE Junior Clerk quota selection in reference to RRC/CR's notification number RRC/CR/GDCE/JT(H)/01/2019 dated 07/10/2019.</td></tr>" +
        //              "<tr><td width='100%'>I certify that the information given in this online application and documents uploaded are correct and as per my Service Record.</td></tr>" +
        //              "<tr><td width='100%'>I am aware that in case any information is found false/incorrect in future, I will be held responsible for that and action under D&AR will be initiated against me.</td></tr>" +
        //              "<tr><td width='100%'>I have read the terms and conditions of the notification carefully.</td></tr>" +
        //              "<tr><td colspan='2'></td>" +
        //                   "<td rowspan='4' class='pull-right' width='250px'></td>   </tr>  </table>  ";

        //        Data += "<hr class='style4' style='border-top: 1px dotted red; width: 96%;'>" +
        //            "<div style='text-align: center;'><h3>Details of Certificates Uploaded </ h3></div>" +
        //            "<table>";

        //        //foreach (string item in lstUploadFile)
        //        //{
        //        //    Data += "<tr><td> - " + item + "</td></tr>";
        //        //}

        //        Data += "</body></html>";

        //        var testHtmlToPDF = new NReco.PdfGenerator.HtmlToPdfConverter();
        //        byte[] aryApplicationFormBinary = testHtmlToPDF.GeneratePdf(Data);
        //        List<byte[]> pdfByteContent = new List<byte[]>
        //        {
        //            aryApplicationFormBinary
        //        };
        //        foreach (string item in lstUploadFilePath)
        //        {
        //            byte[] bytes1 = System.IO.File.ReadAllBytes(item);
        //            pdfByteContent.Add(bytes1);
        //        }

        //        aryApplicationFormBinary = ConcatAndAddContentOfPDF(pdfByteContent);

        //        Response.Clear();
        //        Response.ContentType = "application/pdf";
        //        Response.AddHeader("Content-Disposition", "attachment;filename=" + "Test" + ".pdf");
        //        Response.OutputStream.Write(aryApplicationFormBinary, 0, aryApplicationFormBinary.Length);
        //        Response.Flush();
        //    }
        //    catch (Exception objEx)
        //    {
        //        Logger.Error("Exception", objEx);
        //    }
        //}

        //public static byte[] ConcatAndAddContentOfPDF(List<byte[]> pdfByteContent)
        //{

        //    using (var ms = new MemoryStream())
        //    {
        //        using (var doc = new Document())
        //        {
        //            using (var copy = new PdfSmartCopy(doc, ms))
        //            {
        //                doc.Open();

        //                //Loop through each byte array
        //                foreach (var p in pdfByteContent)
        //                {

        //                    //Create a PdfReader bound to that byte array
        //                    using (PdfReader reader = new PdfReader(p))
        //                    {
        //                        //Add the entire document instead of page-by-page
        //                        copy.AddDocument(reader);
        //                    }
        //                }

        //                doc.Close();
        //            }
        //        }

        //        //Return just before disposing
        //        return ms.ToArray();
        //    }
        //}


    }
}