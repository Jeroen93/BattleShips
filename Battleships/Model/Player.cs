namespace Battleships.Model
{
    public class Player
    {
        public Fleet Fleet { get; }

        public Player()
        {
            Fleet = new Fleet();
        }
    }
}
