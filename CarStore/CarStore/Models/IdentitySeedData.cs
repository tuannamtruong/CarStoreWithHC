using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace CarStore.Models
{
    public class IdentitySeedData
    {
        private const string adminUser = "Admin";
        private const string adminPassword = "Secret123$";
        public static async Task EnsurePopulated(UserManager<IdentityUser> userManager)
        {
            IdentityUser user = await userManager.FindByIdAsync(adminUser).ConfigureAwait(true);
            //IdentityUser user = await userManager.FindByIdAsync(adminUser);
            if (user == null)
            {
                user = new IdentityUser("Admin");
                await userManager.CreateAsync(user, adminPassword).ConfigureAwait(true);
            }
        }
    }
}