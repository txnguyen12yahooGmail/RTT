using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using CS_CommonDataAccessLayer;

namespace CS_CommonBusinessLayer
{
    public static class UploadedFileBL
    {
        #region Variables

        private static string webConfigDataConnection = "DefaultConnection";
        private static string coreStoredProcedure = "spCoreUploadedFiles";
        private static string tasksStoredProcedure = "spTasksUploadedFiles";

        #endregion

        #region Object Conversions

        public static UploadedFile DataToModel(DataRow row, bool headerOnly = false)
        {
            //Create a new model record
            UploadedFile model = new UploadedFile();

            //Save the data row values to the standard model properties
            model.FileID = row.GetGuidValue("FileID");
            model.FileName = row.GetText("FileName");
            model.FileExtension = row.GetText("FileExtension");
            model.FileDescription = row.GetText("FileDescription");
            model.FileContentType = row.GetText("FileContentType");
            model.FileUploadedBy = row.GetText("FileUploadedBy");
            model.FileUploadedByDisplay = row.GetText("FileUploadedByDisplay");

            if (headerOnly)
                model.FileSize = row.GetIntValue("FileSize");
            else
                model.FileContents = row.GetByteArrayValue("FileContents");

            if (row["FileUploadedDate"] != DBNull.Value)
            {
                model.FileUploadedDate = new CompleteDateTime();
                model.FileUploadedDate.FullDateTime = row.GetDateTimeValue("FileUploadedDate");
            }

            return model;
        }
        private static List<SqlParameter> ModelToParameters(UploadedFile model)
        {
            //Build a parameter list for any stored procedure. Only add parameters that are not null, or 0. 
            List<SqlParameter> parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter() { ParameterName = "@FileID", SqlDbType = SqlDbType.UniqueIdentifier, Value = model.FileID });
            parameterList.Add(new SqlParameter() { ParameterName = "@FileName", SqlDbType = SqlDbType.NVarChar, Value = model.FileName });
            parameterList.Add(new SqlParameter() { ParameterName = "@FileExtension", SqlDbType = SqlDbType.NVarChar, Value = model.FileExtension });
            parameterList.Add(new SqlParameter() { ParameterName = "@FileDescription", SqlDbType = SqlDbType.NVarChar, Value = model.FileDescription });
            parameterList.Add(new SqlParameter() { ParameterName = "@FileSize", SqlDbType = SqlDbType.BigInt, Value = model.FileSize });
            parameterList.Add(new SqlParameter() { ParameterName = "@FileContentType", SqlDbType = SqlDbType.NVarChar, Value = model.FileContentType });
            parameterList.Add(new SqlParameter() { ParameterName = "@FileContents", SqlDbType = SqlDbType.VarBinary, Size = Int32.MaxValue, Value = model.FileContents });
            parameterList.Add(new SqlParameter() { ParameterName = "@FileUploadedBy", SqlDbType = SqlDbType.NVarChar, Value = model.FileUploadedBy });
            parameterList.Add(new SqlParameter() { ParameterName = "@FileUploadedDate", SqlDbType = SqlDbType.DateTime, Value = model.FileUploadedDate.FullDateTime ?? DateTime.Now });

            return parameterList;
        }

        #endregion

        #region Core Methods

        public static Guid CreateRecord(this UploadedFile newRecord)
        {
            try
            {
                //Build the parameter list
                List<SqlParameter> parmsList = ModelToParameters(newRecord);
                parmsList.Add(new SqlParameter() { ParameterName = "@Task", SqlDbType = SqlDbType.NVarChar, Value = "CREATE" });
                
                //Create a new instance of the C# SQL Data access layer
                DatabaseFunctions DataFunctions = new DatabaseFunctions();

                //Call the function to execute the list store procedure
                int result = DataFunctions.ExecuteNonQueryStoredProcedure(coreStoredProcedure, parmsList, webConfigDataConnection);

                //Return the result
                return newRecord.FileID;
            }
            catch (Exception e)
            {
                //Throw any exceptions back to the calling process.
                throw e;
            }
        }
        public static UploadedFile ReadRecord(Guid fileID)
        {
            try
            {
                //Build the parameter list
                List<SqlParameter> parameterList = new List<SqlParameter>()
                    {
                        new SqlParameter() {ParameterName = "@Task", SqlDbType = SqlDbType.NVarChar, Value = "READ" },
                        new SqlParameter() {ParameterName = "@FileID", SqlDbType = SqlDbType.UniqueIdentifier, Value = fileID }
                    };

                //Create a new instance of the C# SQL Data access layer
                DatabaseFunctions DataFunctions = new DatabaseFunctions();

                //Call the function to execute the list store procedure
                DataTable dataTable = DataFunctions.ExecuteQueryStoredProcedure(coreStoredProcedure, parameterList, webConfigDataConnection);

                //Create a stock model from the datatable record
                UploadedFile model = dataTable.Rows.Count > 0 ? DataToModel(dataTable.Rows[0]) : null;

                return model;

            }
            catch (Exception)
            {
                //Throw any exceptions back to the calling process.
                throw;
            }
        }
        public static int DeleteRecord(Guid fileID)
        {

            try
            {

                //Build the parameter list
                List<SqlParameter> parameterList = new List<SqlParameter>()
                {
                    new SqlParameter() { ParameterName = "@Task", SqlDbType = SqlDbType.NVarChar, Value = "DELETE_FILE" },
                    new SqlParameter() {ParameterName = "@FileID", SqlDbType = SqlDbType.UniqueIdentifier, Value = fileID },
                };

                //Create a new instance of the C# SQL Data access layer
                DatabaseFunctions DataFunctions = new DatabaseFunctions();

                //Call the function to execute the list store procedure
                int result = DataFunctions.ExecuteNonQueryStoredProcedure(coreStoredProcedure, parameterList, webConfigDataConnection);

                //Return the result
                return result;
            }
            catch (Exception)
            {
                //Throw any exceptions back to the calling process.
                throw;
            }

        }

        #endregion

        #region Tasks

        public static UploadedFile ReadHeader(Guid fileID)
        {
            try
            {
                //Build the parameter list
                List<SqlParameter> parameterList = new List<SqlParameter>()
                    {
                        new SqlParameter() {ParameterName = "@Task", SqlDbType = SqlDbType.NVarChar, Value = "READ_HEADER" },
                        new SqlParameter() {ParameterName = "@FileID", SqlDbType = SqlDbType.UniqueIdentifier, Value = fileID }
                    };

                //Create a new instance of the C# SQL Data access layer
                DatabaseFunctions DataFunctions = new DatabaseFunctions();

                //Call the function to execute the list store procedure
                DataTable dataTable = DataFunctions.ExecuteQueryStoredProcedure(coreStoredProcedure, parameterList, webConfigDataConnection);

                //Create a stock model from the datatable record
                UploadedFile model = dataTable.Rows.Count > 0 ? DataToModel(dataTable.Rows[0], true) : null;

                return model;

            }
            catch (Exception)
            {
                //Throw any exceptions back to the calling process.
                throw;
            }
        }
        
        #endregion

    }
}
