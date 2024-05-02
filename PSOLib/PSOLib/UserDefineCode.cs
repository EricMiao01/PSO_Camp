using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSOLib
{
    public abstract class UserDefineCode
    {
        public abstract void InitializeParameter(CParam param);

        public virtual bool Initialize(CParam param, PSOTuple Curr) { return false; }

        public virtual bool VelocityFactor(CParam param) { return false; }

        public virtual bool UpdateVelocity(PSOTuple Curr, PSOTuple LocalBest, PSOTuple GroupBest, CParam param) { return false; }

        public virtual bool MoveParticle(PSOTuple Curr) { return false; }

        public abstract bool CheckParticle(PSOTuple particle);

        public abstract double Fitness(PSOTuple particle);

        public virtual void AfterFitness(BasePSO pso, int Generation, bool Evolved) { }

        public virtual bool CheckTerminate(BasePSO pso, int Generation, TimeSpan duration) { return Generation > 1000; }

    }

}
