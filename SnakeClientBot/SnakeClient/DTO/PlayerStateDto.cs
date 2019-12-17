using System;
using System.Collections.Generic;
using System.Text;

namespace SnakeClient.DTO
{
    public class PlayerStateDto
    {
        public string Name { get; set; }
        public bool IsSpawnProtected { get; set; }
        public IEnumerable<PointDto> Snake { get; set; }
    }
}
