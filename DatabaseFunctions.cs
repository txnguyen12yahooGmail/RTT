using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Threading;
using System.Diagnostics;


namespace CS_CommonDataAccessLayer
{

    public class DatabaseFunctions
    {
        private bool simulateConnectionDelay = false;
        private int millisecondDelay = 0000;

        /// <summary>
        /// Database connection delay that can be used for testing
        /// </summary>
        private void DelayConnection()
        {
            if (simulateConnectionDelay)
            {
                //Delay for wait modal testing
                Thread.Sleep(millisecondDelay);
            }
        }

        /// <summary>
        /// Executes a scalar query stored procedure and returns results as an Object type.
        /// </summary>
        /// <param name="storedProcedureName">Name of the procedure to execute.</param>
        /// <param name="connectionName">Optional name of the webconfig connection string to use.</param>
        /// <returns>Datatable with the selected records.</returns>
        public Object ExecuteScalarStoredProcedure(string storedProcedureName, string connectionName = "DefaultConnection", int connectionTimeout = 60)
        {
            DelayConnection();
            try
            {
                //Use the default connection listed in webconfig.
                string connectionString = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;

                //Connect to the database with 'using' to ensure the connection is closed when we're done.
                using (SqlConnection Connection = new SqlConnection(connectionString))
                {

                    //Open a connection and execute the stored procedure
                    SqlCommand Command = new SqlCommand(storedProcedureName, Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.CommandTimeout = connectionTimeout;
                    Connection.Open();

                    Object retScalarValue = Command.ExecuteScalar();
                    if (retScalarValue == null || retScalarValue == DBNull.Value)
                    {
                        return null;
                    }
                    else
                    {
                        return retScalarValue;
                    }
                    ;
                }
            }
            catch (Exception e)
            {
                //Pass the exception back to the calling process
                throw e;
            }
        }

        /// <summary>
        /// Executes a scalar query stored procedure and returns results as an Object type.
        /// </summary>
        /// <param name="storedProcedureName">Name of the procedure to execute.</param>
        /// <param name="parameters">Parameter list to be passed to the stored procedure.</param>
        /// <param name="connectionName">Optional name of the webconfig connection string to use.</param>
        /// <returns>Datatable with the selected records.</returns>
        public Object ExecuteScalarStoredProcedure(string storedProcedureName, List<SqlParameter> parameters, string connectionName = "DefaultConnection", int connectionTimeout = 60)
        {
            DelayConnection();
            try
            {
                //Use the default connection listed in webconfig.
                string connectionString = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;

                //Connect to the database with 'using' to ensure the connection is closed when we're done.
                using (SqlConnection Connection = new SqlConnection(connectionString))
                {

                    //Open a connection and execute the stored procedure
                    SqlCommand Command = new SqlCommand(storedProcedureName, Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddRange(parameters.ToArray());
                    Command.CommandTimeout = connectionTimeout;
                    Connection.Open();

                    Object retScalarValue = Command.ExecuteScalar();
                    if (retScalarValue == null || retScalarValue == DBNull.Value)
                    {
                        return null;
                    }
                    else
                    {
                        return retScalarValue;
                    }
                    ;
                }
            }
            catch (Exception e)
            {
                //Pass the exception back to the calling process
                throw e;
            }
        }

        /// <summary>
        /// Executes a query stored procedure and returns results as a datatable.
        /// </summary>
        /// <param name="storedProcedureName">Name of the procedure to execute.</param>
        /// <param name="connectionName">Optional name of the webconfig connection string to use.</param>
        /// <returns>Datatable with the selected records.</returns>
        public DataTable ExecuteQueryStoredProcedure(string storedProcedureName, string connectionName = "DefaultConnection", int connectionTimeout = 60)
        {
            DelayConnection();
            try
            {
                //Use the default connection listed in webconfig.
                string connectionString = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
                DataTable dataTable = new DataTable();

                //Connect to the database with 'using' to ensure the connection is closed when we're done.
                using (SqlConnection Connection = new SqlConnection(connectionString))
                {

                    //Open a connection and execute the stored procedure
                    SqlCommand Command = new SqlCommand(storedProcedureName, Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.CommandTimeout = connectionTimeout;
                    Connection.Open();

                    //Read the results from the database and store the data in a datatable
                    using (SqlDataReader DataReader = Command.ExecuteReader())
                        dataTable.Load(DataReader);
                    {
                        return dataTable;
                    }
                    ;
                }
            }
            catch (Exception e)
            {
                //Pass the exception back to the calling process
                throw e;
            }
        }

        /// <summary>
        /// Executes a query stored procedure with parameter list and returns the results as a datatable.
        /// </summary>
        /// <param name="storedProcedureName">Name of the procedure to execute.</param>
        /// <param name="parameters">Parameter list to be passed to the stored procedure.</param>
        /// <param name="connectionName">Optional name of the webconfig connection string to use.</param>
        /// <returns>Datatable with the selected records.</returns>
        public DataTable ExecuteQueryStoredProcedure(string storedProcedureName, List<SqlParameter> parameters, string connectionName = "DefaultConnection", int connectionTimeout = 60)
        {
            DelayConnection();
            try
            {
                //Use the default connection listed in web config.
                string connectionString = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
                DataTable dataTable = new DataTable();

                //Connect to the database with 'using' to ensure the connection is closed when we're done.
                using (SqlConnection Connection = new SqlConnection(connectionString))
                {
                    //Execute the stored procedure.
                    SqlCommand Command = new SqlCommand(storedProcedureName, Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.CommandTimeout = connectionTimeout;
                    Command.Parameters.AddRange(parameters.ToArray());
                    Connection.Open();

                    //Read the results from the database and store the data in a datatable.
                    using (SqlDataReader DataReader = Command.ExecuteReader())
                    {
                        dataTable.Load(DataReader);
                        //Command.Dispose();
                        return dataTable;
                    }
                    ;
                }
            }
            catch (Exception e)
            {
                //Pass the exception back to the calling process.
                throw e;
            }
        }

        public DataSet ExecuteQueryStoredProcedureDataSet(string storedProcedureName, List<SqlParameter> parameters, string connectionName = "DefaultConnection", int connectionTimeout = 60, int NumTablesReturned = 2)
        {
            DelayConnection();
            DataSet ds = new DataSet();
            try
            {
                //Use the default connection listed in web config.
                string connectionString = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
               
                //Connect to the database with 'using' to ensure the connection is closed when we're done.
                using (SqlConnection Connection = new SqlConnection(connectionString))
                {
                    // Specify the name of your stored procedure.
                    using (SqlCommand cmd = new SqlCommand(storedProcedureName, Connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // If your stored procedure accepts parameters, add them like this:
                        // cmd.Parameters.Add(new SqlParameter("@ParameterName", parameterValue));
                        cmd.CommandTimeout = connectionTimeout;
                        cmd.Parameters.AddRange(parameters.ToArray());

                        // Open the connection
                        Connection.Open();

                        // Use a SqlDataAdapter to execute the command and fill the DataSet.
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(ds);
                        }//using SqlDataAdapter
                    }//using SqlCommand

                }//using Connection
                return ds;
            }
            catch (Exception e)
            {
                //Pass the exception back to the calling process.
                throw e;
            }
        }

        /// <summary>
        /// Executes a query stored procedure that returns one column called [Name]. The [Name] column is returned as a list of strings. Used best for autotype functions. 
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="connectionName"></param>
        /// <param name="connectionTimeout"></param>
        /// <returns></returns>
        public List<string> ExecuteReaderQueryStoredProcedure(string storedProcedureName, List<SqlParameter> parameters, string fieldName = "Name", string connectionName = "DefaultConnection", int connectionTimeout = 60)
        {
            DelayConnection();
            try
            {
                //Use the default connection listed in webconfig.
                string connectionString = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
                List<string> listStockNames = new List<string>();


                //Connect to the database with 'using' to ensure the connection is closed when we're done.
                using (SqlConnection Connection = new SqlConnection(connectionString))
                {

                    //Open a connection and execute the stored procedure
                    SqlCommand Command = new SqlCommand(storedProcedureName, Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.CommandTimeout = connectionTimeout;
                    Command.Parameters.AddRange(parameters.ToArray());
                    Connection.Open();

                    //Read the results from the database and store the data in a datatable
                    using (SqlDataReader DataReader = Command.ExecuteReader())
                    {
                        bool i = DataReader.HasRows;
                        while (DataReader.Read())
                        {
                            listStockNames.Add(DataReader[fieldName].ToString());
                        }
                        return listStockNames;
                    }
                }
            }
            catch (Exception e)
            {
                //Pass the exception back to the calling process
                throw e;
            }
        }

        /// <summary>
        /// Executes a database Function and returns a table with the selected records
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="connectionName"></param>
        /// <param name="connectionTimeout"></param>
        /// <returns></returns>
        public DataTable ExecuteDatabaseFunction(string functionQueryString, List<SqlParameter> parameters, string connectionName = "DefaultConnection", int connectionTimeout = 60)
        {

            try
            {
                //Use the default connection listed in webconfig.
                string connectionString = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
                DataTable dataTable = new DataTable();

                //Connect to the database with 'using' to ensure the connection is closed when we're done.
                using (SqlConnection Connection = new SqlConnection(connectionString))
                {
                    //Open a connection and execute the stored procedure
                    SqlCommand Command = new SqlCommand(functionQueryString, Connection);
                    Command.CommandType = CommandType.Text;
                    Command.CommandText = functionQueryString;
                    //Command.Parameters.AddRange(parameters.ToArray());
                    //Command.CommandTimeout = connectionTimeout;
                    Connection.Open();

                    //Read the results from the database and store the data in a datatable.
                    using (SqlDataReader DataReader = Command.ExecuteReader())
                        dataTable.Load(DataReader);
                    {
                        return dataTable;
                    }
                    ;
                }
            }
            catch (Exception e)
            {
                //Pass the exception back to the calling process
                throw e;
            }



        }

        /// <summary>
        /// Executes a non query stored procedure and returns an integer with the number of rows affected.
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="connectionName"></param>
        /// <returns>int with the number of records affected</returns>
        public int ExecuteNonQueryStoredProcedure(string storedProcedureName, string connectionName = "DefaultConnection", int connectionTimeout = 60)
        {
            DelayConnection();
            try
            {
                //Use the default connection listed in web config.
                string connectionString = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;

                //Connect to the database with 'using' to ensure the connection is closed when we're done.
                using (SqlConnection Connection = new SqlConnection(connectionString))
                {
                    //Execute the stored procedure.
                    SqlCommand Command = new SqlCommand(storedProcedureName, Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.CommandTimeout = connectionTimeout;
                    Connection.Open();

                    //Execute the query and return the result
                    int result = Command.ExecuteNonQuery();
                    return result;
                }
            }
            catch (Exception e)
            {
                //Pass the exception back to the calling process.
                throw e;
            }

        }

        /// <summary>
        /// Executes a non query stored procedure with parameter list and returns an integer with the number of rows affected.
        /// </summary>
        /// <param name="storedProcedureName">Name of the procedure to execute.</param>
        /// <param name="parameters">Parameter list to be passed to the stored procedure.</param>
        /// <param name="connectionName">Optional name of the webconfig connection string to use.</param>
        /// <returns>int with number of rows affected.</returns>
        public int ExecuteNonQueryStoredProcedure(string storedProcedureName, List<SqlParameter> parameters, string connectionName = "DefaultConnection", int connectionTimeout = 60)
        {            
            DelayConnection();
            try
            {
                //Use the default connection listed in web config.
                string connectionString = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;

                //Connect to the database with 'using' to ensure the connection is closed when we're done.
                using (SqlConnection Connection = new SqlConnection(connectionString))
                {
                    //Execute the stored procedure.
                    SqlCommand Command = new SqlCommand(storedProcedureName, Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.CommandTimeout = connectionTimeout;
                    Command.Parameters.AddRange(parameters.ToArray());
                    SqlParameter returnParm = Command.Parameters.Add("returnValue", SqlDbType.Int);
                    returnParm.Direction = ParameterDirection.ReturnValue;
                    Connection.Open();

                    //Execute the query and return the result
                    int result = Command.ExecuteNonQuery();
                    int RecordId = (int)returnParm.Value;
                    return result;
                }
            }
            catch (Exception e)
            {
                //Pass the exception back to the calling process.
                throw e;
            }
        }

        /// <summary>
        /// Executes a non query stored procedure with parameter list and returns an integer with the affected record Id. Best used when creating a new record.
        /// </summary>
        /// <param name="storedProcedureName">Name of the procedure to execute.</param>
        /// <param name="parameters">Parameter list to be passed to the stored procedure.</param>
        /// <param name="connectionName">Optional name of the webconfig connection string to use.</param>
        /// <returns>int with number of rows affected.</returns>
        public int ExecuteNonQueryStoredProcedureWithReturnRecordId(string storedProcedureName, List<SqlParameter> parameters, string connectionName = "DefaultConnection", int connectionTimeout = 60)
        {
            DelayConnection();
            try
            {
                //Use the default connection listed in web config.
                string connectionString = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;

                //Connect to the database with 'using' to ensure the connection is closed when we're done.
                using (SqlConnection Connection = new SqlConnection(connectionString))
                {
                    //Execute the stored procedure.
                    SqlCommand Command = new SqlCommand(storedProcedureName, Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.CommandTimeout = connectionTimeout;
                    Command.Parameters.AddRange(parameters.ToArray());
                    SqlParameter returnParm = Command.Parameters.Add("returnValue", SqlDbType.Int);
                    returnParm.Direction = ParameterDirection.ReturnValue;
                    Connection.Open();

                    //Execute the query and return the result
                    int result = Command.ExecuteNonQuery();
                    int RecordId = (int)returnParm.Value;
                    return RecordId;
                }
            }
            catch (Exception e)
            {
                //Pass the exception back to the calling process.
                throw e;
            }
        }

        /// <summary>
        /// Executes a non query stored procedure with multiple input and output parameters. Returns the parameter collection so that all return parameters are accessable.
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="connectionName"></param>
        /// <param name="connectionTimeout"></param>
        /// <returns>Parameter collection containing all input and return parameters with their values.</returns>
        public SqlParameterCollection ExecuteNonQueryStoredProcedureWithReturnParameters(string storedProcedureName, List<SqlParameter> parameters, string connectionName = "DefaultConnection", int connectionTimeout = 60)
        {
            DelayConnection();
            try
            {
                //Use the default connection listed in web config.
                string connectionString = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;

                //Connect to the database with 'using' to ensure the connection is closed when we're done.
                using (SqlConnection Connection = new SqlConnection(connectionString))
                {
                    //Execute the stored procedure.
                    SqlCommand Command = new SqlCommand(storedProcedureName, Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.CommandTimeout = connectionTimeout;
                    Command.Parameters.AddRange(parameters.ToArray());
                    Connection.Open();

                    //Execute the query and return the parameter list so that all return parameters can be accessed from the calling procedure.
                    int result = Command.ExecuteNonQuery();
                    return Command.Parameters;
                }
            }
            catch (Exception e)
            {
                //Pass the exception back to the calling process.
                throw e;
            }



        }

        /// <summary>
        /// Excel Spreadsheet - Reads the first tab of a spreadsheet and returns the data in a datatable.
        /// </summary>
        /// <param name="SpreadSheetFileAndPath"></param>
        /// <param name="SheetName"></param>
        /// <returns></returns>
        public DataTable ReadSpreadsheetData(string SpreadSheetFileAndPath, string SheetName = "Default")
        {
            DataTable dt = new DataTable();
            DataTable dtExcelSchema = new DataTable();
            try
            {
                //Create and open a connection to the excel spreadsheet
                String strExcelConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + SpreadSheetFileAndPath + "; Extended Properties='Excel 8.0;HDR=Yes'";
                OleDbConnection connExcel = new OleDbConnection(strExcelConn);
                OleDbCommand cmdExcel = new OleDbCommand();
                cmdExcel.Connection = connExcel;
                connExcel.Open();
                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                //Get the first spreadsheet sheet or use the sheet name that was passed in and load the data to a datatable
                OleDbDataAdapter da = new OleDbDataAdapter();
                if (SheetName == "Default")
                { SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString(); }
                cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
                da.SelectCommand = cmdExcel;
                da.Fill(dt);
                connExcel.Close();
            }
            catch (Exception e)
            {
                //Pass the exception back to the calling process.
                throw e;
            }
            return dt;
        }

        /// <summary>
        /// Saves data from a datatable to a matching database table. 
        /// </summary>
        /// <param name="DatabaseTableName"></param>
        /// <param name="dt"></param>
        /// <param name="connectionName"></param>
        public void CopyDataTableToDatabaseTable(string DatabaseTableName, DataTable dt, string connectionName = "DefaultConnection")
        {
            string connectionString = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
            {
                try
                {
                    //Verify database columns exist for each spreadsheet column
                    connection.Open();
                    bulkCopy.DestinationTableName = DatabaseTableName;
                    bulkCopy.WriteToServer(dt);
                }
                catch (Exception e)
                {
                    //Pass the exception back to the calling process.
                    throw e;
                }
            }
        }



        #region "Work in progress"

        public string CreateTABLE(string tableName, DataTable table)
        {
            string sqlsc;
            sqlsc = "CREATE TABLE " + tableName + "(";
            for (int i = 0; i < table.Columns.Count; i++)
            {
                sqlsc += "\n [" + table.Columns[i].ColumnName + "] ";
                string columnType = table.Columns[i].DataType.ToString();
                switch (columnType)
                {
                    case "System.Int32":
                        sqlsc += " int ";
                        break;
                    case "System.Int64":
                        sqlsc += " bigint ";
                        break;
                    case "System.Int16":
                        sqlsc += " smallint";
                        break;
                    case "System.Byte":
                        sqlsc += " tinyint";
                        break;
                    case "System.Decimal":
                        sqlsc += " decimal ";
                        break;
                    case "System.DateTime":
                        sqlsc += " datetime ";
                        break;
                    case "System.String":
                    default:
                        sqlsc += string.Format(" nvarchar({0}) ", table.Columns[i].MaxLength == -1 ? "max" : table.Columns[i].MaxLength.ToString());
                        break;
                }
                if (table.Columns[i].AutoIncrement)
                    sqlsc += " IDENTITY(" + table.Columns[i].AutoIncrementSeed.ToString() + "," + table.Columns[i].AutoIncrementStep.ToString() + ") ";
                if (!table.Columns[i].AllowDBNull)
                    sqlsc += " NOT NULL ";
                sqlsc += ",";
            }
            return sqlsc.Substring(0, sqlsc.Length - 1) + "\n)";



        }
        public DataSet ReaderExcelSpreadSheet(string SpreadSheetFileAndPath)
        {
            //Returns all excel sheets as a DataSet instead of a data table

            //string SheetName = "";
            DataSet ds = new DataSet();
            DataTable dtExcelSchema = new DataTable();

            return ds;

        }
        public DataTable ReadExcelSpreadSheet(string SpreadSheetFileAndPath, bool FindSomethingElseToReplaceThis, string SelectQuery)
        {
            //Use this overload option to return excel load data using query and file name, like only selecting specific rows from the spreadsheet
            DataTable dt = new DataTable();
            return dt;


            //DataTable dt = new DataTable();
            //DataTable dtExcelSchema = new DataTable();
            //try
            //{

            //    //Create and open a connection to the excel spreadsheet
            //    String strExcelConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + SpreadSheetFileAndPath + "; Extended Properties='Excel 8.0;HDR=Yes'";
            //    OleDbConnection connExcel = new OleDbConnection(strExcelConn);
            //    OleDbCommand cmdExcel = new OleDbCommand();
            //    cmdExcel.Connection = connExcel;
            //    connExcel.Open();
            //    dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            //    //Get the first spreadsheet sheet or use the sheet name that was passed in and load the data to a datatable
            //    OleDbDataAdapter da = new OleDbDataAdapter();
            //    if (SheetName == "Default")
            //    { SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString(); }
            //    cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
            //    da.SelectCommand = cmdExcel;
            //    da.Fill(dt);
            //    connExcel.Close();
            //}
            //catch (Exception e)
            //{
            //    //Pass the exception back to the calling process.
            //    throw e;
            //}
            //return dt;


        }
        private DataTable zExecuteQueryStoredProcedure(string storedProcedureName, List<System.Data.Common.DbParameterCollection> parameters, string connectionName = "DefaultConnection")
        {
            DelayConnection();
            try
            {
                //Use the default connection listed in web config
                string ConnectionString = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
                DataTable dataTable = new DataTable();

                //Connect to the database with 'using' to ensure the connection is closed when we're done.
                using (SqlConnection Connection = new SqlConnection(ConnectionString))
                {
                    //Execute the stored procedure
                    SqlCommand Command = new SqlCommand(storedProcedureName, Connection);
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.Add(parameters);
                    Connection.Open();

                    //Read the results from the database and store the data in a datatable
                    using (SqlDataReader DataReader = Command.ExecuteReader())
                        dataTable.Load(DataReader);
                    {
                        return dataTable;
                    }
                    ;
                }
            }
            catch (Exception e)
            {
                //Pass the exception back to the calling process
                throw e;
            }
        }
        private DataTable zExecuteQuerySQL(string sqlText, string connectionName = "DefaultConnection")
        {
            //Use the default connection listed in web config
            string ConnectionString = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
            DataTable dataTable = new DataTable();


            return dataTable;
        }
        private int zExecuteNonQuerySQL(string sqlText, string connectionName = "DefaultConnection")
        {

            return 1;
        }

        #endregion

    }
}
