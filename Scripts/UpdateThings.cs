using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Monogame_Cross_Platform.Scripts.GameObjects.Entities.Player;
using Monogame_Cross_Platform.Scripts.GameObjects.Entities;

namespace Monogame_Cross_Platform.Scripts
{
    /// <summary>
    /// Class you made for updating entities and tiles? atleast entities rn
    /// </summary>
    internal static class UpdateThings
    {

        public static void UpdateEntities(List<Entity> entityList, Player player)
        {
            foreach (Entity entity in entityList)
            {
                entity.Update(player);
            }
        }
    }
}
