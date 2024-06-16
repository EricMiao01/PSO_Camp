using System;
using PSOLib;

public class RC03 : Problem
{
    public override String name()
    {
        return "RC03";
    }

    public RC03()
    {

        x_u = new double[] { 2000, 100, 4000, 100, 100, 20, 200 };
        x_l = new double[] { 1000, 0, 2000, 0, 0, 0, 0 };
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

        int gSize = 14;
        double[] g = new double[gSize];

        //計算限制式
        g[0] = 0.0059553571 * Math.Pow(x6, 2) * x1 + 0.88392857 * x3 - 0.1175625 * x6 * x1 - x1;
        g[1] = 1.1088 * x1 + 0.1303533 * x1 * x6 - 0.0066033 * x1 * Math.Pow(x6, 2) - x3;
        g[2] = 6.66173269 * Math.Pow(x6, 2) - 56.596669 * x4 + 172.39878 * x5 - 10000 - 191.20592 * x6;
        g[3] = 1.08702 * x6 - 0.03762 * Math.Pow(x6, 2) + 0.32175 * x4 + 56.85075 - x5;
        g[4] = 0.006198 * x7 * x4 * x3 + 2462.3121 * x2 - 25.125634 * x2 * x4 - x3 * x4;
        g[5] = 161.18996 * x3 * x4 + 5000.0 * x2 * x4 - 489510.0 * x2 - x3 * x4 * x7;
        g[6] = 0.33 * x7 + 44.333333 - x5;
        g[7] = 0.022556 * x5 - 1.0 - 0.007595 * x7;
        g[8] = 0.00061 * x3 - 1.0 - 0.0005 * x1;
        g[9] = 0.819672 * x1 - x3 + 0.819672;
        g[10] = 24500.0 * x2 - 250.0 * x2 * x4 - x3 * x4;
        g[11] = 1020.4082 * x4 * x2 + 1.2244898 * x3 * x4 - 100000 * x2;
        g[12] = 6.25 * x1 * x6 + 6.25 * x1 - 7.625 * x3 - 100000;
        g[13] = 1.22 * x3 - x6 * x1 - x1 + 1;

        return new ConstractResult(g, null);
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


        return 0.035 * x1 * x6 + 1.715 * x1 + 10.0 * x2 + 4.0565 * x3 - 0.063 * x3 * x5;

    }

};