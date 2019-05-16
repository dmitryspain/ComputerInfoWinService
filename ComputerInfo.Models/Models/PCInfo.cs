using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerInfo.Models.Models
{
    public class PCInfo : BaseEntity
    {
        public int CpuLoad { get; set; }
        public int RamLoad { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public List<string> Users { get; set; }
    }
}
