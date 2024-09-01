using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Monogame_Cross_Platform.Scripts;

namespace Monogame_Cross_Platform.Scripts.GameObjects.Entities
{
    internal class EntityMovement
    {
        AIType aiType;
        float entitySpeed;
        bool isInitialized = false;
        internal Vector2 ValidateMovement(Entity entity, Vector2 entityNewPos)
        {
            (Texture2D entityTexture, Rectangle rectangle) = ContentManagers.ContentLoader.texturesLoaded[entity.textureIndex]; //Maybe unnessacary but just wait and see
            
            (float tileMapX, float tileMapY) = GameObjects.Tiles.TileMap.PosToTileMapPos(entityNewPos);
            float maxVal = 4095f;
            float minVal = 0f;
            if (tileMapX >= maxVal)
                entityNewPos.X = maxVal * 32;
            if (tileMapX <= minVal)
                entityNewPos.X = minVal * 32;
            if (tileMapY >= maxVal)
                entityNewPos.Y = maxVal * 32;
            if (tileMapY <= minVal)
                entityNewPos.Y = minVal * 32;
            return entityNewPos;
        }

        public EntityMovement(AIType aiType, Entity entity)
        {
            this.aiType = aiType;
            this.entitySpeed = entity.entitySpeed;
        }

        public void EnableMovement() { isInitialized = true; }
        public void DisableMovement() { isInitialized = false; }

        public Vector2 GetPathfindingMovement(int attackRange, Vector2 currentPos, float speed)
        {
            (int x, int y) = GetPointToTravel(attackRange, currentPos);
            return new Vector2(x, y);
        }
        private (int, int) GetPointToTravel(int attackRange, Vector2 currentPos)
        {
            Vector2 tileMapPos = Tiles.TileMap.PosToTileMapPos(currentPos);

            if (aiType == AIType.enemy)
            {
                
            }
            if (aiType == AIType.npc)
            {

            }
            if (aiType == AIType.none)
            {

            }
            return (0, 0);
        }
        public enum AIType { none, enemy, npc }
    } 
}
