using System;
using PSOLib;

public class RC09 : Problem
{
    public override String name()
    {
        return "RC09";
    }

    public RC09()
    {

        x_u = new double[] { 1.4, 1.4, 1.49 };
        x_l = new double[] { 0.5, 0.5, -0.51 };
        setDims(x_u, x_l);
    }

    public override ConstractResult GetConstraintResult(PSOTuple pi)
    {
        double x1 = pi.X[0];
        double x2 = pi.X[1];
        double x3 = round(pi.X[2]);

        int gSize = 1;
        int hSize = 1;
        double [] g = new double[gSize];
        double [] h = new double[hSize];
        //計算限制式
        g[0] = x2 - x1 + x3;
        // if ((-2 * Math.Exp(-x2) + x1) != 0) return false;
        h[0] = x1 - (2 * Math.Exp(-x2));

        return new ConstractResult(g, h);
    }

    public override double GetFitness(PSOTuple pi)
    {
        double x1 = pi.X[0];
        double x2 = pi.X[1];
        double x3 = round(pi.X[2]);

        return -x3 + x2 + (2 * x1);
    }

};