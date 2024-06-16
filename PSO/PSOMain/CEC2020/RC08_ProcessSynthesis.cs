using System;
using PSOLib;

public class RC08 : Problem
{
    public override String name()
    {
        return "RC08";
    }

    public RC08()
    {

        x_u = new double[] { 1.6, 1.49 };
        x_l = new double[] { 0, -0.49 };
        setDims(x_u, x_l);
    }

    public override ConstractResult GetConstraintResult(PSOTuple pi)
    {
        double x1 = pi.X[0];
        double x2 = round(pi.X[1]);

        int gSize = 2;
        double[] g = new double[gSize];

        //計算限制式
        g[0] = -Math.Pow(x1, 2) - x2 + 1.25;
        g[1] = x1 + x2 - 1.6;

        return new ConstractResult(g, null);
    }

    public override double GetFitness(PSOTuple pi)
    {
        double x1 = pi.X[0];
        double x2 = round(pi.X[1]);

        return x2 + (2 * x1);
    }

};