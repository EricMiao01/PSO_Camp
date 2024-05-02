using System;
using PSOLib;

public class RC25_ThrustBearing: Problem
{
    public override String name()
	{
		return "RC25_ThrustBearing";
	}

    public RC25_ThrustBearing()
    {
		// xmin25   = [ 1, 1,  1e-6,1];
		// xmax25   = [16, 16, 16*1e-6,16];
		x_l = new double[] { 1, 1,  pow(10,-6), 1 };
		x_u = new double[] { 16, 16, 16*pow(10,-6), 16 };
		setDims(x_u, x_l);
    }

	public override double GetFitness(PSOTuple pi)
	{
		double x1 = pi.X[0];
		double x2 = pi.X[1];
		double x3 = pi.X[2];
		double x4 = pi.X[3];

		double R = x1, Ro = x2, mu = x3, Q = x4;
		double gamma = 0.0307, C = 0.5, n = -3.55, C1 = 10.04;
		double Ws = 101000, Pmax = 1000, delTmax = 50, hmin = 0.001;
		double gg = 386.4, N = 750;
		double P    = (log10(log10(8.122 * pow(10.0,6) * mu + 0.8))-C1) / n;
		double delT = 2.0*(pow(10,P)-560.0);
		double Ef   = 9336.0 *Q *gamma *C *delT;
		double h    = pow((2.0*PI*N/60),2)*2.0*PI*mu/Ef*(pow(R,4)/4.0-pow(Ro,4)/4.0)-1e-5;
		double Po   = (6.0*mu*Q/(PI*pow(h,3)))*log(R/Ro);
		double W    = PI*Po/2.0*(pow(R,2)-pow(Ro,2)) / (log(R/Ro)-1e-5);
		// %%  objective function
		return (Q*Po / 0.7+Ef) / 12.0;
	}

	//public override bool CheckParticle(PSOTuple pi) 
    public override ConstractResult GetConstraintResult(PSOTuple pi)
	{
		double x1 = pi.X[0];
		double x2 = pi.X[1];
		double x3 = pi.X[2];
		double x4 = pi.X[3];
		
		double R = x1, Ro = x2, mu = x3, Q = x4;
		double gamma = 0.0307, C = 0.5, n = -3.55, C1 = 10.04;
		double Ws = 101000, Pmax = 1000, delTmax = 50, hmin = 0.001;
		double gg = 386.4, N = 750;
		double P    = (log10(log10(8.122 * pow(10.0,6) * mu + 0.8))-C1) / n;
		double delT = 2.0*(pow(10,P)-560.0);
		double Ef   = 9336.0 *Q *gamma *C *delT;
		double h    = pow((2.0*PI*N/60),2)*2.0*PI*mu/Ef*(pow(R,4)/4.0-pow(Ro,4)/4.0)-1e-5;
		double Po   = (6.0*mu*Q/(PI*pow(h,3)))*log(R/Ro);
		double W    = PI*Po/2.0*(pow(R,2)-pow(Ro,2)) / (log(R/Ro)-1e-5);

		int gSize = 7;
        double[] g = new double[gSize];

		g[0] = Ws-W;
		g[1] = Po-Pmax;
		g[2] = delT-delTmax;
		g[3] = hmin-h;
		g[4] = Ro-R;
		g[5] = gamma/(gg*Po)*(Q/(2*PI*R*h))-0.001;
		g[6] = W/(PI*(pow(R,2)-pow(Ro,2))+1e-5)-5000;

		//for(int i=0; i<gSize; i++)
		//	cout << g[i] << endl;

        return new ConstractResult(g, null);
        //for(int i=0; i<gSize; i++)
        //    if(g[i] > 0) return false;		
		
        //return true;
	}

};
