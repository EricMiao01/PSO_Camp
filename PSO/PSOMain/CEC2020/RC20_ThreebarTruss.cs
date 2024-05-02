using System;
using PSOLib;

public class RC20_ThreebarTruss: Problem
{
    public override String name()
	{
		return "RC20_ThreebarTruss";
	}

    public RC20_ThreebarTruss()
    {
		x_l = new double [] { 0, 0 };
		x_u = new double [] { 1, 1 };
		setDims(x_u, x_l);
    }

	public override double GetFitness(PSOTuple pi)
	{
		double x1 = pi.X[0];
		double x2 = pi.X[1];
		//f = (2.*sqrt(2).*x(:,1)+x(:,2))*100;
		
		return (2.0 * sqrt(2.0) * x1 + x2) * 100.0;
	}

	//public override bool CheckParticle(PSOTuple pi) 
    public override ConstractResult GetConstraintResult(PSOTuple pi)
	{
		double x1 = pi.X[0];
		double x2 = pi.X[1];
		
		// g(:,1) = (sqrt(2).*x(:,1)+x(:,2))./(sqrt(2).*x(:,1).^2+2.*x(:,1).*x(:,2))*2-2;
		// g(:,2) = x(:,2)./(sqrt(2).*x(:,1).^2+2.*x(:,1).*x(:,2))*2-2;
		// g(:,3) = 1./(sqrt(2).*x(:,2)+x(:,1))*2-2;
		
		//計算限制式

		int gSize = 3;
        double[] g = new double[gSize];

		g[0] = (sqrt(2.0) * x1 + x2) / (sqrt(2.0) * pow(x1, 2) + 2.0 * x1 * x2) * 2.0 - 2.0;
		g[1] = x2 / (sqrt(2.0)*pow(x1,2)+2.0*x1*x2)*2.0 - 2.0;
		g[2] = 1.0 / (sqrt(2.0) * x2 + x1) * 2.0 - 2.0;
		
		//for(int i=0; i<gSize; i++)
		//	cout << g[i] << endl;

        return new ConstractResult(g, null);
        //for(int i=0; i<gSize; i++)
        //    if(g[i] > 0) return false;		

        //return true;
	}

};
