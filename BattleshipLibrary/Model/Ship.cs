using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipLibrary.Model
{
    public class Ship : IShip
    {
        public int health;
        public int length;
        public Board headPosition;
        public Orientation orientation;

        public Ship(Board cell, Orientation orientation, int length)
        {
            headPosition = cell;
            this.orientation = orientation;
            health = length;
            this.length = length;
        }

        public bool DamagedAt()
        {
            health--;
            return IsSunk();
        }


        public bool IsSunk()
        {
            return health == 0;
        }
    }


    public enum Orientation
    {
        Vertical = 0,
        Horiztontal = 1
    };

    interface IShip
    {
        bool IsSunk();
        bool DamagedAt();

    }

}
