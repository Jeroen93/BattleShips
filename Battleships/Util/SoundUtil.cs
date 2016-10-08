using System.Media;

namespace Battleships.Util
{
    public static class SoundUtil
    {
        public static void PlayExplosion()
        {
            Play("explosion");
        }

        public static void PlayWin()
        {
            Play("win");
        }

        public static void PlayLose()
        {
            Play("lose");
        }

        private static void Play(string filename)
        {
            var sp = new SoundPlayer(GlobalVar.Path + $@"\Resources\Sounds\{filename}.wav");
            if (sp != null)
                sp.Play();
        }
    }
}
