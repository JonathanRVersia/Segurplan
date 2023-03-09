namespace Segurplan.Core.Actions.Administration.Users {
    public class UsersListTableState {
        public UsersListTableState() {

        }

        public UsersListTableState(int indexPage, int pageRows, string oderMode, string orderBy) {
            IndexPage = indexPage;
            PageRows = pageRows;
            OrderMode = oderMode;
            OrderBy = orderBy;

        }

        public int IndexPage { get; set; } = 0;
        public int PageRows { get; set; } = 15;
        public string OrderMode { get; set; } = "asc";
        public string OrderBy { get; set; } = "Id";


    }

}

