using SnakeClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace SnakeClient.DTO
{
    public class DirectionDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Direction Direction { get; set; }
        public string Token { get; set; }
    }
}
