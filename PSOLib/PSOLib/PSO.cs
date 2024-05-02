using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;

namespace PSOLib
{
    public class CParam
    {
        // parameter
        public double C1 = 1;
        public double C2 = 1;
        public double W = 0.1;
        public double Ws = 0.9;
        public double We = 0.5;

        // method
        public bool AutoW = true;
    }

    #region "Delegate declaration"
    public delegate double ParticleDelegateDouble(PSOTuple particle);
    public delegate bool ParticleDelegateBool(PSOTuple particle);
    public delegate void ParticleDelegateVoid(PSOTuple particle);
    public delegate bool RunSwarmDelegateBool(Swarm swarm, int iteration, TimeSpan span);
    public delegate void RunSwarmDelegateVoid(Swarm swarm, int iteration, TimeSpan span);
    public delegate void UpdateVelocityDelegate(ParticleUnit particle, PSOTuple gb, CParam Param);
    public delegate ConstractResult GetConstraintResultDelegate(PSOTuple particle);
    #endregion

    public class PSO
    {
        #region "Events"
        /// <summary>
        /// (選填事件) 粒子初始化時叫用, 未設置則使用預設方法;
        /// </summary>
        public event ParticleDelegateVoid Initialize = null;
        /// <summary>
        /// (選填事件) 檢查是否結束最佳化程序叫用, 未設置則使用初始化設置的迭代數;
        /// </summary>
        public event RunSwarmDelegateBool CheckTerminate = null;
        /// <summary>
        /// (選填事件) 找到新的群體最佳時觸發;
        /// </summary>
        public event RunSwarmDelegateVoid OnEvolute = null;
        /// <summary>
        /// (選填事件) 迭代完成時觸發;
        /// </summary>
        public event RunSwarmDelegateVoid OnGeneration = null;
        /// <summary>
        /// (選填事件) 檢查粒子是否合法時叫用(若合法需return True);
        /// </summary>
        /// public event ParticleDelegateBool CheckParticle = null;
        /// <summary>
        /// (選填事件) 取得限制式檢核後的數值陣列(g-不等式, h-等式)
        /// </summary>
        public event GetConstraintResultDelegate GetConstraintResult = null;
        /// <summary>
        /// (必填事件) 計算Fitness value時叫用;
        /// </summary>
        protected event ParticleDelegateDouble Fitness = null;
        /// <summary>
        /// (選填事件) 更新粒子速度時叫用, 未設置則使用預設方法;
        /// </summary>
        public event UpdateVelocityDelegate UpdateVelocity = null;
        /// <summary>
        /// (選填事件) 粒子移動時叫用, 未設置則使用預設方法;
        /// </summary>
        public event ParticleDelegateVoid MoveParticle = null;
        #endregion

        #region "Properties"
        protected Swarm _swarm = null;
        private Random RAND_SEED = new Random(Guid.NewGuid().GetHashCode());
        public CParam Param = new CParam();
        
        protected int ThisGeneration = 0;
        /// <summary>
        /// 取得總耗用迭代數;
        /// </summary>
        public int ElapsedGenerations
        {
            get { return ThisGeneration; }
        }
        private double _ElapsedSeconds = 0;
        /// <summary>
        /// 取得總耗用秒數;
        /// </summary>
        public double ElapsedSeconds
        {
            get { return _ElapsedSeconds; }
        }
        protected int _MaxGen = 0;
        protected int _MaxSec = -1;
        #endregion

        #region "Global variables"
        protected double[] _MinX = null;
        protected double[] _MaxX = null;
        protected double[] _MinV = null;
        protected double[] _MaxV = null;
        #endregion

        #region "Swarm information get methods"
        public PSOTuple GetGroupBest()
        {
            return _swarm.GroupBest;
        }
        protected PSOTuple GetLocalBest(int nIndex)
        {
            return _swarm.Particle[nIndex].ParticleBest;
        }
        protected ParticleUnit GetParticle(int nIndex)
        {
            return _swarm.Particle[nIndex];
        }
        protected PSOTuple GetCurr(int nIndex)
        {
            return _swarm.Particle[nIndex].Curr;
        }
        protected int GetSwarmSize()
        {
            return _swarm.Particle.Length;
        }
        #endregion

