using NLog;
using SnakeClient.DTO;
using SnakeClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnakeClient.GraphAlgorithms
{
    public class AutomaticControl
    {
        private Direction lastDirection;
        private PointDto nearestFood;
        private readonly Logger logger = LogManager.GetLogger(nameof(AutomaticControl));

        public AutomaticControl()
        {
            this.lastDirection = Direction.Up;
            this.nearestFood = null;
        }

        public Direction Control(GameStateDto gameStateDto)
        {
            Graph graph = new Graph(gameStateDto);

            if (gameStateDto.Snake == null)
                return lastDirection;

            PointDto startPoint = gameStateDto.Snake.First();
            startPoint.Direction = lastDirection;

            //сделать проверку на null, если не нашлась еда
            if (!gameStateDto.Food.Contains(nearestFood))
                nearestFood = NearestFoodByAStar(graph, startPoint, gameStateDto.Food);

            var points = graph.AStarSearch(startPoint, nearestFood);

            if (points != null)
            {
                var directions = points.Select(p => p.Direction);
                lastDirection = directions.Last();
            }
            else
            {
                nearestFood = NearestFoodByAStar(graph, startPoint, gameStateDto.Food.Where(p => p != nearestFood));
                logger.Info($"Не найден маршрут до точки. Меняем точку на {nearestFood}");
            }

            return lastDirection;
        }

        private PointDto NearestFoodByAStar(Graph graph, PointDto startPoint, IEnumerable<PointDto> points)
        {
            //сделать многопоточно
            int count = Int32.MaxValue;
            PointDto nearestFood = null;
            foreach (PointDto foodPoint in points)
            {
                var pathPoints = graph.AStarSearch(startPoint, foodPoint);

                if (pathPoints != null)
                {
                    int pathCount = pathPoints.Count();
                    if (pathCount < count)
                    {
                        nearestFood = foodPoint;
                        count = pathCount;
                    }
                }
            }

            return nearestFood;
        }

        public void UpdateNearestFood(PointDto pointDto)
        {
            this.nearestFood = pointDto;
        }
    }
}
