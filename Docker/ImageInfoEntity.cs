using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docker
{
    public class OpenStackImage 
    {
        public string status { get; set; }
        public string virtual_size { get; set; }
        public string description { get; set; }
        public List<string> tags { get; set; }
        public string container_format { get; set; }
        public string created_at { get; set; }
        public string size { get; set; }
        public string disk_format { get; set; }
        public string updated_at { get; set; }
        public string visibility { get; set; }
        public string self { get; set; }
        public int min_disk { get; set; }
        public bool protecte { get; set; }
        public string id { get; set; }
        public string file { get; set; }
        public string checksum { get; set; }
        public string owner { get; set; }
        public string schema { get; set; }
        public int min_ram { get; set; }
        public string name { get; set; }
    }

    public class ImagesInfo 
    {
        public List<OpenStackImage> images { get; set; }
        public string schema { get; set; }
        public string first { get; set; }
    }

}