        /// <summary>
        /// Swarm 建構子;
        /// </summary>
        /// <param name="SwarmSize">Swarm粒子數</param>
        /// <param name="MaxX">Upper Boundary (請注意維度一致性)</param>
        /// <param name="MinX">Lower Boundary (請注意維度一致性)</param>
        /// <param name="MaxV">最大移動速率 (請注意維度一致性)</param>
        /// <param name="MinV">最小移動速率 (請注意維度一致性)</param>
        /// <param name="fitness">最佳化之目標函式</param>
        /// <param name="MaxGen">最大執行迭代數, 預設為1000迭代, 若有設置OnCheckTerminate時本參數無效</param>
        /// <param name="MaxSec">最大執行秒數, 預設為-1(不檢核), 若有設置OnCheckTerminate時本參數無效</param>
        /// <param name="C1">認知學習因子C1, 預設為1</param>
        /// <param name="C2">社會學習因子C2, 預設為1</param>
        /// <param name="W">慣性權重W (固定值), 預設為0.1</param>
        /// <param name="Ws">慣性權重W (隨迭代變動之起始值), 預設為0.9</param>
        /// <param name="We">慣性權重W (隨迭代變動之結束值), 預設為0.5</param>
        /// <param name="AutoW">慣性權重W 參數是否隨迭代變動, 預設為true</param>

        public PSO(int SwarmSize, double[] MaxX, double[] MinX, double[] MaxV, double[] MinV, ParticleDelegateDouble fitness,
                   int MaxGen = 1000, int MaxSec = -1,
                   double C1 = 1.0, double C2 = 1.0, double W = 0.1, double Ws = 0.9, double We = 0.5, bool AutoW = true)
        {
            _MaxX = MaxX;
            _MinX = MinX;
            _MaxV = MaxV;
            _MinV = MinV;

            _swarm = new Swarm(SwarmSize, _MaxX.Length);
            _MaxGen = MaxGen;
            _MaxSec = MaxSec;

            Param.C1 = C1;
            Param.C2 = C2;
            Param.W = W;
            Param.Ws = Ws;
            Param.We = We;
            Param.AutoW = AutoW;

            Fitness = fitness;
        }

        ~PSO()
        {
            _swarm = null;
        }

        private void CheckParameters()
        {
            Utils.Assert(_swarm.Particle.Length > 0, "Swarm size must large than 0.");
            Utils.Assert(_MaxX.Length == _MinX.Length && _MinX.Length == _MaxV.Length && _MaxV.Length == _MinV.Length, "Bounaday size must be the same.");
            Utils.Assert(Fitness != null, "Objective function cannot be null.");
            Utils.Assert(_MaxGen > 0 || _MaxSec > 0, "MaxGen or MaxSec must large than 0.");
        }

        /// <summary>
        /// 開始執行最佳化;
        /// </summary>
        public void StartOptimize()
        {
            // 參數檢查;
            CheckParameters();
            
            // 0.INIT;
            for (int i1 = 0; i1 < _swarm.Particle.Length; i1++)
            {
                if (Initialize != null)
                    Initialize(_swarm.Particle[i1].Curr);
                else
                    for (int i2 = 0; i2 < _swarm.Particle[i1].Curr.X.Length; i2++)
                    {
                        double dX = _MinX[i2] + (_MaxX[i2] - _MinX[i2]) * RAND_SEED.NextDouble();
                        double dV = _MinV[i2] + (_MaxV[i2] - _MinV[i2]) * RAND_SEED.NextDouble();
                        _swarm.Particle[i1].Curr.X[i2] = dX;
                        _swarm.Particle[i1].Curr.Velocity[i2] = dV;
                    }

                if (DoCheckParticle(_swarm.Particle[i1].Curr))
                {
                    _swarm.Particle[i1].Curr.Fitness = Fitness(_swarm.Particle[i1].Curr);
                    UpdateFitness(_swarm.Particle[i1].Curr, _swarm.Particle[i1].ParticleBest);
                }
            }

            ThisGeneration = 0;
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            while (true)
            {
                ThisGeneration++;
                bool bEvolved = false;
                // 0.計算W, if autoW
                VelocityFactor();

                for (int i2 = 0; i2 < _swarm.Particle.Length; i2++)
                {
                    PSOTuple Curr = _swarm.Particle[i2].Curr;
                    PSOTuple ParticleBest = _swarm.Particle[i2].ParticleBest;

                    // 1.計算V
                    if (UpdateVelocity != null)
                        UpdateVelocity(_swarm.Particle[i2], _swarm.GroupBest, Param);
                    else
                        CalcVelocity(_swarm.Particle[i2], _swarm.GroupBest, Param);

                    // 2.移動X
                    if (MoveParticle != null)
                        MoveParticle(Curr);
                    else
                        Curr.Move();

                    // 3.調整X
                    for (int iX = 0; iX < Curr.X.Length; iX++)
                    {
                        if (Curr.X[iX] < _MinX[iX]) Curr.X[iX] = _MinX[iX];
                        if (Curr.X[iX] > _MaxX[iX]) Curr.X[iX] = _MaxX[iX];
                    }

                    // 4.檢查限制式
                    if (DoCheckParticle(Curr))
                    {
                        // 5.計算objective value
                        Curr.Fitness = Fitness(Curr);
                        // 6.更新PB/GB
                        if (UpdateFitness(Curr, ParticleBest)) bEvolved = true;
                    }
                }

                if (OnGeneration != null) OnGeneration(_swarm, ThisGeneration, watch.Elapsed);
                if (bEvolved && OnEvolute != null) OnEvolute(_swarm, ThisGeneration, watch.Elapsed);

                // 7.檢查是否結束;
                if (CheckTerminate != null)
                {
                    if (CheckTerminate(_swarm, ThisGeneration, watch.Elapsed)) break;
                } else {
                    if (_MaxGen > 0 && ThisGeneration >= _MaxGen) break;
                    if (_MaxSec > 0 && watch.Elapsed.TotalSeconds >= _MaxSec) break;
                }
            }
            watch.Stop();
            _ElapsedSeconds = watch.Elapsed.TotalSeconds;
        }

