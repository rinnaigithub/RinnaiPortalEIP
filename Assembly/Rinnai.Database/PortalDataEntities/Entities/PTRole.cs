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
    
    public partial class PTRole
    {
        public int ID { get; set; }
        public string ROLE_NM { get; set; }
        public string ROLE_DESC { get; set; }
        public string ROLE_CD { get; set; }
        public Nullable<bool> DEL_FG { get; set; }
        public string BUD_USRID { get; set; }
        public Nullable<System.DateTime> BUD_DTM { get; set; }
        public string UPD_USRID { get; set; }
        public Nullable<System.DateTime> UPD_DTM { get; set; }
    }
}
