//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BusTicket.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class T_BusDetails
    {
        public T_BusDetails()
        {
            this.T_JourneyDetails = new HashSet<T_JourneyDetails>();
            this.T_AvlSeats = new HashSet<T_AvlSeats>();
        }
    
        public int BusId { get; set; }
        public string BusRegNo { get; set; }
        public string BusType { get; set; }
        public int BusCapacity { get; set; }
    
        public virtual ICollection<T_JourneyDetails> T_JourneyDetails { get; set; }
        public virtual ICollection<T_AvlSeats> T_AvlSeats { get; set; }
    }
}
