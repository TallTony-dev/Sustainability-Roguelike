using System.IO;
using System.Linq;

namespace Monogame_Cross_Platform.Scripts.GameObjects
{
    internal class AnimationHandler
    {
        private List<(int msPerFrame, int frames, ushort startingIndex)> textureAnimations = new List<(int msPerFrame, int frames, ushort startingIndex)>();
        private int currentAnimationIndex = 0;
        private double timeWhenStartedAnim = 0;
        public ushort animationIndex;

        float xToMove = 0;
        float yToMove = 0;
        float rotationToMove = 0;
        public List<(float deltaX, float deltaY, float deltaRotation, float timeRemaining)> movingAnimationsToPlay = new List<(float deltaX, float deltaY, float deltaRotation, float timeRemaining)>();

        public AnimationHandler(ushort animationIndex)
        {
            this.animationIndex = animationIndex;
            string animData = "error";
            animData = File.ReadLines("Content/AnimationData.txt").Skip((animationIndex) * 2).Take(1).First();

            string[] animationsSplit = animData.Split(",");
            foreach (string animation in animationsSplit)
            {
                string[] tokens = animation.Split("-");
                textureAnimations.Add((Convert.ToInt32(tokens[0]), Convert.ToInt32(tokens[1]), (ushort)(Convert.ToUInt16(tokens[2]) - ContentList.animationIndexOffset)));
            }
            if (Game1.gameTime != null)
                timeWhenStartedAnim = Game1.gameTime.TotalGameTime.TotalMilliseconds;
        }
        public void SetAnimationSpeed(int animationIndex, int newMSPerFrame)
        {
            (int msPerFrame, int frames, ushort startingIndex) = textureAnimations[animationIndex].ToTuple();
            textureAnimations[animationIndex] = (newMSPerFrame, frames, startingIndex);
        }
        /// <summary>
        /// picks from things on the same line,
        /// </summary>
        public void SetTextureAnimation(int animationIndex)
        {
            if(animationIndex != currentAnimationIndex)
            {
                currentAnimationIndex = animationIndex;
                timeWhenStartedAnim = Game1.gameTime.TotalGameTime.TotalMilliseconds;
            }
        }
        public void SetTextureAnimationData(ushort animationDatatxtIndex)
        {
            textureAnimations.Clear();
            string animData = "error";
            animData = File.ReadLines("Content/AnimationData.txt").Skip((animationDatatxtIndex) * 2).Take(1).First();

            string[] animationsSplit = animData.Split(",");
            foreach (string animation in animationsSplit)
            {
                string[] tokens = animation.Split("-");
                textureAnimations.Add((Convert.ToInt32(tokens[0]), Convert.ToInt32(tokens[1]), (ushort)(Convert.ToUInt16(tokens[2]) - ContentList.animationIndexOffset)));
            }
        }

        public void AddToMovementAnims(float deltaX, float deltaY, float deltaRotation, float timeRemaining)
        {
            movingAnimationsToPlay.Add((deltaX, deltaY, deltaRotation, timeRemaining));
        }

        /// <summary>
        /// Returns the current things that the gameobject should be
        /// </summary>
        public (ushort textureIndex, Vector2 position, float roation) Update(Vector2 position, float rotation)
        {
            double time = Game1.gameTime.TotalGameTime.TotalMilliseconds;
            ushort currentFrame = (ushort)((ushort)((time - timeWhenStartedAnim) / textureAnimations[currentAnimationIndex].msPerFrame) % textureAnimations[currentAnimationIndex].frames);
            
            if (movingAnimationsToPlay.Count > 0)
            {
                for (var x = movingAnimationsToPlay.Count - 1; x >= 0; x--)
                {
                    var animation = movingAnimationsToPlay[x];
                    xToMove += animation.deltaX * (1 / (float)(animation.timeRemaining - Game1.gameTime.ElapsedGameTime.TotalSeconds + 0.8)) / 10f;
                    yToMove += animation.deltaY * (1 / (float)(animation.timeRemaining - Game1.gameTime.ElapsedGameTime.TotalSeconds + 0.8)) / 10f;
                    rotationToMove += animation.deltaRotation * (1 / (float)(animation.timeRemaining - Game1.gameTime.ElapsedGameTime.TotalSeconds + 0.5)) / 10f;
                    movingAnimationsToPlay[x] = (animation.deltaX, animation.deltaY, animation.deltaRotation, animation.timeRemaining - (float)Game1.gameTime.ElapsedGameTime.TotalSeconds);
                    if (animation.timeRemaining < 0)
                        movingAnimationsToPlay.RemoveRange(x, 1);
                }
            }
            position.X += xToMove;
            position.Y += yToMove;
            rotation += rotationToMove;

            xToMove = 0;
            yToMove = 0;
            rotationToMove = 0;

            return ((ushort)(currentFrame + textureAnimations[currentAnimationIndex].startingIndex), position, rotation);
        }
    }
}