using Microsoft.Xna.Framework;

namespace OgmoEditorLibrary
{
    public class LevelEntity
    {
        public int id;
        public string name;
        public Point position;

        LevelEntity(int id, string name, int x, int y)
        {
            this.id = id;
            this.name = name;
            position = new Point(x, y);
        }
    }
}
