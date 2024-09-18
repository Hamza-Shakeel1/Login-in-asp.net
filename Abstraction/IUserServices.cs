using Logins.Models;

namespace Logins.Abstraction
{
	public interface IUserServices
	{
		User? GetUser(string username, string password);
		void Register(User user);
	}
}
