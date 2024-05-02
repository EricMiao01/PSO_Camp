using System;
using PSOLib;

public class RC24_RobotGripper: Problem
{
    public override String name()
	{
		return "RC24_RobotGripper";
	}

    public RC24_RobotGripper()
    {
		// xmin24   = [10,10,100,0,10,100,1];
		// xmax24   = [150,150,200,50,150,300,3.14];
		x_l = new double[] {10,10,100,0,10,100,1};
		x_u = new double[] {150,150,200,50,150,300,3.14};
		setDims(x_u, x_l);		
    }

// function ff = OBJ11(x,n)
// a = x(1); b = x(2); c = x(3); e = x(4); f = x(5); l = x(6); 
 // Zmax = 99.9999; P = 100;
// if n == 1
     // fhd = @(z) P.*b.*sin(acos((a.^2+(l-z).^2+e.^2-b.^2)./(2.*a.*sqrt((l-z).^2+e.^2)))+acos((b.^2+(l-z).^2+e.^2-a.^2)./(2.*b.*sqrt((l-z).^2+e.^2))))./....
       // (2.*c.*cos(acos((a.^2+(l-z).^2+e.^2-b.^2)./(2.*a.*sqrt((l-z).^2+e.^2)))+atan(e./(l-z))));
// else
    // fhd = @(z) -(P.*b.*sin(acos((a.^2+(l-z).^2+e.^2-b.^2)./(2.*a.*sqrt((l-z).^2+e.^2)))+acos((b.^2+(l-z).^2+e.^2-a.^2)./(2.*b.*sqrt((l-z).^2+e.^2))))./....
       // (2.*c.*cos(acos((a.^2+(l-z).^2+e.^2-b.^2)./(2.*a.*sqrt((l-z).^2+e.^2)))+atan(e./(l-z)))));
// end
// options = optimset('Display','off');
 // [~,ff]= fminbnd(fhd,0,Zmax,options); 
// end

	double OBJ11(double[] x, double n)
	{
		double a = x[0], b = x[1], c = x[2], e = x[3], f = x[4], l = x[5];
		//cout << a << "," << b << "," << c << "," << e << "," << f << "," << l << endl;
		
		double Zmax = 99.9999, P = 100, fhd = 0;
		double z = n;
        /*
         * ³o¬q¦³¿ù, «Ý­×;
		if (n == 1)
			 fhd = P*b*sin(acos((a, 2.0+pow((l-z), 2.0)+pow(e, 2.0)-pow(b, 2.0))/(2.0*a*sqrt(pow((l-z), 2.0)+pow(e, 2.0))))+acos((pow(b, 2.0)+pow((l-z), 2.0)+pow(e, 2.0)-pow(a, 2.0))
				   /(2.0*b*sqrt(pow((l-z), 2.0)+pow(e, 2.0)))))
				   /(2.0*c*cos(acos((pow(a, 2.0)+pow((l-z), 2.0)+pow(e, 2.0)-pow(b, 2.0))
				   /(2.0*a*sqrt(pow((l-z), 2.0)+pow(e, 2.0))))+atan(e/(l-z))));
		else
			fhd = -(P*b*sin(acos((pow(a, 2.0)+pow((l-z), 2.0)+pow(e, 2.0)-pow(b, 2.0))/(2.0*a*sqrt(pow((l-z), 2.0)+pow(e, 2.0))))+acos((pow(b, 2.0)+pow((l-z), 2.0)+pow(e, 2.0)-pow(a, 2.0))
				   /(2.0*b*sqrt(pow((l-z), 2.0)+pow(e, 2.0)))))
				   /(2.0*c*cos(acos((pow(a, 2.0)+pow((l-z), 2.0)+pow(e, 2.0)-pow(b, 2.0))/(2.0*a*sqrt(pow((l-z), 2.0)+pow(e, 2.0))))+atan(e/(l-z)))));

        */
        return fhd;
	}

