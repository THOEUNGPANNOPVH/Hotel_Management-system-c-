using System;
using System.Data;
using System.Data.SqlClient;

namespace Hotel_Management
{
    internal class HotelCDB
    {
        public SqlConnection con;
        string str = "Data Source=.;Initial Catalog=HotelDB;Integrated Security=true;";

        public void Connection()
        {
            SqlDependency.Stop(str);
            SqlDependency.Start(str);
            con = new SqlConnection(str);
            con.Open();
        }
    }
}
