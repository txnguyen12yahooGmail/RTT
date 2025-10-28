using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CS_CommonDataAccessLayer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;


namespace CS_CommonBusinessLayer
{
    public class EmployeeBL
    {

        #region Variables

        private string webConfigDataConnection = "DefaultConnection";
        private string pimsConfigDataConnection = "PimsConnection";
        private string tasksStoredProcedure = "spTaskEmployee";
        private string taskEmployeeAll = "spTaskEmployeeAll";
        private string usp_EDOList = "usp_sspur_Employee_EDO_by_year";

        #endregion

        #region Object Conversions

        private Employee DataToModel(DataRow row)
        {
            //Create a new model record
            Employee model = new Employee();

            //Save the data row values to the standard model properties
            model.CAC = row.GetText("CAC");
            model.UserId = row.GetText("UserId");
            model.AccountingId = row.GetText("AccountingId");
            model.SupervisorAccountingId = row.GetText("SupervisorAccountingId");
            model.SupervisorCAC = row.GetText("SupervisorCAC");
            model.EmployeeStatus = row.GetText("EmployeeStatus");
            model.FirstName = row.GetText("FirstName").Trim();
            model.MiddleName = row.GetText("MiddleName").Trim();
            model.LastName = row.GetText("LastName").Trim();
            model.PreferredName = row.GetText("PreferredName").Trim();
            model.HomeFacility = row.GetText("HomeFacility");
            model.Department = row.GetText("Department");
            model.Email = row.GetText("Email");
            model.WorkPhone = row.GetText("WorkPhone");
            model.Company = row.GetText("Company");
            model.AccruedLastUpdate = row.GetDateTimeValue("LastAccruedHourUpdate");
            model.TimeKeeperAccess = row.GetText("TimeKeeperAccess");
            model.TimeReviewerAccess = row.GetText("TimeReviewerAccess");
            model.DefaultWorkFacility = row.GetText("DefaultWorkFacility");
            if (row["DefaultChargeCodeId"] != DBNull.Value) { model.DefaultChargeCodeId = row.GetIntValue("DefaultChargeCodeId"); };
            model.IsEmployee = false;
            if(row.GetIntValue("IsEmployee") == 1) { model.IsEmployee = true; }
            model.IsGovUser = false;
            if (row.GetIntValue("IsGovUser") == 1) { model.IsGovUser = true; }
            model.IsSupervisor = false;
            if (row.GetIntValue("IsSupervisor") == 1) { model.IsSupervisor = true; }
            model.IsDirector = false;
            if (row.GetIntValue("IsDirector") == 1) { model.IsDirector = true; }
            return model;
        }
        private List<SqlParameter> ModelToParameters(Employee model)
        {
            //Build a parameter list for any stored procedure. Only add parameters that are not null, or 0. 
            List<SqlParameter> parameterList = new List<SqlParameter>();
            if (model.CAC != null) { parameterList.Add(new SqlParameter() { ParameterName = "@CAC", SqlDbType = SqlDbType.NVarChar, Value = model.CAC }); }
            if (model.UserId != null) { parameterList.Add(new SqlParameter() { ParameterName = "@UserId", SqlDbType = SqlDbType.NVarChar, Value = model.UserId }); }
            if (model.AccountingId != null) { parameterList.Add(new SqlParameter() { ParameterName = "@AccountingId", SqlDbType = SqlDbType.NVarChar, Value = model.AccountingId }); }
            if (model.SupervisorAccountingId != null) { parameterList.Add(new SqlParameter() { ParameterName = "@SupervisorAccountingId", SqlDbType = SqlDbType.NVarChar, Value = model.SupervisorAccountingId }); }
            if (model.CAC != null) { parameterList.Add(new SqlParameter() { ParameterName = "@SupervisorCAC", SqlDbType = SqlDbType.NVarChar, Value = model.SupervisorCAC }); }
            if (model.EmployeeStatus != null) { parameterList.Add(new SqlParameter() { ParameterName = "@EmployeeStatus", SqlDbType = SqlDbType.NVarChar, Value = model.EmployeeStatus }); }
            if (model.FirstName != null) { parameterList.Add(new SqlParameter() { ParameterName = "@FirstName", SqlDbType = SqlDbType.NVarChar, Value = model.FirstName }); }
            if (model.MiddleName != null) { parameterList.Add(new SqlParameter() { ParameterName = "@MiddleName", SqlDbType = SqlDbType.NVarChar, Value = model.MiddleName }); }
            if (model.LastName != null) { parameterList.Add(new SqlParameter() { ParameterName = "@LastName", SqlDbType = SqlDbType.NVarChar, Value = model.LastName }); }
            if (model.PreferredName != null) { parameterList.Add(new SqlParameter() { ParameterName = "@PreferredName", SqlDbType = SqlDbType.NVarChar, Value = model.PreferredName }); }
            if (model.HomeFacility != null) { parameterList.Add(new SqlParameter() { ParameterName = "@HomeFacility", SqlDbType = SqlDbType.NVarChar, Value = model.HomeFacility }); }
            if (model.Department != null) { parameterList.Add(new SqlParameter() { ParameterName = "@Department", SqlDbType = SqlDbType.NVarChar, Value = model.Department }); }
            if (model.Email != null) { parameterList.Add(new SqlParameter() { ParameterName = "@Email", SqlDbType = SqlDbType.NVarChar, Value = model.Email }); }
            if (model.WorkPhone != null) { parameterList.Add(new SqlParameter() { ParameterName = "@WorkPhone", SqlDbType = SqlDbType.NVarChar, Value = model.WorkPhone }); }
            if (model.Company != null) { parameterList.Add(new SqlParameter() { ParameterName = "@Company", SqlDbType = SqlDbType.NVarChar, Value = model.Company }); }
            if (model.AccruedLastUpdate != null) { parameterList.Add(new SqlParameter() { ParameterName = "@AccruedLastUpdate", SqlDbType = SqlDbType.DateTime, Value = model.AccruedLastUpdate }); }
            if (model.VacationBalance != 0) { parameterList.Add(new SqlParameter() { ParameterName = "@VacationBalance", SqlDbType = SqlDbType.Decimal, Value = model.VacationBalance }); }
            if (model.VacationBalanceBeginning != 0) { parameterList.Add(new SqlParameter() { ParameterName = "@VacationBalanceBeginning", SqlDbType = SqlDbType.Decimal, Value = model.VacationBalanceBeginning }); }
            if (model.SickLeaveBalance != 0) { parameterList.Add(new SqlParameter() { ParameterName = "@SickLeaveBalance", SqlDbType = SqlDbType.Decimal, Value = model.SickLeaveBalance }); }
            if (model.PersonalLeaveBalance != 0) { parameterList.Add(new SqlParameter() { ParameterName = "@PersonalLeaveBalance", SqlDbType = SqlDbType.Decimal, Value = model.PersonalLeaveBalance }); }
            if (model.PersonalLeaveBalanceBeginning != 0) { parameterList.Add(new SqlParameter() { ParameterName = "@PersonalLeaveBalanceBeginning", SqlDbType = SqlDbType.Decimal, Value = model.PersonalLeaveBalanceBeginning }); }
            if (model.TimeKeeperAccess != null) { parameterList.Add(new SqlParameter() { ParameterName = "@TimeKeeperAccess", SqlDbType = SqlDbType.NVarChar, Value = model.TimeKeeperAccess }); }
            if (model.TimeReviewerAccess != null) { parameterList.Add(new SqlParameter() { ParameterName = "@TimeReviewerAccess", SqlDbType = SqlDbType.NVarChar, Value = model.TimeReviewerAccess }); }
            if (model.DefaultChargeCodeId != null) { parameterList.Add(new SqlParameter() { ParameterName = "@DefaultChargeCodeId", SqlDbType = SqlDbType.Int, Value = model.DefaultChargeCodeId }); }
            if (model.DefaultWorkFacility != null) { parameterList.Add(new SqlParameter() { ParameterName = "@DefaultWorkFacility", SqlDbType = SqlDbType.NVarChar, Value = model.DefaultWorkFacility }); }
            return parameterList;
        }
        private EmployeeListItem DataToListItem(DataRow row)
        {
            //Create a new model record
            EmployeeListItem listItem = new EmployeeListItem();

            //Save the data row values to the standard model properties
            listItem.CAC = row.GetText("CAC");
            listItem.FirstName = row.GetText("FirstName").Trim();
            listItem.MiddleName = row.GetText("MiddleName").Trim();
            listItem.LastName = row.GetText("LastName").Trim();
            listItem.PreferredName = row.GetText("PreferredName").Trim();
            listItem.Location = row.GetText("LocationDescription");

            return listItem;
        }
        #endregion

