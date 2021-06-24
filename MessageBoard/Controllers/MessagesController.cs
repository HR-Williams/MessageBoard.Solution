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
    public async Task<ActionResult<IEnumerable<Message>>> Get(string group, DateTime postTime, string userName)
    {
      var query = _db.Messages.AsQueryable();

      if (group != null)
      {
        query = query.Where(entry => entry.Group == group);
      }
      
      if (userName != null)
      {
        query = query.Where(entry => entry.UserName == userName);
      }

      // if (postTime.ToString() != "00010101T000000")
      // {
      //   query = query.Where(entry => entry.PostTime == postTime);
      // }
            
      return await query.ToListAsync();
    }

    [HttpPut("{userName}/{id}")]
    public async Task<IActionResult> Put(int id, Message message, string userName)
    {
      if ((id != message.MessageId) && (userName != message.UserName))
      {
        return BadRequest();
      }
      _db.Entry(message).State = EntityState.Modified;
      try
      {
        await _db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if(!MessageExists(id))
        {
          return NotFound();
        }
        else
        {
          throw;
        }
      }
      return NoContent();
    }

      private bool MessageExists(int id)
      {
        return _db.Messages.Any(e => e.MessageId == id);
      }

      [HttpDelete("{userName}/{id}")]
      public async Task<IActionResult> DeleteMessage(int id)
      {
        var message = await _db.Messages.FindAsync(id);
        if(message == null)
        {
          return NotFound();
        }

        _db.Messages.Remove(message);
        await _db.SaveChangesAsync();
        return NoContent();
    

      }

  }
}