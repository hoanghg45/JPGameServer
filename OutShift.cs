//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JPGame
{
    using System;
    using System.Collections.Generic;
    
    public partial class OutShift
    {
        public int Id { get; set; }
        public Nullable<int> IdInShift { get; set; }
        public string IdUsers { get; set; }
        public Nullable<int> Cashiers { get; set; }
        public Nullable<double> RealMoneySale { get; set; }
        public Nullable<double> EndShiftMoney { get; set; }
        public Nullable<double> FirstShiftMoney { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public string ModifyBy { get; set; }
        public Nullable<bool> Status { get; set; }
    }
}
