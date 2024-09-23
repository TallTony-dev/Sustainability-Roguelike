using System.IO;
using System.Linq;

namespace Monogame_Cross_Platform.Scripts.GameObjects
{
    internal class AnimationHandler
    {
        private List<(int msPerFrame, int frames, ushort startingIndex)> animations = new List<(int msPerFrame, int frames, ushort startingIndex)>();
        private int currentAnimationIndex = 0;
        private double timeWhenStartedAnim = 0;
        public AnimationHandler(ushort textureIndex)
        {
            string animData = "error";
            animData = File.ReadLines("Content/AnimationData.txt").Skip((textureIndex) * 2).Take(1).First();

            string[] animationsSplit = animData.Split(",");
            foreach (string animation in animationsSplit)
            {
                string[] tokens = animation.Split("-");
                animations.Add((Convert.ToInt32(tokens[0]), Convert.ToInt32(tokens[1]), (ushort)(Convert.ToUInt16(tokens[2]) - ContentList.animationIndexOffset)));
            }
        }
        public void SetAnimationSpeed(int animationIndex, int newMSPerFrame)
        {
            (int msPerFrame, int frames, ushort startingIndex) = animations[animationIndex].ToTuple();
            animations[animationIndex] = (newMSPerFrame, frames, startingIndex);
        }
        public void SetAnimation(int animationIndex)
        {
            if(animationIndex != currentAnimationIndex)
            {
                currentAnimationIndex = animationIndex;
                timeWhenStartedAnim = Game1.gameTime.TotalGameTime.TotalMilliseconds;
            }
        }
        public void SetAnimationData(ushort animationDatatxtIndex)
        {
            animations.Clear();
            string animData = "error";
            animData = File.ReadLines("Content/AnimationData.txt").Skip((animationDatatxtIndex) * 2).Take(1).First();

            string[] animationsSplit = animData.Split(",");
            foreach (string animation in animationsSplit)
            {
                string[] tokens = animation.Split("-");
                animations.Add((Convert.ToInt32(tokens[0]), Convert.ToInt32(tokens[1]), (ushort)(Convert.ToUInt16(tokens[2]) - ContentList.animationIndexOffset)));
            }
        }

        /// <summary>
        /// Returns the current textureindex that the gameobject should be
        /// </summary>
        public ushort Update()
        {
            double time = Game1.gameTime.TotalGameTime.TotalMilliseconds;
            ushort currentFrame = (ushort)((ushort)((time - timeWhenStartedAnim) / animations[currentAnimationIndex].msPerFrame) % animations[currentAnimationIndex].frames);
            return (ushort)(currentFrame + animations[currentAnimationIndex].startingIndex);
        }
    }
}