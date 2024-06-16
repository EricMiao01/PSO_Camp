using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;

namespace PSOLib
{

    /// <summary>
    /// Swarm 物件: 由多個ParticleUnit組成, 並額外設置GroupBest;
    /// Particle的集合體, 並提供檔案存取方法;
    /// </summary>
    public class Swarm
    {
        public PSOTuple GroupBest;
        public ParticleUnit[] Particle;

        // 初始化 Swarm 的方法，會在建立 PSO 物件時通過傳入 Swarm 大小以及問題維度數量來產生;
        public Swarm(int nParticleSize, int nParamCount)
        {
            GroupBest = new PSOTuple(nParamCount);

            Particle = new ParticleUnit[nParticleSize];
            for (int i = 0; i < nParticleSize; i++)
            {
                Particle[i] = new ParticleUnit(nParamCount);
                Particle[i].Curr.ID = i;
            }
        }

        public void LoadFromFile(string sFilename)
        {
            string sData = Utils.ReadText(sFilename);
            int nRowCnt = 0;
            int nIdx = 0;
            foreach (string token in sData.Split('\n'))
            {
                string[] aRow = token.Split(' ');
                if (nRowCnt == 0) nRowCnt = aRow.Length;
                if (aRow.Length != nRowCnt) continue;

                PSOTuple pso = null;
                if (aRow[0].StartsWith("GB")) pso = GroupBest;
                if (aRow[0].StartsWith("PX")) pso = Particle[nIdx].Curr;
                if (aRow[0].StartsWith("LB")) { pso = Particle[nIdx].ParticleBest; nIdx++; }

                aRow[0] = aRow[0].Substring(11).Trim();
                pso.Fitness = Double.Parse(aRow[0]);
                for (int i = 0; i < pso.X.Length; i++)
                {
                    aRow[2 + i] = aRow[2 + i].Replace(string.Format("X{0}=", i + 1), "").Trim();
                    pso.X[i] = Double.Parse(aRow[2 + i]);
                }
            }
        }

        public void SaveToFile(string sFilename)
        {
            SaveParticle(sFilename, 2, GroupBest);

            foreach (ParticleUnit pair in Particle)
            {
                SaveParticle(sFilename, 0, pair.Curr);
                SaveParticle(sFilename, 1, pair.ParticleBest);
            }
        }

        private void SaveParticle(string sFilename, int nType, PSOTuple particle)
        {
            if (nType == 0)
                Utils.WriteText(sFilename, string.Format("PX:{0}", particle.ToString()));
            else if (nType == 1)
                Utils.WriteText(sFilename, string.Format("LB:{0}", particle.ToString()));
            else if (nType == 2)
                Utils.WriteText(sFilename, string.Format("GB:{0}", particle.ToString()));
            else if (nType == 3)
                Utils.WriteText(sFilename, string.Format("GW:{0}", particle.ToString()));
        }
    }

    /// <summary>
    /// Particle單元: 由兩個PSOTuple組成;
    /// 一個Particle單元包含當前資訊及歷史最佳資訊;
    /// </summary>
    public class ParticleUnit
    {
        public PSOTuple Curr;
        public PSOTuple ParticleBest;

        public ParticleUnit(int nParamCount)
        {
            Curr = new PSOTuple(nParamCount);
            ParticleBest = new PSOTuple(nParamCount);
        }
    }

    /// <summary>
    /// PSOTuple粒子元: 粒子的各維度, 速度, 及FitnessValue;
    /// 並提供粒子移動, 粒子複製等方法;
    /// </summary>
    public class PSOTuple
    {
        public double[] X;
        public double[] LastX;
        public double[] Velocity;
        public double Fitness;
        public double Convx;
        public double LastFitness;
        public double LastConvx;
        //public bool IsFeasible;
        public bool IsUpdate;
        public int ParticleType;
        public int ID;

        // memroy region
        /* ===================== */
        public double[] MemoryC1;
        public double[] MemoryC2;
        public double[] MemoryW;
        public int MemoryIndicator;
        public double C1;
        public double C2;
        public double W;

        public double[] SuccessC1;
        public double[] SuccessC2;
        public double[] ObjDelta;
        public int SuccessCounter;

        public int ConvCounter;
        /* ===================== */

        public double[] DimensionC1;
        public double[] DimentionC2;

