using System;
using PSOLib;

public class RC14 : Problem
{
    public override String name()
    {
        return "RC14";
    }

    public RC14()
    {

        x_u = new double[] { 3.49, 3.49, 3.49, 2500, 2500, 2500, 20, 16, 700, 450 };
        x_l = new double[] { 0.51, 0.51, 0.51, 250, 250, 250, 6, 4, 40, 10 };
        setDims(x_u, x_l);
    }

    public override ConstractResult GetConstraintResult(PSOTuple pi)
    {
        double x1 = round(pi.X[0]); // N1
        double x2 = round(pi.X[1]); // N2
        double x3 = round(pi.X[2]); // N3
        double x4 = pi.X[3]; // V1
        double x5 = pi.X[4]; // V2
        double x6 = pi.X[5]; // V3
        double x7 = pi.X[6]; // TL1
        double x8 = pi.X[7]; // TL2
        double x9 = pi.X[8]; // B1
        double x10 = pi.X[9]; // B2
        int[,] S = { { 2, 3, 4 }, { 4, 6, 3 } };
        int[,] t = { { 8, 20, 8 }, { 16, 4, 4 } };
        int H = 6000;
        int Q1 = 40000;
        int Q2 = 20000;

        int gSize = 10;
        double[] g = new double[gSize];

        g[0] = Q1 * x7 / x9 + Q2 * x8 / x10 - H;
        g[1] = S[0, 0] * x9 + S[1, 0] * x10 - x4;
        g[2] = S[0, 1] * x9 + S[1, 1] * x10 - x5;
        g[3] = S[0, 2] * x9 + S[1, 2] * x10 - x6;
        g[4] = t[0, 0] - x1 * x7;
        g[5] = t[0, 1] - x1 * x7;
        g[6] = t[0, 2] - x3 * x7;
        g[7] = t[1, 0] - x1 * x8;
        g[8] = t[1, 1] - x2 * x8;
        g[9] = t[1, 2] - x3 * x8;

        return new ConstractResult(g, null);
    }

    public override double GetFitness(PSOTuple pi)
    {
        double x1 = Math.Round(pi.X[0]); // N1
        double x2 = Math.Round(pi.X[1]); // N2
        double x3 = Math.Round(pi.X[2]); // N3
        double x4 = pi.X[3]; // V1
        double x5 = pi.X[4]; // V2
        double x6 = pi.X[5]; // V3
        double x7 = pi.X[6]; // TL1
        double x8 = pi.X[7]; // TL2
        double x9 = pi.X[8]; // B1
        double x10 = pi.X[9]; // B2
        int alp = 250;
        double beta = 0.6;

        return alp * (x1 * Math.Pow(x4, beta) + x2 * Math.Pow(x5, beta) + x3 * Math.Pow(x6, beta));
    }

};