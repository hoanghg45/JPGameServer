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
    
    public partial class NFCReader
    {
        public string ReaderID { get; set; }
        public string ReaderName { get; set; }
        public Nullable<int> Cashier { get; set; }
    
        public virtual Cashier Cashier1 { get; set; }
    }
}