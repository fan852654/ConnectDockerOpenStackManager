using Docker.DotNet;
using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockerCarryOut
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }
    class DockerOut
    {
        async Task Docker()
        {
            DockerClient client = new DockerClientConfiguration(new Uri("http://192.168.0.100:2375"))
     .CreateClient();
            IList<ContainerListResponse> containers = await client.Containers.ListContainersAsync(
             new ContainersListParameters()
             {
                   Limit = 10,
             });
        }
    }
}