	public override double GetFitness(PSOTuple pi)
	{
		double a = pi.X[0];
		double b = pi.X[1];
		double c = pi.X[2];
		double e = pi.X[3];
		double ff = pi.X[4];
		double l = pi.X[5];
		double delta = pi.X[6];

		// a = x(:,1); b = x(:,2); c = x(:,3); e = x(:,4); ff = x(:,5); l = x(:,6); delta = x(:,7);
		double Ymin = 50, Ymax = 100, YG = 150, Zmax = 99.9999, P = 100;
		//alpha_0 = acos((a.^2+l.^2+e.^2-b.^2)./(2.*a.*sqrt(l.^2+e.^2)))+atan(e./l);
		double alpha_0 = acos((a*a+l*l+e*e-b*b) / (2.0*a*sqrt(l*l+e*e)))+atan(e/l);
		double beta_0  = acos((b*b+l*l+e*e-a*a) / (2.0*b*sqrt(l*l+e*e)))-atan(e/l);
		// alpha_m = acos((a.^2+(l-Zmax).^2+e.^2-b.^2)./(2.*a.*sqrt((l-Zmax).^2+e.^2)))+atan(e./(l-Zmax));
		double alpha_m = acos((a*a+pow((l-Zmax),2.0)+e*e-b*b) / (2.0*a*sqrt(pow((l-Zmax),2.0)+e*e)))+atan(e/(l-Zmax));
		double beta_m  = acos((b*b+pow((l-Zmax),2.0)+e*e-a*a) / (2.0*b*sqrt(pow((l-Zmax),2.0)+e*e)))-atan(e/(l-Zmax));
		// %% objective function
		double ret = 0;
		
		//min + max??
		// for i = 1:ps
		   // f(i,1) = -OBJ11(x(i,:),2)-OBJ11(x(i,:),1);
		// end
		//cout << "OBJ11: " << OBJ11(pi.X, 2.0) << endl;
		
		
		return ret;
	}

	// public override bool CheckParticle(PSOTuple pi) 
    public override ConstractResult GetConstraintResult(PSOTuple pi)
    {
		double a = pi.X[0];
		double b = pi.X[1];
		double c = pi.X[2];
		double e = pi.X[3];
		double ff = pi.X[4];
		double l = pi.X[5];
		double delta = pi.X[6];

		double Ymin = 50, Ymax = 100, YG = 150, Zmax = 99.9999, P = 100;

		double beta_0  = acos((b*b+l*l+e*e-a*a) / (2.0*b*sqrt(l*l+e*e)))-atan(e/l);
		// alpha_m = acos((a.^2+(l-Zmax).^2+e.^2-b.^2)./(2.*a.*sqrt((l-Zmax).^2+e.^2)))+atan(e./(l-Zmax));
		//double alpha_m = acos((a*a+pow((l-Zmax),2.0)+e*e-b*b) / (2.0*a*sqrt(pow((l-Zmax),2.0)+e*e)))+atan(e/(l-Zmax));
		double beta_m  = acos((b*b+pow((l-Zmax),2.0)+e*e-a*a) / (2.0*b*sqrt(pow((l-Zmax),2.0)+e*e)))-atan(e/(l-Zmax));
		
		int gSize = 7;
        double[] g = new double[gSize];
		
		double Yxmin = 2.0 *(e+ff+c*sin(beta_m+delta));
		double Yxmax = 2.0 *(e+ff+c*sin(beta_0+delta));
		g[0] = Yxmin-Ymin;
		g[1] = -Yxmin;
		g[2] = Ymax-Yxmax;
		g[3] = Yxmax-YG;
		g[4] = l*l+e*e-pow((a+b),2);
		g[5] = b*b-pow((a-e),2)-pow((l-Zmax),2);
		g[6] = Zmax-l;

        return new ConstractResult(g, null);
		//for(int i=0; i<gSize; i++)
		//	cout << g[i] << endl;

        //for(int i=0; i<gSize; i++)
        //    if(g[i] > 0) return false;		

        //return true;
	}

};
