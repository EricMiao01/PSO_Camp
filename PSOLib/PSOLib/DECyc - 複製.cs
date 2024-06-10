using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSOLib
{
    public class CycDE : Cyclic_D
    {
        public double DE_MutateRate = 0.25;
        protected event ParticleDelegateDouble Fitness = null;

        public List<PSOTuple> Archives = new List<PSOTuple>();

        public CycDE(int SwarmSize, double[] MaxX, double[] MinX, double[] MaxV, double[] MinV, ParticleDelegateDouble fitness,
                   int MaxGen = 1000, int MaxSec = -1,
                   double C1 = 1.0, double C2 = 1.0, double W = 0.1, double Ws = 0.9, double We = 0.5, bool AutoW = true)
               :base(SwarmSize, MaxX, MinX, MaxV, MinV, fitness, MaxGen, MaxSec, C1, C2, W, Ws, We, AutoW)
        {
            MoveParticle += DoMoveParticle;
            DE_MutateRate = 1;
            Fitness = fitness;
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
                // 大改這裡!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                //for (int j = 0; j < Curr.X.Length; j++)
                //{
                //    if (RAND_SEED.NextDouble() > DE_MutateRate) continue; // 判斷粒子的某個維度是否需要突變
                //    int nMutateIndex = (int)(RAND_SEED.NextDouble() * GetSwarmSize()); // 隨機從 Swarm/Population 中選擇突變粒子(取得它的 Index i)

                //    PSOTuple Mutator = base.GetLocalBest(nMutateIndex); // 取得突變粒子的 LocalBest (個體最佳值)
                //    Curr.X[j] = Mutator.X[j]; // 另當前粒子的某維度值等於突變例子的某維度值 (其實就是在進型 crossover 的動作; 這個 block 中的 MutatieRate 更像是 CrossoverRate)
                //}
                PSOTuple TrialVector;
                PSOTuple MutationVector = Rand_1_Mutation(Curr, Curr.ID, 0.5);
                TrialVector = Bin_Crossover(Curr, MutationVector, 0.5);


                // 調整 X
                for (int iX = 0; iX < Curr.X.Length; iX++)
                {
                    if (Curr.X[iX] < _MinX[iX]) Curr.X[iX] = _MinX[iX];
                    if (Curr.X[iX] > _MaxX[iX]) Curr.X[iX] = _MaxX[iX];

                    if (TrialVector.X[iX] < _MinX[iX]) TrialVector.X[iX] = _MinX[iX];
                    if (TrialVector.X[iX] > _MaxX[iX]) TrialVector.X[iX] = _MaxX[iX];
                }
                ConstractResult constraint = base.GetConstaint(Curr);
                Curr.Fitness = Fitness(Curr);
                TrialVector.Fitness = Fitness(TrialVector);
                Curr.SetConvxFitness(constraint.g, constraint.h, Curr.Fitness);
                TrialVector.SetConvxFitness(constraint.g, constraint.h, TrialVector.Fitness);


                if (TrialVector.IsBetter(Curr))
                {
                    for (int j = 0; j < Curr.X.Length; j++)
                    {
                        Curr.LastX[j] = TrialVector.X[j];
                    }
                    Curr.LastFitness = TrialVector.Fitness;
                    Curr.LastConvx = TrialVector.Convx;
                    Curr.IsUpdate = true;
                    if (Archives.Count >= GetSwarmSize())
                    {
                        int index = (int)RAND_SEED.Next(0, Archives.Count);
                        Archives.RemoveAt(index);
                    }
                    Archives.Add(Curr);
                }
                else
                {
                    if (Archives.Count >= GetSwarmSize())
                    {
                        int index = (int)RAND_SEED.Next(0, Archives.Count);
                        Archives.RemoveAt(index);
                    }
                    Archives.Add(Curr);
                }
                Curr.ParticleType = 1;
            }
        }

        protected override bool UpdateFitness(PSOTuple Curr, PSOTuple ParticleBest)
        {
            // revise for cec2020rw
            ConstractResult constraint = base.GetConstaint(Curr);
            Curr.SetConvxFitness(constraint.g, constraint.h, Curr.Fitness); 


            // 如果個體的有進步就將當前的 C1 與 C2 記錄下來
            if (Curr.ParticleType != 1)
            {
            /* ============================================================= */
                bool IsCurrImprove = false;
                if (!double.IsNaN(Curr.LastFitness) || !double.IsNaN(Curr.LastConvx))
                {
                    if (Curr.Convx < Curr.LastConvx) IsCurrImprove = true;
                    else if (Curr.Convx == Curr.LastConvx && Curr.Fitness < Curr.LastFitness) IsCurrImprove = true;
                    else IsCurrImprove = false;
                }
                if (IsCurrImprove)
                {
                    Curr.MemoryC1[Curr.MemoryIndicator] = Curr.C1;
                    Curr.MemoryC2[Curr.MemoryIndicator] = Curr.C2;
                    //Curr.MemoryW[Curr.MemoryIndicator] = Curr.W;
                    Curr.MemoryIndicator++;
                    if (Curr.MemoryIndicator == 10) Curr.MemoryIndicator = 0;
                }
            }
            /* ============================================================= */
            //else
            //{
            //    if (Curr.IsUpdate)
            //    {
            //        for (int j = 0; j < Curr.X.Length; j++)
            //        {
            //            Curr.X[j] = Curr.LastX[j];
            //        }
            //        Curr.Fitness = Curr.LastFitness;
            //        Curr.Convx = Curr.LastConvx;
            //        Curr.IsUpdate = false;
            //    }
            //}
            if (double.IsNaN(ParticleBest.Fitness) || Curr.IsBetter(ParticleBest)) Curr.CopyTo(ParticleBest);
            if (double.IsNaN(_swarm.GroupBest.Fitness) || Curr.IsBetter(_swarm.GroupBest))
            {
                Curr.CopyTo(_swarm.GroupBest);
                return true;
            }

            return false;
        }

        protected virtual PSOTuple Rand_1_Mutation(PSOTuple Curr, int i, double MutationFactor)
        {
            int PopulationSize = GetSwarmSize();
            //int PopulationSize = GetPopulationSize();
            //int ArchiveSize = Param.Archives.Count;

            Random RAND_SEED = new Random(Guid.NewGuid().GetHashCode());
            int r1, r2, r3;
            do { r1 = (int)(RAND_SEED.NextDouble() * (PopulationSize + Archives.Count)); } while (r1 == i);
            do { r2 = (int)(RAND_SEED.NextDouble() * (PopulationSize + Archives.Count)); } while (r2 == i || r2 == r1);
            do { r3 = (int)(RAND_SEED.NextDouble() * (PopulationSize + Archives.Count)); } while (r3 == i || r3 == r2 || r3 == r1);

            PSOTuple X_r1, X_r2, X_r3;


            if (r1 >= PopulationSize)
            {
                X_r1 = Archives[r1 - PopulationSize];
            }
            else
            {
                X_r1 = base._swarm.Particle[r1].Curr;
            }

            if (r2 >= PopulationSize)
            {
                X_r2 = Archives[r2 - PopulationSize];
            }
            else
            {
                X_r2 = base._swarm.Particle[r2].Curr;
            }

            if (r3 >= PopulationSize)
            {
                X_r3 = Archives[r3 - PopulationSize];
            }
            else
            {
                X_r3 = base._swarm.Particle[r3].Curr;
            }

            //PSOTuple X_r1 = base._swarm.Particle[r1].Curr;
            //PSOTuple X_r2 = base._swarm.Particle[r2].Curr;
            //PSOTuple X_r3 = base._swarm.Particle[r3].Curr;

            PSOTuple MutationVector = new PSOTuple(Curr.X.Length);
            for (int j = 0; j < Curr.X.Length; j++)
            {
                MutationVector.X[j] = X_r1.X[j] + MutationFactor * (X_r2.X[j] - X_r3.X[j]);
            }

            return MutationVector;
        }


        protected virtual PSOTuple Bin_Crossover(PSOTuple Curr, PSOTuple MutationVector, double CrossoverRate)
        {
            int Dimension = Curr.X.Length;
            Random RAND_SEED = new Random(Guid.NewGuid().GetHashCode());
            int j_rand = (int)(RAND_SEED.NextDouble() * Dimension);

            PSOTuple TrialVector = new PSOTuple(Dimension);
            for (int j = 0; j < Dimension; j++)
            {
                if (RAND_SEED.NextDouble() <= CrossoverRate || j == j_rand)
                    TrialVector.X[j] = MutationVector.X[j];
                else
                    TrialVector.X[j] = Curr.X[j];
            }

            return TrialVector;
        }

    }
}