        #region Read

        public Employee GetEmployeeRecord(List<SqlParameter> parameterList)
        {
            Employee model = new Employee();
            try
            {
                //Create a new instance of the C# SQL Data access layer
                DatabaseFunctions DataFunctions = new DatabaseFunctions();

                parameterList.Add(new SqlParameter() { ParameterName = "@Task", SqlDbType = SqlDbType.NVarChar, Value = "GET_EMPLOYEE" });

                //Call the function to execute the list store procedure
                DataTable dt = DataFunctions.ExecuteQueryStoredProcedure(tasksStoredProcedure, parameterList, webConfigDataConnection);

                //Convert the data row to an employee model record
                if (dt.Rows.Count != 0)
                {
                    //Create a model from the datatable record
                    model = DataToModel(dt.Rows[0]);
                    //model = DataReaderToModelWTSEmployee(dataRecord);

                }
            }
            catch (Exception e)
            {
                //Throw any exceptions back to the calling process.
                throw e;
            }
            return model;
        }
        public Employee GetEmployeeRecord(string CACNumber)
        {
            Employee model = null;
            try
            {

                //Create a new instance of the C# SQL Data access layer
                DatabaseFunctions DataFunctions = new DatabaseFunctions();


                List<SqlParameter> parameterList = new List<SqlParameter>()
                {
                    new SqlParameter() {ParameterName = "@Task", SqlDbType = SqlDbType.NVarChar, Value = "GET_EMPLOYEE" },
                    new SqlParameter() {ParameterName = "@CACnumber", SqlDbType = SqlDbType.VarChar, Value = CACNumber }
                };

                //Call the function to execute the list store procedure
                DataTable dt = DataFunctions.ExecuteQueryStoredProcedure(tasksStoredProcedure, parameterList, webConfigDataConnection);

                //Convert the data row to an employee model record
                if (dt.Rows.Count != 0)
                {
                    //Create a model from the datatable record
                    model = DataToModel(dt.Rows[0]);
                    return model;
                }

                //If the employee record could not be found the check for terminted employees
                if (dt.Rows.Count == 0)
                {

                    List<SqlParameter> parameterListAll = new List<SqlParameter>()
                {
                    new SqlParameter() {ParameterName = "@Task", SqlDbType = SqlDbType.NVarChar, Value = "GET_EMPLOYEE" },
                    new SqlParameter() {ParameterName = "@CACnumber", SqlDbType = SqlDbType.VarChar, Value = CACNumber }
                };

                    DataTable dtAll = DataFunctions.ExecuteQueryStoredProcedure(taskEmployeeAll, parameterListAll, webConfigDataConnection); // Bug Needed [vwEmployees_All] 
                    if (dtAll.Rows.Count != 0)
                    {
                        //Create a model from the datatable record
                        model = DataToModel(dtAll.Rows[0]);
                        return model;
                    }
                }
            }
            catch (Exception e)
            {
                //Throw any exceptions back to the calling process.
                throw e;
            }
            return model;
        }
        public Employee GetEmployeeByName(string FullName)
        {
            Employee model = new Employee();
            try
            {
                //Create a new instance of the C# SQL Data access layer
                DatabaseFunctions DataFunctions = new DatabaseFunctions();

                List<SqlParameter> parameterList = new List<SqlParameter>()
                {
                    new SqlParameter() {ParameterName = "@Task", SqlDbType = SqlDbType.NVarChar, Value = "GET_EMPLOYEE_BY_FULLNAME" },
                    new SqlParameter() {ParameterName = "@FullName", SqlDbType = SqlDbType.VarChar, Value = FullName }
                };

                parameterList.Add(new SqlParameter() { ParameterName = "@Task", SqlDbType = SqlDbType.NVarChar, Value = "GET_EMPLOYEE_BY_FULLNAME" });

                //Call the function to execute the list store procedure
                DataTable dt = DataFunctions.ExecuteQueryStoredProcedure(tasksStoredProcedure, parameterList, webConfigDataConnection);

                //Convert the data row to an employee model record
                if (dt.Rows.Count != 0)
                {
                    //Create a model from the datatable record
                    model = DataToModel(dt.Rows[0]);
                    //model = DataReaderToModelWTSEmployee(dataRecord);

                }
            }
            catch (Exception e)
            {
                //Throw any exceptions back to the calling process.
                throw e;
            }
            return model;

        }
        public List<EmployeeListItem> ListEmployees()
        {
            try
            {
                List<EmployeeListItem> retList = new List<EmployeeListItem>();

                //Create a new instance of the C# SQL Data access layer
                DatabaseFunctions DataFunctions = new DatabaseFunctions();

                List<SqlParameter> parameterList = new List<SqlParameter>()
                {
                    new SqlParameter() {ParameterName = "@Task", SqlDbType = SqlDbType.NVarChar, Value = "LIST_EMPLOYEES" }
                };

                //Call the function to execute the list store procedure
                DataTable dt = DataFunctions.ExecuteQueryStoredProcedure(tasksStoredProcedure, parameterList, webConfigDataConnection);

                //Convert the data row to an employee model record
                foreach (DataRow row in dt.Rows)
                {
                    //Create a list item from the datatable record
                    retList.Add(DataToListItem(row));
                }

                return retList;
            }
            catch (Exception e)
            {
                //Throw any exceptions back to the calling process.
                throw e;
            }
        }
        public DataTable ListEDODays(List<SqlParameter> parameterList)
        {
            try
            {
                //Create a new instance of the C# SQL Data access layer
                DatabaseFunctions DataFunctions = new DatabaseFunctions();
                DataTable recordList = DataFunctions.ExecuteQueryStoredProcedure(usp_EDOList, parameterList, pimsConfigDataConnection);
                return recordList;
            }
            catch (Exception)
            {
                //Throw any exceptions back to the calling process.
                throw;
            }

        }

