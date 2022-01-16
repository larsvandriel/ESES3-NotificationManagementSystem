using LoggingService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using NotificationManagementSystem.API.Filters;
using NotificationManagementSystem.Contracts;
using NotificationManagementSystem.Entities.Extensions;
using NotificationManagementSystem.Entities.Models;
using NotificationManagementSystem.Entities.Parameters;
using NotificationManagementSystem.Entities.ShapedEntities;

namespace NotificationManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly LinkGenerator _linkGenerator;

        public NotificationController(ILoggerManager logger, IRepositoryWrapper repository, LinkGenerator linkGenerator)
        {
            _logger = logger;
            _repository = repository;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public IActionResult GetNotifications([FromQuery] NotificationParameters notificationParameters)
        {
            try
            {
                var notifications = _repository.Notification.GetAllNotifications(notificationParameters);

                var metadata = new
                {
                    notifications.TotalCount,
                    notifications.PageSize,
                    notifications.CurrentPage,
                    notifications.TotalPages,
                    notifications.HasNext,
                    notifications.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                _logger.LogInfo($"Returned {notifications.Count} notifications from database.");

                var shapedNotifications = notifications.Select(i => i.Entity).ToList();

                var mediaType = (MediaTypeHeaderValue)HttpContext.Items["AcceptHeaderMediaType"];

                if (!mediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase))
                {
                    return Ok(shapedNotifications);
                }

                for (var index = 0; index < notifications.Count; index++)
                {
                    var brandLinks = CreateLinksForNotification(notifications[index].Id, notificationParameters.Fields);
                    shapedNotifications[index].Add("Links", brandLinks);
                }

                var notificationsWrapper = new LinkCollectionWrapper<Entity>(shapedNotifications);

                return Ok(CreateLinksForNotifications(notificationsWrapper));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllNotifications action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}", Name = "NotificationById")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public IActionResult GetNotificationById(Guid id, [FromQuery] string fields)
        {
            try
            {
                var notification = _repository.Notification.GetNotificationById(id, fields);

                if (notification.Id == Guid.Empty)
                {
                    _logger.LogError($"Notification with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                var mediaType = (MediaTypeHeaderValue)HttpContext.Items["AcceptHeaderMediaType"];

                if (!mediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase))
                {
                    _logger.LogInfo($"Returned shaped notification with id: {id}");
                    return Ok(notification.Entity);
                }

                notification.Entity.Add("Links", CreateLinksForNotification(notification.Id, fields));

                return Ok(notification.Entity);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wring inside GetNotificationById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public IActionResult CreateNotification([FromBody] Notification notification)
        {
            try
            {
                if (notification.IsObjectNull())
                {
                    _logger.LogError("Notification object sent from client is null.");
                    return BadRequest("Notification object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid notification object sent from client.");
                    return BadRequest("Invalid model object");
                }

                _repository.Notification.CreateNotification(notification);
                _repository.Save();

                return CreatedAtRoute("NotificationById", new { id = notification.Id }, notification);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateNotification action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateNotification(Guid id, [FromBody] Notification notification)
        {
            try
            {
                if (notification.IsObjectNull())
                {
                    _logger.LogError("Notification object sent from client is null.");
                    return BadRequest("Notification object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid notification object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var dbNotification = _repository.Notification.GetNotificationById(id);
                if (dbNotification.IsEmptyObject())
                {
                    _logger.LogError($"Notification with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _repository.Notification.UpdateNotification(dbNotification, notification);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateNotification action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteNotification(Guid id)
        {
            try
            {
                var notification = _repository.Notification.GetNotificationById(id);
                if (notification.IsEmptyObject())
                {
                    _logger.LogError($"Notification with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _repository.Notification.DeleteNotification(notification);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteNotification action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        private IEnumerable<Link> CreateLinksForNotification(Guid id, string fields = "")
        {
            var links = new List<Link>
            {
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetNotificationById), values: new {id, fields}), "self", "GET"),
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(DeleteNotification), values: new {id}), "delete_notification", "DELETE"),
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(UpdateNotification), values: new {id}), "update_notification", "PUT")
            };

            return links;
        }

        private LinkCollectionWrapper<Entity> CreateLinksForNotifications(LinkCollectionWrapper<Entity> notificationsWrapper)
        {
            notificationsWrapper.Links.Add(new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetNotifications), values: new { }), "self", "GET"));

            return notificationsWrapper;
        }
    }
}
