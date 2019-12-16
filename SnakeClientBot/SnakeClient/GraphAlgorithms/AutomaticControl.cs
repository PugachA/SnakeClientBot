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
        private int lastSnakeLength;

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

            if (lastSnakeLength > gameStateDto.Snake.Count())
                nearestFood = null;

            lastSnakeLength = gameStateDto.Snake.Count();

            PointDto startPoint = gameStateDto.Snake.First();
            startPoint.Direction = lastDirection;

            //сделать проверку на null, если не нашлась еда
            if (!gameStateDto.Food.Contains(nearestFood))
                nearestFood = NearestFoodByHeuristic(graph, startPoint, gameStateDto.Food);

            var points = graph.AStarSearch(startPoint, nearestFood);

            if (points != null)
            {
                var directions = points.Select(p => p.Direction);
                lastDirection = directions.Last();
            }
            else
            {
                nearestFood = NearestFoodByHeuristic(graph, startPoint, gameStateDto.Food.Where(p => p != nearestFood));
                logger.Info($"Не найден маршрут до точки. Меняем точку на {nearestFood}");
            }

            return lastDirection;
        }

        private PointDto NearestFoodByPath(Graph graph, PointDto startPoint, IEnumerable<PointDto> points)
        {
            //сделать многопоточно
            int count = Int32.MaxValue;
            PointDto nearestFood = null;
            var cameFrom = graph.WideSearch(startPoint);

            foreach (PointDto foodPoint in points)
            {
                var pathPoints = graph.SearchPath(startPoint, foodPoint, cameFrom);

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

        private PointDto NearestFoodByHeuristic(Graph graph, PointDto startPoint, IEnumerable<PointDto> points)
        {
            //сделать многопоточно
            double k1 = 1;
            double k2 = 1.5;
            int radius = 7;
            double minWeight = Double.MaxValue;
            PointDto nearestFood = null;
            var cameFrom = graph.WideSearch(startPoint);

            foreach (PointDto foodPoint in points)
            {
                var pathPoints = graph.SearchPath(startPoint, foodPoint, cameFrom);

                if (pathPoints != null)
                {
                    int pathCount = pathPoints.Count();
                    int foodCountInRadius = FoodCountInRadius(foodPoint, radius, points);
                    double weight = k1 * pathCount - k2 * foodCountInRadius;

                    if (weight < minWeight)
                    {
                        nearestFood = foodPoint;
                        minWeight = weight;
                    }
                }
            }

            return nearestFood;
        }

        private int FoodCountInRadius(PointDto centerPoint, int radius, IEnumerable<PointDto> foodPoints)
        {
            int count = -1;

            for (int i = centerPoint.X - radius; i <= centerPoint.X + radius; i++)
                for (int j = centerPoint.Y - radius; j <= centerPoint.Y + radius; j++)
                    if (foodPoints.Contains(new PointDto(i, j)))
                        count++;

            return count;
        }

        public void UpdateNearestFood(PointDto pointDto)
        {
            this.nearestFood = pointDto;
        }
    }
}
