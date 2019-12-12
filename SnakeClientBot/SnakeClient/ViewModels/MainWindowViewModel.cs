using NLog;
using Prism.Commands;
using Prism.Mvvm;
using RestSharp;
using SnakeClient.DTO;
using SnakeClient.Models;
using System;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Threading;
using Size = SnakeClient.Models.Size;

namespace SnakeClient.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        //private readonly Logger logger; 
        //private readonly int rectangleSize;
        //private readonly int margin;
        //private DispatcherTimer _timer;
        //private readonly RestClient _restClient;
        //private string gameException;

        //public string GameException
        //{
        //    get { return gameException; }
        //    set
        //    {
        //        gameException = value;
        //        RaisePropertyChanged(nameof(GameException));
        //    }
        //}
        //public Size GameBoardSize { get; private set; }
        //public ObservableCollection<ViewPoint> Snake { get; private set; }
        //public ObservableCollection<ViewPoint> Food { get; private set; }
        //public DelegateCommand<string> PostDirectionCommand { get; private set; }

        //public MainWindowViewModel()
        //{
        //    this.logger = LogManager.GetCurrentClassLogger();

        //    try
        //    {
        //        PostDirectionCommand = new DelegateCommand<string>(async (s) => await PostDirection(s));
        //        rectangleSize = Properties.Settings.Default.RectangleSize;
        //        margin = Properties.Settings.Default.Margin;
        //        Snake = new ObservableCollection<ViewPoint>();
        //        Food = new ObservableCollection<ViewPoint>();
        //        GameBoardSize = new Size();

        //        _restClient = new RestClient(Properties.Settings.Default.Uri);

        //        InitializeGame().ConfigureAwait(true);
        //        logger.Info("Игра успешно запущена");
        //    }
        //    catch (Exception ex)
        //    {
        //        GameException = "Не удалось запустить игру";
        //        this.logger.Error(ex, "Не удалось запустить игру");
        //    }
        //}

        //private async Task InitializeGame()
        //{
        //    try
        //    {
        //        GameBoardDto gameBoardDto = await GetGameBoard();

        //        this.GameBoardSize.Height = ParseCoordinate(gameBoardDto.GameBoardSize.Height);
        //        this.GameBoardSize.Width = ParseCoordinate(gameBoardDto.GameBoardSize.Width);

        //        _timer = new DispatcherTimer(DispatcherPriority.Send);
        //        _timer.Tick += DoWork;
        //        _timer.Interval = TimeSpan.FromMilliseconds(gameBoardDto.TimeUntilNextTurnMilliseconds / 10);
        //        _timer.Start();
        //        this.logger.Info("Игра инициализирована");
        //    }
        //    catch(Exception ex)
        //    {
        //        GameException = "Не удалось инициализировать игру";
        //        this.logger.Error(ex, "Не удалось инициализировать игру");
        //    }
        //}

        //private async Task<GameBoardDto> GetGameBoard()
        //{
        //    var request = new RestRequest("api/gameboard");
        //    this.logger.Info($"Отправляем GET запрос {request.Resource}");

        //    var response = await _restClient.ExecuteGetTaskAsync<GameBoardDto>(request);

        //    if (!response.IsSuccessful)
        //    {
        //        GameException = $"Не удалось отправить GET запрос {request.Resource}";
        //        this.logger.Error($"Не удалось отправить GET запрос {request.Resource}");
        //    }

        //    this.logger.Info($"Получили ответ {JsonSerializer.Serialize(response.Data)}");
        //    return response.Data;
        //}

        //private async Task PostDirection(string direction)
        //{
        //    try
        //    {
        //        switch (direction)
        //        {
        //            case "Top":
        //                await SendRequest(new DirectionDto { Direction = Direction.Top });
        //                break;
        //            case "Bottom":
        //                await SendRequest(new DirectionDto { Direction = Direction.Bottom });
        //                break;
        //            case "Left":
        //                await SendRequest(new DirectionDto { Direction = Direction.Left });
        //                break;
        //            case "Right":
        //                await SendRequest(new DirectionDto { Direction = Direction.Right });
        //                break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        GameException = "Не удалось отправить POST запрос";
        //        this.logger.Error(ex, "Не удалось отправить POST запрос");
        //    }
        //}
        //private async Task SendRequest(DirectionDto directionDto)
        //{
        //    var request = new RestRequest("api/direction");
        //    request.Method = Method.POST;
        //    request.AddParameter("application/json", JsonSerializer.Serialize(directionDto), ParameterType.RequestBody);
        //    this.logger.Info($"Отправляем POST запрос {request.Resource} {JsonSerializer.Serialize(directionDto)}");

        //    var response = await _restClient.ExecutePostTaskAsync(request);

        //    if (!response.IsSuccessful)
        //    {
        //        GameException = $"Не удалось получить ответ на POST запрос {request.Resource}";
        //        this.logger.Error($"Не удалось получить ответ на POST запрос {request.Resource}");
        //    }
        //}

        //private async void DoWork(object obj, EventArgs args)
        //{
        //    try
        //    {
        //        GameBoardDto gameBoardDto = await GetGameBoard();
        //        ProcessResponse(gameBoardDto);
        //    }
        //    catch (Exception ex)
        //    {
        //        GameException = "Не удалось обработать запрос";
        //        this.logger.Error(ex, $"Не удалось обработать запрос");
        //    }
        //}

        //private void ProcessResponse(GameBoardDto gameBoardDto)
        //{
        //    Snake.Clear();
        //    foreach (Point point in gameBoardDto.Snake)
        //    {
        //        ViewPoint processPoint = new ViewPoint(ParseCoordinate(point.X),
        //            ParseCoordinate(point.Y),
        //            rectangleSize,
        //            margin);
        //        Snake.Add(processPoint);
        //    }

        //    Food.Clear();
        //    foreach (Point point in gameBoardDto.Food)
        //    {
        //        ViewPoint processPoint = new ViewPoint(ParseCoordinate(point.X),
        //            ParseCoordinate(point.Y),
        //            rectangleSize,
        //            margin);
        //        Food.Add(processPoint);
        //    }

        //    GameException = String.Empty;
        //}

        //private int ParseCoordinate(int coordinate) => coordinate * (rectangleSize + margin);
    }
}
