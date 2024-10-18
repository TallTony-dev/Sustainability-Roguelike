using Monogame_Cross_Platform.Scripts.Hitboxes;
using Monogame_Cross_Platform.Scripts.Level;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame_Cross_Platform.Scripts.GameObjects.Objects
{
    internal class Exit : InteractableObject
    {
        public override void Interact()
        {
            GameState.NextLevel();
        }
        public Exit(ushort animIndex, Vector2 startingTile, Vector2 hitboxSize) : base(animIndex, startingTile, hitboxSize, false)
        {
            isEnabled = true;
            textureIndex = 1; //temp
        }
    }
    
    //Partly implemented, not tested
    internal class Chest : InteractableObject
    {
        GameObject itemToRelease;
        public void OpenChest()
        {
            Game1.currentGameObjects.Add(itemToRelease);
            itemToRelease.isEnabled = true;
            LevelGenerator.PosToRoom(itemToRelease.position).gameObjects.Add(itemToRelease);
            Destroy();
        }
        public override void Interact()
        {
            OpenChest();
        }
        public Chest(ushort animIndex, Vector2 startingTile, Vector2 hitboxSize, GameObject itemToRelease) : base(animIndex, startingTile, hitboxSize, false)
        {
            isEnabled = true;
            this.itemToRelease = itemToRelease;
            Game1.currentGameObjects.Remove(itemToRelease);
        }
    }

    internal abstract class InteractableObject : CollidableObject
    {
        double timeWhenInteracted = 0;
        public abstract void Interact();
        public void Update(Hitbox playerHitbox)
        {
            if (isEnabled && hitBox.Intersects(playerHitbox) && (Keyboard.GetState().IsKeyDown(Keys.E) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A)) && Game1.gameTime.TotalGameTime.TotalSeconds - timeWhenInteracted > 0.3)
            {
                Interact();
                timeWhenInteracted = Game1.gameTime.TotalGameTime.TotalSeconds;
            }
        }
        public InteractableObject(ushort animIndex, Vector2 startingTile, Vector2 hitboxSize, bool isBarrier) : base(animIndex, startingTile, hitboxSize, isBarrier)
        {

        }
    }
}
