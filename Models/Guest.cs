using System.Collections.Generic;

namespace EventPlanner.Models
{
  public class Guest
    {
        public int GuestId { get; set; }
        public string GuestFirstName { get; set; }
        public string GuestLastName { get; set; }
        public List<Association> Visitors { get; set; }
    }
}
