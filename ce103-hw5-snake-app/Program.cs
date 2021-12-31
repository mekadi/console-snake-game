using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using ce103_hw5_snake_dll;

namespace ce103_hw5_snake_app
{
    public class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                SnakeGameFunctions SG = new SnakeGameFunctions();
                Console.Title = "Snake Game";
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                // after clear, bg and fg colors will be filled in console

                Console.Clear();
                SG.mainMenu();
            }
        }
    }
}
