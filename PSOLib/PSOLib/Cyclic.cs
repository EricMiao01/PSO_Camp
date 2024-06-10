using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using System.Security.Cryptography;

namespace PSOLib
{
    public enum emCyclicType
    {
        Individual = 0,
        Explosion = 1,
    }

    public class Cyclic
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
        ///public event ParticleDelegateBool CheckParticle = null;
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

        #region "For Cyclic"
        public int StagThreshold = 100; // 幾個迭代無異動就視為停滯粒子
        public double MutateRate = 0.01;    // 停滯粒子轉成NLP粒子的轉換率 (for 脫離停滯狀態)
        public double RestoreRate = 0.001;    // 從NLP狀態回到一般狀態的機率
        public double RestoreGap = 0.1; // 重生位置在奇點(GB)位置的接近程度
        protected int[] _ParticleCyclicStatus = null;
        protected int[] _ParticalStag = null;
        protected int[] _ParticalStag1 = null;
        protected int[] _ParticalStag2 = null;
        //protected double[] _MemoryC1 = null;
        //protected double[] _MemoryC2 = null;
        //public List<double> _SuccessC1 = new List<double>();
        //public List<double> _SuccessC2 = new List<double>();
        //public List<double> _delta_f = new List<double>();
        //public double AvgStag = 0;
        #endregion

        public double epsilon = 0.0;
        public int cp = 2;
        public double theta = 0.2;
        public double Tc = 100000;
        public double alpha = 0.5;
        public double tau = 0.1;

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
        public Cyclic(int SwarmSize, double[] MaxX, double[] MinX, double[] MaxV, double[] MinV, ParticleDelegateDouble fitness,
                   int MaxGen = 1000, int MaxSec = -1,
                   double C1 = 2.0, double C2 = 2.0, double W = 0.1, double Ws = 0.9, double We = 0.5, bool AutoW = true)
        {
            _MaxX = MaxX;
            _MinX = MinX;
            _MaxV = MaxV;
            _MinV = MinV;

            _swarm = new Swarm(SwarmSize, _MaxX.Length);
            _MaxGen = MaxGen;
            _MaxSec = MaxSec;

            StagThreshold = (int)(MaxGen * 0.01);

            Param.C1 = C1;
            Param.C2 = C2;
            Param.W = W;
            Param.Ws = Ws;
            Param.We = We;
            Param.AutoW = AutoW;

            Fitness = fitness;

            //AvgStag = 0;

            // 以下為cyclic增加的部份;
            _ParticleCyclicStatus = new int[SwarmSize];
            for (int i = 0; i < SwarmSize; i++)
                if (i < SwarmSize * 0.8)
                    _ParticleCyclicStatus[i] = 1; // 設置90%為Cyclic;
                else
                    _ParticleCyclicStatus[i] = 0;

            //_ParticalStag = new int[SwarmSize];
            //for (int i = 0; i < SwarmSize; i++) // 初始化停滯次數;
            //    _ParticalStag[i] = 0;

            _ParticalStag1 = new int[SwarmSize];
            _ParticalStag2 = new int[SwarmSize];
            for (int i = 0; i < SwarmSize; i++) // 初始化停滯次數;
            {
                _ParticalStag1[i] = 0;
                _ParticalStag2[i] = 0;
            }

            /*=========================================*/
            //_MemoryC1 = new double[10];
            //for (int i = 0; i < 10; i++)
            //    _MemoryC1[i] = 2.0;

            //_MemoryC2 = new double[10];
            //for (int i = 0; i < 10; i++)
            //    _MemoryC2[i] = 2.0;
            /*=========================================*/

            ShowParticleCount();
        }

        ~Cyclic()
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

                    // initialize the c1 and c2 memory for each particle
                    /* ================================================ */
                    for (int i2 = 0; i2 < 10; i2++)
                    {
                        _swarm.Particle[i1].Curr.MemoryC1[i2] = 2.0;
                        _swarm.Particle[i1].Curr.MemoryC2[i2] = 2.0;
                        _swarm.Particle[i1].Curr.MemoryW[i2] = 1.0;
                    }
                    /* ================================================ */

