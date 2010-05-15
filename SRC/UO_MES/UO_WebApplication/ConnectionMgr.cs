using System;
using System.Collections.Generic;
using System.Web;
using Oracle.DataAccess.Client;

public static class ConnectionMgr
{
    private static OracleConnection conn;

    public static OracleConnection Connection
    {
        get
        {
            if (conn != null)
                return conn;
            else
            {
                System.Configuration.AppSettingsReader configurationAppSettings = new System.Configuration.AppSettingsReader();

                conn = new OracleConnection(((string)(configurationAppSettings.GetValue("OracleConnectionString", typeof(string)))));
                conn.Open();
                return conn;
            }
        }
    }

    public static OracleConnection NewDBConnection(string username, string password, string connectString)
    {
        CloseConnection();
        // Connection Information	
        string connectionString =

            // Username
            "User Id=" + username +

            // Password
            ";Password=" + password +

            // Datasource  
            ";Data Source=" + connectString;


        // Create Connection object using the above connect string
        conn = new OracleConnection(connectionString);

        // Open database connection
        conn.Open();
        return conn;
    }

    public static void CloseConnection()
    {
        if (conn != null)
        {
            conn.Close();
            conn.Dispose();
        }
    }
}