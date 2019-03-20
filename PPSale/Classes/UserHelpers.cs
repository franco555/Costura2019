using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PPSale.Models;
using PPSale.Models.Conexion;
using PPSale.Models.Globals;
using PPSale.Models.Other;
using System;
using System.Linq;
using System.Web.Configuration;

namespace PPSale.Classes
{
    public class UsersHelper : IDisposable
    {
        private static ApplicationDbContext userContext = new ApplicationDbContext();
        private static ConexionContext db = new ConexionContext();

        public static bool DeleteUser(string userName)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP= userManager.FindByEmail(userName);
            if (userASP==null)
            {
                return false;
            }

            var response=userManager.Delete(userASP);
            return response.Succeeded;
        }

        public static bool RemoveRol(string userName, string rolName)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByEmail(userName);
            if (userASP == null)
            {
                return false;
            }

            var response = userManager.RemoveFromRole(userASP.Id, rolName);     //var response=userManager.Delete(userASP); //Borra
            return response.Succeeded;
        }

        //Modificar Email de usuario
        public static bool UpdateUser(string currentUserName, string NewUserName)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByEmail(currentUserName);
            if (userASP == null)
            {
                return false;
            }

            userASP.UserName = NewUserName;
            userASP.Email = NewUserName;

            var response = userManager.Update(userASP);
            return response.Succeeded;
        }

        public static void CheckRole(string roleName,bool locked)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(userContext));

            // Verefica se esxiste rol, if not create it
            if (!roleManager.RoleExists(roleName))
            {
                roleManager.Create(new IdentityRole(roleName));

                var rol = db.Rols.Where(r=>r.Name==roleName).ToList();
                var response= CrateRol(roleName,locked);
             
            }
        }

        //Crear Rol en Alternativo
        public static Response CrateRol(string roleName, bool locked) {

            var rols = new Rol { RolId = 0, Name = roleName, Lock = locked, };
            db.Rols.Add(rols);

            var response = DBHelpers.SaveChage(db);

            return response;
        }

        //TODO:Falta
        public static void UpdateRole(string userName, string password, string roleName)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByEmail(userName);

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(userContext));
            var roleASP = roleManager.FindByName(roleName);

            if (userManager.IsInRole(userASP.Id, roleName))
            {
                //userManager.(user.Id, "Admin");
            }


            //var response = roleManager.Update(roleName);
            //return true;response.Succeeded;
        }
        
        public static void CheckSuperUser()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var email = WebConfigurationManager.AppSettings["AdminUser"];
            var password = WebConfigurationManager.AppSettings["AdminPassWord"];
            var userASP = userManager.FindByName(email);
            if (userASP == null)
            {
                CreateUserASP(email, "Admin", password);
                return;
            }
            userManager.AddToRole(userASP.Id, "Admin");
        }

        public static void CreateUserASP(string email,string roleName)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByEmail(email);
            if (userASP == null)
            {
                userASP = new ApplicationUser
                {
                    Email = email,
                    UserName = email,
                };

                userManager.Create(userASP, email);
           
            }

            CheckRole(roleName,false);
            userManager.AddToRole(userASP.Id, roleName);
        }
        
        public static void CreateUserASP(string email, string roleName, string password)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByEmail(email);

            if (userASP == null) {
                userASP = new ApplicationUser
                {
                    Email = email,
                    UserName = email,
                };

                userManager.Create(userASP, password);
            }

            CheckRole(roleName, false);
            userManager.AddToRole(userASP.Id, roleName);
        }

        /* public static async Task PasswordRecovery(string email)
         {
             var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
             var userASP = userManager.FindByEmail(email);
             if (userASP == null)
             {
                 return;
             }

             var user = db.Users.Where(tp => tp.UserName == email).FirstOrDefault();
             if (user == null)
             {
                 return;
             }

             var random = new Random();
             var newPassword = string.Format("{0}{1}{2:04}*",
                 user.FirstName.Trim().ToUpper().Substring(0, 1),
                 user.LastName.Trim().ToLower(),
                 random.Next(10000));

             userManager.RemovePassword(userASP.Id);
             userManager.AddPassword(userASP.Id, newPassword);

             var subject = "Taxes Password Recovery";
             var body = string.Format(@"
                 <h1>Taxes Password Recovery</h1>
                 <p>Yor new password is: <strong>{0}</strong></p>
                 <p>Please change it for one, that you remember easyly",
                 newPassword);

             await MailHelper.SendMail(email, subject, body);
         }*/

        /* copia de userCrateASPNET
          public static void CreateUserASP(string email, string roleName, string password)
    {
        var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));

        var userASP = new ApplicationUser
        {
            Email = email,
            UserName = email,
        };

        userManager.Create(userASP, password);
        userManager.AddToRole(userASP.Id, roleName);
    }
         */

        public void Dispose()
        {
            userContext.Dispose();
            db.Dispose();
        }
    }
}