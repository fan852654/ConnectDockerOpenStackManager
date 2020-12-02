using System.Threading.Tasks;

namespace Docker
{
    public interface ISample
    {
        void PrintTasks();
        Task Run(string identityEndpoint, string username, string password, string project, string region);
    }
}
