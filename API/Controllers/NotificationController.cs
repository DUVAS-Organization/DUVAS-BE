using DTO;
using Microsoft.AspNetCore.Mvc;
using Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationRepository _repository;

        public NotificationController(INotificationRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<NotificationDTO>>> GetNotificationsByUser(int userId)
        {
            try
            {
                var notifications = await _repository.GetNotificationsByUserAsync(userId);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("unread/{userId}")]
        public async Task<ActionResult<List<NotificationDTO>>> GetNotificationUnreadByUser(int userId)
        {
            try
            {
                var notifications = await _repository.GetNotificationUnreadByUserAsync(userId);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<NotificationDTO>>> GetAllNotifications()
        {
            try
            {
                var notifications = await _repository.GetAllNotificationsAsync();
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("type/{type}")]
        public async Task<ActionResult<List<NotificationDTO>>> GetNotificationsByType(string type)
        {
            try
            {
                var notifications = await _repository.GetNotificationsByTypeAsync(type);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("mark-as-read/{notificationId}")]
        public async Task<ActionResult> MarkAsRead(int notificationId)
        {
            try
            {
                await _repository.MarkAsReadAsync(notificationId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{notificationId}")]
        public async Task<ActionResult> DeleteNotification(int notificationId)
        {
            try
            {
                await _repository.DeleteNotificationAsync(notificationId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("count-unread/{userId}")]
        public async Task<ActionResult<int>> CountUnreadNotifications(int userId)
        {
            try
            {
                var count = await _repository.CountUnreadNotificationsAsync(userId);
                return Ok(count);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}