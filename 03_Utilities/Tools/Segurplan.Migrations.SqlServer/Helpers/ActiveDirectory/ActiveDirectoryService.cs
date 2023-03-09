//using System;
//using System.Collections.Generic;
//using System.DirectoryServices;

//namespace Segurplan.Migrations.SqlServer.Helpers.ActiveDirectory {

//    public class ActiveDirectoryService {

//        private ActiveDirectoryOptions activeDirectoryOptions;

//        public ActiveDirectoryService(ActiveDirectoryOptions activeDirectoryOptions) {
//            this.activeDirectoryOptions = activeDirectoryOptions;
//        }

//        public List<ActiveDirectoryInfo> GetActiveDirectoryUsers(Dictionary<string, string> usersSearch) {
//            var usersInfo = new List<ActiveDirectoryInfo>();

//            SearchResult searchResult;

//            var searcher = new DirectorySearcher(new DirectoryEntry(activeDirectoryOptions.ConnectionString) {
//                Username = activeDirectoryOptions.UserName,
//                Password = activeDirectoryOptions.UserPassword
//            });

//            foreach (var item in usersSearch) {
//                ActiveDirectoryInfo userInfo = new ActiveDirectoryInfo();

//                searcher.Filter = string.Format(activeDirectoryOptions.ActiveDirectoryFilter, item.Key);

//                try {
//                    searchResult = searcher.FindOne();
//                } catch (Exception ex) {
//                    searchResult = null;
//                }


//                if (searchResult != null) {

//                    foreach (string property in searchResult.Properties.PropertyNames) {
//                        switch (property.ToUpper()) {
//                            case "MAIL":
//                                userInfo.UserEmail = searchResult.Properties[property][0].ToString();
//                                break;
//                            case "OBJECTGUID":
//                                byte[] objectGuid = (byte[])searchResult.Properties[property][0];
//                                userInfo.UserGuid = new Guid(objectGuid).ToString();
//                                break;
//                            case "SAMACCOUNTNAME":
//                                userInfo.UserName = searchResult.Properties[property][0].ToString();
//                                break;
//                            case "NAME":
//                                userInfo.Name = searchResult.Properties[property][0].ToString();
//                                break;
//                        }
//                    }
//                    usersInfo.Add(userInfo);
//                }
//            }

//            return usersInfo;
//        }


//    }
//}