        #endregion

        public List<string> AutoCompleteEmployeeSearch(string EmployeeName)
        {
            try
            {
                //Build a parameter list for the search stored procedure.
                List<SqlParameter> ParameterList = new List<SqlParameter>();
                ParameterList.Add(new SqlParameter() { ParameterName = "@Task", SqlDbType = SqlDbType.VarChar, Value = "AUTOCOMPLETE_EMPLOYEE_NAMES" });
                ParameterList.Add(new SqlParameter() { ParameterName = "@Name", SqlDbType = SqlDbType.VarChar, Value = EmployeeName });

                DatabaseFunctions dataFunctions = new DatabaseFunctions();
                return dataFunctions.ExecuteReaderQueryStoredProcedure(tasksStoredProcedure, ParameterList, "EmployeeName", "MainConnection");
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public DataTable AutoCompleteEmployeeSearchTable(string EmployeeName)
        {
            try
            {
                //Build a parameter list for the search stored procedure.
                List<SqlParameter> ParameterList = new List<SqlParameter>();
                ParameterList.Add(new SqlParameter() { ParameterName = "@Task", SqlDbType = SqlDbType.VarChar, Value = "AUTOCOMPLETE_EMPLOYEE_NAMES" });
                ParameterList.Add(new SqlParameter() { ParameterName = "@Name", SqlDbType = SqlDbType.VarChar, Value = EmployeeName });

                DatabaseFunctions dataFunctions = new DatabaseFunctions();
                return dataFunctions.ExecuteQueryStoredProcedure(tasksStoredProcedure, ParameterList);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
