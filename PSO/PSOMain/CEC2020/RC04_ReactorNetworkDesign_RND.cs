using System;
using PSOLib;

public class RC04 : Problem
{
    public override String name()
    {
        return "RC04";
    }

    public RC04()
    {

        x_u = new double[] { 1, 1, 1, 1, 16, 16 };
        x_l = new double[] { 0, 0, 0, 0, 0.00001, 0.00001 };
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
        double k1 = 0.09755988;
        double k2 = 0.99 * k1;
        double k3 = 0.0391908;
        double k4 = 0.9 * k3;

        int gSize = 1;
        int hSize = 4;
        double[] g = new double[gSize];
        double[] h = new double[hSize];

        //計算限制式
        h[0] = k1 * x5 * x2 + x1 - 1;
        h[1] = k3 * x5 * x3 + x3 + x1 - 1;
        h[2] = k2 * x6 * x2 - x1 + x2;
        h[3] = k4 * x6 * x4 + x2 - x1 + x4 - x3;
        g[0] = Math.Pow(x5, 0.5) + Math.Pow(x6, 0.5) - 4;

        return new ConstractResult(g, h);
    }

    public override double GetFitness(PSOTuple pi)
    {
        double x4 = pi.X[3];

        return x4;

    }

};