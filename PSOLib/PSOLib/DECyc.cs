using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSOLib
{
    public class DECyc : Cyclic
    {
        public double DE_MutateRate = 0.2;

        public DECyc(int SwarmSize, double[] MaxX, double[] MinX, double[] MaxV, double[] MinV, ParticleDelegateDouble fitness,
                   int MaxGen = 1000, int MaxSec = -1,
                   double C1 = 1.0, double C2 = 1.0, double W = 0.1, double Ws = 0.9, double We = 0.5, bool AutoW = true)
               :base(SwarmSize, MaxX, MinX, MaxV, MinV, fitness, MaxGen, MaxSec, C1, C2, W, Ws, We, AutoW)
        {
            MoveParticle += DoMoveParticle;
            DE_MutateRate = 0.2;
        }

        // 非突變粒子才需要Move; 突變粒子直接調整X, 不需移動;
        protected void DoMoveParticle(PSOTuple particle)
        {
            if (particle.ParticleType == 0) particle.Move();
        }

        protected override void CalcVelocity(ParticleUnit particle, PSOTuple gb, CParam Param)
        {
            PSOTuple Curr = particle.Curr;
            PSOTuple LocalBest = particle.ParticleBest;
            PSOTuple GroupBest = gb;

            Random RAND_SEED = new Random(Guid.NewGuid().GetHashCode());
            Param.W = Utils.GetRandomNormal();

            if (RAND_SEED.NextDouble() > DE_MutateRate)
            {
                base.CalcVelocity(particle, gb, Param);
                Curr.ParticleType = 0;
            }
            else
            {
                for (int j = 0; j < Curr.X.Length; j++)
                {
                    if (RAND_SEED.NextDouble() > DE_MutateRate) continue; // 判斷粒子的某個維度是否需要突變
                    int nMutateIndex = (int)(RAND_SEED.NextDouble() * GetSwarmSize()); // 隨機從 Swarm/Population 中選擇突變粒子(取得它的 Index i)

                    PSOTuple Mutator = base.GetLocalBest(nMutateIndex); // 取得突變粒子的 LocalBest (個體最佳值)
                    Curr.X[j] = Mutator.X[j]; // 另當前粒子的某維度值等於突變例子的某維度值 (其實就是在進型 crossover 的動作; 這個 block 中的 MutatieRate 更像是 CrossoverRate)
                }
                Curr.ParticleType = 1;
            }
        }

    }
}
