using System;
using PSOLib;

public class RC33_Topology: Problem
{
    public override String name()
	{
		return "RC33_Topology";

		/*
		%% Topology Optimization
		nely = 10;
		nelx = 3;
		penal = 3;
		for i = 1:ps
			X = [x(i,1:10);x(i,11:20);x(i,21:30)]';
			% FE-ANALYSIS
			[U]=FE(3,10,X,3);         
			% OBJECTIVE FUNCTION AND SENSITIVITY ANALYSIS
			[KE] = lk;
			c = 0.;
			for ely = 1:nely
				for elx = 1:nelx
					n1 = (nely+1)*(elx-1)+ely; 
					n2 = (nely+1)* elx   +ely;
					Ue = U([2*n1-1;2*n1; 2*n2-1;2*n2; 2*n2+1;2*n2+2; 2*n1+1;2*n1+2],1);
					c = c + X(ely,elx)^penal*Ue'*KE*Ue;
					dc(ely,elx) = -penal*X(ely,elx)^(penal-1)*Ue'*KE*Ue;
				end
			end
			% FILTERING OF SENSITIVITIES
			[dc]   = check(3,10,1.5,X,dc); 
			f(i,1) = c;
			g(i,:) = dc(1:end);
		end
		h = zeros(ps,1);
		*/		
	}

	int dims = 30;
    public RC33_Topology()
    {
		//xmin33   = 0.001.*ones(1,par.n);
		//xmax33   = ones(1,par.n);
		
		double[] x_l = new double[dims];
		double[] x_u = new double[dims];
		for(int i=0;i<dims;i++) {
			x_l[i] = 1.0e-03;
			x_u[i] = 1.0;
		}		
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
		double Mf = 3, Ms = 40, Iz = 55, n = 250, Tmax = 15, s = 1.5, delta = 0.5;
		double Vsrmax = 10, rho = 0.0000078, pmax = 1, mu = 0.6, Lmax = 30, delR = 20;
		double Rsr = 2.0 / 3.0 * ((pow(x2,3) - pow(x1,3)) / (pow(x2,2) * pow(x1,2)));
		double Vsr = PI * Rsr * n / 30.0;
		double A   = PI * (pow(x2,2)-pow(x1,2));
		double Prz = x4 / A;
		double w   = PI * n / 30.0;		             
		double Mh  = 2.0 / 3.0 * mu * x4 * x5 * ((pow(x2,3)-pow(x1,3)) / (pow(x2,2)-pow(x1,2)));
		double T   = Iz * w / (Mh + Mf);

        double[] g = new double[8];
		g[0] = -x2+x1+delR;
		//if(g1>0) return false;
        g[1] = (x5 + 1) * (x3 + delta) - Lmax;
		//if(g2>0) return false;
		g[2] = Prz-pmax;
		//if(g3>0) return false;
		g[3] = Prz*Vsr-pmax*Vsrmax;
		//if(g4>0) return false;
		g[4] = Vsr-Vsrmax;
		//if(g5>0) return false;
		g[5] = T-Tmax;
		//if(g6>0) return false;
		g[6] = s*Ms-Mh;
		//if(g7>0) return false;
		g[7] = -T;
		//if(g8>0) return false;

        return new ConstractResult(g, null);
		//符合就return 0,不合就return 1 
		//return true;
	}

};
