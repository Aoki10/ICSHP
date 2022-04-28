using System;

namespace FEI_MAN_Lotz
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Game1())
                game.Run();
                
        }
        
    }
}
