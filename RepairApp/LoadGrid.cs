using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RepairApp
{
    public class LoadGrid
    {
        static public DataSet Loadgrid( string cmd) //Достает с базы PSIGMA FLAT и преобразует в грид, таблицу
        {
            try
            {
                //SqlConnection sqlcon = new SqlConnection(@"Data Source=(LocalDb)\MSSQLLocalDB; Initial Catalog= FAS; integrated security=True;");
                SqlConnection sqlcon = new SqlConnection(@"Data Source=traceability\flat; Initial Catalog= FAS; user id=volodin;password=volodin;");
                SqlCommand c = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                DataSet ds = new DataSet();
                sqlcon.Open();
                c = sqlcon.CreateCommand();
                c.CommandText = cmd;
                da.SelectCommand = c;
                da.Fill(ds, "Table1");                
                sqlcon.Close();
                return ds;
            }

            catch (Exception e)
            {                
                var k = e.Message;
                return null;
            }

        }

        public static object SelectString(string cmd) //Достает с базы PSIGMA FLAT строковые значения и числовые
        {

            //SqlConnection sqlcon = new SqlConnection(@"Data Source=(LocalDb)\MSSQLLocalDB; Initial Catalog= FAS; integrated security=True;");
            SqlConnection sqlcon = new SqlConnection(@"Data Source=traceability\flat; Initial Catalog= FAS; user id=volodin;password=volodin;");
            SqlCommand c = new SqlCommand();
            SqlDataReader r;
            string k = "";
            c = sqlcon.CreateCommand();
            c.CommandType = CommandType.Text;
            c.CommandText = cmd;
            try
            {
                sqlcon.Open();
                r = c.ExecuteReader();
                if (r.Read())
                {
                    k = r.GetString(0);
                    r.Close();
                }

                sqlcon.Dispose();
                return k;
            }


            catch (Exception )
            {                
                return k;
            }

        }

    }
}