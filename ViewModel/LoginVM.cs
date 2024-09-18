namespace Logins.ViewModel
{
	public class LoginVM
	{
		public string UserName { get; set; }=string.Empty;
		public string Password { get; set; } = string.Empty;

		public bool KeepLoggedIn { get; set; }
	}
}
