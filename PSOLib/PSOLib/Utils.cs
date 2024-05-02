using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;

namespace PSOLib
{
    public static class Utils
    {
        public static double GetRandomNormal()
        {
            double mean = 0.5;
            double stdDev = 0.1;

            Random rand = new Random(Guid.NewGuid().GetHashCode()); //reuse this if you are generating many
            double u1 = 1.0 - rand.NextDouble(); //uniform(0,1] random doubles
            double u2 = 1.0 - rand.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
            double randNormal = mean + stdDev * randStdNormal; //random normal(mean,stdDev^2)

            return randNormal;
        }

        public static void Assert(Boolean bTest, String sException)
        {
            if (!bTest) throw new Exception("Assert fail: " + sException);
        }

        public static string ReadText(string sFile)
        {
            StringBuilder slInfo = new StringBuilder();
            if (!File.Exists(sFile)) return "";

            using (StreamReader MySF = new StreamReader(sFile))
            {
                while (!MySF.EndOfStream)
                {
                    slInfo.AppendLine(MySF.ReadLine());
                }
                MySF.Close();
            }

            return slInfo.ToString();
        }

        public static void WriteText(string sFile, string msg)
        {
            FileStream fs = new FileStream(sFile, FileMode.Append);
            StreamWriter sr = new StreamWriter(fs);

            try
            {
                sr.WriteLine(msg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                sr.Close();
                fs.Close();
            }
        }

        public static double Distance(PSOTuple ParticleA, PSOTuple ParticleB)
        {
            double ret = 0;
            for (int nLoop = 0; nLoop < ParticleA.X.Length; nLoop++)
                ret += Math.Pow((ParticleA.X[nLoop] - ParticleB.X[nLoop]), 2);
            return Math.Sqrt(ret);
        }

        public static string Bool2Str(bool b, string sT = "Y", string sF = "N")
        {
            if (b)
                return sT;
            else
                return sF;
        }

        public static int[] DecimalToAny(int nDecimal, int nCarry, int nSize)
        {
            int[] nRet = new int[nSize];
            for (int i = 0; i < nSize; i++) nRet[i] = -1;
            if (nDecimal >= nCarry * nCarry) return nRet;

            for (int i = 0; i < nSize; i++) nRet[i] = 0;
            try
            {
                int nIndex = 0;
                int nLeave = nDecimal;
                while (nLeave >= nCarry)
                {
                    nRet[nIndex] = nLeave % nCarry;
                    nLeave = nLeave / nCarry;
                    nIndex++;
                }
                nRet[nIndex] = nLeave;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return nRet;
        }

        public static double Sigmoid(double x, double a = 0.1, double b = 50.0)
        {
            return 1/(1 + Math.Exp(-a*(x-b)));
        }

        public static double GetRandomNormal(double mean = 0.5, double stdDev = 0.1)
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            double u1 = 1.0 - rand.NextDouble();
            double u2 = 1.0 - rand.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            double randNormal = mean + stdDev * randStdNormal;
            return randNormal;
        }

        public static double GetRandomCauchy(double mean = 0.5, double stdDev = 0.1)
        {
            double N_1 = GetRandomNormal(mean, stdDev);
            double N_2 = GetRandomNormal(mean, stdDev);
            double randCauchy = N_1 / N_2;
            return randCauchy;
        }

        public static double WeightedLehmerMean(double[] weights, double[] values)
        {
            double weightedSum = 0;
            double weightedSquareSum = 0;
            for (int i = 0; i < weights.Length; i++)
            {
                weightedSum += weights[i] * values[i];
                weightedSquareSum += weights[i] * values[i] * values[i];
            }
            double mean = weightedSquareSum / weightedSum;
            //double mean = weightedSum / weights.Sum();
            return mean;
        }

        public static double WeightedMean(double[] weights, double[] values)
        {
            double weightedSum = 0;
            double weightedSquareSum = 0;
            for (int i = 0; i < weights.Length; i++)
            {
                weightedSum += weights[i] * values[i];
                weightedSquareSum += weights[i] * values[i] * values[i];
            }
            //double mean = weightedSquareSum / weightedSum;
            double mean = weightedSum;
            return mean;
        }
    }
}
