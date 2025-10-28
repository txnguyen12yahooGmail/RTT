using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_CommonBusinessLayer
{
    public class Employee
    {
        private string mvarcac;
        private string mvaraccountingid;

        public string CAC { get { return mvarcac; } set { mvarcac = value.Trim().ToUpper(); } }
        public string UserId { get; set; }
        public string AccountingId { get { return mvaraccountingid; } set { mvaraccountingid = value.Trim().ToUpper(); } }
        public string SupervisorAccountingId { get; set; }
        public string SupervisorCAC { get; set; }
        public string EmployeeStatus { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PreferredName { get; set; }
        public string FullName
        {
            get
            {
                string CompleteName = "";
                
                if (!string.IsNullOrWhiteSpace(PreferredName)) { CompleteName = "(" + PreferredName.Trim() + ") "; }
                if (!string.IsNullOrWhiteSpace(FirstName)) { CompleteName = CompleteName + FirstName.Trim() + " "; }
                if (!string.IsNullOrWhiteSpace(MiddleName)) { CompleteName = CompleteName + MiddleName.Trim() + " "; }
                if (!string.IsNullOrWhiteSpace(LastName)) { CompleteName = CompleteName + LastName.Trim(); }
                return CompleteName;
            }
        }
        public string HomeFacility { get; set; }
        public string Department { get; set; }
        public string Email { get; set; }
        public string WorkPhone { get; set; }
        public string Company { get; set; }
        public decimal? VacationBalance { get; set; }
        public decimal? VacationBalanceBeginning { get; set; }
        public decimal? SickLeaveBalance { get; set; }
        public decimal? PersonalLeaveBalance { get; set; }
        public decimal? PersonalLeaveBalanceBeginning { get; set; }
        public DateTime? AccruedLastUpdate { get; set; }
        public int? WorkScheduleCode { get; set; }
        public string TimeKeeperAccess { get; set; }
        public string TimeReviewerAccess { get; set; }
        public int? DefaultChargeCodeId { get; set; }
        public string DefaultWorkFacility { get; set; }
        public bool IsEmployee { get; set; }
        public bool IsGovUser { get; set; }
        public bool IsSupervisor { get; set; }
        public bool IsDirector { get; set; }

    }

    public class EmployeeListItem
    {
        #region Fields
        private string _CAC = "";
        private string _FirstName = "";
        private string _MiddleName = "";
        private string _LastName = "";
        private string _PreferredName = "";
        private string _Location = "";
        #endregion

        #region Properties
        public string CAC
        {
            get { return _CAC; }
            set { _CAC = value; }
        }
        public string FirstName
        {
            get { return _FirstName; }
            set { _FirstName = value; }
        }
        public string MiddleName
        {
            get { return _MiddleName; }
            set { _MiddleName = value; }
        }
        public string LastName
        {
            get { return _LastName; }
            set { _LastName = value; }
        }
        public string PreferredName
        {
            get { return _PreferredName; }
            set { _PreferredName = value; }
        }
        public string Location
        {
            get { return _Location; }
            set { _Location = value; }
        }
        #endregion
    }
}
