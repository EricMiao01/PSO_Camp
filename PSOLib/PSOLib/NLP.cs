using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSOLib
{
    public enum emMode
    {
        Deactive = 0,   // no NLPs particles;
        Auto = 1,       // use NLPs particles according to iterations;
        UserDefine = 2  // use some particles as NLPs with user defined;
    }

    public class NLP : PSO
    {
        public double NLP_Ratio = 0; // -1: no use, 0: by gen, 0.01~0.99: rate;
        public double NLP_DirCngThres = -1; // -1: byGen, 0.01~0.99: rate;
        private emMode _NLPMode = emMode.Auto;
        private Random RAND_SEED = new Random(Guid.NewGuid().GetHashCode());

        public emMode NLPMode
        {
            get { return _NLPMode; }
            set
            {
                switch (value)
                {
                    case emMode.Deactive:
                        NLP_Ratio = -1;
                        break;
                    case emMode.UserDefine:
                        NLP_Ratio = 0.1;
                        NLP_DirCngThres = 0.1;
                        break;
                    case emMode.Auto:
                        NLP_Ratio = 0;
                        NLP_DirCngThres = -1;
                        break;
                }
            }
        }

        public NLP(int SwarmSize, double[] MaxX, double[] MinX, double[] MaxV, double[] MinV, ParticleDelegateDouble fitness,
                   int MaxGen = 1000, int MaxSec = -1,
                   double C1 = 1.0, double C2 = 1.0, double W = 0.1, double Ws = 0.9, double We = 0.5, bool AutoW = true)
               :base(SwarmSize, MaxX, MinX, MaxV, MinV, fitness, MaxGen, MaxSec, C1, C2, W, Ws, We, AutoW)
        {
            //
        }

        protected bool IsNLP(PSOTuple Curr)
        {
            double _rate = NLP_Ratio;
            if (_rate == 0) _rate = 1 - (base.ThisGeneration / base._MaxGen);
            if (_rate > 0.9) _rate = 0.9;
            if (_rate < 0.1) _rate = 0.1;

            return (Curr.ID < GetSwarmSize() * _rate);
        }


        protected override void CalcVelocity(ParticleUnit particle, PSOTuple gb, CParam Param)
        {
            PSOTuple Curr = particle.Curr;
            PSOTuple LocalBest = particle.ParticleBest;
            PSOTuple GroupBest = gb;

            double r1;
            double r2;
            double dChangeRate = NLP_DirCngThres;
            double r = 0.9 - 0.8 * (base.ThisGeneration / base._MaxGen);

            if (dChangeRate == -1) dChangeRate = 1 - r;

            for (int i = 0; i < Curr.Velocity.Length; i++)
            {
                r1 = RAND_SEED.NextDouble();
                r2 = RAND_SEED.NextDouble();

                if (IsNLP(Curr)) // 若為NLP粒子時, 依NLP算法;
                {
                    bool bDir = (Curr.Velocity[i] >= 0);

                    if (RAND_SEED.NextDouble() < dChangeRate) bDir = !bDir; // 換方向的變異;
                    if (bDir)
                        Curr.Velocity[i] = _MaxV[i] * Utils.GetRandomNormal(); // r_3,j 採用服從標準常態分佈的隨機數
                    else
                        Curr.Velocity[i] = _MinV[i] * Utils.GetRandomNormal();
                }
                else // 若為一般粒子時, 依PSO算法;
                {
                    base.CalcVelocity(particle, gb, Param);
                }
            }
        }

    }
}
