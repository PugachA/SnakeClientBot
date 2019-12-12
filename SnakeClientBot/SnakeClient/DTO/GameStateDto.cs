using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace SnakeClient.DTO
{
    public class GameStateDto
    {
        [JsonPropertyName("isStarted")]
        public bool IsStarted { get; set; }

        [JsonPropertyName("isPaused")]
        public bool IsPaused { get; set; }

        [JsonPropertyName("roundNumber")]
        public int RoundNumber { get; set; }

        [JsonPropertyName("turnNumber")]
        public int TurnNumber { get; set; }

        [JsonPropertyName("turnTimeMilliseconds")]
        public int TurnTimeMilliseconds { get; set; }

        [JsonPropertyName("timeUntilNextTurnMilliseconds")]
        public int TimeUntilNextTurnMilliseconds { get; set; }

        [JsonPropertyName("gameBoardSize")]
        public SizeDto GameBoardSize { get; set; }

        [JsonPropertyName("maxFood")]
        public int MaxFood { get; set; }

        [JsonPropertyName("players")] 
        public IEnumerable<PlayerStateDto> Players { get; set; }

        [JsonPropertyName("snake")]
        public IEnumerable<PointDto> Snake { get; set; }

        [JsonPropertyName("food")]
        public IEnumerable<PointDto> Food { get; set; }

        [JsonPropertyName("walls")]
        public IEnumerable<RectangleDto> Walls { get; set; }
    }
}
public class Rootobject
{
    public bool isStarted { get; set; }
    public bool isPaused { get; set; }
    public int roundNumber { get; set; }
    public int turnNumber { get; set; }
    public int turnTimeMilliseconds { get; set; }
    public int timeUntilNextTurnMilliseconds { get; set; }
    public Gameboardsize gameBoardSize { get; set; }
    public int maxFood { get; set; }
    public Player[] players { get; set; }
    public object snake { get; set; }
    public Food[] food { get; set; }
    public Wall[] walls { get; set; }
}

public class Gameboardsize
{
    public int width { get; set; }
    public int height { get; set; }
}

public class Player
{
    public string name { get; set; }
    public bool isSpawnProtected { get; set; }
    public object snake { get; set; }
}

public class Food
{
    public int x { get; set; }
    public int y { get; set; }
}

public class Wall
{
    public int x { get; set; }
    public int y { get; set; }
    public int width { get; set; }
    public int height { get; set; }
}
