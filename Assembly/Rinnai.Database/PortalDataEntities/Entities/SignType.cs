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
    
    public partial class SignType
    {
        public SignType()
        {
            this.SignForm_Main = new HashSet<SignForm_Main>();
        }
    
        public int FormID { get; set; }
        public string FormType { get; set; }
        public string SignID_FK { get; set; }
        public string FormSeries { get; set; }
        public string FilingDepartmentID_FK { get; set; }
        public string Creator { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string Modifier { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
    
        public virtual ICollection<SignForm_Main> SignForm_Main { get; set; }
        public virtual SignProcedure SignProcedure { get; set; }
    }
}
