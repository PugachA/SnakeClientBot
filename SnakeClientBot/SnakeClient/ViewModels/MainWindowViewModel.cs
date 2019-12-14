using NLog;
using Prism.Commands;
using Prism.Mvvm;
using SnakeClient.DTO;
using SnakeClient.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Threading;
using Size = SnakeClient.Models.Size;

namespace SnakeClient.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly Logger logger;
        private readonly int rectangleSize;
        private readonly int margin;
        private DispatcherTimer _timer;
        private readonly SnakeAPIClient _snakeApiClient;
        private string gameException;
        private string gameInfo;
        private string myName;
        private Direction lastDirection;
        private PointDto nearestFood;

        public string GameException
        {
            get { return gameException; }
            set
            {
                gameException = value;
                RaisePropertyChanged(nameof(GameException));
            }
        }
        public string GameInfo
        {
            get { return gameInfo; }
            set
            {
                gameInfo = value;
                RaisePropertyChanged(nameof(GameInfo));
            }
        }
        public Size GameBoardSize { get; private set; }
        public ObservableCollection<ViewPoint> Snake { get; private set; }
        public ObservableCollection<ViewPoint> Food { get; private set; }
        public ObservableCollection<PlayerStateView> Players { get; private set; }
        public ObservableCollection<RectangleDto> Walls { get; private set; }
        public DelegateCommand<string> PostDirectionCommand { get; private set; }

        public MainWindowViewModel()
        {
            this.logger = LogManager.GetCurrentClassLogger();

            try
            {
                PostDirectionCommand = new DelegateCommand<string>(async (s) => await PostDirection(s));
                rectangleSize = Properties.Settings.Default.RectangleSize;
                margin = Properties.Settings.Default.Margin;
                Snake = new ObservableCollection<ViewPoint>();
                Food = new ObservableCollection<ViewPoint>();
                Players = new ObservableCollection<PlayerStateView>();
                Walls = new ObservableCollection<RectangleDto>();
                GameBoardSize = new Size();

                lastDirection = Direction.Up;

                _snakeApiClient = new SnakeAPIClient(new Uri(Properties.Settings.Default.Uri), "9jzuz6FEa64j7TADCtzF");

                InitializeGame().ConfigureAwait(true);
                logger.Info("Игра успешно запущена");
            }
            catch (Exception ex)
            {
                GameException = "Не удалось запустить игру";
                this.logger.Error(ex, "Не удалось запустить игру");
            }
        }

        private async Task InitializeGame()
        {
            try
            {
                var responseName = await _snakeApiClient.GetNameResponse();
                if (!responseName.IsSuccess)
                    throw new InvalidOperationException($"Запрос был неуспешен. {responseName.ErrorMessage}");
                myName = responseName.Data.Name;

                _ = await _snakeApiClient.PostDirection(Direction.Up);
                var response = await _snakeApiClient.GetGameState();
                if (!response.IsSuccess)
                    throw new InvalidOperationException($"Запрос был неуспешен. {response.ErrorMessage}");

                GameStateDto gameBoardDto = response.Data;

                this.GameBoardSize.Height = ParseCoordinate(gameBoardDto.GameBoardSize.Height);
                this.GameBoardSize.Width = ParseCoordinate(gameBoardDto.GameBoardSize.Width);

                await Task.Delay(gameBoardDto.TimeUntilNextTurnMilliseconds).ConfigureAwait(true);
                _timer = new DispatcherTimer(DispatcherPriority.Send);
                _timer.Tick += DoWork;
                _timer.Interval = TimeSpan.FromMilliseconds(gameBoardDto.TurnTimeMilliseconds / 2);
                _timer.Start();
                this.logger.Info("Игра инициализирована");
            }
            catch (Exception ex)
            {
                GameException = "Не удалось инициализировать игру";
                this.logger.Error(ex, "Не удалось инициализировать игру");
            }
        }

        private async Task PostDirection(string direction)
        {
            try
            {
                switch (direction)
                {
                    case "Top":
                        await _snakeApiClient.PostDirection(Direction.Up);
                        break;
                    case "Bottom":
                        await _snakeApiClient.PostDirection(Direction.Down);
                        break;
                    case "Left":
                        await _snakeApiClient.PostDirection(Direction.Left);
                        break;
                    case "Right":
                        await _snakeApiClient.PostDirection(Direction.Right);
                        break;
                }
            }
            catch (Exception ex)
            {
                GameException = "Не удалось отправить POST запрос";
                this.logger.Error(ex, "Не удалось отправить POST запрос");
            }
        }

        private async void DoWork(object obj, EventArgs args)
        {
            try
            {
                var response = await _snakeApiClient.GetGameState();

                if (!response.IsSuccess)
                    throw new InvalidOperationException($"Запрос не успешен. {response.ErrorMessage}");

                //нужно обнулять lastdirection
                GameStateDto gameStateDto = response.Data;
                Graph graph = new Graph(gameStateDto);

                PointDto startPoint = gameStateDto.Snake.First();
                startPoint.Direction = lastDirection;

                if (!gameStateDto.Food.Contains(nearestFood))
                    nearestFood = NearestFood(startPoint, gameStateDto.Food);

                var points = graph.WideSearch(startPoint, nearestFood);

                if (points != null)
                {
                    var directions = points.Select(p => p.Direction);
                    lastDirection = directions.Last();

                }
                else
                {
                    nearestFood = NearestFood(startPoint, gameStateDto.Food.Where(p => p != nearestFood));
                    logger.Info($"Не найден маршрут до точки. Меняем точку на {nearestFood}");
                }


                await _snakeApiClient.PostDirection(lastDirection);

                ProcessResponse(gameStateDto);
            }
            catch (Exception ex)
            {
                GameException = "Не удалось обработать запрос";
                this.logger.Error(ex, $"Не удалось обработать запрос");
            }
        }

        private PointDto NearestFood(PointDto startPoint, IEnumerable<PointDto> points)
        {
            Dictionary<PointDto, double> pointDistanses = new Dictionary<PointDto, double>();
            foreach (PointDto point in points)
                pointDistanses.Add(point, Distanse(startPoint, point));

            return pointDistanses.OrderBy(p => p.Value).First().Key;
        }

        private double Distanse(PointDto firstPoint, PointDto secondPoint)
        {
            return Math.Sqrt(Math.Pow(secondPoint.X - firstPoint.X, 2) + Math.Pow(secondPoint.Y - firstPoint.Y, 2));
        }

        private void ProcessResponse(GameStateDto gameBoardDto)
        {

            Snake.Clear();
            int count = 0;
            foreach (PointDto point in gameBoardDto.Snake)
            {
                ViewPoint processPoint = new ViewPoint(point, rectangleSize, margin);

                if (count == 0)
                    processPoint.Description = gameBoardDto.Snake.Count().ToString();

                Snake.Add(processPoint);
                count++;
            }

            Food.Clear();
            foreach (PointDto point in gameBoardDto.Food)
            {
                ViewPoint processPoint = new ViewPoint(point, rectangleSize, margin);
                Food.Add(processPoint);
            }

            Players.Clear();
            foreach (PlayerStateDto player in gameBoardDto.Players)
            {
                if (player == null || player.Name == myName)
                    continue;

                PlayerStateView playerState = new PlayerStateView(player, rectangleSize, margin);
                Players.Add(playerState);
            }

            Walls.Clear();
            foreach (RectangleDto wall in gameBoardDto.Walls)
            {
                Walls.Add(wall.TransformForView(rectangleSize, margin));
            }

            GameInfo = GenerateGameInfo(gameBoardDto);
            GameException = String.Empty;
        }

        private int ParseCoordinate(int coordinate) => coordinate * (rectangleSize + margin);

        private string GenerateGameInfo(GameStateDto gameBoardDto)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"{nameof(gameBoardDto.IsStarted)}: {gameBoardDto.IsStarted}");
            stringBuilder.AppendLine($"{nameof(gameBoardDto.IsPaused)}: {gameBoardDto.IsPaused}");
            stringBuilder.AppendLine($"{nameof(gameBoardDto.RoundNumber)}: {gameBoardDto.RoundNumber}");
            stringBuilder.AppendLine($"{nameof(gameBoardDto.TurnNumber)}: {gameBoardDto.TurnNumber}");
            stringBuilder.AppendLine($"{nameof(gameBoardDto.TurnTimeMilliseconds)}: {gameBoardDto.TurnTimeMilliseconds}");
            stringBuilder.AppendLine($"{nameof(gameBoardDto.TimeUntilNextTurnMilliseconds)}: {gameBoardDto.TimeUntilNextTurnMilliseconds}");
            stringBuilder.AppendLine($"{nameof(gameBoardDto.MaxFood)}: {gameBoardDto.MaxFood}");

            stringBuilder.AppendLine($"{nameof(gameBoardDto.Players)}:");
            var players = gameBoardDto.Players.Where(p => p.Snake != null).Select(p => $"{p.Name}: {p.Snake.Count()}");
            foreach (var item in players)
                stringBuilder.AppendLine(item);

            return stringBuilder.ToString();
        }
    }
}
