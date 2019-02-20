namespace EventPlanner.Models
{
  public class Association
    {
        public int AssociationId { get; set; }
        public int WeddingId { get; set; } 
        public int GuestId { get; set; } 
        public Wedding Wedding { get; set; } 
        public Guest Guest { get; set; } 
    }
}
