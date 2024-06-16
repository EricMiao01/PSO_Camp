using System;
using PSOLib;

public class RC11 : Problem
{
    public override String name()
    {
        return "RC11";
    }

    public RC11()
    {

        x_u = new double[] { 20, 20, 10, 10, 1.49, 1.49, 40 };
        x_l = new double[] { 0, 0, 0, 0, -0.51, -0.51, 0 };
        setDims(x_u, x_l);
    }

    public override ConstractResult GetConstraintResult(PSOTuple pi)
    {
        double x1 = pi.X[0];
        double x2 = pi.X[1];
        double x3 = pi.X[2];
        double x4 = pi.X[3];
        double x5 = round(pi.X[4]);
        double x6 = round(pi.X[5]);
        double x7 = pi.X[6];

        int gSize = 4;
        int hSize = 4;
        double[] g = new double[gSize];
        double[] h = new double[hSize];

        double z1 = 0.9 * (1 - exp(-0.5 * x3)) * x1;
        double z2 = 0.8 * (1 - exp(-0.4 * x4)) * x2;

        //計算限制式
        h[0] = x5 + x6 - 1;
        h[1] = z1 + z2 - 10;
        h[2] = x1 + x2 - x7;
        h[3] = z1*x5 + z2*x6 - 10;
        g[0] = x3 - (10 * x5);
        g[1] = x4 - (10 * x6);
        g[2] = x1 - (20 * x5);
        g[3] = x2 - (20 * x6);

        return new ConstractResult(g, h);
    }

    public override double GetFitness(PSOTuple pi)
    {
        double x1 = pi.X[0];
        double x2 = pi.X[1];
        double x3 = pi.X[2];
        double x4 = pi.X[3];
        double x5 = round(pi.X[4]);
        double x6 = round(pi.X[5]);
        double x7 = pi.X[6];

        return (7.5 * x5) + (5.5 * x6) + (7 * x3) + (6 * x4) + (5 * x7);
    }

};