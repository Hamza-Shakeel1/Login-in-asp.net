using Logins.Abstraction;
using Logins.Models;

namespace Logins.Service
{
	public class UserServices:IUserServices
	{
		private readonly HmsContext _context;
        private readonly IWebHostEnvironment _environment;
        public UserServices(HmsContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        public User? GetUser(string username, string password) 
        {
            return _context.Users.
                FirstOrDefault(x=>x.Email==username && x.Password==password);
        }
        public void Register( User user)
        {
            user.Id = Guid.NewGuid();
            _context.Users.Add(user);
            _context.SaveChanges();
        }
        public async Task SaveUserAsync(User user, IFormFile profilePicture)
        {
            if (profilePicture != null)
            {
                string pictureName = $"pp-{Guid.NewGuid()}";
                string filePath = Path.Combine(_environment.WebRootPath, "images", pictureName);

                // Save the file to wwwroot/images
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await profilePicture.CopyToAsync(stream);
                }

                user.Picture = pictureName; // Save picture name in the database
            }

            // Add or update the user in the database
            if (user.Id == Guid.Empty)
            {
                user.Id = Guid.NewGuid();
                _context.Users.Add(user);
            }
            else
            {
                _context.Users.Update(user);
            }

            await _context.SaveChangesAsync();
        }
    }
}
