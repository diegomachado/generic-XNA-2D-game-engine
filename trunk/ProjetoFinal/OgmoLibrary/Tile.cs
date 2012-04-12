using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace OgmoLibrary
{
    public class Tile
    {
        public Point Position { get; set; }
        public int Id { get; set; }
        
        public Tile(Point position, int id)
        {
            Position = new Point(position.X, position.Y);            
            Id = id;
        }
    }
}
