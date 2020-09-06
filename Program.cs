using System;

namespace CellED
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new CellED())
                game.Run();
        }
    }
}
