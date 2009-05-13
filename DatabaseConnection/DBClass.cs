using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Data;

using IBM.Data.DB2;
using Elab.Rtls.Engines.WsnEngine;

namespace DatabaseConnection
{
    /// <summary>
    /// Base class for the DB2-database linker
    /// </summary>
    public class DBClassDB2
    {
        /// <summary>
        /// String with the needed specifications/arguments to set up a connection to the database.
        /// </summary>
        public string ConnString;

        /// <summary>
        /// Constructor of DBClassDB2
        /// </summary>
        /// <param name="ConnectionString">Connection-string with the needed specifications and arguments to set up a connection.</param>
        public DBClassDB2(string ConnectionString)
        {
            ConnString = ConnectionString;
        }

        /// <summary>
        /// Function that executes the given command on the database and returns the result.
        /// </summary>
        /// <param name="myquery">the query (stored procedure) to execute on the database.</param>
        /// <returns>A dataset which is filled with the table(s) with results from the stored procedure.</returns>
        public DataSet Query(string myquery)
        {
            DataSet TempDataSet = new DataSet();
            try
            {
                DB2DataAdapter DataAdapter = new DB2DataAdapter(myquery, ConnString);
                DataAdapter.Fill(TempDataSet);
            }
            catch (SqlException e_dbconn_sql)
            {
                //SocketClass.LogError(e_dbconn_sql, "LogServer.txt");
                //Create an error message
                TempDataSet = new DataSet("Replies");
                TempDataSet.Tables.Add("Reply");
                TempDataSet.Tables[0].Columns.Add("INT");
                DataRow newRow = TempDataSet.Tables[0].NewRow();
                newRow[0] = -404;
                TempDataSet.Tables[0].Rows.Add(newRow);
            }
            catch (TimeoutException e_dbconn_conn)
            {
                //SocketClass.LogError(e_dbconn_conn, "LogServer.txt");
                //Create an error message
                TempDataSet = new DataSet("Replies");
                TempDataSet.Tables.Add("Reply");
                TempDataSet.Tables[0].Columns.Add("INT");
                DataRow newRow = TempDataSet.Tables[0].NewRow();
                newRow[0] = -404;
                TempDataSet.Tables[0].Rows.Add(newRow);
            }
            return TempDataSet;
        }
    }

    /// <summary>
    /// Baseclass from which we inherit to create a class specific to a database-type. (or can also be used as a generic class to connect to any database).
    /// </summary>
    public class DBClassOdbc
    {
        /// <summary>
        /// Builder which helps create a syntax-correct connection-string.
        /// </summary>
        public OdbcConnectionStringBuilder ConnStringBuild;

        /// <summary>
        /// Base constructor that takes a connection-string and initializes the ConnectionStringBuilder with it.
        /// </summary>
        /// <param name="ConnectionString">Connection-string with the needed specifications and arguments to set up a connection.</param>
        public DBClassOdbc(string ConnectionString)
        {
            ConnStringBuild = new OdbcConnectionStringBuilder(ConnectionString);
        }

        /// <summary>
        /// Base constructor that takes a ConnectionStringBuilder and initializes the ConnectionStringBuilder with it.
        /// </summary>
        /// <param name="ConnectionStringBuilder">ConnectionStringBuilder with a connectionstring (preferably) to connect to a database.</param>
        public DBClassOdbc(OdbcConnectionStringBuilder ConnectionStringBuilder)
        {
            ConnStringBuild = ConnectionStringBuilder;
        }

        /// <summary>
        /// Function that sends a query to the database and returns the results in a dataset.
        /// </summary>
        /// <param name="myquery">The query (in string-format) that we want to execute on the database</param>
        /// <returns>Dataset with the results of the query</returns>
        public DataSet Query(string myquery)
        {
            DataSet TempDataSet = new DataSet();
            try
            {
                //connect to the dbms
                OdbcDataAdapter DataAdapter = new OdbcDataAdapter(myquery, ConnStringBuild.ConnectionString);
                //fills the dataset
                DataAdapter.Fill(TempDataSet);
            }
            catch (OdbcException OdbcEx)
            {
                Logger.LogOdbcException(OdbcEx, myquery);
                
                //OdbcEx.
                //Return the Error Message
                TempDataSet = new DataSet("Replies");
                TempDataSet.Tables.Add("Reply");
                TempDataSet.Tables[0].Columns.Add("INT");
                DataRow newRow = TempDataSet.Tables[0].NewRow();
                newRow[0] = -404;
                TempDataSet.Tables[0].Rows.Add(newRow);                
            }
            catch (SqlException e_dbconn_sql)
            {
                //Return the Error Message
                TempDataSet = new DataSet("Replies");
                TempDataSet.Tables.Add("Reply");
                TempDataSet.Tables[0].Columns.Add("INT");
                DataRow newRow = TempDataSet.Tables[0].NewRow();
                newRow[0] = -404;
                TempDataSet.Tables[0].Rows.Add(newRow);
            }
            return TempDataSet;
        }
    }

    /// <summary>5
    /// Class specific for MySQL.
    /// </summary>
    public class MySQLClass : DBClassOdbc
    {
        /// <summary>
        /// Constructor for MySQLClass
        /// </summary>
        /// <param name="MySQLConnectionString">connectionstring to use for the connection to the database (MySQL!)</param>
        public MySQLClass(string MySQLConnectionString)
            : base(MySQLConnectionString)
        {
        }

        /// <summary>
        /// Constructor that automatically adds a standard MySQL connectionstring
        /// </summary>
        public MySQLClass():
            base("DRIVER={MySQL ODBC 3.51 Driver};" +
                                            "SERVER=localhost;" +
                                            "DATABASE=mysqlsample;" +
                                            "UID=root;" +
                                            "PASSWORD=root;" +
                                            "OPTION=3")
        {

        }
    }

    /// <summary>
    /// Class specific for DB2.
    /// </summary>
    public class DB2Class : DBClassDB2
    {
        /// <summary>
        /// Constructor for DB2Class
        /// </summary>
        /// <param name="DB2ConnectionString">connectionstring to use for the connection to the database (DB2!)</param>
        public DB2Class(string DB2ConnectionString)
            : base(DB2ConnectionString)
        {
        }

        /// <summary>
        /// Constructor that automatically adds a standard DB2 connectionstring
        /// </summary>
        public DB2Class():
            base("provider = IBMDADB2.1;"+
                    "persist security info = false;"+
                    "SERVER=localhost"+
                    "data source = sample"+
                    "UID=root;" +
                    "PASSWORD=root;")
        {

        }
    }
}
