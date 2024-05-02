using System;
using PSOLib;

public class RC18_PressureVessel: Problem
{
    public override String name()
	{
		return "RC18_PressureVessel";
	}

    public RC18_PressureVessel()
    {
		x_l = new double[] { 1, 1, 10, 10 };
		x_u = new double[] { 99, 99, 200, 200 };
		setDims(x_u, x_l);
    }

	//public override bool CheckParticle(PSOTuple pi)
    public override ConstractResult GetConstraintResult(PSOTuple pi)
	{
		//double x1 = pi.X[0];
		//double x2 = pi.X[1];
		double x1 = 0.0625*round(pi.X[0]);
		double x2 = 0.0625*round(pi.X[1]);
		double x3 = pi.X[2];
		double x4 = pi.X[3];

		// g(:,1) = -x(:,1)+0.0193.*x(:,3);
		// g(:,2) = -x(:,2)+0.00954.*x(:,3);
		// g(:,3) = -pi.*x(:,3).^2.*x(:,4)-4/3.*pi.*x(:,3).^3+1296000;
		// g(:,4) = x(:,4)-240;

		int gSize = 4;
        double[] g = new double[gSize];

		//計算限制式
		g[0] = -x1 + 0.0193 * x3;
		g[1] = -x2 + 0.00954 * x3;
		g[2] = -PI * pow(x3,2) * x4 - 4.0 / 3.0 * PI * pow(x3,3) + 1296000.0;
		g[3] = x4 - 240.0;

        return new ConstractResult(g, null);
		//for(int i=0; i<gSize; i++)
		//	cout << g[i] << endl;

        //for(int i=0; i<gSize; i++)
        //    if(g[i] > 0) return false;

		//符合就return 0,不合就return 1 
        //return true;
	}

	public override double GetFitness(PSOTuple pi)
	{
		double x1 = 0.0625*round(pi.X[0]);
		double x2 = 0.0625*round(pi.X[1]);
		double x3 = pi.X[2];
		double x4 = pi.X[3];
		
		// x(:,1) = 0.0625.*round(x(:,1));
		// x(:,2) = 0.0625.*round(x(:,2));
		// %% Pressure vessel design
		// f = 0.6224.*x(:,1).*x(:,3).*x(:,4)+1.7781.*x(:,2).*x(:,3).^2....
		//     +3.1661.*x(:,1).^2.*x(:,4)+19.84.*x(:,1).^2.*x(:,3);
		
		return 0.6224 * x1 * x3 * x4 + 1.7781 * x2 * x3 * x3
			   +3.1661 * x1 * x1 * x4 + 19.84 * x1 * x1 * x3;
	}

};
