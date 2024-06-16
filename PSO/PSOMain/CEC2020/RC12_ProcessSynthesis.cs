using System;
using PSOLib;

public class RC12 : Problem
{
    public override String name()
    {
        return "RC12";
    }

    public RC12()
    {

        x_u = new double[] { 100, 100, 100, 1.49, 1.49, 1.49, 1.49 };
        x_l = new double[] { 0, 0, 0, -0.51, -0.51, -0.51, -0.51 };
        setDims(x_u, x_l);
    }

    public override ConstractResult GetConstraintResult(PSOTuple pi)
    {
        double x1 = pi.X[0];
        double x2 = pi.X[1];
        double x3 = pi.X[2];
        double x4 = Math.Round(pi.X[3]);
        double x5 = Math.Round(pi.X[4]);
        double x6 = Math.Round(pi.X[5]);
        double x7 = Math.Round(pi.X[6]);

        int gSize = 9;
        double[] g = new double[gSize];

        //計算限制式
        g[0] = x1 + x2 + x3 + x4 + x5 + x6 - 5;
        g[1] = Math.Pow(x6, 3) + Math.Pow(x1, 2) + Math.Pow(x2, 2) + Math.Pow(x3, 2) - 5.5;
        g[2] = x1 + x4 - 1.2;
        g[3] = x2 + x5 - 1.8;
        g[4] = x3 + x6 - 2.5;
        g[5] = x1 + x7 - 1.2;
        g[6] = Math.Pow(x5, 2) + Math.Pow(x2, 2) - 1.64;
        g[7] = Math.Pow(x6, 2) + Math.Pow(x3, 2) - 4.25;
        g[8] = Math.Pow(x5, 2) + Math.Pow(x3, 2) - 4.64;

        return new ConstractResult(g, null);
    }

    public override double GetFitness(PSOTuple pi)
    {
        double x1 = pi.X[0];
        double x2 = pi.X[1];
        double x3 = pi.X[2];
        double x4 = round(pi.X[3]); //y1
        double x5 = round(pi.X[4]); //y2
        double x6 = round(pi.X[5]); //y3
        double x7 = round(pi.X[6]); //y4
                                   // return Math.Pow((x4 - 1), 2) + Math.Pow((x5 - 1), 2) + Math.Pow((x6 - 1), 2) - Math.Log(x7 + 1) + Math.Pow((x1 - 1), 22) + Math.Pow((x2 - 2), 2) + Math.Pow((x3 - 3), 2);
        return Math.Pow((x4 - 1), 2) + Math.Pow((x5 - 1), 2) + Math.Pow((x6 - 1), 2) -
               Math.Log((x7 + 1)) + Math.Pow((x1 - 1), 22) + Math.Pow((x2 - 2), 2) + Math.Pow((x3 - 3), 2);
    }

};