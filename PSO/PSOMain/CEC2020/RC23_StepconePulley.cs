using System;
using PSOLib;

public class RC23_StepconePulley: Problem
{
    public override String name()
	{
		return "RC23_StepconePulley";
	}

    public RC23_StepconePulley()
    {
		// xmin23   = [0,0,0,0,0];
		// xmax23   = [60,60,90,90,90];
		x_l = new double[] {0,0,0,0,0};
		x_u = new double[] {60,60,90,90,90};
		setDims(x_u, x_l);
    }

	public override double GetFitness(PSOTuple pi)
	{
		double x1 = pi.X[0];
		double x2 = pi.X[1];
		double x3 = pi.X[2];
		double x4 = pi.X[3];
		double x5 = pi.X[4];

		double d1 = x1*1e-3, d2 = x2*1e-3, d3 = x3*1e-3, d4 = x4*1e-3, w = x5*1e-3;
		double N = 350, N1 = 750, N2 = 450, N3 = 250, N4 = 150;
		double rho = 7200, a = 3, mu = 0.35, s = 1.75*1e6, t = 8*1e-3;
		
		//f = rho.*w.*pi./4.*(d1.^2.*(1+(N1./N).^2)+d2.^2.*(1+(N2./N).^2)+d3.^2.*(1+(N3./N).^2)+d4.^2.*(1+(N4./N).^2));

		return rho*w*PI/4.0*(pow(d1,2)*(1.0+pow((N1/N),2))+pow(d2,2)*(1.0+pow((N2/N),2))+pow(d3,2)*(1.0+pow((N3/N),2))+pow(d4,2)*(1.0+pow((N4/N),2)));
	}

