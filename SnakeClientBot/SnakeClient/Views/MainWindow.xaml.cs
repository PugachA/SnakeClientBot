using RestSharp;
using SnakeClient.DTO;
using SnakeClient.Models;
using SnakeClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SnakeClient.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SnakeAPIClient snakeAPIClient;
        public MainWindow()
        {
            InitializeComponent();
            snakeAPIClient = new SnakeAPIClient(new Uri(@"http://167.172.186.24"), "");
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var nameResponse = await snakeAPIClient.GetNameResponse();
            NameResponseDto nameResponseDto = nameResponse.Data;
            var gameStateResponse= await snakeAPIClient.GetGameState();
            GameStateDto gameStateDto = gameStateResponse.Data;
            var response = await snakeAPIClient.PostDirection(Direction.Up);
            //PointDto[] points = gameStateDto.Food;
        }

    }
}
