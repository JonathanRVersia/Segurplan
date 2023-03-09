using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Segurplan.Web.Utils {

    public static class SessionHelper {

        public const string AVA_ACTIVITIES_SESSION_ID = "availableActivities";
        public const string SEL_ACTIVITIES_SESSION_ID = "selectedActivities";
        public const string SEL_ARTICLES_SESSION_ID = "selectedArticles";
        public const string BUDGET_INFO_SESSION_ID = "budget";
        public const string ALL_PLANS = "allPlans";
        public const string CUSTOM_SUBCHAPTERS = "customSubChapters";
        public const string ACTIVITY_VERSION = "activityVersion";
        public const string ACTIVITY_SELECTED_DATA = "activitySelectedData";

        public static void SetObjectAsJson(this ISession session, string key, object value) {
            var str = JsonConvert.SerializeObject(value);
            session.SetString(key, str);
        }

        public static T GetObjectFromJson<T>(this ISession session, string key) {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public static IEnumerable<T> GetListFromJson<T>(this ISession session, string key) {
            var value = session.GetString(key);
            return value == null ? default(IEnumerable<T>) : JsonConvert.DeserializeObject<IEnumerable<T>>(value);
        }

        public static void RemoveActivities(this ISession session) {

            if (session != null) {
                
                session.Remove(AVA_ACTIVITIES_SESSION_ID);
                session.Remove(SEL_ACTIVITIES_SESSION_ID);
                session.Remove(CUSTOM_SUBCHAPTERS);
            }            
        }

        public static void RemoveSelectedActivities(this ISession session) {

            if (session != null)
                session.Remove(SEL_ACTIVITIES_SESSION_ID);

        }

        public static void RemoveCustomSubChapters(this ISession session) {

            if (session != null)
                session.Remove(CUSTOM_SUBCHAPTERS);
        }
    }
}
