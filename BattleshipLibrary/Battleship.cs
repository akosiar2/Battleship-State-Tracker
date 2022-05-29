using System;
using System.Collections.Generic;
using BattleshipLibrary.Model;

namespace BattleshipLibrary
{
    public class Battleship : IBattleship
    {
        public List<List<Board>> _board { get; set; }
        public List<Ship> _ships { get; set; }
        public int _boardSize { get; set; }
        public string errorMessage { get; set; }
        public string statusUpdate { get; set; }
        public bool gameOver { get; set; }

        public Battleship(int boardSize)
        {
            _ships = new List<Ship>();
            CreateBoard(boardSize);
        }

        /// <summary>
        /// Board size is customisable, square grid only.
        /// </summary>
        /// <param name="boardSize"></param>
        private void CreateBoard(int boardSize)
        {
            _boardSize = boardSize;
            _board = new List<List<Board>>();

            for (int i = 0; i != _boardSize; ++i)
            {
                _board.Add(new List<Board>());

                for (int j = 0; j != _boardSize; ++j)
                {
                    _board[i].Add(new Board(i, j));
                }
            }

            InitializeBoards();
        }

        /// <summary>
        /// Empty board only has water cells
        /// </summary>
        private void InitializeBoards()
        {
            for (int i = 0; i != _boardSize; ++i)
            {
                for (int j = 0; j != _boardSize; ++j)
                {
                    _board[i][j].ResetCell(BoardCellType.Water);
                }
            }
        }

        /// <summary>
        /// Adding ship with position and length
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="orientation"></param>
        /// <param name="length"></param>
        /// <returns>boolean</returns>
        public bool AddShip(Board cell, Orientation orientation, int length)
        {
            Ship ship = new Ship(cell, orientation, length);

            bool isAdded = false;
            int remainingLength = ship.health;


            if (ship.orientation == Orientation.Horiztontal)
                isAdded = AddHorizontally(ship);
            else
                isAdded = AddVertically(ship);

            if (isAdded)
            {
                errorMessage = string.Empty;
                _ships.Add(ship);
            }

            return isAdded;
        }

        /// <summary>
        /// Check placement of ship if possible
        /// </summary>
        /// <param name="ship"></param>
        /// <returns>boolean</returns>
        private bool IsPlacementPossible(Ship ship)
        {
            try
            {
                int row = ship.headPosition._row;
                int col = ship.headPosition._col;
                int lenght = ship.length;

                if (ship.orientation == Orientation.Horiztontal)
                {
                    for (int currentCol = col; lenght != 0; currentCol++)
                    {
                        if (BoardFree(row, currentCol) == false)
                        {
                            if (string.IsNullOrEmpty(errorMessage))
                                errorMessage = "Not all cells are available for this ship to be place at these coordinates";
                            return false;
                        }
                        --lenght;
                    }
                    return true;
                }
                else
                {
                    for (int currentRow = row; lenght != 0; currentRow++)
                    {
                        if (BoardFree(currentRow, col) == false)
                        {
                            if (string.IsNullOrEmpty(errorMessage))
                                errorMessage = "Not all cells are available for this ship to be place at these coordinates";
                            return false;
                        }
                        --lenght;
                    }
                    return true;
                }
            }
            catch (Exception e)
            {
                errorMessage = "There was an internal issue and the ship cannot be added. Please refer to execption: " + e.Message;
                return false;
            }
        }

        /// <summary>
        /// Check if Board has enough space
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns>boolean</returns>
        private bool BoardFree(int row, int col)
        {
            if (checkCoordinatesValidity(row, col))
            {
                return (_board[row][col]._shipIndex == -1) ? true : false;
            }
            else
            {
                errorMessage = "Coordinates for the ship do not match board witdh and height";
                return false;
            }
        }

