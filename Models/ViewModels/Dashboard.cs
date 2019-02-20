using System.Collections.Generic;
using EventPlanner.Models;

namespace EventPlanner
{
  public class Dashboard
  {
    public List<Wedding> Weddings { get; set; }
    public User User { get; set; }

    public Dashboard(List<Wedding> weddings, User user)
    {
      Weddings = weddings;
      User = user;
    }
  }
}