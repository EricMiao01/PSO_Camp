using System;
using PSOLib;

public class RC01 : Problem
{
    public override String name()
    {
        return "RC01";
    }

    public RC01()
    {

        x_u = new double[] { 10, 200, 100, 200, 2000000, 600, 600, 600, 900 };
        x_l = new double[] {  0,   0,   0,   0,    1000,   0, 100, 100, 100 };
        setDims(x_u, x_l);
    }

    public override ConstractResult GetConstraintResult(PSOTuple pi)
    {
        double x1 = pi.X[0];
        double x2 = pi.X[1];
        double x3 = pi.X[2];
        double x4 = pi.X[3];
        double x5 = pi.X[4];
        double x6 = pi.X[5];
        double x7 = pi.X[6];
        double x8 = pi.X[7];
        double x9 = pi.X[8];

        int hSize = 8;
        double[] h = new double[hSize];

        //計算限制式
        h[0] = 200 * x1 * x4 - x3;
        h[1] = 200 * x2 * x6 - x5;
        h[2] = x3 - 10000 * (x7 - 100);
        h[3] = x5 - 10000 * (300 - x7);
        h[4] = x3 - 10000 * (600 - x8);
        h[5] = x5 - 10000 * (900 - x9);
        h[6] = x4 * Math.Log(x8 - 100) - x4 * Math.Log(600 - x7) - x8 + x7 + 500;
        h[7] = x6 * Math.Log(x9 - x7) - x6 * Math.Log(600) - x9 + x7 + 600;

        return new ConstractResult(null, h);
    }

    public override double GetFitness(PSOTuple pi)
    {
        double x1 = pi.X[0];
        double x2 = pi.X[1];
        double x3 = pi.X[2];
        double x4 = pi.X[3];
        double x5 = pi.X[4];
        double x6 = pi.X[5];
        double x7 = pi.X[6];
        double x8 = pi.X[7];
        double x9 = pi.X[8];

        return 35 * Math.Pow(x1, 0.6) + 35 * Math.Pow(x2, 0.6);

    }

};