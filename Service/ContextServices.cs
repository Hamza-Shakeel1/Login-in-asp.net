using Logins.Abstraction;

namespace Logins.Service
{
	public class ContextServices:IContextServices
	{
		private readonly IHttpContextAccessor _contextAccessor;

		public ContextServices(IHttpContextAccessor contextAccessor)
		{
			_contextAccessor = contextAccessor;
		}


		public bool IsUserLoggedIn() 
		{
			return _contextAccessor?
				.HttpContext?
				.User?
				.Identity?
				.IsAuthenticated??true;
		}
	
	
	}

}