using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame_Cross_Platform.Scripts.ContentManagers
{
    public class AudioPlayer
    {
        private SoundEffect[] soundEffects;
        Vector2 listenerPos;
        Vector2 listenerVelocity;
        internal static List<(SoundEffectInstance instance, Vector2 emitterPos)> activeSoundEffectInstances = new List<(SoundEffectInstance, Vector2)>();

        public AudioPlayer(SoundEffect[] soundEffects)
        {
            this.soundEffects = soundEffects;
        }
        public void UpdateListener(Vector2 listenerPos, Vector2 listenerNewPos)
        {
            this.listenerPos = listenerPos;
            listenerVelocity = new Vector2(listenerPos.X - listenerNewPos.X, listenerPos.Y - listenerNewPos.Y);
        }
        public void Update()
        {
            float overallMovement = listenerVelocity.X + listenerVelocity.Y / 1000;

            for (int i = 0; i < activeSoundEffectInstances.Count; i++)
            {
                if (activeSoundEffectInstances[i].instance.State == SoundState.Stopped)
                {
                    activeSoundEffectInstances[i].instance.Dispose();
                    activeSoundEffectInstances.RemoveAt(i);
                    return;
                }

                var instance = Apply3DTransform(activeSoundEffectInstances[i].emitterPos, activeSoundEffectInstances[i].instance, null);
                activeSoundEffectInstances[i] = (instance, activeSoundEffectInstances[i].emitterPos);
            }

        }
        public void PlaySoundEffect(ushort index)
        {
            soundEffects[index].Play();
        }
        public void PlaySoundEffect2D(ushort index, Vector2 emitterPos, Vector2 emitterNewPos)
        {
            SoundEffectInstance instance = soundEffects[index].CreateInstance();
            Apply3DTransform(emitterPos, instance, emitterNewPos);
            instance.Play();
            activeSoundEffectInstances.Add((instance, emitterPos));
        }
        public void PlaySoundEffect2D(ushort index, Vector2 emitterPos)
        {
            SoundEffectInstance instance = soundEffects[index].CreateInstance();
            Apply3DTransform(emitterPos, instance, null);
            instance.Play();
            activeSoundEffectInstances.Add((instance, emitterPos));
        }
        /// <summary>
        /// EmitterVelocity is a vector of the new position of the emitter, not relative movement.
        /// </summary>
        public SoundEffectInstance Apply3DTransform(Vector2 emitterPos, SoundEffectInstance instance, Vector2? emitterNewPos)
        {
            float distance = Vector2.Distance(emitterPos, listenerPos);
            if (distance < 1000)
                instance.Volume = 1 - distance / 1000;
            else
                instance.Volume = 0;
            float deltaX = listenerPos.X - emitterPos.X;
            float deltaY = listenerPos.Y - emitterPos.Y;
            Vector2 normalizedDeltaVals = Vector2.Normalize(new Vector2(deltaX, deltaY));
            instance.Pan = -normalizedDeltaVals.X + 0.9f * normalizedDeltaVals.X;

            float overallMovement = 0;
            if (emitterNewPos != null)
            {
                float velocityTotalX = listenerVelocity.X - (emitterNewPos.Value.X - emitterPos.X);
                float velocityTotalY = listenerVelocity.Y - (emitterNewPos.Value.Y - emitterPos.Y);
                overallMovement = ((normalizedDeltaVals.X * velocityTotalX) + (normalizedDeltaVals.Y * velocityTotalY));
            }
            else
            {
                overallMovement = (normalizedDeltaVals.X * listenerVelocity.X + normalizedDeltaVals.Y * listenerVelocity.Y);
            }

            float pitchChange = overallMovement / 20;
            if (pitchChange > -1 && pitchChange < 1)
                instance.Pitch = pitchChange;
            else throw new ArgumentOutOfRangeException();

            return instance;
        }
    }
}
