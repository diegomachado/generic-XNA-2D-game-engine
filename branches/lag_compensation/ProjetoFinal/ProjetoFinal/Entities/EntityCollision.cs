using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjetoFinal.Entities
{
    class EntityCollision
    {
        public static List<EntityCollision> EntityCollisions = new List<EntityCollision>();

        public Entity entityA, entityB;

        public EntityCollision(Entity entityA, Entity entityB)
        {
            this.entityA = entityA;
            this.entityB = entityB;
        }
    }
}
