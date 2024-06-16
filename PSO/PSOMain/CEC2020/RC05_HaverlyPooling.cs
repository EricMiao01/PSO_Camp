using System;
using PSOLib;

public class RC05 : Problem
{
    public override String name()
    {
        return "RC05";
    }

    public RC05()
    {

        x_u = new double[] { 100, 200, 100, 100, 100, 100, 200, 100, 200 };
        x_l = new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
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

        int gSize = 2;
        int hSize = 4;
        double[] g = new double[gSize];
        double[] h = new double[hSize];

        //計算限制式
        h[0] = x9 * x7 + x9 * x8 - 3 * x3 - x4;
        h[1] = x1 - x5 - x7;
        h[2] = x2 - x6 - x8;
        h[3] = x9 * x7 + x9 * x8 - 3 * x3 - x4;
        g[0] = x9 * x7 + 2 * x5 - 2.5 * x1;
        g[1] = x9 * x8 + 2 * x6 - 1.5 * x2;

        return new ConstractResult(g, h);
    }

    public override double GetFitness(PSOTuple pi)
    {
        double x1 = pi.X[0];
        double x2 = pi.X[1];
        double x3 = pi.X[2];
        double x4 = pi.X[3];
        double x5 = pi.X[4];
        double x6 = pi.X[5];

        return 9 * x1 + 15 * x2 - 6 * x3 - 16 * x4 - 10 * (x5 + x6);
    }

};