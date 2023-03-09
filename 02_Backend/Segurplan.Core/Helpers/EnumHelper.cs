using System;

namespace Segurplan.Core.Helpers {
    public class EnumHelper {

        /// <summary>
        /// from text to Enum 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="texto"></param>
        /// <returns></returns>
        public static T ToEnum<T>(string text) {
            try {
                return (T)Enum.Parse(typeof(T), text, true);
            } catch (Exception ex) {
                throw ex;
            }
        }
    }
}
