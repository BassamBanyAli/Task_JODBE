//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Task_JODBE.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ExcelImportLog
    {
        public int LogID { get; set; }
        public Nullable<System.DateTime> ImportStartTime { get; set; }
        public Nullable<System.DateTime> ImportEndTime { get; set; }
        public Nullable<int> TotalRecords { get; set; }
        public Nullable<int> ImportedRecords { get; set; }
        public Nullable<int> ErrorRecords { get; set; }
        public string Status { get; set; }
    }
}
