using Forum_v1.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System.IO;
using System.Web;

[assembly: OwinStartupAttribute(typeof(Forum_v1.Startup))]
namespace Forum_v1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createAdminUserAndApplicationRoles();
        }

        private void createAdminUserAndApplicationRoles
()      {
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new
        RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>( new
        UserStore<ApplicationUser>(context));
            // Se adauga rolurile aplicatiei
            if(!roleManager.RoleExists("Administrator"))
            {
                // Se adauga rolul de administrator
                var role = new IdentityRole();
                role.Name = "Administrator";
                roleManager.Create(role);
                // se adauga utilizatorul administrator
                var user = new ApplicationUser();
                user.UserName = "admin@admin.com";
                user.Email ="admin@admin.com";
                user.FirstName = "David";
                user.LastName = "Jitca";
                user.Adress = "ad s";
                user.City = "Bucuresti";
                user.State = "Romania";

                string fileName = System.Web.Hosting.HostingEnvironment.MapPath(@"/Images/noImg.png");
                byte[] imageData = null;
                FileInfo fileInfo = new FileInfo(fileName);
                long imageFileLength = fileInfo.Length;
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                imageData = br.ReadBytes((int)imageFileLength);

                user.UserPhoto = imageData;

                var adminCreated = UserManager.Create(user,"Admin1.");
                 if(adminCreated.Succeeded)
                 {
                      UserManager.AddToRole(user.Id,"Administrator");
                 }
            }
            if(!roleManager.RoleExists("Moderator"))
            {
                var role = new IdentityRole(); role.Name = "Moderator";
                roleManager.Create(role);
            }
            if(!roleManager.RoleExists("User"))
            {
                var role = new IdentityRole();
                role.Name = "User";
                roleManager.Create(role);
            }
        }
    }
}
