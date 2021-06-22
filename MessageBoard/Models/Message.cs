using System;

namespace MessageBoard.Models
{
  public class Message
  {
    public int MessageId { get; set; }
    public string Group { get; set; }
    public string UserName { get; set; }
    public DateTime PostTime { get; set; }
  }
}
