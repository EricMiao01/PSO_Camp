using System;
using PSOLib;

public class RC28_RollingBearing: Problem
{
    public override String name()
	{
		return "RC28_RollingBearing";
	}

    public RC28_RollingBearing()
    {
		// xmin28   = [125,10.5,4.51,0.515,0.515,0.4,0.6,0.3,0.02,0.6];
		// xmax28   = [150,31.5,50.49,0.6,0.6,0.5,0.7,0.4,0.1,0.85];
		double[] x_l = {125, 10.5, 4.51, 0.515, 0.515, 0.4, 0.6, 0.3, 0.02, 0.6};
		double[] x_u = {150, 31.5, 50.49, 0.6, 0.6, 0.5, 0.7, 0.4, 0.1, 0.85};
		setDims(x_u, x_l);
    }

	public override double GetFitness(PSOTuple pi)
	{
		double x1 = pi.X[0];
		double x2 = pi.X[1];
		double x3 = pi.X[2];
		double x4 = pi.X[3];
		double x5 = pi.X[4];
		double x6 = pi.X[5];
		double x7 = pi.X[6];
		double x8 = pi.X[7];
		double x9 = pi.X[8];
		double x10 = pi.X[9];

		// %% Rolling element bearing
		double Dm = x1, Db = x2, Z = round(x3), fi = x4, fo = x5;
		double KDmin = x6, KDmax = x7, eps = x8, e = x9, chi = x10;
		double D = 160.0, d = 90.0, Bw = 30.0, T = D - d - 2.0 * Db;
		
		double phi_o = 2.0 * PI - 2.0 * acos((pow(((D-d) * 0.5 - 0.75 * T),2.0)+pow((0.5 * D - 0.25 * T - Db),2.0)
			   -pow((0.5 * d + 0.25 * T),2.0)) / (2.0 *(0.5 * (D-d) - 0.75 * T) * (0.5 * D-0.25 * T-Db)));
		double gamma = Db / Dm;
		double fc    = 37.91 * pow((1.0+pow((1.04 *pow(((1.0-gamma)/(1.0+gamma)),1.72)*pow((fi*(2.0*fo-1)/(fo*(2.0*fi-1.0))),0.41)),(10.0/3.0))),(-0.3))
			   *(pow(gamma,0.3) * pow((1.0-gamma),1.39) / pow((1.0+gamma),(1.0/3.0))) * pow((2.0*fi/(2.0*fi-1.0)),0.41);
		// %% objective function
		//ind    = find(Db > 25.4);
		// cout << fc << endl;
		// cout << pow(Z,(2.0/3.0)) << endl;
		// cout << pow(Db,(1.4)) << endl;
		// cout << "UUU" << endl;
		//cout << Db << endl;
		// cout << 3.647 * 32.5297306939062381 * 13.5720880829745312 * 125.2087775160501053 << endl;
		if(Db <= 25.4)
			return fc * pow(Z,(2.0/3.0)) * pow(Db,(1.8));
		else
			//return 3.647 * 32.5297306939062381 * 13.5720880829745312 * 125.2087775160501053;
			return 3.647 * fc * pow(Z,(2.0/3.0)) * pow(Db,1.4); //3.647.*fc(ind).*Z(ind).^(2/3).*Db(ind).^1.4;
	}

	// public override bool CheckParticle(PSOTuple pi) 
    public override ConstractResult GetConstraintResult(PSOTuple pi)
	{
		double x1 = pi.X[0];
		double x2 = pi.X[1];
		double x3 = pi.X[2];
		double x4 = pi.X[3];
		double x5 = pi.X[4];
		double x6 = pi.X[5];
		double x7 = pi.X[6];
		double x8 = pi.X[7];
		double x9 = pi.X[8];
		double x10 = pi.X[9];
		
		// g(:,1) = Z-1-phi_o./(2.*asin(Db./Dm));
		// g(:,2) = KDmin.*(D-d)-2.*Db;
		// g(:,3) = 2.*Db-KDmax.*(D-d);
		// g(:,4) = chi.*Bw -Db;
		// g(:,5) = 0.5.*(D+d)-Dm;
		// g(:,6) = Dm-(0.5+e).*(D+d);
		// g(:,7) = eps.*Db-0.5.*(D-Dm-Db);
		// g(:,8) = 0.515 - fi;
		// g(:,9) = 0.515 - fo;
		
		//計算限制式
		double Dm = x1, Db = x2, Z = round(x3), fi = x4, fo = x5;
		double KDmin = x6, KDmax = x7, eps = x8, e = x9, chi = x10;
		double D = 160.0, d = 90.0, Bw = 30.0, T = D - d - 2.0 * Db;
		
		double phi_o = 2.0 * PI - 2.0 * acos((pow(((D-d) * 0.5 - 0.75 * T),2.0)+pow((0.5 * D - 0.25 * T - Db),2.0)
			   -pow((0.5 * d + 0.25 * T),2.0)) / (2.0 *(0.5 * (D-d) - 0.75 * T) * (0.5 * D-0.25 * T-Db)));
		double gamma = Db / Dm;
		double fc    = 37.91 * pow((1.0+pow((1.04 *pow(((1.0-gamma)/(1.0+gamma)),1.72)*pow((fi*(2.0*fo-1)/(fo*(2.0*fi-1.0))),0.41)),(10.0/3.0))),(-0.3))
			   *(pow(gamma,0.3) * pow((1.0-gamma),1.39) / pow((1.0+gamma),(1.0/3.0))) * pow((2.0*fi/(2.0*fi-1.0)),0.41);

		int gSize = 9;
        double[] g = new double[gSize];
		
		g[0] = Z - 1.0 - phi_o / (2.0 * asin(Db/Dm));
		g[1] = KDmin * (D-d) - 2.0 * Db;
		g[2] = 2.0 * Db-KDmax*(D-d);
		g[3] = chi * Bw - Db;
		g[4] = 0.5 * (D+d)-Dm;
		g[5] = Dm - (0.5+e)*(D+d);
		g[6] = eps*Db-0.5*(D-Dm-Db);
		g[7] = 0.515 - fi;
		g[8] = 0.515 - fo;

        return new ConstractResult(g, null);

		//for(int i=0; i<gSize; i++)
		//	cout << g[i] << endl;

        //for(int i=0; i<gSize; i++)
        //    if(g[i] > 0) return false;		

        //return true;
	}

};