        public PSOTuple(int nParamCount)
        {
            X = new double[nParamCount];
            LastX = new double[nParamCount];
            Velocity = new double[nParamCount];
            Fitness = Double.NaN;
            Convx = 0;
            LastFitness = Double.NaN;
            LastConvx = 0;
            IsUpdate = false;
            //IsFeasible = false;
            ParticleType = 1;
            ID = -1;

            // 個體根據過去經驗決定自我學習與社會學習的程度
            MemoryC1 = new double[10];
            MemoryC2 = new double[10];
            MemoryW = new double[10];
            C1 = 2.0;
            C2 = 2.0;
            W = 1.0;
            MemoryIndicator = 0;

            // 間隔五次紀錄平均的成功參數
            SuccessC1 = new double[5];
            SuccessC2 = new double[5];
            ObjDelta = new double[5];
            SuccessCounter = 0;

            ConvCounter = 0;
    }
        public void FlyBack()
        {
            for (int i = 0; i < X.Length; i++)
            {
                X[i] = LastX[i];
            }
            Fitness = LastFitness;
            Convx = LastConvx;
        }
        public void Move()
        {
            for (int i = 0; i < X.Length; i++)
            {
                LastX[i] = X[i];
                X[i] += Velocity[i];
            }
            LastFitness = Fitness;
            LastConvx = Convx;
        }
        public void CopyTo(PSOTuple tuple)
        {
            tuple.Fitness = this.Fitness;
            tuple.Convx = this.Convx;
            //tuple.IsFeasible = this.IsFeasible;
            for (int i = 0; i < X.Length; i++) tuple.X[i] = this.X[i];
        }
        public void SetConvxFitness(double[] _g, double[] _h, double fx)
        {
            // const_num = Par.gn(I_fno) + Par.hn(I_fno); %% constraint number
            double const_num = 0;
            if (_g != null) const_num += _g.Length;
            if (_h != null) const_num += _h.Length;
            int n = 0;
            if (const_num == 0)
                this.Convx = 0;
            else
            {
                // convx(i) = (sum([sum(g.*(g>0),1); sum(abs(h).*(abs(h)>1e-4),1)],1)./const_num);
                if (_g != null)
                {
                    for (int i = 0; i < _g.Length; i++)
                    {
                        if (_g[i] > 0) n++;
                    }
                }
                if (_h != null)
                {
                    for (int i = 0; i < _h.Length; i++)
                    {
                        if (Math.Abs(_h[i]) > 1e-4) n++;
                    }
                }
                this.Convx = n;
            }
            this.Fitness = fx;
        }

        public bool IsBetter(PSOTuple tuple, double epsilon=0.0)
        {

            //// 要加入 epsilon 技術的話可以新增在這邊!!!!!!!!!! 以後要 adaptive
            //if (this.Convx < tuple.Convx) return true;
            //if (this.Convx == tuple.Convx && this.Fitness < tuple.Fitness) return true;
            ////epsilon = 0.00;
            //if ((this.Convx < epsilon && tuple.Convx < epsilon) && this.Fitness < tuple.Fitness) return true;
            //return false;

            double this_conv;
            double tuple_conv;
            if (this.Convx - epsilon > 0.0)
                this_conv = this.Convx - epsilon;
            else
                this_conv = 0.0;
            if (tuple.Convx - epsilon > 0.0)
                tuple_conv = tuple.Convx - epsilon;
            else
                tuple_conv = 0.0;

            if (this_conv < tuple_conv) return true;
            if (this_conv == tuple_conv && this.Fitness < tuple.Fitness) return true;
            return false;

            //double epsilon = 0.0;
            //if (this.Convx <= epsilon && tuple.Convx <= epsilon)
            //{
            //    if (this.Fitness < tuple.Fitness) return true;
            //    else return false;
            //}
            //else if (this.Convx == tuple.Convx)
            //{
            //    if (this.Fitness < tuple.Fitness) return true;
            //    else return false;
            //}
            //else
            //{
            //    if (this.Convx < tuple.Convx) return true;
            //    return false;
            //}

        }
        public string XInfo()
        {
            string sRet = "";
            for (int i = 0; i < X.Length; i++) sRet += string.Format("X{0}={1} ", i + 1, this.X[i]);
            return sRet;
        }
        public string LastXInfo()
        {
            string sRet = "";
            for (int i = 0; i < LastX.Length; i++) sRet += string.Format("X{0}={1} ", i + 1, this.LastX[i]);
            return sRet;
        }
        public new string ToString()
        {
            string sRet = string.Format("Fitness:{0}, Constraint:{1} -> ", this.Fitness, this.Convx);
            sRet += XInfo();
            return sRet;
        }
    }

    public class ConstractResult
    {
        public double[] g;
        public double[] h;

        public ConstractResult(double[] _g, double[] _h)
        {
            g = _g;
            h = _h;
        }

        ~ConstractResult()
        {
            // delete g;
            // delete h;
        }

    }
}
