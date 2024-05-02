using System;
using PSOLib;

public class RC26_GearBox: Problem
{
    public override String name()
	{
		return "RC26_GearBox";
	}

    public RC26_GearBox()
    {
		// xmin26   = [ 6.51.*ones(1,8), 0.51.*ones(1,14)];
		// xmax26   = [ 76.49.*ones(1,8), 4.49.*ones(1,4), 9.49.*ones(1,10)];
		double[] x_l = new double[22];
		double[] x_u = new double[22];
		for(int i=0; i<22; i++) {
			if(i<8) {
				x_l[i] = 6.51;
				x_u[i] = 76.49;
			} else {
				x_l[i] = 0.51;
				if(i<12)
					x_u[i] = 4.49;
				else
					x_u[i] = 9.49;
			}
		}

		setDims(x_u, x_l);
    }

	public override double GetFitness(PSOTuple pi)
	{
		int Np1 = round(pi.X[0]);
		int Ng1 = round(pi.X[1]);
		int Np2 = round(pi.X[2]);
		int Ng2 = round(pi.X[3]);
		int Np3 = round(pi.X[4]);
		int Ng3 = round(pi.X[5]);
		int Np4 = round(pi.X[6]);
		int Ng4 = round(pi.X[7]);
		double[] Pvalue = new double[] { 3.175, 5.715, 8.255, 12.7};
		double b1 = Pvalue[(int)round(pi.X[8])-1], b2 = Pvalue[(int)round(pi.X[9])-1], b3 = Pvalue[(int)round(pi.X[10])-1], b4 = Pvalue[(int)round(pi.X[11])-1];

		// x = round(x);
		// Np1 = x(:,1); Ng1 = x(:,2); Np2 = x(:,3); Ng2 = x(:,4);
		// Np3 = x(:,5); Ng3 = x(:,6); Np4 = x(:,7); Ng4 = x(:,8);
		// Pvalue = [ 3.175, 5.715, 8.255, 12.7];
		// b1 = Pvalue(x(:,9))'; b2 = Pvalue(x(:,10))'; b3 = Pvalue(x(:,11))';
		// b4 = Pvalue(x(:,12))';

		double[] XYvalue = new double[] { 12.7, 25.4, 38.1, 50.8, 63.5, 76.2, 88.9, 101.6, 114.3};
		double xp1 = XYvalue[(int)round(pi.X[12])-1], xg1 = XYvalue[(int)round(pi.X[13])-1];
		double xg2 = XYvalue[(int)round(pi.X[14])-1], xg3 = XYvalue[(int)round(pi.X[15])-1];
		double xg4 = XYvalue[(int)round(pi.X[16])-1], yp1 = XYvalue[(int)round(pi.X[17])-1];
		double yg1 = XYvalue[(int)round(pi.X[18])-1], yg2 = XYvalue[(int)round(pi.X[19])-1];
		double yg3 = XYvalue[(int)round(pi.X[20])-1], yg4 = XYvalue[(int)round(pi.X[21])-1];
		
		// %% value initilized
		double c1 = sqrt(pow((xg1-xp1),2)+pow((yg1-yp1),2));
		double c2 = sqrt(pow((xg2-xp1),2)+pow((yg2-yp1),2));
		double c3 = sqrt(pow((xg3-xp1),2)+pow((yg3-yp1),2));
		double c4 = sqrt(pow((xg4-xp1),2)+pow((yg4-yp1),2));
		double CRmin = 1.4, dmin = 25.4, phi = 20.0*PI/180.0, W = 55.9, JR = 0.2, Km = 1.6, Ko = 1.5, Lmax = 127.0;
		double sigma_H = 3290.0, sigma_N = 2090.0, w1 = 5000.0, wmin = 245.0, wmax = 255.0, Cp = 464.0;

		// %% objective function
		double ret = PI / 1000.0*(b1*pow(c1,2)*(pow(Np1,2)+pow(Ng1,2))/pow((Np1+Ng1),2)+b2*pow(c2,2)*(pow(Np2,2)+pow(Ng2,2))/pow((Np2+Ng2),2)
			   + b3*pow(c3,2)*(pow(Np3,2)+pow(Ng3,2))/pow((Np3+Ng3),2)+b4*pow(c4,2)*(pow(Np4,2)+pow(Ng4,2))/pow((Np4+Ng4),2));

		return ret;
	}

