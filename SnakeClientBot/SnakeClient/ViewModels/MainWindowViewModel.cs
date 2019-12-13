using NLog;
using Prism.Commands;
using Prism.Mvvm;
using SnakeClient.DTO;
using SnakeClient.Models;
using System;
using System.Collections.ObjectModel;
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

        public string GameException
        {
            get { return gameException; }
            set
            {
                gameException = value;
                RaisePropertyChanged(nameof(GameException));
            }
        }
        public Size GameBoardSize { get; private set; }
        public ObservableCollection<ViewPoint> Snake { get; private set; }
        public ObservableCollection<ViewPoint> Food { get; private set; }
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
                GameBoardSize = new Size();

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
                _ = await _snakeApiClient.PostDirection(Direction.Up);
                var response = await _snakeApiClient.GetGameState();
                if (!response.IsSuccess)
                    throw new InvalidOperationException($"Запрос был неуспешен. {response.ErrorMessage}");

                GameStateDto gameBoardDto = response.Data;

                this.GameBoardSize.Height = ParseCoordinate(gameBoardDto.GameBoardSize.Height);
                this.GameBoardSize.Width = ParseCoordinate(gameBoardDto.GameBoardSize.Width);

                _timer = new DispatcherTimer(DispatcherPriority.Send);
                _timer.Tick += DoWork;
                _timer.Interval = TimeSpan.FromMilliseconds(gameBoardDto.TimeUntilNextTurnMilliseconds / 10);
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

                ProcessResponse(response.Data);
            }
            catch (Exception ex)
            {
                GameException = "Не удалось обработать запрос";
                this.logger.Error(ex, $"Не удалось обработать запрос");
            }
        }

        private void ProcessResponse(GameStateDto gameBoardDto)
        {
            Snake.Clear();
            foreach (PointDto point in gameBoardDto.Snake)
            {
                ViewPoint processPoint = new ViewPoint(ParseCoordinate(point.X),
                    ParseCoordinate(point.Y),
                    rectangleSize,
                    margin);
                Snake.Add(processPoint);
            }

            Food.Clear();
            foreach (PointDto point in gameBoardDto.Food)
            {
                ViewPoint processPoint = new ViewPoint(ParseCoordinate(point.X),
                    ParseCoordinate(point.Y),
                    rectangleSize,
                    margin);
                Food.Add(processPoint);
            }

            GameException = String.Empty;
        }

        private int ParseCoordinate(int coordinate) => coordinate * (rectangleSize + margin);
    }
}
