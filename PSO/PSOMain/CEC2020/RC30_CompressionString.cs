using System;
using PSOLib;

public class RC30_CompressionString: Problem
{
    public override String name()
	{
		return "RC30_CompressionString";
	}

    public RC30_CompressionString()
    {
		// xmin30 = [0.51,0.6,0.51];
		// xmax30 = [70.49,3,42.49];
		double[] x_l = { 0.51, 0.6, 0.51 };
		double[] x_u = { 70.49, 3, 42.49 };
		setDims(x_u, x_l);
    }

	public override double GetFitness(PSOTuple pi)
	{
		double x1 = pi.X[0];
		double x2 = pi.X[1];
		double x3 = pi.X[2];

		x1 = round(x1);
		double[] d = new double[] {0.009, 0.0095, 0.0104, 0.0118, 0.0128, 0.0132, 0.014,
					  0.015, 0.0162, 0.0173, 0.018, 0.020, 0.023, 0.025,
					  0.028, 0.032, 0.035, 0.041, 0.047, 0.054, 0.063,
					  0.072, 0.080, 0.092, 0.0105, 0.120, 0.135, 0.148,
					  0.162, 0.177, 0.192, 0.207, 0.225, 0.244, 0.263,
					  0.283, 0.307, 0.331, 0.362,0.394,0.4375,0.500};
		int n3 = round(x3) - 1;
		x3 = d[n3];

		//%% objective function
		//f = (pi.^2.*x2.*x3.^2.*(x1+2))./4;

		//cout << x1 << "," << x2 << "," << n3 << "," << x3 <<endl;
		return (pow(PI,2) * x2 * pow(x3,2) * (x1 + 2)) / 4.0;
	}

	// public override bool CheckParticle(PSOTuple pi) 
    public override ConstractResult GetConstraintResult(PSOTuple pi)
	{
		double x1 = pi.X[0];
		double x2 = pi.X[1];
		double x3 = pi.X[2];
		x1 = round(x1);
		double[] d = new double[] {0.009, 0.0095, 0.0104, 0.0118, 0.0128, 0.0132, 0.014,
					  0.015, 0.0162, 0.0173, 0.018, 0.020, 0.023, 0.025,
					  0.028, 0.032, 0.035, 0.041, 0.047, 0.054, 0.063,
					  0.072, 0.080, 0.092, 0.0105, 0.120, 0.135, 0.148,
					  0.162, 0.177, 0.192, 0.207, 0.225, 0.244, 0.263,
					  0.283, 0.307, 0.331, 0.362,0.394,0.4375,0.500};
		int n3 = round(x3) - 1;
		x3 = d[n3];

		// %% constants
		// cf = (4.*x2./x3-1)./(4.*x2./x3-4)+0.615.*x3./x2;
		// K  = (11.5.*10.^6.*x3.^4)./(8.*x1.*x2.^3);
		// lf = 1000./K + 1.05.*(x1+2).*x3;
		// sigp = 300./K;
		// g(:,1) = (8000.*cf.*x2)./(pi.*x3.^3)-189000;
		// g(:,2) = lf-14;
		// g(:,3) = 0.2-x3;
		// g(:,4) = x2-3;
		// g(:,5) = 3-x2./x3;
		// g(:,6) = sigp - 6;
		// g(:,7) = sigp+700./K+1.05.*(x1+2).*x3-lf;
		// g(:,8) = 1.25-700./K;

		double cf = (4.0 * x2 / x3 - 1.0) / (4.0 * x2 / x3 - 4.0) + 0.615 * x3 / x2;
		double K  = (11.5 * pow(10,6) * pow(x3,4)) / (8.0 * x1 * pow(x2,3));
		double lf = 1000.0 / K + 1.05 * (x1 + 2.0) * x3;
		double sigp = 300.0 / K;

		int gSize = 8;
        double[] g = new double[gSize];

		g[0] = (8000.0 * cf * x2) / (PI * pow(x3,3)) - 189000.0;
		g[1] = lf - 14.0;
		g[2] = 0.2 - x3;
		g[3] = x2 - 3.0;
		g[4] = 3.0 - x2 / x3;
		g[5] = sigp - 6.0;
		g[6] = sigp + 700.0 / K + 1.05 * (x1 + 2.0) * x3 - lf;
		g[7] = 1.25 - 700.0 / K;

		//for(int i=0; i<gSize; i++)
		//	cout << g[i] << endl;

        return new ConstractResult(g, null);
        //for(int i=0; i<gSize; i++)
        //    if(g[i] > 0) return false;		

        //return true;
	}

};
