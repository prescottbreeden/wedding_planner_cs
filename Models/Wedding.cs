using System;
using System.Collections.Generic;

namespace EventPlanner.Models
{
  public class Wedding
    {
        public int WeddingId { get; set; }
        public string WedderOneFirstName { get; set; } 
        public string WedderOneLastName { get; set; } 
        public string WedderTwoFirstName { get; set; } 
        public string WedderTwoLastName { get; set; } 
        public DateTime Date { get; set; }
        public string PlaceName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int Zipcode { get; set; }
        public List<Association> Wedders { get; set; }
    }
}
