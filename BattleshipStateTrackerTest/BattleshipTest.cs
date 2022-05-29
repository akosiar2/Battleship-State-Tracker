using System;
using Xunit;
using BattleshipLibrary;
using BattleshipLibrary.Model;

namespace BattleshipStateTrackerTest
{
    public class UnitTest1
    {
        [Fact]
        public void Create_Board_Expect_CorrectCellCreation()
        {
            Board ship1Cell = new Board(5, 6);

            Assert.NotNull(ship1Cell);
        }

        [Fact]
        public void Create_Board_Expect_CorrectCoordinates()
        {
            Board ship1Cell = new Board(5, 6);

            Assert.Equal(5, ship1Cell._row);
            Assert.Equal(6, ship1Cell._col);
        }

        [Fact]
        public void Create_Board_Expect_CorrectBoardCreation()
        {
            Battleship game = new Battleship(10);

            Assert.NotNull(game._board);
        }

        [Fact]
        public void Create_Board_Expect_CorrectRowNumber()
        {
            Battleship game = new Battleship(10);

            Assert.Equal(10, game._board.Count);
        }

        [Fact]
        public void Create_Board_Expect_CorrectColNumber()
        {
            Battleship game = new Battleship(10);

            for (int i = 0; i != game._boardSize; ++i)
            {
                Assert.Equal(10, game._board[i].Count);
            }
        }

        [Fact]
        public void Initialize_BoardWithWater_ExpectAllCellsToBeWater()
        {
            Battleship game = new Battleship(10);

            for (int i = 0; i != game._boardSize; ++i)
            {
                for (int j = 0; j != game._boardSize; ++j)
                {
                    Assert.Equal(BoardCellType.Water, game._board[i][j]._type);
                }
            }
        }

        [Fact]
        public void Create_Ship_Expect_CorrectCreation()
        {
            Board ship1Cell = new Board(5, 6);
            Ship ship = new Ship(ship1Cell, Orientation.Horiztontal, 3);

            Assert.NotNull(ship);
        }

        [Fact]
        public void Create_Ship_Expect_CorrectHeadPosition()
        {
            Board ship1Cell = new Board(5, 6);
            Ship ship = new Ship(ship1Cell, Orientation.Horiztontal, 3);

            Assert.Equal(5, ship.headPosition._row);
            Assert.Equal(6, ship.headPosition._col);
        }

        [Fact]
        public void Create_Ship_Expect_CorrectOrientation()
        {
            Board ship1Cell = new Board(5, 6);
            Ship ship = new Ship(ship1Cell, Orientation.Horiztontal, 3);

            Assert.Equal(Orientation.Horiztontal, ship.orientation);
        }


        [Fact]
        public void Add_VerticalShipToBoard_Expect_CorrectPlacement()
        {
            Battleship game = new Battleship(10);

            Board ship1Cell = new Board(5, 6);
            Ship ship = new Ship(ship1Cell, Orientation.Horiztontal, 3);

            Assert.Equal(Orientation.Horiztontal, ship.orientation);
        }

        [Fact]
        public void Add_HorizontalShipToBoard_Expect_AddedShip()
        {
            Battleship game = new Battleship(10);

            Board ship1Cell = new Board(5, 6);

            game.AddShip(ship1Cell, Orientation.Horiztontal, 3);

            Assert.Single(game._ships);
        }

        [Fact]
        public void Add_VerticalShipToBoard_Expect_AddedShip()
        {
            Battleship game = new Battleship(10);

            Board ship1Cell = new Board(5, 6);

            game.AddShip(ship1Cell, Orientation.Vertical, 3);

            Assert.Single(game._ships);
        }

        [Fact]
        public void Add_TooLongVerticalShipToBoard_Expect_OutOfBoardErrorMessage()
        {
            Battleship game = new Battleship(2);

            Board ship1Cell = new Board(1, 1);

            var returnValue = game.AddShip(ship1Cell, Orientation.Vertical, 15);

            Assert.False(returnValue);
            Assert.Equal("Coordinates for the ship do not match board witdh and height", game.errorMessage);
        }

        [Fact]
        public void Add_TooLongHorizontalShipToBoard_OutOfBoardErrorMessage()
        {
            Battleship game = new Battleship(10);

            Board ship1Cell = new Board(5, 6);

            var returnValue = game.AddShip(ship1Cell, Orientation.Horiztontal, 15);

            Assert.False(returnValue);
            Assert.Equal("Coordinates for the ship do not match board witdh and height", game.errorMessage);
        }

        [Fact]
        public void Add_SeveralValidShips_Expect_CorrectCountAddedShips()
        {
            Battleship game = new Battleship(100);

            Board ship1Cell = new Board(1, 1);
            Board ship2Cell = new Board(10, 10);
            Board ship3Cell = new Board(20, 20);
            Board ship4Cell = new Board(30, 30);

            game.AddShip(ship1Cell, Orientation.Vertical, 4);
            game.AddShip(ship2Cell, Orientation.Horiztontal, 3);
            game.AddShip(ship3Cell, Orientation.Vertical, 5);
            game.AddShip(ship4Cell, Orientation.Horiztontal, 1);

            Assert.Equal(4, game._ships.Count);
        }

