using System;
using PSOLib;

public class RC13 : Problem
{
    public override String name()
    {
        return "RC13";
    }

    public RC13()
    {

        x_u = new double[] { 45, 45, 45, 102.49, 45.49 };
        x_l = new double[] { 27, 27, 27, 77.51, 32.51 };
        setDims(x_u, x_l);
    }

    public override ConstractResult GetConstraintResult(PSOTuple pi)
    {
        double x1 = pi.X[0];
        double x2 = pi.X[1];
        double x3 = pi.X[2];
        double x4 = Math.Round(pi.X[3]);
        double x5 = Math.Round(pi.X[4]);

        int gSize = 3;
        double[] g = new double[gSize];

        //計算限制式
        double[] a = { 85.334407, 0.0056858, 0.0006262, 0.0022053, 80.51249, 0.0071317, 0.0029955, 0.0021813, 9.300961, 0.0047026, 0.0012547, 0.0019085 };
        g[0] = a[0] + (a[1] * x4 * x3) + (a[2] * x4 * x2) - (a[3] * x4 * x3) - 92;
        g[1] = a[4] + (a[5] * x5 * x3) + (a[6] * x4 * x2) + (a[7] * Math.Pow(x1, 2)) - 110;
        g[1] = a[8] + (a[9] * x4 * x2) + (a[10] * x4 * x1) + (a[11] * x1 * x2) - 25;

        return new ConstractResult(g, null);
    }

    public override double GetFitness(PSOTuple pi)
    {
        double x1 = pi.X[0];
        double x2 = pi.X[1];
        double x3 = pi.X[2];
        double x4 = round(pi.X[3]);
        double x5 = round(pi.X[4]);

        return (-5.357854 * Math.Pow((x1), 2)) + 40792.141 - (37.29329 * x4) - (0.835689 * x4 * x3);
    }

};