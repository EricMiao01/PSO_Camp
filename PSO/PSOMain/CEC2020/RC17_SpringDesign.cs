using System;
using PSOLib;

public class RC17_SpringDesign: Problem
{
    public override String name()
	{
		return "RC17_SpringDesign";
	}

    public RC17_SpringDesign()
    {
		x_l = new double[] { 0.05, 0.25, 2.0 };
        x_u = new double[] { 2.0, 1.3, 15.0 };
		setDims(x_u, x_l);
    }

	public override ConstractResult GetConstraintResult(PSOTuple pi)
    {
		double x1 = pi.X[0];
		double x2 = pi.X[1];
		double x3 = pi.X[2];

		int gSize = 4;
        double[] g = new double[gSize];

		// g(:,1) = 1-(x(:,2).^3.*x(:,3))./(71785.*x(:,1).^4);
		// g(:,2) = (4.*x(:,2).^2-x(:,1).*x(:,2))./(12566.*(x(:,2).*x(:,1).^3-x(:,1).^4))....
				 // + 1./(5108.*x(:,1).^2)-1;
		// g(:,3) = 1-140.45.*x(:,1)./(x(:,2).^2.*x(:,3));
		// g(:,4) = (x(:,1)+x(:,2))./1.5-1;

		//計算限制式
		g[0] = 1.0-(pow(x2,3)*x3)/(71785.0 * pow(x1,4));
		g[1] = (4.0 * pow(x2,2) - x1 * x2) / (12566.0 * (x2 * pow(x1, 3) - pow(x1,4))) + 1.0 / (5108.0*pow(x1,2)) - 1.0;
		g[2] = 1.0-140.45*x1/(pow(x2,2)*x3);
		g[3] = (x1+x2)/1.5-1.0;

		//for(int i=0; i<gSize; i++)
		//	cout << g[i] << endl;

        return new ConstractResult(g, null);
        //for(int i=0; i<gSize; i++)
        //    if(g[i] > 0) return false;

        //return true;
	}

	public override double GetFitness(PSOTuple pi)
	{
		double x1 = pi.X[0];
		double x2 = pi.X[1];
		double x3 = pi.X[2];
		
		// f = x(:,1).^2.*x(:,2).*(x(:,3)+2);
		return x1 * x1 * x2 * (2.0 + x3);
	}

};
