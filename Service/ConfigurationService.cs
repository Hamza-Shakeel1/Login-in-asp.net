using Logins.Abstraction;
using Logins.Models;

namespace Logins.Service
{
	public class ConfigurationService:IConfigurationServices
	{
		private readonly IConfiguration _configuration;

		public ConfigurationService(IConfiguration configuration)
		{
			_configuration = configuration;
		}



		public int GetExpiryTime() 
		{
			int expiry = _configuration.GetValue<int>("Authentication:ExpiryTime");
			return expiry;
		}



	}
}
