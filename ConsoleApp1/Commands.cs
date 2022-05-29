using System;
using BattleshipLibrary;
using BattleshipLibrary.Model;

namespace BattleshipTracker
{
    public class Commands
    {
        public enum CommandType
        {
            AddShip,
            Attack,
            Status
        };

        private CommandType ParseCommand(string[] commandArguments)
        {
            if (commandArguments.Length >= 1)
            {
                switch (commandArguments[0].ToLower())
                {
                    case "addship":
                        if (ArgumentsAddShip(commandArguments, 5))
                            return CommandType.AddShip;
                        else
                            goto default;

                    case "attack":
                        if (ArgumentsAttack(commandArguments, 3))
                            return CommandType.Attack;
                        else
                            goto default;

                    case "status":
                        return CommandType.Status;

                    default:
                        return CommandType.Status;
                }
            }
            return CommandType.Status;
        }

        private bool ArgumentsAddShip(string[] commandArguments, int index)
        {
            if (commandArguments.Length == index)
            {
                if (!int.TryParse(commandArguments[1], out int x))
                    return false;

                if (!int.TryParse(commandArguments[2], out int y))
                    return false;

                switch (commandArguments[3].ToLower())
                {
                    case "vertical":
                        break;
                    case "horizontal":
                        break;
                    default:
                        return false;
                }

                if (!int.TryParse(commandArguments[4], out int length))
                    return false;

                return true;
            }
            else
            {
                return false;
            }
        }

        private bool ArgumentsAttack(string[] commandArguments, int index)
        {
            if (commandArguments.Length == index)
            {
                bool isNumeric = false;
                for (int i = 1; i < index; i++)
                {
                    isNumeric = int.TryParse(commandArguments[i], out int x);
                    if (isNumeric == false)
                        return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public string BattleShipAction(Battleship game, string[] command)
        {
            CommandType cmdType = ParseCommand(command);
            string updateMessage = "\nYour command was invalid. Please type \\help for command details";
            try
            {
                switch (cmdType)
                {
                    case CommandType.AddShip:
                        updateMessage = "\n" + AddShip(game, command);
                        break;

                    case CommandType.Attack:

                        updateMessage = "\n" + Attack(game, command);
                        break;

                    case CommandType.Status:
                        updateMessage = "\n" + CheckStatus(game);
                        break;

                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                updateMessage = "There was an internal error. Please refer to the following message:" + e.Message;
            }

            updateMessage += "\n";
            return updateMessage;
        }

        private static string AddShip(Battleship game, string[] command)
        {
            string messageUpdate = string.Empty;
            int x = Convert.ToInt32(command[1]);
            int y = Convert.ToInt32(command[2]);
            Orientation orientation = command[3].ToLower() == "vertical" ? Orientation.Vertical : Orientation.Horiztontal;
            int length = Convert.ToInt32(command[4]);

            Board shipCell = new Board(x, y);
            var isPlaced = game.AddShip(shipCell, orientation, length);
            if (isPlaced)
                messageUpdate = "Your ship has been added to the board";
            else
                messageUpdate = game.errorMessage;

            return messageUpdate;
        }

        private static string Attack(Battleship game, string[] command)
        {
            string messageUpdate = string.Empty;
            int x = Convert.ToInt32(command[1]);
            int y = Convert.ToInt32(command[2]);

            try
            {
                var resultType = game.Attack(x, y);
                switch (resultType)
                {
                    case BoardCellType.Damaged:
                        messageUpdate = "Attack succesful. Ship has been hit at x:" + x + ", y:" + y + ".";
                        break;
                    case BoardCellType.Water:
                        messageUpdate = "Missed! There is no ship at x:" + x + ", y:" + y + ".";
                        break;
                    case BoardCellType.Sunk:
                        messageUpdate = "That's a hit! The battle ship has sunk!";
                        break;
                }
            }
            catch (Exception ex)
            {
                messageUpdate = ex.Message;
            }

            return messageUpdate;
        }

        private static string CheckStatus(Battleship game)
        {
            string messageUpdate = game.GetStatusString();

            return messageUpdate;
        }
    }
}
