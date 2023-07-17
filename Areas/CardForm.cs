using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JPGame.Areas
{
    public class CardForm
    {
        public string partNumber { get; set; }
        public int ordinal { get; set; }
        public string partName { get; set; }
        public string unit { get; set; }
        public string origin { get; set; }
        public string manufacturer { get; set; }
        public int scanOption { get; set; }
        public int scanSerialOption { get; set; }
        public string status { get; set; }
        public string createdUserID { get; set; }
        public DateTime createdDate { get; set; }
        public string updatedUserID { get; set; }
        public DateTime? updatedDate { get; set; }
    }
}