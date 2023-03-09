using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Segurplan.Core.Database;
using Segurplan.DataAccessLayer.Database.Identity;

namespace Segurplan.DataAccessLayer.DataAccessManagers {
    public class UserDam {

        protected readonly SegurplanContext context;
        protected readonly UserManager<User> userManager;

        public UserDam(SegurplanContext context, UserManager<User> userManager) {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<List<User>> SelectAll() {
            return await (from x in context.User
                          select x).ToListAsync();
        }

        public User SelectUserById(int userId) {
            return (from u in context.Users
                    where u.Id == userId
                    select u).FirstOrDefault();
        }

        public async Task<bool> DeleteUserAsync(int userId) {
            try {
                context.Users.Remove(SelectUserById(userId));
                var response = await context.SaveChangesAsync().ConfigureAwait(true) > 0 ? true : false;
                return response;
            } catch (Exception e) {
                return false;
            }
        }

        public async Task<bool> UpdateUser(User usr) {
            try {
                var response = await userManager.UpdateAsync(usr);
                return response.Succeeded;
            } catch (Exception e) {
                return false;
            }
        }

        public async Task<int> CreateUserAsync(User usr) {
            try {
                var response = await userManager.CreateAsync(usr);
                if (response.Succeeded) {
                    return usr.Id;
                } else {
                    return -1;
                }
            } catch (Exception) {
                return -1;
            }
        }

        public async Task<User> CheckUniqueFields(int id, string userName, string email) {
            try {
                var response = new List<int>();
                return await (from x in context.Users
                              where x.Email == email ||
                              x.UserName == userName &&
                              x.Id != id
                              select x).FirstOrDefaultAsync();
            } catch (Exception) {
                return null;
            }
        }

        public async Task<User> CheckUniqueFields(string userName, string email) {
            try {
                return await (from x in context.Users
                              where x.Email == email ||
                              x.UserName == userName
                              select x).FirstOrDefaultAsync();
            } catch (Exception) {
                return null;
            }
        }

        public string CompleteNameFromId(int userId) {
            return (from x in context.Users
                    where x.Id == userId
                    select x.CompleteName).FirstOrDefault().ToString();
        }
    }
}
