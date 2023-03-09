using System.Threading.Tasks;

namespace OldDBDataMigrator {
    public interface ISeedInitializer {
        Task Seed();
        Task GetOriginalData();
        void Convert();
        Task SetDestinationData();
    }
}
