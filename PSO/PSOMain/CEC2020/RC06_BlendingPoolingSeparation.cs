using System;
using System.Security.Cryptography;
using PSOLib;

public class RC06 : Problem
{

    public override String name()
    {
        return "RC06";
    }

    public RC06()
    {
        // VAR /1..64/ W;
        double[] x_l = { 
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0
        };
        double[] x_u = { 
            90, 150,  90, 150,  90,  90, 150,  90,  90,  90,
           150, 150,  90,  90, 150,  90, 150,  90, 150,  90,
             1, 1.2,   1,   1,   1, 0.5,   1,   1, 0.5, 0.5,
           0.5, 1.2, 0.5, 1.2, 1.2, 0.5, 1.2, 1.2 
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
        
        int hSize = 32;
        double[] h = new double[hSize];


        h[0] = x[0] + x[1] + x[2] + x[3] - 300;
        h[1] = x[5] - x[6] - x[7];
        h[2] = x[8] - x[9] - x[10] - x[11];
        h[3] = x[13] - x[14] - x[15] - x[16];
        h[4] = x[17] - x[18] - x[19];

        h[5] = x[4] * x[20] - x[5] * x[21] - x[8] * x[22];
        h[6] = x[4] * x[23] - x[5] * x[24] - x[8] * x[25];
        h[7] = x[4] * x[26] - x[5] * x[27] - x[8] * x[27];

        h[8] = x[12] * x[29] - x[13] * x[30] - x[17] * x[31];
        h[9] = x[12] * x[32] - x[13] * x[33] - x[17] * x[34];
        h[10] = x[12] * x[35] - x[13] * x[36] - x[17] * x[37];

        h[11] = (1.0 / 3.0) * x[0] + x[14] * x[30] - x[4] * x[20];
        h[12] = (1.0 / 3.0) * x[0] + x[14] * x[33] - x[4] * x[23];
        h[13] = (1.0 / 3.0) * x[0] + x[14] * x[36] - x[4] * x[26];

        h[14] = (1.0 / 3.0) * x[1] + x[9] * x[22] - x[12] * x[29];
        h[15] = (1.0 / 3.0) * x[1] + x[9] * x[25] - x[12] * x[32];
        h[16] = (1.0 / 3.0) * x[1] + x[9] * x[28] - x[12] * x[35];

        h[17] = (1.0 / 3.0) * x[2] + x[6] * x[21] + x[10] * x[22] + x[15] * x[30] + x[18] * x[31] - 30;
        h[18] = (1.0 / 3.0) * x[2] + x[6] * x[24] + x[10] * x[25] + x[15] * x[33] + x[18] * x[34] - 50;
        h[19] = (1.0 / 3.0) * x[2] + x[6] * x[27] + x[10] * x[28] + x[15] * x[36] + x[18] * x[37] - 30;

        h[20] = x[20] + x[23] + x[26] - 1;
        h[21] = x[21] + x[24] + x[27] - 1;
        h[22] = x[22] + x[25] + x[28] - 1;
        h[23] = x[29] + x[32] + x[35] - 1;
        h[24] = x[30] + x[33] + x[36] - 1;
        h[25] = x[31] + x[34] + x[37] - 1;

        h[26] = x[24];
        h[27] = x[27];
        h[28] = x[22];
        h[29] = x[28];
        h[30] = x[32];
        h[31] = x[35];

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