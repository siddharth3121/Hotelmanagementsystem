using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Hotelmanagementsystem.Models
{
    public class EmployeeDB
    {
        //declare connection string  
        string cs = ConfigurationManager.ConnectionStrings["Dbconnection"].ConnectionString;

        //Return list of all Employees  
        public List<Adminmodel> ListAll()
        {
            List<Adminmodel> lst = new List<Adminmodel>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("SelectCustomers", con);
                com.CommandType = CommandType.StoredProcedure;
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    var objData = new Adminmodel();
                    objData.nid = Convert.ToInt32(rdr["nid"]);
                    objData.vusername =  rdr["vcustomer"].ToString();
                    objData.vgender = rdr["vgender"].ToString();
                    objData.vnationality = rdr["vnationality"].ToString();
                    //if (rdr["vgender"].ToString() != "M")
                    //{
                    //    objData.vgender = "Male";
                    //}
                    //else
                    //{
                    //    objData.vgender = "Female";
                    //}
                    objData.vmobnumber = rdr["vmobile"].ToString();
                    objData.vadharcard =  rdr["vidproof"].ToString();
                    //objData.vpassport = rdr["vpassport"].ToString();

                    if (rdr["vpassport"].ToString() != " " || rdr["vpassport"].ToString() != "NULL")
                    {
                        objData.vpassport = rdr["vpassport"].ToString();
                    }
                    else
                    {
                        objData.vpassport = "--";
                    }



                    //if (rdr["vnationality"].ToString() != "O")
                    //{
                    //    objData.vnationality = "Others";
                    //}
                    //else
                    //{
                    //    objData.vnationality = "Indian";
                    //}

                    lst.Add(objData);
                }
                return lst;
            }
        }

        //Method for Adding an Employee  
        public int Add(Adminmodel emp)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("InsertUpdateCustomers", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@nid", 1);
                com.Parameters.AddWithValue("@vusername", emp.vusername.ToUpper());
                com.Parameters.AddWithValue("@vgender", emp.vgender);
                com.Parameters.AddWithValue("@vnationality", emp.vnationality);
                com.Parameters.AddWithValue("@vpassport", emp.vpassport);
                com.Parameters.AddWithValue("@vmobnumber", emp.vmobnumber);
                com.Parameters.AddWithValue("@vadharcard", emp.vadharcard);
                com.Parameters.AddWithValue("@Action", "Insert");
                i = com.ExecuteNonQuery();
            }
            return i;
        }

        //Method for Updating Employee record  
        public int Update(Adminmodel emp)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("InsertUpdateUsers", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@nid", emp.nid);
                com.Parameters.AddWithValue("@vusername", emp.vusername.ToUpper());
                com.Parameters.AddWithValue("@vusertype", emp.vusertype);
                com.Parameters.AddWithValue("@vpassword", emp.vpassword);
                com.Parameters.AddWithValue("@vmobnumber", emp.vmobnumber);
                com.Parameters.AddWithValue("@Action", "Update");
                i = com.ExecuteNonQuery();
            }
            return i;
        }

        //Method for Deleting an Employee  
        public int Delete(int ID)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("DeleteUsers", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@nid", ID);
                i = com.ExecuteNonQuery();
            }
            return i;
        }

        public bool ChangePassword(Adminmodel objChangePasswordModel)
        {
            try
            {
                string sQuery = "Update Users " +
                                "SET  vpassword = @NewPassword   " +
                                "WHERE   nid = @UserID;";

                using (SqlConnection objSqlConnection = new SqlConnection(cs))
                {
                    using (SqlCommand objSqlCommand = new SqlCommand(sQuery, objSqlConnection))
                    {
                        objSqlCommand.Parameters.AddWithValue("@UserID", objChangePasswordModel.nid);
                        objSqlCommand.Parameters.AddWithValue("@OldPassword", objChangePasswordModel.vpassword);
                        objSqlCommand.Parameters.AddWithValue("@NewPassword", objChangePasswordModel.vnewConfirmPassword);
                        //objSqlCommand.Parameters.AddWithValue("@ENPassword", Encrypto.Encrypt(objChangePasswordModel.NewPassword));

                        objSqlConnection.Open();
                        int n = objSqlCommand.ExecuteNonQuery();

                        if (n >= 1)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        public List<Roomandhotelmodel> GetDetails()
        {
            var lstData = new List<Roomandhotelmodel>();

            string sQuery = "SELECT * FROM Roomandhall Where vstatus = 'O'";

            using (SqlConnection objSqlConnection = new SqlConnection(cs))
            {
                using (SqlCommand objSqlCommand = new SqlCommand(sQuery, objSqlConnection))
                {
                    objSqlConnection.Open();
                    var objReader = objSqlCommand.ExecuteReader();

                    try
                    {
                        while (objReader.Read())
                        {
                            var objData = new Roomandhotelmodel();
                            objData.customername = objReader["vcustomer"].ToString();
                            objData.customermobilenum = objReader["vcustomernumber"].ToString();
                            objData.roomandhalltype = objReader["vtype"].ToString();
                            objData.Availableroomandhall = objReader["vroomandhall"].ToString();
                            objData.Finalamount = objReader["nfinalamount"].ToString();
                            objData.checkindate = objReader["checkintime"].ToString();
                            lstData.Add(objData);
                        }
                    }
                    finally
                    {
                        objReader.Close();
                    }
                }
            }

            return lstData;
        }

        public List<Menuitem> GetDetails1()
        {
            var lstData = new List<Menuitem>();

            string sQuery = "SELECT * FROM Customer_Food";

            using (SqlConnection objSqlConnection = new SqlConnection(cs))
            {
                using (SqlCommand objSqlCommand = new SqlCommand(sQuery, objSqlConnection))
                {
                    objSqlConnection.Open();
                    var objReader = objSqlCommand.ExecuteReader();

                    try
                    {
                        while (objReader.Read())
                        {
                            var objData = new Menuitem();
                            objData.customername = objReader["Customername"].ToString();
                            objData.customermob = objReader["Customermob"].ToString();
                            objData.foodtype = objReader["Foodtype"].ToString();
                            objData.food = objReader["Food"].ToString();
                            objData.amount = objReader["Amount"].ToString();                            
                            lstData.Add(objData);
                        }
                    }
                    finally
                    {
                        objReader.Close();
                    }
                }
            }

            return lstData;
        }

        //public List<Roomandhotelmodel> GetDetails1()
        //{
        //    List<Roomandhotelmodel> lstData = new List<Roomandhotelmodel>();

        //    string sQuery = "SELECT * FROM Temproomandhallrecord";

        //    using (SqlConnection objSqlConnection = new SqlConnection(cs))
        //    {
        //        using (SqlCommand objSqlCommand = new SqlCommand(sQuery, objSqlConnection))
        //        {
        //            objSqlConnection.Open();
        //            var objReader = objSqlCommand.ExecuteReader();

        //            try
        //            {
        //                while (objReader.Read())
        //                {
        //                    var objData = new Roomandhotelmodel();
        //                    objData.customername = objReader["vcustomer"].ToString();
        //                    objData.customermobilenum = objReader["vcustomernumber"].ToString();
        //                    objData.roomandhalltype = objReader["vtype"].ToString();
        //                    objData.Availableroomandhall = objReader["vroomandhall"].ToString();
        //                    objData.checkindate = objReader["checkintime"].ToString();
        //                    objData.checkoutdate = objReader["checkouttime"].ToString();
        //                    objData.Finalamount = objReader["nfinalamount"].ToString();
        //                    lstData.Add(objData);
        //                }
        //            }
        //            finally
        //            {
        //                objReader.Close();
        //            }
        //        }
        //    }

        //    return lstData;
        //}

    }
}