using System;
using PSOLib;

public class RC02 : Problem
{
    public override String name()
    {
        return "RC02";
    }

    public RC02()
    {

        x_u = new double[] { 819000, 1131000, 2050000, 0.05074, 0.05074, 0.05074, 200, 300, 300, 300, 400 };
        x_l = new double[] { 10000, 10000, 10000, 0, 0, 0, 100, 100, 100, 100, 100 };
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
        double x10 = pi.X[9];
        double x11 = pi.X[10];

        int hSize = 9;
        double[] h = new double[hSize];

        //計算限制式
        h[0] = x1 - 10000 * (x7 - 100);
        h[1] = x2 - 10000 * (x8 - x7);
        h[2] = x3 - 10000 * (500 - x8);
        h[3] = x1 - 10000 * (300 - x8);
        h[4] = x2 - 10000 * (400 - x8);
        h[5] = x3 - 10000 * (600 - x8);
        h[6] = x4 * Math.Log(x9 - 100) - x4 * Math.Log(300 - x7) - x9 - x7 + 400;
        h[7] = x5 * Math.Log(x10 - x7) - x5 * Math.Log(400 - x8) - x10 + x7 - x8 + 400;
        h[8] = x6 * Math.Log(x11 - x8) - x6 * Math.Log(100) - x11 + x8 + 100;

        return new ConstractResult(null, h);
    }

    public override double GetFitness(PSOTuple pi)
    {
        double x1 = pi.X[0];
        double x2 = pi.X[1];
        double x3 = pi.X[2];
        double x4 = pi.X[3];
        double x5 = pi.X[4];
        double x6 = pi.X[5];


        return Math.Pow((x1 / (120 * x4)), 0.6) + Math.Pow((x2 / (80 * x5)), 0.6) + Math.Pow((x3 / (40 * x6)), 0.6);

    }

};