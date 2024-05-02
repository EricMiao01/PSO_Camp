using System;
using PSOLib;

public class RC31_GearTrain: Problem
{
    public override String name()
	{
		return "RC31_GearTrain";
	}

    public RC31_GearTrain()
    {
		double[] x_l = { 12, 12, 12, 12 };
		double[] x_u = { 60, 60, 60, 60 };
		setDims(x_u, x_l);
    }

	public override double GetFitness(PSOTuple pi)
	{
		double x1 = pi.X[0];
		double x2 = pi.X[1];
		double x3 = pi.X[2];
		double x4 = pi.X[3];
		
		// f = (1/6.931-(x1.*x2)./(x3.*x4)).^2;		
		
		double ret = pow((1.0 / 6.931) - ((x1 * x2) / (x3 * x4)), 2.0);
		//if(ret < pow(10.0,-32)) ret = 0;
		return ret;
	}

};
