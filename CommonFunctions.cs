using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CS_CommonDataAccessLayer;
using CS_CommonBusinessLayer;


namespace CS_CommonBusinessLayer
{
    public static class CommonFunctions
    {
        public static string GenerateHashID(string idTypeIdentifier, string year, string cacNumber)
        {
            string newUniqueId = "";

            //StockReorderId SD
            //PurchaseRequestId PR
            //PurchaseRequestDetailId PD
            //ShippingRequestId SH
            //ReceiverListNumber RL
            //RecapNumber REcap
            //RecapNumber CAP
            //StockReorderId SD
            //TransferNumber TR
            //TransferNumber TIN
            //RecapNumber REcap

            idTypeIdentifier = (idTypeIdentifier.ToUpper() + "XX").Left(2);
           
            if (idTypeIdentifier == "PR") { newUniqueId = GetUniqueID("PR"); return(newUniqueId) ; }
            if (idTypeIdentifier == "PD") { newUniqueId = GetUniqueID("PD"); return (newUniqueId); }
            if (idTypeIdentifier == "SH") { newUniqueId = GetUniqueID("SH"); return (newUniqueId); }
            if (idTypeIdentifier == "SD") { newUniqueId = GetUniqueID("SD"); return (newUniqueId); }

            /// string fiscalYearString = now.Month >= 10 ? (now.Year + 1).ToString() : now.Year.ToString();
            ///HERE or in the Caller? Adjust for FY starting October 1st so check current Month and increment year if its October, 11, 12 only:  
            /// Ignore year from parameter in:  
            ///year = year.Length >= 2 ? year.Right(2) : DateTime.Now.Year.ToString().Right(2);
            int fiscalYear = DateTime.Now.Month >= 10 ? DateTime.Now.Year + 1 : DateTime.Now.Year;
            year = fiscalYear.ToString().Right(2);

            cacNumber = cacNumber.Length == 10 ? char.ConvertFromUtf32(Convert.ToInt32(cacNumber.Substring(9, 1)) + 65) : "X";

            string hash = "";
            String chars = "0123456789ABCDEFGHJKLMNPQRSTUVWXYZ";
            DateTime now = DateTime.Now;
            int num = (now.DayOfYear * 86400) + (now.Hour * 3600) + (now.Minute * 60) + now.Second;

            while (num >= 34)
            {
                int r = num % 34;
                hash = chars[r] + hash;
                num = num / 34;
            }
            hash = chars[num] + hash;

            string newHashID = idTypeIdentifier + year + ("00000" + hash).Right(5) + cacNumber;
            return newHashID;
        }

        public static string GetJulianDate(DateTime SelectedDate, int FixedLength = 3)
        {

            DateTime startDate = new DateTime(SelectedDate.Year, 1, 1);
            string dayCount = Convert.ToString(SelectedDate.Date.Subtract(startDate.Date).Days + 1).PadLeft(FixedLength, (char)48);
            string selectedYear = SelectedDate.Year.ToString().Right(1);
            string julianDate = selectedYear + dayCount;
            return julianDate;

        }
        public static Dictionary<string, string> GetStatesList()
        {
            Dictionary<string, string> retList = new Dictionary<string, string>();

            retList.Add("AL", "Alabama");
            retList.Add("AK", "Alaska");
            retList.Add("AZ", "Arizona");
            retList.Add("AR", "Arkansas");
            retList.Add("CA", "California");
            retList.Add("CO", "Colorado");
            retList.Add("CT", "Connecticut");
            retList.Add("DE", "Delaware");
            retList.Add("DC", "District of Columbia");
            retList.Add("FL", "Florida");
            retList.Add("GA", "Georgia");
            retList.Add("HI", "Hawaii");
            retList.Add("ID", "Idaho");
            retList.Add("IL", "Illinois");
            retList.Add("IN", "Indiana");
            retList.Add("IA", "Iowa");
            retList.Add("KS", "Kansas");
            retList.Add("KY", "Kentucky");
            retList.Add("LA", "Louisiana");
            retList.Add("ME", "Maine");
            retList.Add("MD", "Maryland");
            retList.Add("MA", "Massachusetts");
            retList.Add("MI", "Michigan");
            retList.Add("MN", "Minnesota");
            retList.Add("MS", "Mississippi");
            retList.Add("MO", "Missouri");
            retList.Add("MT", "Montana");
            retList.Add("NE", "Nebraska");
            retList.Add("NV", "Nevada");
            retList.Add("NH", "New Hampshire");
            retList.Add("NJ", "New Jersey");
            retList.Add("NM", "New Mexico");
            retList.Add("NY", "New York");
            retList.Add("NC", "North Carolina");
            retList.Add("ND", "North Dakota");
            retList.Add("OH", "Ohio");
            retList.Add("OK", "Oklahoma");
            retList.Add("OR", "Oregon");
            retList.Add("PA", "Pennsylvania");
            retList.Add("RI", "Rhode Island");
            retList.Add("SC", "South Carolina");
            retList.Add("SD", "South Dakota");
            retList.Add("TN", "Tennessee");
            retList.Add("TX", "Texas");
            retList.Add("UT", "Utah");
            retList.Add("VT", "Vermont");
            retList.Add("VA", "Virginia");
            retList.Add("WA", "Washington");
            retList.Add("WV", "West Virginia");
            retList.Add("WI", "Wisconsin");
            retList.Add("WY", "Wyoming");

            return retList;
        }

        private static string GetUniqueID(string type)
        {
            try
            {
                //Build the parameter list
                List<SqlParameter> ParameterList = new List<SqlParameter>();
                ParameterList.Add(new SqlParameter() { ParameterName = "@Task", SqlDbType = SqlDbType.VarChar, Value = "GET_UNIQUI_ID" });
                ParameterList.Add(new SqlParameter() { ParameterName = "@RecordType", SqlDbType = SqlDbType.VarChar, Value = type });

                //Create a new instance of the C# SQL Data access layer
                DatabaseFunctions DataFunctions = new DatabaseFunctions();

                //Call the function to execute the list store procedure
                DataTable dataRecord = DataFunctions.ExecuteQueryStoredProcedure("spCoreUniqueId", ParameterList, "DefaultConnection");

                //Create a model from the datatable record
                string newID = dataRecord.Rows[0][1].ToString();
                return newID;
            }
            catch (Exception e)
            {
                //Throw any exceptions back to the calling process.
                throw e;
            }
        }

    }
}