        #region "PSO procedures"
        
        // 0.計算W, if AutoW;
        protected void VelocityFactor()
        {
            if (!Param.AutoW) return;
            Param.W = Param.Ws - (Param.Ws - Param.We) * ((double)ThisGeneration / Math.Max(ThisGeneration, _MaxGen));
        }

        // 1.計算V
        protected virtual void CalcVelocity(ParticleUnit particle, PSOTuple gb, CParam Param)
        {
            PSOTuple Curr = particle.Curr;
            PSOTuple LocalBest = particle.ParticleBest;
            PSOTuple GroupBest = gb;

            double r1;
            double r2;

            for (int i1 = 0; i1 < Curr.Velocity.Length; i1++)
            {
                r1 = RAND_SEED.NextDouble();
                r2 = RAND_SEED.NextDouble();

                double dCurr = Curr.X[i1];
                double dLocalBest = LocalBest.X[i1];
                double dGlobalBest = GroupBest.X[i1];

                double dValue = Param.W * Curr.Velocity[i1] + Param.C1 * r1 * (dLocalBest - dCurr) + Param.C2 * r2 * (dGlobalBest - dCurr);

                if (dValue > _MaxV[i1]) dValue = _MaxV[i1];
                if (dValue < _MinV[i1]) dValue = _MinV[i1];

                Curr.Velocity[i1] = dValue;
            }
        }

        // 4.檢查限制式;
        protected bool DoCheckParticle(PSOTuple Curr)
        {
            return true; // 因cec2020rw採用convx作為CHT, CheckParticle永遠為true, 並改寫UpdateFitness;

            /*
            if (CheckParticle == null)
                Curr.IsFeasible = true;
            else
                Curr.IsFeasible = CheckParticle(Curr);

            return Curr.IsFeasible;
             */
        }

        // 6.更新PB/GB
        protected bool UpdateFitness(PSOTuple Curr, PSOTuple ParticleBest)
        {
            // revise for cec2020rw
            ConstractResult constraint = GetConstraintResult(Curr);
            Curr.SetConvxFitness(constraint.g, constraint.h, Curr.Fitness);
            if (double.IsNaN(ParticleBest.Fitness) || Curr.IsBetter(ParticleBest)) Curr.CopyTo(ParticleBest);
            if (double.IsNaN(_swarm.GroupBest.Fitness) || Curr.IsBetter(_swarm.GroupBest))
            {
                Curr.CopyTo(_swarm.GroupBest);
                return true;
            }

            return false;

            // original version
            /*
            Boolean ret = false;
            if (double.IsNaN(Curr.Fitness)) return ret;
            
            if (double.IsNaN(ParticleBest.Fitness) || Curr.Fitness < ParticleBest.Fitness) Curr.CopyTo(ParticleBest);

            if (double.IsNaN(_swarm.GroupBest.Fitness) || Curr.Fitness < _swarm.GroupBest.Fitness)
            {
                Curr.CopyTo(_swarm.GroupBest);
                ret = true;
            }
            return ret;
             */
        }
        #endregion

    }
}