        [Fact]
        public void Add_InvalidShips_Expect_InvalidCoordinatesError()
        {
            Battleship game = new Battleship(10);

            Board ship1Cell = new Board(15, 12);
            bool returnValue = game.AddShip(ship1Cell, Orientation.Vertical, 4);

            Assert.False(returnValue);
            Assert.Equal("Coordinates for the ship do not match board witdh and height", game.errorMessage);
        }

        [Fact]
        public void Add_ShipOnAlreadyPlacedShip_Expect_NoCellAvailableError()
        {
            Battleship game = new Battleship(10);

            Board ship1Cell = new Board(1, 1);
            Board ship2Cell = new Board(1, 1);

            game.AddShip(ship1Cell, Orientation.Vertical, 3);
            bool returnValue = game.AddShip(ship2Cell, Orientation.Vertical, 3);

            Assert.False(returnValue);
            Assert.Equal("Not all cells are available for this ship to be place at these coordinates", game.errorMessage);
        }

        [Fact]
        public void Add_Ship_WhenShipOverlay_Expect_NoCellAvailableError()
        {
            Battleship game = new Battleship(50);

            Board ship1Cell = new Board(20, 20);
            Board ship2Cell = new Board(18, 22);

            game.AddShip(ship1Cell, Orientation.Horiztontal, 5);
            bool returnValue = game.AddShip(ship2Cell, Orientation.Vertical, 5);

            Assert.False(returnValue);
            Assert.Equal("Not all cells are available for this ship to be place at these coordinates", game.errorMessage);
        }

        [Fact]
        public void Add_Ship_OnAlreadySunkShip_Expect_NoCellAvailableError()
        {
            Battleship game = new Battleship(50);

            Board ship1Cell = new Board(20, 20);
            Board ship2Cell = new Board(18, 22);

            game.AddShip(ship1Cell, Orientation.Horiztontal, 5);
            game.Attack(20, 20);
            game.Attack(20, 21);
            game.Attack(20, 22);
            game.Attack(20, 23);
            game.Attack(20, 24);
            //ship1 is now sunk
            Assert.True(game._ships[0].IsSunk());
            bool returnValue = game.AddShip(ship2Cell, Orientation.Vertical, 5);

            Assert.False(returnValue);
            Assert.Equal("Not all cells are available for this ship to be place at these coordinates", game.errorMessage);
        }

        [Fact]
        public void Attack_Ship_Expect_ShipToBeDamaged()
        {
            Battleship game = new Battleship(10);
            Board ship1Cell = new Board(1, 1);
            int x = 1;
            int y = 2;

            game.AddShip(ship1Cell, Orientation.Horiztontal, 5);
            int shipHealth = game._ships[0].health;

            game.Attack(x, y);

            Assert.Equal(game._ships[0].health, shipHealth - 1);
        }

        [Fact]
        public void Attack_Ship_Expect_CellUpdated()
        {
            Battleship game = new Battleship(10);
            Board ship1Cell = new Board(1, 1);
            int x = 1;
            int y = 2;

            game.AddShip(ship1Cell, Orientation.Horiztontal, 5);
            int shipHealth = game._ships[0].health;

            game.Attack(x, y);

            Assert.Equal(BoardCellType.Damaged, game._board[x][y]._type);
        }

        [Fact]
        public void Attack_Ship_Expect_CellAroundDamageToBeUndamaged()
        {
            Battleship game = new Battleship(10);
            Board ship1Cell = new Board(1, 1);
            int x = 1;
            int y = 2;

            game.AddShip(ship1Cell, Orientation.Horiztontal, 5);
            int shipHealth = game._ships[0].health;

            game.Attack(x, y);

            Assert.Equal(BoardCellType.Undamaged, game._board[x][y - 1]._type);
            Assert.Equal(BoardCellType.Damaged, game._board[x][y]._type);
            Assert.Equal(BoardCellType.Undamaged, game._board[x][y + 1]._type);
        }

        [Fact]
        public void Attack_Ship_Expect_ShipToBeDamagedButNotSunk()
        {
            Battleship game = new Battleship(10);
            Board ship1Cell = new Board(1, 1);
            int x = 1;
            int y = 2;

            game.AddShip(ship1Cell, Orientation.Horiztontal, 5);
            int shipHealth = game._ships[0].health;

            game.Attack(x, y);

            Assert.False(game._ships[0].IsSunk());
        }

        [Fact]
        public void Attack_AllShipCells_Expect_ShipToBeSunk()
        {
            Battleship game = new Battleship(10);
            Board ship1Cell = new Board(1, 1);
            int x = 1;
            int y = 1;

            game.AddShip(ship1Cell, Orientation.Horiztontal, 5);
            int shipHealth = game._ships[0].health;

            // Carrier has 5 cells
            game.Attack(x, y);
            game.Attack(x, y + 1);
            game.Attack(x, y + 2);
            game.Attack(x, y + 3);
            game.Attack(x, y + 4);

            Assert.True(game._ships[0].IsSunk());
        }

        [Fact]
        public void Attack_WaterCell_Expect_FailAttackAndWaterCell()
        {
            Battleship game = new Battleship(10);
            Board ship1Cell = new Board(1, 1);
            int x = 8;
            int y = 9;

            game.AddShip(ship1Cell, Orientation.Horiztontal, 3);

            game.Attack(x, y);

            Assert.Equal(BoardCellType.Water, game._board[x][y]._type);
        }

    }
}
