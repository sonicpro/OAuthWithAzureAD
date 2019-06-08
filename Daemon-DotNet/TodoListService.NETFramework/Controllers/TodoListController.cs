using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using TodoListService.NETFramework.Models;

namespace TodoListService.NETFramework.Controllers
{
	[Authorize]
	public class TodoListController : ApiController
	{
		//
		// To Do list for all users. Stored in memory, therefore it will go away when the service is cycled.
		static ConcurrentBag<TodoItem> todoBag = new ConcurrentBag<TodoItem>();

		// GET api/todolist
		public IEnumerable<TodoItem> Get()
		{
			//
			// The Role claim tells you what permissions the client application has through the role defined for this application in Azure AD.
			// In this case we look for a role value of access_as_application, or full access as the daemon (no logged-in user is required).
			//
			Claim roleClaim = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
			if (roleClaim != null)
			{
				if (roleClaim.Value != "access_as_application")
				{
					throw new HttpResponseException(new HttpResponseMessage
					{
						StatusCode = HttpStatusCode.Unauthorized,
						ReasonPhrase = "The scope claim does not contain 'access_as_application' or scope claim not found."
					});
				}
			}

			// A user's To Do List is keyed off the NameIdentifier claim, which contains an immutable, unique identifier for the user.
			Claim subject = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier);

			return from todo in todoBag
				   where todo.Owner == subject.Value
				   select todo;
		}

		// POST api/todolist
		public void Post(TodoItem todo)
		{
			Claim roleClaim = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
			if (roleClaim != null)
			{
				if (roleClaim.Value != "access_as_application")
				{
					throw new HttpResponseException(new HttpResponseMessage
					{
						StatusCode = HttpStatusCode.Unauthorized,
						ReasonPhrase = "The scope claim does not contain 'access_as_application' or scope claim not found."
					});
				}
			}

			if (todo != null && !string.IsNullOrWhiteSpace(todo.Title))
			{
				todoBag.Add(new TodoItem
				{
					Title = todo.Title,
					Owner = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value
				});
			}
		}
	}
}