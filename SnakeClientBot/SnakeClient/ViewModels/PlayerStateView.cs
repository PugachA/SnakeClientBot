using SnakeClient.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace SnakeClient.ViewModels
{
    public class PlayerStateView
    {
        public string Name { get; set; }
        public bool IsSpawnProtected { get; set; }
        public ObservableCollection<ViewPoint> Snake { get; set; }

        public PlayerStateView(PlayerStateDto playerStateDto, int rectangleSize, int margin)
        {
            this.Name = playerStateDto.Name;
            this.IsSpawnProtected = playerStateDto.IsSpawnProtected;


            if (playerStateDto.Snake != null)
            {
                Snake = new ObservableCollection<ViewPoint>();
                int count = 0;
                foreach (PointDto point in playerStateDto.Snake)
                {
                    var pointView = new ViewPoint(point, rectangleSize, margin);
                    if (count == 0)
                        pointView.Description = playerStateDto.Snake.Count().ToString();

                    Snake.Add(pointView);
                    count++;
                }
            }
        }
    }
}
