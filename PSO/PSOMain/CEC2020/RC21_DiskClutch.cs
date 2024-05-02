using System;
using PSOLib;

public class RC21_DiskClutch: Problem
{
    public override String name()
	{
		return "RC21_DiskClutch";
	}

    public RC21_DiskClutch()
    {
		x_l = new double[] { 60, 90, 1, 0, 2 };
		x_u = new double[] { 80, 110, 3, 1000, 9 };
		setDims(x_u, x_l);
    }

	public override double GetFitness(PSOTuple pi)
	{
		double x1 = pi.X[0];
		double x2 = pi.X[1];
		double x3 = pi.X[2];
		double x4 = pi.X[3];
		double x5 = pi.X[4];
		// f = pi.*(x(:,2).^2-x(:,1).^2).*x(:,3).*(x(:,5)+1).*rho;
		
		double rho = 0.0000078;
		return PI * (pow(x2,2) - pow(x1,2)) * x3 * (x5 + 1) * rho;
	}

	// public override bool CheckParticle(PSOTuple pi) 
    public override ConstractResult GetConstraintResult(PSOTuple pi)
    {
		double x1 = pi.X[0];
		double x2 = pi.X[1];
		double x3 = pi.X[2];
		double x4 = pi.X[3];
		double x5 = pi.X[4];
		
	   // Mf = 3; Ms = 40; Iz = 55; n = 250; Tmax = 15; s = 1.5; delta = 0.5; 
	   // Vsrmax = 10; rho = 0.0000078; pmax = 1; mu = 0.6; Lmax = 30; delR = 20;
	   // Rsr = 2./3.*(x(:,2).^3-x(:,1).^3)./(x(:,2).^2.*x(:,1).^2);
	   // Vsr = pi.*Rsr.*n./30;
	   // A   = pi.*(x(:,2).^2-x(:,1).^2);
	   // Prz = x(:,4)./A;
	   // w   = pi.*n./30;
	   // Mh  = 2/3.*mu.*x(:,4).*x(:,5).*(x(:,2).^3-x(:,1).^3)./(x(:,2).^2-x(:,1).^2);
	   // T   = Iz.*w./(Mh+Mf);
	   // %%
	   // f = pi.*(x(:,2).^2-x(:,1).^2).*x(:,3).*(x(:,5)+1).*rho;
	   // g(:,1) = -x(:,2)+x(:,1)+delR;
	   // g(:,2) = (x(:,5)+1).*(x(:,3)+delta)-Lmax;
	   // g(:,3) = Prz-pmax;
	   // g(:,4) = Prz.*Vsr-pmax.*Vsrmax;
	   // g(:,5) = Vsr-Vsrmax;
	   // g(:,6) = T-Tmax;
	   // g(:,7) = s.*Ms-Mh;
	   // g(:,8) = -T;
		
		//計算限制式
		double Mf = 3.0, Ms = 40.0, Iz = 55.0, n = 250.0, Tmax = 15.0, s = 1.5, delta = 0.5;
		double Vsrmax = 10.0, rho = 0.0000078, pmax = 1, mu = 0.6, Lmax = 30.0, delR = 20.0;
		double Rsr = 2.0 / 3.0 * ((pow(x2,3) - pow(x1,3)) / (pow(x2,2) * pow(x1,2)));
		double Vsr = PI * Rsr * n / 30.0;
		double A   = PI * (pow(x2,2)-pow(x1,2));
		double Prz = x4 / A;
		double w   = PI * n / 30.0;		             
		double Mh  = 2.0 / 3.0 * mu * x4 * x5 * ((pow(x2,3)-pow(x1,3)) / (pow(x2,2)-pow(x1,2)));
		double T   = Iz * w / (Mh + Mf);

		int gSize = 8;
        double[] g = new double[gSize];

		g[0] = -x2+x1+delR;
		g[1] = (x5+1)*(x3+delta)-Lmax;
		g[2] = Prz-pmax;
		g[3] = Prz*Vsr-pmax*Vsrmax;
		g[4] = Vsr-Vsrmax;
		g[5] = T-Tmax;
		g[6] = s*Ms-Mh;
		g[7] = -T;

        return new ConstractResult(g, null);
		//for(int i=0; i<gSize; i++)
		//	cout << g[i] << endl;

        //for(int i=0; i<gSize; i++)
        //    if(g[i] > 0) return false;		
				
		//符合就return 0,不合就return 1 
        //return true;
	}

};
