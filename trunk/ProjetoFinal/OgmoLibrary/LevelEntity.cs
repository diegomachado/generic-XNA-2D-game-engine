using Microsoft.Xna.Framework;

namespace OgmoEditorLibrary
{
    public class LevelEntity
    {
        public string type;
        public int id;
        public Point position;

        public LevelEntity(string type, int id, int x, int y)
        {
            this.type = type;
            this.id = id;
            position = new Point(x, y);
        }
    }
}
