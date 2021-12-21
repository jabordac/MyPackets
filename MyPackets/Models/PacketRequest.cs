using System.Collections.Generic;

namespace MyPackets.Models
{
    public class PacketRequest
    {
        public List<int> lstPaquetes { get; set; }
        public int tamanioCamion { get; set; }
    }
}
