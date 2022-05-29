using System;
using BattleshipLibrary;

namespace BattleshipTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            string command;
            string messageUpdate;
            bool quitNow = false;
            Battleship game = new Battleship(10); // Board size is customisable

            Console.WriteLine("Hello lets start the game!");
            Commands cmd = new Commands();

            DisplayInitializeGame();
            while (!quitNow)
            {
                command = Console.ReadLine();
                switch (command)
                {   
                    case "/quit":
                        Console.WriteLine("Thank you for playing. Now exiting...");
                        quitNow = true;
                        break;

                    default:
                        messageUpdate = cmd.BattleShipAction(game, command.Split(' '));
                        Console.WriteLine(messageUpdate);
                        break;
                }
            }

        }

        public static void DisplayCommandList()
        {
            Console.WriteLine("Commands list:\n");
            Console.WriteLine("'addship [x] [y] [orientation] [length]'");
            Console.WriteLine("with x: number (no decimal), y: number (no decimal)");
            Console.WriteLine("orientation: 'vertical' or 'hozirontal'(without ''), length: number (no decimal)\n");
            Console.WriteLine("'attack [x] [y]'");
            Console.WriteLine("with x: number (no decimal), y: number (no decimal)\n");
            Console.WriteLine("'status' for current game status\n");
            Console.WriteLine("'/quit' to extit the game\n");
        }

        public static void DisplayInitializeGame()
        {
            Console.WriteLine("An empty battleship board of 10x10 has been created.");
            DisplayCommandList();
        }
    }
}
