using System;
using PSOLib;

public class RC19_WeldedBeam: Problem
{
    public override String name()
	{
		return "RC19_WeldedBeam";
	}

    public RC19_WeldedBeam()
    {
		// xmin19   = [0.125,0.1,0.1,0.1];
		// xmax19   = [2,10,10,2];
		x_l = new double[] { 0.125, 0.1, 0.1, 0.1 };
		x_u= new double[] { 2, 10, 10, 2 };
		setDims(x_u, x_l);
    }

	public override double GetFitness(PSOTuple pi)
	{
		double x1 = pi.X[0];
		double x2 = pi.X[1];
		double x3 = pi.X[2];
		double x4 = pi.X[3];
		
		// f = 1.10471.*x(:,1).^2.*x(:,2)+0.04811.*x(:,3).*x(:,4).*(14+x(:,2));
		// return 0.04811 * x3 * x4 * (x2 + 14) + 1.10471 * pow(x1, 2) * x2;

		// f = 1.10471.*x(:,1).^2.*x(:,2)+0.04811.*x(:,3).*x(:,4).*(14+x(:,2));
		
		return 1.10471 * pow(x1,2) * x2 + 0.04811 * x3 * x4 * (14.0 + x2);
	}

	//public override bool CheckParticle(PSOTuple pi) 
    public override ConstractResult GetConstraintResult(PSOTuple pi)
	{
		//const double P = 6000.0, L = 14.0, E = pow(30.10,6), G = pow(12.10,6), Taomax = 13600.0, Sigmamax = 30000.0, Deltamax = 0.25;
		double x1 = pi.X[0];
		double x2 = pi.X[1];
		double x3 = pi.X[2];
		double x4 = pi.X[3];
		
		double P = 6000.0, L = 14.0, delta_max = 0.25, E = 30*1e6, G = 12*1e6;
		double T_max = 13600.0, sigma_max = 30000.0;
		double Pc = 4.013 * E * sqrt(pow(x3,2)*pow(x4,6)/30.0) / pow(L,2) * (1.0-x3/(2.0*L)*sqrt(E/(4.0*G)));
		double sigma = 6.0*P*L/(x4*pow(x3,2));
		double delta = 6.0*P*pow(L,3)/(E*pow(x3,2)*x4);
		double J = 2.0*(sqrt(2)*x1*x2*(pow(x2,2) / 4.0+pow((x1+x3),2)/4.0));
		double R = sqrt(pow(x2,2) / 4.0+pow((x1+x3),2) / 4.0);
		double M = P*(L+x2/2.0);
		double ttt = M*R/J;
		double tt = P/(sqrt(2.0)*x1*x2);
		double t  = sqrt(pow(tt,2)+2.0*tt*ttt*x2/(2.0*R)+pow(ttt,2));
		// %% constraints
		// g(:,1) = t-T_max;
		// g(:,2) = sigma-sigma_max;
		// g(:,3) = x(:,1)-x(:,4);
		// g(:,4) = delta-delta_max;
		// g(:,5) = P-Pc;
		
		int gSize = 5;
        double[] g = new double[gSize];

		g[0] = t-T_max;
		g[1] = sigma-sigma_max;
		g[2] = x1-x4;
		g[3] = delta-delta_max;
		g[4] = P-Pc;

        return new ConstractResult(g, null);
		//for(int i=0; i<gSize; i++)
		//	cout << g[i] << endl;

        //for(int i=0; i<gSize; i++)
        //    if(g[i] > 0) return false;		
		
        //return true;
	}

};
