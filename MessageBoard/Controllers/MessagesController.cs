using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MessageBoard.Models;
using System;
using System.Linq;

namespace MessageBoard.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class MessagesController : ControllerBase
  {
    private readonly MessageBoardContext _db;

    public MessagesController(MessageBoardContext db)
    {
      _db = db;
    }

    // [HttpGet]
    // public async Task<ActionResult<IEnumerable<Message>>> Get()
    // {
    //   return await _db.Messages.ToListAsync();
    // }

    [HttpPost]
    public async Task<ActionResult<Message>> Post(Message message)
    {
      _db.Messages.Add(message);
      await _db.SaveChangesAsync();

      return CreatedAtAction(nameof(GetMessage), new { id = message.MessageId }, message);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Message>> GetMessage(int id)
    {
      var message = await _db.Messages.FindAsync(id);

      if (message == null)
      {
        return NotFound();
      }
      return message;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Message>>> Get(string group)
    {
      var query = _db.Messages.AsQueryable();

      if (group != null)
      {
        query = query.Where(entry => entry.Group == group);
      }
      return await query.ToListAsync();
    }

  }
}