	//public override bool CheckParticle(PSOTuple pi) 
    public override ConstractResult GetConstraintResult(PSOTuple pi)
	{
		double Np1 = round(pi.X[0]);
		double Ng1 = round(pi.X[1]);
		double Np2 = round(pi.X[2]);
		double Ng2 = round(pi.X[3]);
		double Np3 = round(pi.X[4]);
		double Ng3 = round(pi.X[5]);
		double Np4 = round(pi.X[6]);
		double Ng4 = round(pi.X[7]);
		double[] Pvalue = new double[] { 3.175, 5.715, 8.255, 12.7};
		double b1 = Pvalue[(int)round(pi.X[8])-1], b2 = Pvalue[(int)round(pi.X[9])-1], b3 = Pvalue[(int)round(pi.X[10])-1], b4 = Pvalue[(int)round(pi.X[11])-1];

		// x = round(x);
		// Np1 = x(:,1); Ng1 = x(:,2); Np2 = x(:,3); Ng2 = x(:,4);
		// Np3 = x(:,5); Ng3 = x(:,6); Np4 = x(:,7); Ng4 = x(:,8);
		// Pvalue = [ 3.175, 5.715, 8.255, 12.7];
		// b1 = Pvalue(x(:,9))'; b2 = Pvalue(x(:,10))'; b3 = Pvalue(x(:,11))';
		// b4 = Pvalue(x(:,12))';

		double[] XYvalue = new double[] { 12.7, 25.4, 38.1, 50.8, 63.5, 76.2, 88.9, 101.6, 114.3};
		double xp1 = XYvalue[(int)round(pi.X[12])-1], xg1 = XYvalue[(int)round(pi.X[13])-1];
		double xg2 = XYvalue[(int)round(pi.X[14])-1], xg3 = XYvalue[(int)round(pi.X[15])-1];
		double xg4 = XYvalue[(int)round(pi.X[16])-1], yp1 = XYvalue[(int)round(pi.X[17])-1];
		double yg1 = XYvalue[(int)round(pi.X[18])-1], yg2 = XYvalue[(int)round(pi.X[19])-1];
		double yg3 = XYvalue[(int)round(pi.X[20])-1], yg4 = XYvalue[(int)round(pi.X[21])-1];
		
		// %% value initilized
		double c1 = sqrt(pow((xg1-xp1),2)+pow((yg1-yp1),2));
		double c2 = sqrt(pow((xg2-xp1),2)+pow((yg2-yp1),2));
		double c3 = sqrt(pow((xg3-xp1),2)+pow((yg3-yp1),2));
		double c4 = sqrt(pow((xg4-xp1),2)+pow((yg4-yp1),2));
		
		double CRmin = 1.4, dmin = 25.4, phi = 20.0*PI/180.0, W = 55.9, JR = 0.2, Km = 1.6, Ko = 1.5, Lmax = 127.0;
		double sigma_H = 3290.0, sigma_N = 2090.0, w1 = 5000.0, wmin = 245.0, wmax = 255.0, Cp = 464.0;
		
		int gSize = 86;
		double[] g = new double[gSize];
		
		g[0] = (366000.0/(PI*w1)+2.0*c1*Np1/(Np1+Ng1))*(pow((Np1+Ng1),2)/(4.0*b1*pow(c1,2)*Np1))-sigma_N*JR/(0.0167*W*Ko*Km);
		g[1] = (366000.0*Ng1/(PI*w1*Np1)+2.0*c2*Np2/(Np2+Ng2))*(pow((Np2+Ng2),2)/(4.0*b2*pow(c2,2)*Np2))-sigma_N*JR/(0.0167*W*Ko*Km);
		g[2] = (366000.0*Ng1*Ng2/(PI*w1*Np1*Np2)+2.0*c3*Np3/(Np3+Ng3))*(pow((Np3+Ng3),2)/(4.0*b3*pow(c3,2)*Np3))-sigma_N*JR/(0.0167*W*Ko*Km);
		g[3] = (366000.0*Ng1*Ng2*Ng3/(PI*w1*Np1*Np2*Np3)+2.0*c4*Np4/(Np4+Ng4))*(pow((Np4+Ng4),2)/(4.0*b4*pow(c4,2)*Np4))-sigma_N*JR/(0.0167*W*Ko*Km);
		g[4] = (366000.0/(PI*w1)+2.0*c1*Np1/(Np1+Ng1))*(pow((Np1+Ng1),3)/(4.0*b1*pow(c1,2)*Ng1*pow(Np1,2)))-pow((sigma_H/Cp),2)*(sin(phi)*cos(phi))/(0.0334*W*Ko*Km);
		g[5] = (366000.0*Ng1/(PI*w1*Np1)+2.0*c2*Np2/(Np2+Ng2))*(pow((Np2+Ng2),3)/(4.0*b2*pow(c2,2)*Ng2*pow(Np2,2)))-pow((sigma_H/Cp),2)*(sin(phi)*cos(phi))/(0.0334*W*Ko*Km);
		g[6] = (366000.0*Ng1*Ng2/(PI*w1*Np1*Np2)+2.0*c3*Np3/(Np3+Ng3))*(pow((Np3+Ng3),3)/(4.0*b3*pow(c3,2)*Ng3*pow(Np3,2)))-pow((sigma_H/Cp),2)*(sin(phi)*cos(phi))/(0.0334*W*Ko*Km);
		g[7] = (366000.0*Ng1*Ng2*Ng3/(PI*w1*Np1*Np2*Np3)+2.0*c4*Np4/(Np4+Ng4))*(pow((Np4+Ng4),3)/(4.0*b4*pow(c4,2)*Ng4*pow(Np4,2)))-pow((sigma_H/Cp),2)*(sin(phi)*cos(phi))/(0.0334*W*Ko*Km);
		g[8] = CRmin*PI*cos(phi) - Np1*sqrt(pow(sin(phi),2)/4.0+1.0/Np1+pow((1.0/Np1),2))-Ng1*sqrt(pow(sin(phi),2)/4.0+1.0/Ng1+pow((1.0/Ng1),2))+sin(phi)*(Np1+Ng1)/2.0;
		g[9] = CRmin*PI*cos(phi) - Np2*sqrt(pow(sin(phi),2)/4.0+1.0/Np2+pow((1.0/Np2),2))-Ng2*sqrt(pow(sin(phi),2)/4.0+1.0/Ng2+pow((1.0/Ng2),2))+sin(phi)*(Np2+Ng2)/2.0;
		
		g[10] = CRmin*PI*cos(phi) - Np3*sqrt(pow(sin(phi),2)/4.0+1.0/Np3+pow((1.0/Np3),2))-Ng3*sqrt(pow(sin(phi),2)/4.0+1.0/Ng3+pow((1.0/Ng3),2))+sin(phi)*(Np3+Ng3)/2.0;
		g[11] = CRmin*PI*cos(phi) - Np4*sqrt(pow(sin(phi),2)/4.0+1.0/Np4+pow((1.0/Np4),2))-Ng4*sqrt(pow(sin(phi),2)/4.0+1.0/Ng4+pow((1.0/Ng4),2))+sin(phi)*(Np4+Ng4)/2.0;
		g[12] = dmin - 2.0*c1*Np1/(Np1+Ng1);
		g[13] = dmin - 2.0*c2*Np2/(Np2+Ng2);
		g[14] = dmin - 2.0*c3*Np3/(Np3+Ng3);
		g[15] = dmin - 2.0*c4*Np4/(Np4+Ng4);
		g[16] = dmin - 2.0*c1*Ng1/(Np1+Ng1);
		g[17] = dmin - 2.0*c2*Ng2/(Np2+Ng2);
		g[18] = dmin - 2.0*c3*Ng3/(Np3+Ng3);
		g[19] = dmin - 2.0*c4*Ng4/(Np4+Ng4); 
		
		g[20] = xp1 +((Np1+2)*c1/(Np1+Ng1))-Lmax;
		g[21] = xg2 +((Np2+2)*c2/(Np2+Ng2))-Lmax;
		g[22] = xg3 +((Np3+2)*c3/(Np3+Ng3))-Lmax;
		g[23] = xg4 +((Np4+2)*c4/(Np4+Ng4))-Lmax;
		g[24] = -xp1 +((Np1+2)*c1/(Np1+Ng1));
		g[25] = -xg2 +((Np2+2)*c2/(Np2+Ng2));
		g[26] = -xg3 +((Np3+2)*c3/(Np3+Ng3));
		g[27] = -xg4 +((Np4+2)*c4/(Np4+Ng4));
		g[28] = yp1 +((Np1+2)*c1/(Np1+Ng1))-Lmax;
		g[29] = yg2 +((Np2+2)*c2/(Np2+Ng2))-Lmax;
		
		g[30] = yg3 +((Np3+2)*c3/(Np3+Ng3))-Lmax;
		g[31] = yg4 +((Np4+2)*c4/(Np4+Ng4))-Lmax;
		g[32] = -yp1 +((Np1+2)*c1/(Np1+Ng1));
		g[33] = -yg2 +((Np2+2)*c2/(Np2+Ng2));
		g[34] = -yg3 +((Np3+2)*c3/(Np3+Ng3));
		g[35] = -yg4 +((Np4+2)*c4/(Np4+Ng4));
		g[36] = xg1 +((Ng1+2)*c1/(Np1+Ng1))-Lmax;
		g[37] = xg2 +((Ng2+2)*c2/(Np2+Ng2))-Lmax;
		g[38] = xg3 +((Ng3+2)*c3/(Np3+Ng3))-Lmax;
		g[39] = xg4 +((Ng4+2)*c4/(Np4+Ng4))-Lmax;

		g[40] = -xg1 +((Ng1+2)*c1/(Np1+Ng1));
		g[41] = -xg2 +((Ng2+2)*c2/(Np2+Ng2));
		g[42] = -xg3 +((Ng3+2)*c3/(Np3+Ng3));
		g[43] = -xg4 +((Ng4+2)*c4/(Np4+Ng4));
		g[44] =  yg1 +((Ng1+2)*c1/(Np1+Ng1))-Lmax;
		g[45] =  yg2 +((Ng2+2)*c2/(Np2+Ng2))-Lmax;
		g[46] =  yg3 +((Ng3+2)*c3/(Np3+Ng3))-Lmax;
		g[47] =  yg4 +((Ng4+2)*c4/(Np4+Ng4))-Lmax;
		g[48] =  -yg1 +((Ng1+2)*c1/(Np1+Ng1));
		g[49] =  -yg2 +((Ng2+2)*c2/(Np2+Ng2));
		
		g[50] = -yg3 +((Ng3+2)*c3/(Np3+Ng3));
		g[51] = -yg4 +((Ng4+2)*c4/(Np4+Ng4));
		g[52] = (0.945*c1-Np1-Ng1)*(b1-5.715)*(b1-8.255)*(b1-12.70)*(-1);
		g[53] = (0.945*c2-Np2-Ng2)*(b2-5.715)*(b2-8.255)*(b2-12.70)*(-1);
		g[54] = (0.945*c3-Np3-Ng3)*(b3-5.715)*(b3-8.255)*(b3-12.70)*(-1);
		g[55] = (0.945*c4-Np4-Ng4)*(b4-5.715)*(b4-8.255)*(b4-12.70)*(-1);
		g[56] = (0.646*c1-Np1-Ng1)*(b1-3.175)*(b1-8.255)*(b1-12.70)*(+1);
		g[57] = (0.646*c2-Np2-Ng2)*(b2-3.175)*(b2-8.255)*(b2-12.70)*(+1);
		g[58] = (0.646*c3-Np3-Ng3)*(b3-3.175)*(b3-8.255)*(b3-12.70)*(+1);
		g[59] = (0.646*c4-Np4-Ng4)*(b4-3.175)*(b4-8.255)*(b4-12.70)*(+1);

		g[60] = (0.504*c1-Np1-Ng1)*(b1-3.175)*(b1-5.715)*(b1-12.70)*(-1);
		g[61] = (0.504*c2-Np2-Ng2)*(b2-3.175)*(b2-5.715)*(b2-12.70)*(-1);
		g[62] = (0.504*c3-Np3-Ng3)*(b3-3.175)*(b3-5.715)*(b3-12.70)*(-1);
		g[63] = (0.504*c4-Np4-Ng4)*(b4-3.175)*(b4-5.715)*(b4-12.70)*(-1);
		g[64] = (0.0*c1-Np1-Ng1)*(b1-3.175)*(b1-5.715)*(b1-8.255)*(+1);
		g[65] = (0.0*c2-Np2-Ng2)*(b2-3.175)*(b2-5.715)*(b2-8.255)*(+1);
		g[66] = (0.0*c3-Np3-Ng3)*(b3-3.175)*(b3-5.715)*(b3-8.255)*(+1);
		g[67] = (0.0*c4-Np4-Ng4)*(b4-3.175)*(b4-5.715)*(b4-8.255)*(+1);
		g[68] = (-1.812*c1+Np1+Ng1)*(b1-5.715)*(b1-8.255)*(b1-12.70)*(-1);
		g[69] = (-1.812*c2+Np2+Ng2)*(b2-5.715)*(b2-8.255)*(b2-12.70)*(-1);
		
		g[70] = (-1.812*c3+Np3+Ng3)*(b3-5.715)*(b3-8.255)*(b3-12.70)*(-1);
		g[71] = (-1.812*c4+Np4+Ng4)*(b4-5.715)*(b4-8.255)*(b4-12.70)*(-1);
		g[72] = (-0.945*c1+Np1+Ng1)*(b1-3.175)*(b1-8.255)*(b1-12.70)*(+1);
		g[73] = (-0.945*c2+Np2+Ng2)*(b2-3.175)*(b2-8.255)*(b2-12.70)*(+1);
		g[74] = (-0.945*c3+Np3+Ng3)*(b3-3.175)*(b3-8.255)*(b3-12.70)*(+1);
		g[75] = (-0.945*c4+Np4+Ng4)*(b4-3.175)*(b4-8.255)*(b4-12.70)*(+1);
		g[76] = (-0.646*c1+Np1+Ng1)*(b1-3.175)*(b1-5.715)*(b1-12.70)*(-1);
		g[77] = (-0.646*c2+Np2+Ng2)*(b2-3.175)*(b2-5.715)*(b2-12.70)*(-1);
		g[78] = (-0.646*c3+Np2+Ng3)*(b3-3.175)*(b3-5.715)*(b3-12.70)*(-1);
		g[79] = (-0.646*c4+Np3+Ng4)*(b4-3.175)*(b4-5.715)*(b4-12.70)*(-1);
		
		g[80] = (-0.504*c1+Np1+Ng1)*(b1-3.175)*(b1-5.715)*(b1-8.255)*(+1);
		g[81] =  (-0.504*c2+Np2+Ng2)*(b2-3.175)*(b2-5.715)*(b2-8.255)*(+1);
		g[82] =  (-0.504*c3+Np3+Ng3)*(b3-3.175)*(b3-5.715)*(b3-8.255)*(+1);
		g[83] =  (-0.504*c4+Np4+Ng4)*(b4-3.175)*(b4-5.715)*(b4-8.255)*(+1);
        g[84] = wmin - w1 * (Np1 * Np2 * Np3 * Np4) / (Ng1 * Ng2 * Ng3 * Ng4);
        g[85] = -wmax + w1 * (Np1 * Np2 * Np3 * Np4) / (Ng1 * Ng2 * Ng3 * Ng4);

		for(int i=0; i<gSize; i++)
            if (double.IsInfinity(g[i])) g[i] = 1e6;

		//for(int i=0; i<gSize; i++)
		//	cout << g[i] << endl;

        return new ConstractResult(g, null);
		//for(int i=0; i<gSize; i++)
		//	if(g[i] > 0) return false;		

		// for(int i=0; i<86; i++) {
			// if(g[i] > 1e-6) {
				// return false;
			// }
		// }

		// for(int i=0; i<22; i++)
			// cout << pi.X[i] << ", ";
		// cout << endl;
		
		// for(int i=0; i<86; i++) {
			// if(g[i] > 1e-6) {
				// cout << "Err g" << i << ": " << g[i] << endl;
				// return false;
			// }
		// }
		
		//return true;
	}

};
