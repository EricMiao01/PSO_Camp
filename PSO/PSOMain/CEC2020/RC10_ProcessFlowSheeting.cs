using System;
using PSOLib;

public class RC10 : Problem
{
    public override String name()
    {
        return "RC10";
    }

    public RC10()
    {

        x_u = new double[] { 1, -1, 1.49 };
        x_l = new double[] { 0.2, -2.22554, -0.49 };
        setDims(x_u, x_l);
    }

    public override ConstractResult GetConstraintResult(PSOTuple pi)
    {
        double x1 = pi.X[0];
        double x2 = pi.X[1];
        double x3 = round(pi.X[2]);

        int gSize = 3;
        double[] g = new double[gSize];

        //計算限制式
        g[0] = -Math.Exp(x1 - 0.2) - x2;
        g[1] = x2 + (1.1 * x3) + 1;
        g[2] = (x1 - x3) - 0.2;

        return new ConstractResult(g, null);
    }

    public override double GetFitness(PSOTuple pi)
    {
        double x1 = pi.X[0];
        double x2 = pi.X[1];
        double x3 = round(pi.X[2]);

        return (-0.7 * x3) + 0.8 + (5 * Math.Pow((x1-0.5), 2));
    }

};