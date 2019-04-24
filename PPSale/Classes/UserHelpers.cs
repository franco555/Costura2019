using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PPSale.Models;
using PPSale.Models.Conexion;
using PPSale.Models.Globals;
using PPSale.Models.Other;
using PPSale.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;

namespace PPSale.Classes
{
    public class UsersHelper : IDisposable
    {
        private static ApplicationDbContext userContext = new ApplicationDbContext();
        private static ConexionContext db = new ConexionContext();
        private static FunctionHelpers fn = new FunctionHelpers();

        public static bool DeleteUser(string userName)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByEmail(userName);
            if (userASP == null)
            {
                return false;
            }

            var response = userManager.Delete(userASP);
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

        // Verefica si existe rol, if not create it
        public static void CheckRole(string roleName, bool IsSeed)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(userContext));


            if (!roleManager.RoleExists(roleName))
            {
                roleManager.Create(new IdentityRole(roleName));

                if (IsSeed)
                {
                    var rol = new Rol() { RolId = 0, Name = roleName, Lock = roleName == "Admin" ? true : false };
                    db.Rols.Add(rol);
                    var response = DBHelpers.SaveChage(db);
                }

            }
            return;
        }

        // para los modelos no ASP - CheckRole
        public void CheckRol(string roleName, bool IsSeed) { CheckRole(roleName, IsSeed); }
        public void UpdateRol(string oldNamerRol, string roleName) { UpdateRole(oldNamerRol, roleName); }

        //Modificar Rol
        public static void UpdateRole(string oldNamerRol, string roleName)
        {

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(userContext));
            var roleASP = roleManager.FindByName(oldNamerRol);

            if (roleASP != null)
            {
                roleASP.Name = roleName;
                var response = roleManager.UpdateAsync(roleASP);
            }

            return;
        }

        //Lista de roles
        public List<RolViewModel> listRole()
        {

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(userContext));
            var roles = roleManager.Roles.OrderBy(r => r.Name).ToList();

            var newRolView = new List<RolViewModel>();

            roles.ForEach(item => newRolView.Add(
                new RolViewModel()
                {
                    RolId = item.Id,
                    Name = item.Name,
                }
            ));

            return newRolView;
        }

        public static void CheckSuperUser()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var email = WebConfigurationManager.AppSettings["AdminUser"];
            var password = WebConfigurationManager.AppSettings["AdminPassWord"];
            var userASP = userManager.FindByName(email);
            if (userASP == null)
            {
                var rol = "Admin";
                CreateUserASP(email, password, rol);

                //Creado Usuario
                var user = fn.user;
                db.Users.Add(fn.user);
                var response = DBHelpers.SaveChage(db);

                //Asignando Rol y Usuario
                var rolId = db.Rols.Where(r => r.Name == rol).FirstOrDefault().RolId;
                var userRol = new AsingRolAndUser() { UserId = user.UserId, CompanyId = fn.CompanyId, RolId = rolId, Email = email, Lock = true };
                db.AsingRolAndUsers.Add(userRol);
                var response2 = DBHelpers.SaveChage(db);

                return;
            }
            userManager.AddToRole(userASP.Id, "Admin");
        }

        public static void CreateUserASP(string email, string roleName)
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
            userManager.AddToRole(userASP.Id, roleName);
        }

        public static void CreateUserASP(string email, string password, string roleName)
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

                userManager.Create(userASP, password);
            }

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