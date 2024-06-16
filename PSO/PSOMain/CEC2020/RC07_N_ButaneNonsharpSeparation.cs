using System;
using System.Security.Cryptography;
using PSOLib;

public class RC07 : Problem
{

    public override String name()
    {
        return "RC07";
    }

    public RC07()
    {
        // VAR /1..64/ W;
        double[] x_l = {
             0, 0, 0,    0, 0,    0, 0,    0, 0, 0,
             0, 0, 0,    0, 0,    0, 0,    0, 0, 0,
             0, 0, 0, 0.85, 0, 0.85, 0, 0.85, 0, 0,
          0.85, 0, 0,    0, 0,    0, 0,    0, 0, 0,
             0, 0, 0,    0, 0,    0, 0,    0
        };
        double[] x_u = {
            35, 90, 90, 140, 90, 35, 35, 35, 35, 35,
            35, 35, 90,  90, 90, 35, 35, 35, 35, 35,
             1,  1,  1,   1, 30,  1, 30,  1, 30,  1,
             1, 30,  1,   1, 30,  1, 30,  1,  1,  1,
             1,  1,  1,   1,  1,  1,  1,  1
        };
        
        setDims(x_u, x_l);
    }

    public override ConstractResult GetConstraintResult(PSOTuple pi)
    {
        //RC51-54 Declaration
        double[] x = new double[pi.X.Length];
        
        for (int iX = 0; iX < pi.X.Length; iX++)
        {
            x[iX] = pi.X[iX];
        }
        
        int hSize = 38;
        double[] h = new double[hSize];


        h[0] = x[0] + x[1] + x[2] + x[3] - 300;
        h[1] = x[5] - x[6] - x[7];
        h[2] = x[8] - x[9] - x[10] - x[11];
        h[3] = x[13] - x[14] - x[15] - x[16];
        h[4] = x[17] - x[18] - x[19];
        h[5] = x[5] * x[20] - x[23] * x[24];
        h[6] = x[13] * x[21] - x[25] * x[26];
        h[7] = x[8] * x[22] - x[27] * x[28];
        h[8] = x[17] * x[29] - x[30] * x[31];

        h[9] = x[24] - x[4] * x[32];
        h[10] = x[28] - x[4] * x[33];
        h[11] = x[34] - x[4] * x[35];

        h[12] = x[36] - x[12] * x[37];
        h[13] = x[26] - x[12] * x[38];
        h[14] = x[31] - x[12] * x[39];

        h[15] = x[24] - x[5] * x[20] - x[8] * x[40];
        h[16] = x[28] - x[5] * x[41] - x[8] * x[22];
        h[17] = x[34] - x[5] * x[43] - x[8] * x[43];

        h[18] = x[36] - x[13] * x[44] - x[17] * x[45];
        h[19] = x[26] - x[13] * x[21] - x[17] * x[46];
        h[20] = x[31] - x[13] * x[47] - x[17] * x[29];

        h[21] = (1.0 / 3.0) * x[0] + x[14] * x[44] - x[24];
        h[22] = (1.0 / 3.0) * x[0] + x[14] * x[21] - x[28];
        h[23] = (1.0 / 3.0) * x[0] + x[14] * x[47] - x[34];
        h[24] = (1.0 / 3.0) * x[1] + x[9] * x[40] - x[36];
        h[25] = (1.0 / 3.0) * x[1] + x[9] * x[22] - x[26];
        h[26] = (1.0 / 3.0) * x[1] + x[9] * x[43] - x[31];

        h[27] = x[32] + x[33] + x[35] - 1;
        h[28] = x[20] + x[41] + x[42] - 1;
        h[29] = x[40] + x[22] + x[43] - 1;
        h[30] = x[37] + x[38] + x[39] - 1;
        h[31] = x[44] + x[21] + x[47] - 1;
        h[32] = x[45] + x[46] + x[29] - 1;

        h[33] = x[42];
        h[34] = x[45];

        h[35] = (1.0 / 3.0) * x[2] + x[6] * x[20] + x[10] * x[40] + x[15] * x[44] + x[18] * x[45] - 30;
        h[36] = (1.0 / 3.0) * x[2] + x[6] * x[41] + x[10] * x[22] + x[15] * x[21] + x[18] * x[46] - 50;
        h[37] = (1.0 / 3.0) * x[2] + x[6] * x[42] + x[10] * x[23] + x[15] * x[22] + x[18] * x[47] - 30;

        return new ConstractResult(null, h);
    }

    public override double GetFitness(PSOTuple pi)
    {
        //RC51-54 Declaration
        double[] x = new double[pi.X.Length];

        for (int iX = 0; iX < pi.X.Length; iX++)
        {
            x[iX] = pi.X[iX];
        }

        double fitness = 0.9979 + 0.00432 * x[4] + 0.01517 * x[12];

        return fitness;
    }

};