        /// <summary>
        /// Check if coordinates exist inside the board
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        private bool checkCoordinatesValidity(int row, int col)
        {
            if (row < _boardSize && col < _boardSize && row >= 0 && col >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Add ship horizontally on the board
        /// </summary>
        /// <param name="ship"></param>
        /// <returns></returns>
        private bool AddHorizontally(Ship ship)
        {
            int row = ship.headPosition._row;
            int col = ship.headPosition._col;
            int lenght = ship.length;

            if (IsPlacementPossible(ship))
            {
                for (int currentCol = col; lenght != 0; ++currentCol)
                {
                    _board[row][currentCol]._type = BoardCellType.Undamaged;
                    _board[row][currentCol]._shipIndex = _ships.Count;
                    --lenght;
                }
                return true;
            }

            return false;
        }

        /// <summary>
        /// Add shjip vertically on the board
        /// </summary>
        /// <param name="ship"></param>
        /// <returns></returns>
        private bool AddVertically(Ship ship)
        {
            int row = ship.headPosition._row;
            int col = ship.headPosition._col;
            int lenght = ship.length;

            if (IsPlacementPossible(ship))
            {
                for (int currentRow = row; lenght != 0; ++currentRow)
                {
                    _board[currentRow][col]._type = BoardCellType.Undamaged;
                    _board[currentRow][col]._shipIndex = _ships.Count;
                    --lenght;
                }
                return true;
            }

            return false;
        }

        /// <summary>
        /// Create an attack on the board
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>Status update</returns>
        public BoardCellType Attack(int x, int y)
        {
            if (checkCoordinatesValidity(x, y))
            {
                switch (_board[x][y]._type)
                {
                    case BoardCellType.Water:
                        return BoardCellType.Water;

                    case BoardCellType.Undamaged:
                        var attackedShip = _ships[_board[x][y]._shipIndex];
                        attackedShip.health = attackedShip.health - 1;

                        if (attackedShip.health > 0)
                        {
                            _board[x][y]._type = BoardCellType.Damaged;
                            return BoardCellType.Damaged;
                        }
                        else
                        {
                            _board[x][y]._type = BoardCellType.Sunk;
                            gameOver = CheckGameOver();
                            return BoardCellType.Sunk;
                        }

                    case BoardCellType.Damaged:
                        statusUpdate = "Ship is already damaged at these coordinates";
                        throw new Exception(statusUpdate);

                    case BoardCellType.Sunk:
                        statusUpdate = "Ship at these coordinates is already sunk";
                        throw new Exception(statusUpdate);

                    default:
                        throw new Exception("There was an error with the attack");
                }
            }
            else
            {
                throw new Exception("Attack coordinates are invalid");
            }
        }

        /// <summary>
        /// Return true if all ships are gone
        /// </summary>
        /// <returns></returns>
        private bool CheckGameOver()
        {
            int shipsAlive = 0;

            foreach (Ship ship in _ships)
            {
                if (!ship.IsSunk())
                    shipsAlive++;
            }

            return shipsAlive == 0 ? true : false;
        }

        /// <summary>
        /// Get all ships
        /// </summary>
        /// <returns></returns>
        public string GetShipsList()
        {
            string shipList = string.Empty;
            for (int index = 0; index != _ships.Count; index++)
            {
                string status = string.Empty;
                if (_ships[index].health == _ships[index].length)
                    status = "Unadamaged";
                else if (_ships[index].health == 0)
                    status = "Sunk";
                else
                    status = "Damaged";

                var orientation = _ships[index].orientation == Orientation.Horiztontal ? "Horizontal" : "Vertical";

                shipList += String.Format("Ship #{0} - Head Position: [{1}][{2}] - Health:{3}/{4} - Orientation: {5} - Status: {6}\n",
                    index, _ships[index].headPosition._row, _ships[index].headPosition._col, _ships[index].health,
                    _ships[index].length, orientation, status);
            }

            return shipList;
        }

        public string GetStatusString()
        {
            string status;
            int shipsAlive = 0;
            int shipsSunk = 0;

            foreach (Ship ship in _ships)
            {
                if (!ship.IsSunk())
                    shipsAlive++;
                else
                    shipsSunk++;
            }

            if (shipsAlive == 0 && shipsSunk == 0)
            {
                status = "You currently have no ship placed on the board. Please add at least one ship to continue.";
            }
            else if (shipsAlive == 0)
            {
                status = "All of your ships sunk. GAME OVER...";
            }
            else
            {
                status = String.Format("You currently have {0} ship(s) afloat, and {1} ship(s) sunk\n", shipsAlive,
                    shipsSunk);
                status += "\nShips List:\n" + GetShipsList();
            }

            return status;
        }
    }
}