                if (DoCheckParticle(_swarm.Particle[i1].Curr))
                    UpdateFitness(_swarm.Particle[i1].Curr, _swarm.Particle[i1].ParticleBest, epsilon);
            }

            ThisGeneration = 0;
            epsilon = UpdateEpsilon(_swarm.Particle);

            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            while (true)
            {
                ThisGeneration++;
                bool bEvolved = false;
                //int StagCnt = 0;
                //for (int i1 = 0; i1 < _swarm.Particle.Length; i1++)
                //{
                //    StagCnt += _ParticalStag[i1];
                //}
                //AvgStag = StagCnt / _swarm.Particle.Length;

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

                    /*=======================================================================================*/
                    // 3.調整X : 如果出界了 就被拉回到 GB 附近，RestoreGap = 0.1 (不同於傳統 PSO 會直接調整成 MIN_X 或 MAX_X)
                    for (int iX = 0; iX < Curr.X.Length; iX++)
                    {
                        // for Cyclic
                        if (Curr.X[iX] < _MinX[iX] || Curr.X[iX] > _MaxX[iX])
                        {
                            Curr.X[iX] = _swarm.GroupBest.X[iX] + (_swarm.Particle[i2].ParticleBest.X[iX]
                                         - _swarm.GroupBest.X[iX]) * RAND_SEED.NextDouble() * 0.01;
                            //particle.X[i] = GetGroupBest().X[i];
                        }

                        if (Curr.X[iX] < _MinX[iX]) Curr.X[iX] = _MinX[iX];
                        if (Curr.X[iX] > _MaxX[iX]) Curr.X[iX] = _MaxX[iX];
                    }
                    /*=======================================================================================*/

                    // 4.檢查限制式
                    if (DoCheckParticle(Curr))
                    {
                        // 5.計算objective value
                        Curr.Fitness = Fitness(Curr);
                        // 6.更新PB/GB
                        if (UpdateFitness(Curr, ParticleBest, epsilon)) bEvolved = true;
                    }

                    /////////////////////////////////////////////////
                    // 以下為Cyclic增加的流程;
                    if (_ParticleCyclicStatus[i2] == 0) continue; // 一般粒子不做任何變動;
                    PSOTuple tuple = GetCurr(i2);

                    //if (!bEvolved)
                    //    _ParticalStag[i2]++;
                    //else
                    //    _ParticalStag[i2] = 0;

                    if (!bEvolved)
                    {
                        _ParticalStag1[i2]++;
                        _ParticalStag2[i2]++;
                    }
                    else if (Curr.Convx > 0)
                        _ParticalStag1[i2]++;

                    
                    if (_ParticleCyclicStatus[i2] == 1)
                    {
                        double NormalizeStag = (double)_ParticalStag1[i2] / (double)this.StagThreshold;
                        double MutateRate = Utils.Sigmoid(NormalizeStag);

                        // 100代沒變化, 1%機會變成發散; -> 改成利用sigmoid考慮停滯比例調整變異機率
                        //if (_ParticalStag[i2] >= 100 && RAND_SEED.NextDouble() < 0.01)
                        if (RAND_SEED.NextDouble() < MutateRate)
                        {
                            _ParticleCyclicStatus[i2] = 2; // 變異為發散粒子;
                            _ParticalStag1[i2] = 0;
                            _ParticalStag2[i2] = 0;
                        }
                    }
                    else
                    {
                        // 發散時, 0.1%機會回到收歛; 並且從GB附近重生;
                        double NormalizeStag = _ParticalStag2[i2] / (this.StagThreshold);
                        double RestoreRate = Utils.Sigmoid(NormalizeStag);

                        //if (_swarm.GroupBest.Convx == 0)
                        //    RestoreRate = 0.4;

                        //if (_swarm.Particle[i2].ParticleBest.Convx > 0) RestoreRate = 0.0001;
                        if (RAND_SEED.NextDouble() < RestoreRate)
                        //if (RAND_SEED.NextDouble() < 0.001)
                        {
                            //Console.WriteLine("{0} change to exploitating.", i2);
                            _ParticleCyclicStatus[i2] = 1; // 變異為收歛粒子;
                            _ParticalStag1[i2] = 0;
                            _ParticalStag2[i2] = 0;
                            //從GB附近重生;
                            for (int iX = 0; iX < tuple.X.Length; iX++)
                            {
                                tuple.X[iX] = GetGroupBest().X[iX] + (_swarm.Particle[i2].ParticleBest.X[iX]
                                                - GetGroupBest().X[iX]) * RAND_SEED.NextDouble() * this.RestoreGap;
                                if (tuple.X[iX] < _MinX[iX]) tuple.X[iX] = _MinX[iX];
                                if (tuple.X[iX] > _MaxX[iX]) tuple.X[iX] = _MaxX[iX];
                            }
                        }
                    }
                    
                    /////////////////////////////////////////////////
                }

                epsilon = UpdateEpsilon(_swarm.Particle);
                //Console.WriteLine(epsilon);
                //ShowParticleCount();

                if (bEvolved)
                {
                    ShowParticleCount();
                    ResetAll_ParticleCyclicStatus(true);
                    //ShowParticleCount();
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
            double dChangeRate = 0.1;

            for (int i1 = 0; i1 < Curr.Velocity.Length; i1++)
            {
                r1 = RAND_SEED.NextDouble();
                r2 = RAND_SEED.NextDouble();

                if (_ParticleCyclicStatus[i1] == 2)
                {
                    //若是Cyclic之expr粒子, 採NLP移動方式;
                    bool bDir = (Curr.Velocity[i1] >= 0);
                    if (RAND_SEED.NextDouble() < dChangeRate) bDir = !bDir; // 換方向的變異;
                    if (bDir)
                        Curr.Velocity[i1] = _MaxV[i1] * Utils.GetRandomNormal();
                    else
                        Curr.Velocity[i1] = _MinV[i1] * Utils.GetRandomNormal();

                    Curr.ParticleType = 0; // for DECyc reference;
                }
                else
                {
                    //若一般粒子 或是 Cyclic之expi粒子, 採PSO(NLP paper 中提到的)移動方式;
                    double dCurr = Curr.X[i1];
                    double dLocalBest = LocalBest.X[i1];
                    double dGlobalBest = GroupBest.X[i1];

                    // C1 and C2;
                    /* ================================================== */
                    int C1_idx = (int)RAND_SEED.NextDouble() * 10;
                    int C2_idx = (int)RAND_SEED.NextDouble() * 10;
                    //int W_idx = (int)RAND_SEED.NextDouble() * 10;
                    do { Curr.C1 = Utils.GetRandomNormal(Curr.MemoryC1[C1_idx]); } while (Curr.C1 < 0);
                    do { Curr.C2 = Utils.GetRandomNormal(Curr.MemoryC2[C2_idx]); } while (Curr.C2 < 0);
                    //do { Curr.W = Utils.GetRandomNormal(Curr.MemoryW[W_idx]); } while (Curr.W <= 0);
                    /* ================================================== */

                    //double dValue = Param.W * Curr.Velocity[i1] + Param.C1 * r1 * (dLocalBest - dCurr) + Param.C2 * r2 * (dGlobalBest - dCurr);
                    double dValue = Param.W * Curr.Velocity[i1] + Curr.C1 * r1 * (dLocalBest - dCurr) + Curr.C2 * r2 * (dGlobalBest - dCurr);

                    if (dValue > _MaxV[i1]) dValue = _MaxV[i1];
                    if (dValue < _MinV[i1]) dValue = _MinV[i1];

                    Curr.Velocity[i1] = dValue;
                }
            }
        }

        // 4.檢查限制式;
        protected bool DoCheckParticle(PSOTuple Curr)
        {
            return true;
            //if (CheckParticle == null)
            //    Curr.IsFeasible = true;
            //else
            //    Curr.IsFeasible = CheckParticle(Curr);

            //return Curr.IsFeasible;
        }

        // 6.更新PB/GB
        protected bool UpdateFitness(PSOTuple Curr, PSOTuple ParticleBest, double epsilon)
        {
            // revise for cec2020rw
            ConstractResult constraint = GetConstraintResult(Curr);
            Curr.SetConvxFitness(constraint.g, constraint.h, Curr.Fitness);


            // 如果個體的有進步就將當前的 C1 與 C2 記錄下來
            /* ======================================================== */
            bool IsCurrImprove = false;
            if (!double.IsNaN(Curr.LastFitness) || !double.IsNaN(Curr.LastConvx))
            {
                if (Curr.Convx < Curr.LastConvx)
                {
                    IsCurrImprove = true;
                    Curr.ObjDelta[Curr.SuccessCounter] = Curr.Convx - Curr.LastConvx;
                }
                else if (Curr.Convx == Curr.LastConvx && Curr.Fitness < Curr.LastFitness)
                {
                    IsCurrImprove = true;
                    Curr.ObjDelta[Curr.SuccessCounter] = Curr.Fitness - Curr.LastFitness;
                }
                else IsCurrImprove = false;
            }
            if (IsCurrImprove)
            {
                //Curr.MemoryC1[Curr.MemoryIndicator] = Curr.C1;
                //Curr.MemoryC2[Curr.MemoryIndicator] = Curr.C2;
                ////Curr.MemoryW[Curr.MemoryIndicator] = Curr.W;
                //Curr.MemoryIndicator++;
                //if (Curr.MemoryIndicator == 9) Curr.MemoryIndicator = 0;
                Curr.SuccessC1[Curr.SuccessCounter] = Curr.C1;
                Curr.SuccessC2[Curr.SuccessCounter] = Curr.C2;
                Curr.SuccessCounter++;
                if (Curr.SuccessCounter == 4)
                {
                    Curr.MemoryC1[Curr.MemoryIndicator] = Utils.WeightedLehmerMean(Curr.ObjDelta, Curr.SuccessC1);
                    Curr.MemoryC2[Curr.MemoryIndicator] = Utils.WeightedLehmerMean(Curr.ObjDelta, Curr.SuccessC2);
                    Curr.MemoryIndicator++;
                    if (Curr.MemoryIndicator == 9) Curr.MemoryIndicator = 0;
                    Curr.SuccessCounter = 0;
                }
            }
            /* ======================================================== */

            if (double.IsNaN(ParticleBest.Fitness) || Curr.IsBetter(ParticleBest, epsilon)) Curr.CopyTo(ParticleBest);
            if (double.IsNaN(_swarm.GroupBest.Fitness) || Curr.IsBetter(_swarm.GroupBest, epsilon))
            {
                Curr.CopyTo(_swarm.GroupBest);
                return true;
            }

            return false;

            //Boolean ret = false;
            //if (double.IsNaN(Curr.Fitness)) return ret;
            
            //if (double.IsNaN(ParticleBest.Fitness) || Curr.Fitness < ParticleBest.Fitness) Curr.CopyTo(ParticleBest);

            //if (double.IsNaN(_swarm.GroupBest.Fitness) || Curr.Fitness < _swarm.GroupBest.Fitness)
            //{
            //    Curr.CopyTo(_swarm.GroupBest);
            //    ret = true;
            //}
            //return ret;
        }
        #endregion

        #region "For Cyclic"
        protected void ResetAll_ParticleCyclicStatus(bool bToExpi)
        {
            for (int i = 0; i < _swarm.Particle.Length; i++)
            {
                if (_ParticleCyclicStatus[i] == 0) continue; // 一般粒子不做任何變動;

                //if (_swarm.GroupBest.Convx > 0.0)
                //{
                if (bToExpi) // if (True) >>> 全部粒子均變為 Expi 模式 (!? 也包括原本設定 "永遠" 為PSO表現的粒子!? )
                {
                    _ParticleCyclicStatus[i] = 1;
                    _ParticalStag1[i] = 0;
                    _ParticalStag2[i] = 0;
                    //break;
                }
                else
                    _ParticleCyclicStatus[i] = 2;
                //break;
                //}
                //else
                //{
                //    if (bToExpi) // if (True) >>> 全部粒子均變為 Expi 模式 (!? 也包括原本設定 "永遠" 為PSO表現的粒子!? )
                //    {
                //        _ParticleCyclicStatus[i] = 1;
                //        //_ParticalStag[i] = 0;
                //    }
                //    else
                //        _ParticleCyclicStatus[i] = 2;
                //}
            }
        }

        protected virtual double UpdateEpsilon(ParticleUnit[] population)
        {
            if (ThisGeneration == 0)
            {
                int index = (int)(population.Length * theta);
                double convx_theta = population.OrderByDescending(x => x.Curr.Convx).ToArray()[(int)(population.Length * theta)].Curr.Convx;
                return convx_theta;
            }

            double convx_max = population.Max(x => x.Curr.Convx);
            double PFS = (double)population.Count(x => x.Curr.Convx == 0) / (double)population.Length;
            double _epsilon = 0.0;
            if (PFS < alpha && ThisGeneration < Tc) _epsilon = epsilon * Math.Pow((1 - ThisGeneration / (Tc)), cp);
            else if (PFS >= alpha && ThisGeneration < Tc) _epsilon = (1 + tau) * convx_max;
            else if (ThisGeneration >= Tc) epsilon = 0.0;

            //_epsilon = 0.0; // 不知道為甚麼設計成0更好?
            return _epsilon;
        }

        protected void ShowParticleCount()
        {
            int p0 = 0, p1 = 0, p2 = 0;
            for (int i = 0; i < _swarm.Particle.Length; i++)
            {
                if (_ParticleCyclicStatus[i] == 0) p0++;
                if (_ParticleCyclicStatus[i] == 1) p1++;
                if (_ParticleCyclicStatus[i] == 2) p2++;
            }

            Console.WriteLine("*** Particle Size: totol({0}), 0({1}), 1({2}), 2({3})", p0 + p1 + p2, p0, p1, p2);
        }
        #endregion
    }
}
