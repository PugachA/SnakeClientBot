using SnakeClient.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnakeClient.Models
{
    public class Graph
    {
        public Dictionary<PointDto, PointDto[]> Edges;
        private GameStateDto gameBoardDto;
        private IEnumerable<PointDto> obstacles;

        public Graph(GameStateDto gameBoardDto)
        {
            this.gameBoardDto = gameBoardDto;
            Edges = new Dictionary<PointDto, PointDto[]>();

            obstacles = GenarateObstacles(gameBoardDto);

            for (int i = 0; i < gameBoardDto.GameBoardSize.Width; i++)
                for (int j = 0; j < gameBoardDto.GameBoardSize.Height; j++)
                {
                    PointDto point = new PointDto(i, j);//здесь нужен direction
                    PointDto[] neighbors = GenereateNeighbors(point);
                    Edges.Add(point, neighbors);
                }
        }

        private IEnumerable<PointDto> GenarateObstacles(GameStateDto gameBoardDto)
        {
            List<PointDto> PointDtos = new List<PointDto>();

            foreach (PlayerStateDto player in gameBoardDto.Players)
            {
                if (player.Snake == null)
                    continue;

                if (player.IsSpawnProtected || player.Snake.Count() >= gameBoardDto.Snake.Count())
                    PointDtos.AddRange(player.Snake);
            }

            foreach (RectangleDto rectangle in gameBoardDto.Walls)
            {
                for (int i = 0; i < rectangle.Width; i++)
                    for (int j = 0; j < rectangle.Height; j++)
                    {
                        PointDto point = new PointDto(rectangle.X + i, rectangle.Y + j);
                        PointDtos.Add(point);
                    }

            }

            return PointDtos;
        }

        public PointDto[] Neighbors(PointDto id)
        {
            switch (id.Direction)
            {
                case Direction.Down:
                    return Edges[id].Where(p => p.Direction != Direction.Up).ToArray();
                case Direction.Up:
                    return Edges[id].Where(p => p.Direction != Direction.Down).ToArray();
                case Direction.Right:
                    return Edges[id].Where(p => p.Direction != Direction.Left).ToArray();
                case Direction.Left:
                    return Edges[id].Where(p => p.Direction != Direction.Right).ToArray();
                default:
                    return null;
            }
        }

        private PointDto[] GenereateNeighbors(PointDto point)
        {
            List<PointDto> neighbors = new List<PointDto>();
            neighbors.Add(new PointDto(point.X, point.Y - 1, Direction.Up));
            neighbors.Add(new PointDto(point.X, point.Y + 1, Direction.Down));
            neighbors.Add(new PointDto(point.X - 1, point.Y, Direction.Left));
            neighbors.Add(new PointDto(point.X + 1, point.Y, Direction.Right));
            return neighbors.Where(p => InBorders(p) && OutOfWalls(p)).ToArray();
        }

        private bool InBorders(PointDto PointDto) => !(PointDto.X < 0
            || PointDto.X > gameBoardDto.GameBoardSize.Width - 1
            || PointDto.Y < 0
            || PointDto.Y > gameBoardDto.GameBoardSize.Height - 1);

        private bool OutOfWalls(PointDto PointDto)
        {
            return !obstacles.Contains(PointDto);
        }

        public IEnumerable<PointDto> WideSearch(PointDto start, PointDto goal)
        {

            Queue<PointDto> frontier = new Queue<PointDto>();
            frontier.Enqueue(start);

            Dictionary<PointDto, PointDto> cameFrom = new Dictionary<PointDto, PointDto>();
            cameFrom.Add(start, start);

            while (frontier.Count > 0)
            {
                var current = frontier.Dequeue();

                if (current == goal)
                    break;

                foreach (var next in this.Neighbors(current))
                {
                    if (!cameFrom.ContainsKey(next))
                    {
                        frontier.Enqueue(next);
                        cameFrom.Add(next, current);
                    }
                }
            }

            //ищем путь
            PointDto current1 = goal;
            HashSet<PointDto> path = new HashSet<PointDto>();

            while (current1 != start)
            {
                if (cameFrom.TryGetValue(current1, out PointDto newCurrent))
                {
                    path.Add(cameFrom.Select(d => d.Key).Single(d => d == current1));
                    current1 = newCurrent;
                }
                else
                {
                    return null;
                }
            }

            return path;
        }
    }
}
