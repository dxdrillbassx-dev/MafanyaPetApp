using System;
using System.IO;

namespace MafanyaPetApp
{
    public static class SoundData
    {
        // Динамический путь к папке sound
        public static string BaseSoundPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sound");

        public static string[] PushSounds =
        {
            Path.Combine(BaseSoundPath, "push_sound1.wav"),
            Path.Combine(BaseSoundPath, "push_sound2.wav"),
            Path.Combine(BaseSoundPath, "push_sound3.wav"),
            Path.Combine(BaseSoundPath, "push_sound4.wav"),
            Path.Combine(BaseSoundPath, "push_sound5.wav"),
            Path.Combine(BaseSoundPath, "push_sound6.wav")
        };

        public static string[] Trousers2Sounds =
        {
            Path.Combine(BaseSoundPath, "trousers2_sound1.wav"),
            Path.Combine(BaseSoundPath, "trousers2_sound2.wav"),
            Path.Combine(BaseSoundPath, "trousers2_sound3.wav"),
            Path.Combine(BaseSoundPath, "trousers2_sound4.wav")
        };

        public static string[] TrousersStartSounds =
        {
            Path.Combine(BaseSoundPath, "trousers_start_sound1.wav"),
            Path.Combine(BaseSoundPath, "trousers_start_sound2.wav"),
            Path.Combine(BaseSoundPath, "trousers_start_sound3.wav"),
            Path.Combine(BaseSoundPath, "trousers_start_sound4.wav")
        };
    }
}