	//public override bool CheckParticle(PSOTuple pi) 
    public override ConstractResult GetConstraintResult(PSOTuple pi)
	{
		double x1 = pi.X[0];
		double x2 = pi.X[1];
		double x3 = pi.X[2];
		double x4 = pi.X[3];
		double x5 = pi.X[4];

		//cout << "CheckParticle " << pi.X[0] << endl;
			
		
		// C1 = pi.*d1./2.*(1+N1./N)+(N1./N-1).^2.*d1.^2./(4.*a)+2.*a;
		// C2 = pi.*d2./2.*(1+N2./N)+(N2./N-1).^2.*d2.^2./(4.*a)+2.*a;
		// C3 = pi.*d3./2.*(1+N3./N)+(N3./N-1).^2.*d3.^2./(4.*a)+2.*a;
		// C4 = pi.*d4./2.*(1+N4./N)+(N4./N-1).^2.*d4.^2./(4.*a)+2.*a;
		// R1 = exp(mu.*(pi-2.*asin((N1./N-1).*d1./(2.*a))));
		// R2 = exp(mu.*(pi-2.*asin((N2./N-1).*d2./(2.*a))));
		// R3 = exp(mu.*(pi-2.*asin((N3./N-1).*d3./(2.*a))));
		// R4 = exp(mu.*(pi-2.*asin((N4./N-1).*d4./(2.*a))));
		// P1 = s.*t.*w.*(1-exp(-mu.*(pi-2.*asin((N1/N-1).*d1/(2.*a))))).*pi.*d1.*N1./60;
		// P2 = s.*t.*w.*(1-exp(-mu.*(pi-2.*asin((N2/N-1).*d2/(2.*a))))).*pi.*d2.*N2./60;
		// P3 = s.*t.*w.*(1-exp(-mu.*(pi-2.*asin((N3/N-1).*d3/(2.*a))))).*pi.*d3.*N3./60;
		// P4 = s.*t.*w.*(1-exp(-mu.*(pi-2.*asin((N4/N-1).*d4/(2.*a))))).*pi.*d4.*N4./60;

		// g(:,1) = -R1+2;
		// g(:,2) = -R2+2;
		// g(:,3) = -R3+2;
		// g(:,4) = -R4+2;
		// g(:,5) = -P1+(0.75*745.6998);
		// g(:,6) = -P2+(0.75*745.6998);
		// g(:,7) = -P3+(0.75*745.6998);
		// g(:,8) = -P4+(0.75*745.6998);
		// h(:,1) = C1-C2;
		// h(:,2) = C1-C3;
		// h(:,3) = C1-C4;		
		
		double d1 = x1*1e-3, d2 = x2*1e-3, d3 = x3*1e-3, d4 = x4*1e-3, w = x5*1e-3;
		double N = 350, N1 = 750, N2 = 450, N3 = 250, N4 = 150;
		double rho = 7200, a = 3, mu = 0.35, s = 1.75*1e6, t = 8*1e-3;
		
		//計算限制式
		double C1 = PI*d1/2*(1+N1/N)+pow((N1/N-1),2)*pow(d1,2)/(4*a)+2*a;
		double C2 = PI*d2/2*(1+N2/N)+pow((N2/N-1),2)*pow(d2,2)/(4*a)+2*a;
		double C3 = PI*d3/2*(1+N3/N)+pow((N3/N-1),2)*pow(d3,2)/(4*a)+2*a;
		double C4 = PI*d4/2*(1+N4/N)+pow((N4/N-1),2)*pow(d4,2)/(4*a)+2*a;
		double R1 = exp(mu*(PI-2*asin((N1/N-1)*d1/(2*a))));
		double R2 = exp(mu*(PI-2*asin((N2/N-1)*d2/(2*a))));
		double R3 = exp(mu*(PI-2*asin((N3/N-1)*d3/(2*a))));
		double R4 = exp(mu*(PI-2*asin((N4/N-1)*d4/(2*a))));
		double P1 = s*t*w*(1-exp(-mu*(PI-2*asin((N1/N-1)*d1/(2*a)))))*PI*d1*N1/60;
		double P2 = s*t*w*(1-exp(-mu*(PI-2*asin((N2/N-1)*d2/(2*a)))))*PI*d2*N2/60;
		double P3 = s*t*w*(1-exp(-mu*(PI-2*asin((N3/N-1)*d3/(2*a)))))*PI*d3*N3/60;
		double P4 = s*t*w*(1-exp(-mu*(PI-2*asin((N4/N-1)*d4/(2*a)))))*PI*d4*N4/60;

		int gSize = 8;
        double[] g = new double[gSize];

		g[0] = -R1+2.0;
		g[1] = -R2+2.0;
		g[2] = -R3+2.0;
		g[3] = -R4+2.0;
		g[4] = -P1+(0.75*745.6998);
		g[5] = -P2+(0.75*745.6998);
		g[6] = -P3+(0.75*745.6998);
		g[7] = -P4+(0.75*745.6998);
		
		//for(int i=0; i<gSize; i++)
		//	cout << g[i] << endl;

        double[] h = new double[3];
        h[0] = C1 - C2;
        h[1] = C1 - C3;
        h[2] = C1 - C4;
        //double h1 = abs(C1 - C2);
        //double h2 = abs(C1 - C3);
        //double h3 = abs(C1 - C4);
		//cout << h1 << endl;
		//cout << h2 << endl;
		//cout << h3 << endl;

        return new ConstractResult(g, h);

        //for(int i=0; i<gSize; i++)
        //    if(g[i] > 0) return false;		

		//cout << x1 <<","<< x2 <<","<< x3 <<","<< x4 << endl;
		//cout << C1 <<","<< C2 <<","<< C3 << "," << abs(C1-C2) << endl;
		//return true;
		
		// h(:,1) = C1-C2;
		// h(:,2) = C1-C3;
		// h(:,3) = C1-C4;		
        //double epsilon = 0.001;

        //if(h1>epsilon) return false;
        //if(h2>epsilon) return false;
        //if(h3>epsilon) return false;

        //return true;
	}

};
