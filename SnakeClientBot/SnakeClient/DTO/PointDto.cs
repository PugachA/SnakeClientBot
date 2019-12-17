using SnakeClient.Models;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SnakeClient.DTO
{
    public class PointDto
    {
        public int X { get; set; }

        public int Y { get; set; }

        [JsonIgnore]
        public Direction Direction { get; set; }

        public PointDto()
        {
        }

        public PointDto(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public PointDto(int x, int y, Direction direction)
        {
            this.X = x;
            this.Y = y;
            this.Direction = direction;
        }

        public static bool operator ==(PointDto p1, PointDto p2)
        {
            if (p2 is null && p1 is null)
                return true;

            if ((p1 is null) ^ (p2 is null))
                return false;

            return p1.X == p2.X && p1.Y == p2.Y;
        }

        public static bool operator !=(PointDto p1, PointDto p2)
        {
            if (p2 is null || p1 is null)
                return true;

            return p1.X != p2.X || p1.Y != p2.Y;
        }

        public override bool Equals(object obj)
        {
            return obj is PointDto point &&
                   X == point.X &&
                   Y == point.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }


    }
}