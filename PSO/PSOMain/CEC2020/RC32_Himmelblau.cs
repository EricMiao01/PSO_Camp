using System;
using PSOLib;

public class RC32_Himmelblau: Problem
{
    public override String name()
	{
		return "RC32_Himmelblau";
	}

    public RC32_Himmelblau()
    {
		double[] x_l = { 78, 33, 27, 27, 27 };
		double[] x_u = { 102, 45, 45, 45, 45 };
		setDims(x_u, x_l);
    }

	public override double GetFitness(PSOTuple pi)
	{
		double x1 = pi.X[0];
		double x2 = pi.X[1];
		double x3 = pi.X[2];
		double x4 = pi.X[3];
		double x5 = pi.X[4];
		
		//f = 5.3578547.*x3.^2 + 0.8356891.*x1.*x5 + 37.293239.*x1 - 40792.141;
		
		return 5.3578547 * pow(x3,2) + 0.8356891 * x1 * x5 + 37.293239 * x1 - 40792.141;
	}

	// public override bool CheckParticle(PSOTuple pi) 
    public override ConstractResult GetConstraintResult(PSOTuple pi)
	{
		double x1 = pi.X[0];
		double x2 = pi.X[1];
		double x3 = pi.X[2];
		double x4 = pi.X[3];
		double x5 = pi.X[4];
				
		double G1 = 85.334407 + 0.0056858 * x2 * x5 + 0.0006262 * x1 * x4 - 0.0022053 * x3 * x5;
		double G2 = 80.51249 + 0.0071317 * x2 * x5 + 0.0029955 * x1 * x2 + 0.0021813 * pow(x3,2);
		double G3 = 9.300961 + 0.0047026 * x3 * x5 + 0.0012547 * x1 * x3 + 0.0019085 * x3 * x4;
		
		int gSize = 6;
        double[] g = new double[gSize];

		g[0] = G1 - 92;
		g[1] = -G1;
		g[2] = G2 - 110;
		g[3] = -G2 + 90;
		g[4] = G3 - 25;
		g[5] = -G3 + 20;

        return new ConstractResult(g, null);

		//for(int i=0; i<gSize; i++)
        ////	cout << g[i] << endl;

        //for(int i=0; i<gSize; i++)
        //    if(g[i] > 0) return false;		

        //return true;
	}

};
