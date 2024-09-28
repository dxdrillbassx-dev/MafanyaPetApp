using System;
using System.Drawing;
using System.Windows.Forms;

namespace MafanyaPetApp
{
    public static class AnimationData
    {
        public static string BaseImagePath = "C:\\Users\\DXDRILLBASSX\\source\\repos\\MafanyaPetApp\\MafanyaPetApp\\img\\";

        public static Image[] LoadFrames(string weapon, int frameCount)
        {
            Image[] frames = new Image[frameCount];
            for (int i = 0; i < frameCount; i++)
            {
                string filePath = $"{BaseImagePath}{weapon}\\{i + 1}.png";
                try
                {
                    frames[i] = Image.FromFile(filePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке изображения: {ex.Message}");
                }
            }
            return frames;
        }

        public static Image[] StickFrames = LoadFrames("stick", 8);
        public static Image[] GunFrames = LoadFrames("gun", 8);
        public static Image[] RifleFrames = LoadFrames("rifle", 8);
        public static Image[] DragFrames = LoadFrames("drag", 4);
        public static Image[] PushStickFrames = LoadFrames("stick_push", 4);
        public static Image[] PushGunFrames = LoadFrames("gun_push", 4);
        public static Image[] PushRifleFrames = LoadFrames("rifle_push", 4);
        public static Image[] TrousersFrames = LoadFrames("trousers", 3);
        public static Image[] Trousers2Frames = LoadFrames("trousers2", 3);
        public static Image[] Trousers3Frames = LoadFrames("trousers3", 19);
        public static Image[] CrawlFrames = LoadFrames("crawl", 6);
        public static Image[] RollFrames = LoadFrames("roll", 6);
    }
}
