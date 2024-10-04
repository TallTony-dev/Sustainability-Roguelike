using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame_Cross_Platform.Scripts.Particles
{
    internal class ParticleEmitter
    {
        Particle particleToEmit;

        public void Update()
        {

        }

        public ParticleEmitter(Particle particleToEmit) 
        { 
            this.particleToEmit = particleToEmit;
        }
    }
}
