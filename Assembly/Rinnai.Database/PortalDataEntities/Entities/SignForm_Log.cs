//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace PortalDataEntities.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class SignForm_Log
    {
        public int sn { get; set; }
        public string SignDocID_FK { get; set; }
        public Nullable<int> FormID_FK { get; set; }
        public string EmployeeID_FK { get; set; }
        public Nullable<System.DateTime> SendDate { get; set; }
        public string CurrentSignLevelDeptID_FK { get; set; }
        public Nullable<int> FinalStatus { get; set; }
        public Nullable<int> Remainder { get; set; }
        public Nullable<bool> AutoInsert { get; set; }
        public string Creator_Main { get; set; }
        public Nullable<System.DateTime> CreateDate_Main { get; set; }
        public string Modifier_Main { get; set; }
        public Nullable<System.DateTime> ModifyDate_Main { get; set; }
        public string DetailSignDocID_FK { get; set; }
        public string ChiefID_FK { get; set; }
        public string Remark { get; set; }
        public Nullable<int> Status { get; set; }
        public string Creator_Detail { get; set; }
        public Nullable<System.DateTime> CreateDate_Detail { get; set; }
        public string Modifier_Detail { get; set; }
        public Nullable<System.DateTime> ModifyDate_Detail { get; set; }
        public System.DateTime LogDatetime { get; set; }
    }
}
