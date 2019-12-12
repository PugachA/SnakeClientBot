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