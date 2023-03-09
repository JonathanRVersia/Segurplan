using System;
using System.Collections.Generic;
using System.Linq;
using Segurplan.DataAccessLayer.Database.Identity;

namespace OldDBDataMigrator.Utils {
    public class SeedUtils {

        /// <summary>
        /// Check if user exists in newDB and returns his Id, if does't returns default (1)
        /// </summary>
        /// <param name="users">Users from new DB</param>
        /// <param name="usuarios">Users from original DB</param>
        /// <param name="oldDBUserId">userId from original DB</param>
        /// <returns></returns>
        public int GetUserIdFromNewDB(List<User>users, List<ProduccionDBModels.Usuarios> usuarios, int oldDBUserId) {

            var nombreUsuario = usuarios.Where(x => x.Id == oldDBUserId).Select(x => x.Usuario).FirstOrDefault();

            if (!string.IsNullOrEmpty(nombreUsuario)) {
                var user = users.Where(x => x.UserName.ToUpper() == nombreUsuario.ToUpper()).FirstOrDefault();

                if (user != null) {
                    return user.Id;
                }
            }

            return 1;
        }

        public void PrintErrorMessage(string message) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public void PrintSuccessMessage(string message) {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public string PrintMessageAndReadLine(string message) {
            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.WriteLine(message);

            Console.ResetColor();

            return Console.ReadLine();
        }

        public string PrintMessageAndReadKey(string message) {
            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.WriteLine(message);

            Console.ResetColor();

            return Console.ReadKey().KeyChar.ToString();
        }
    }
}
