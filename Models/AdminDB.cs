using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Hotelmanagementsystem.Models
{
    public class AdminDB
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
                SqlCommand com = new SqlCommand("SelectUsers", con);
                com.CommandType = CommandType.StoredProcedure;
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    var objData = new Adminmodel();
                    objData.nid = Convert.ToInt32(rdr["nid"]);
                    objData.vusername = rdr["vusername"].ToString();
                    objData.vpassword = rdr["vpassword"].ToString();
                    objData.vmobnumber = rdr["vmobnumber"].ToString();
                    objData.vusertype = rdr["vusertype"].ToString();
                    //if (rdr["vusertype"].ToString() == "A")
                    //{
                    //    objData.vusertype = "Admin";
                    //}
                    //else
                    //{
                    //    objData.vusertype = "Employee";
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
                SqlCommand com = new SqlCommand("InsertUpdateUsers", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@nid", emp.nid);
                com.Parameters.AddWithValue("@vusername", emp.vusername.ToUpper());
                com.Parameters.AddWithValue("@vusertype", emp.vusertype);
                com.Parameters.AddWithValue("@vpassword", emp.vpassword);
                com.Parameters.AddWithValue("@vmobnumber", emp.vmobnumber);
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
                com.Parameters.AddWithValue("@vusername", emp.vusername);
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

        public bool ChangePassword(Adminmodel objChangePasswordModel,string nid , string vmobnumber)
        {
            try
            {
                string sQuery = "Update Users " +
                                "SET  vpassword = @NewPassword   " +
                                "WHERE   nid = @nid and vmobnumber=@vmobnumber and vpassword=@OldPassword";

                using (SqlConnection objSqlConnection = new SqlConnection(cs))
                {
                    using (SqlCommand objSqlCommand = new SqlCommand(sQuery, objSqlConnection))
                    {
                        objSqlCommand.Parameters.AddWithValue("@nid", nid);
                        objSqlCommand.Parameters.AddWithValue("@vmobnumber", vmobnumber);
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
    }
}