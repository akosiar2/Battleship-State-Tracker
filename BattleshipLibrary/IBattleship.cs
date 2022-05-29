using BattleshipLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipLibrary
{
    public interface IBattleship
    {
        bool AddShip(Board cell, Orientation orientation, int length);
        BoardCellType Attack(int x, int y);
        string GetShipsList();
        string GetStatusString();
    }
}
