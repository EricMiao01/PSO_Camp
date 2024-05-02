using System;
using PSOLib;

public class RC29_GasCompressor: Problem
{
    public override String name()
	{
		return "RC29_GasCompressor";
	}

    public RC29_GasCompressor()
    {
		// xmin29   = [20,1,20,0.1];
		// xmax29   = [50,10,50,60];
		double[] x_l = { 20, 1, 20, 0.1 };
		double[] x_u = { 50, 10, 50, 60 };
		setDims(x_u, x_l);
    }

	public override double GetFitness(PSOTuple pi)
	{
		double x1 = pi.X[0];
		double x2 = pi.X[1];
		double x3 = pi.X[2];
		double x4 = pi.X[3];

		// f = 8.61.*1e5.*x(:,1).^0.5.*x(:,2).*x(:,3).^(-2/3).*x(:,4).^(-1/2) +3.69.*1e4.*x(:,3)....
		//   + 7.72.*1e8.*x(:,1).^(-1).*x(:,2).^(0.219)-765.43.*1e6.*x(:,1).^(-1);

		return 8.61 * pow(10.0,5.0) * pow(x1,0.5) * x2 * pow(x3,(-2.0/3.0)) * pow(x4,(-1.0/2.0)) + 3.69 * pow(10.0,4.0) * x3
			   + 7.72 * pow(10.0,8.0) * pow(x1,-1.0) * pow(x2,0.219) - 765.43 * pow(10.0,6.0) * pow(x1,-1.0);
	}

	// public override bool CheckParticle(PSOTuple pi) 
    public override ConstractResult GetConstraintResult(PSOTuple pi)
	{
		double x1 = pi.X[0];
		double x2 = pi.X[1];
		double x3 = pi.X[2];
		double x4 = pi.X[3];

		// g(:,1) = x(:,4).*x(:,2).^(-2)+x(:,2).^(-2)-1;

		double g1 = x4 * pow(x2,-2.0) + pow(x2,-2.0) - 1.0;
		//cout << g1 << endl;
		//return g1 <= 1e-8;
        double[] g = new double[1];
        g[0] = g1 - 1e-8;

        return new ConstractResult(g, null);
	}

};
