using System;
using PSOLib;

public class RC16_IndustRefrig: Problem
{
    public override String name()
	{
		return "RC16_IndustRefrig";
	}

    public RC16_IndustRefrig()
    {
		int dims = 14;
		x_u = new double[dims];
		x_l = new double[dims];
		for(int i=0; i<dims; i++) {
			x_u[i] = 5.0;
			x_l[i] = 0.001;
		}		
		setDims(x_u, x_l);
    }

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
		double x11 = pi.X[10];
		double x12 = pi.X[11];
		double x13 = pi.X[12];
		double x14 = pi.X[13];

		int gSize = 15;
		double[] g = new double[gSize];

		// g(:,1) = 1.524.*x(:,7).^(-1)-1;
		// g(:,2) = 1.524.*x(:,8).^(-1)-1;
		// g(:,3) = 0.07789.*x(:,1)-2.*x(:,7).^(-1).*x(:,9)-1;
		// g(:,4) = 7.05305.*x(:,9).^(-1).*x(:,1).^2.*x(:,10).*x(:,8).^(-1).*x(:,2).^(-1).*x(:,14).^(-1)-1;
		// g(:,5) = 0.0833./x(:,13).*x(:,14)-1;
		// g(:,6) = 0.04771.*x(:,10).*x(:,8).^1.8812.*x(:,12).^0.3424-1;
		// g(:,7) = 0.0488.*x(:,9).*x(:,7).^1.893.*x(:,11).^0.316-1;
		// g(:,8) = 0.0099.*x(:,1)./x(:,3)-1;
		// g(:,9) = 0.0193.*x(:,2)./x(:,4)-1;
		// g(:,10) = 0.0298.*x(:,1)./x(:,5)-1;

		// g(:,11) = 47.136.*x(:,2).^0.333./x(:,10).*x(:,12)-1.333.*x(:,8).*x(:,13).^2.1195+62.08.*x(:,13).^2.1195.*x(:,8).^0.2./(x(:,12).*x(:,10))-1;
		// g(:,12) = 0.056.*x(:,2)./x(:,6)-1;
		// g(:,13) = 2./x(:,9)-1;
		// g(:,14) = 2./x(:,10)-1;
		// g(:,15) = x(:,12)./x(:,11)-1;

		//計算限制式
		g[0] = 1.524 * pow(x7,(-1))-1;
		g[1] = 1.524 * pow(x8,(-1))-1;
		g[2] = 0.07789 * x1-2 * pow(x7,(-1)) * x9-1;
		g[3] = 7.05305 * pow(x9,(-1)) * pow(x1,2) * x10 * pow(x8,(-1)) * pow(x2,(-1)) * pow(x14,(-1))-1;
		g[4] = 0.0833 / x13 * x14-1;
		g[5] = 0.04771 * x10 * pow(x8,1.8812) * pow(x12,0.3424)-1;
		g[6] = 0.0488 * x9 * pow(x7,1.893) * pow(x11,0.316)-1;
		g[7] = 0.0099 * x1 / x3-1;
		g[8] = 0.0193 * x2 / x4-1;
		g[9] = 0.0298 * x1 / x5-1;

		g[10] = 47.136 * pow(x2,0.333) / x10 * x12-1.333 * x8 * pow(x13,2.1195)+62.08 * pow(x13,2.1195) * pow(x8,0.2) / (x12 * x10)-1;
		g[11] = 0.056 * x2 / x6-1;
		g[12] = 2 / x9-1;
		g[13] = 2 / x10-1;
		g[14] = x12 / x11-1;

		// for(int i=0; i<gSize; i++)
			// cout << g[i] << endl;

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
		double x4 = pi.X[3];
		double x5 = pi.X[4];
		double x6 = pi.X[5];
		double x7 = pi.X[6];
		double x8 = pi.X[7];
		double x9 = pi.X[8];
		double x10 = pi.X[9];
		double x11 = pi.X[10];
		double x12 = pi.X[11];
		double x13 = pi.X[12];
		double x14 = pi.X[13];
		
		// f = 63098.88.*x(:,2).*x(:,4).*x(:,12)+5441.5.*x(:,2).^2.*x(:,12)+115055.5.*x(:,2).^1.664.*x(:,6).....
		//     +6172.27.*x(:,2).^2.*x(:,6)+63098.88.*x(:,1).*x(:,3).*x(:,11)+5441.5.*x(:,1).^2.*x(:,11).....
		//     +115055.5.*x(:,1).^1.664.*x(:,5)+6172.27.*x(:,1).^2.*x(:,5)+140.53.*x(:,1).*x(:,11)+281.29.*x(:,3).*x(:,11)....
		//     +70.26.*x(:,1).^2+281.29.*x(:,1).*x(:,3)+281.29.*x(:,3).^2+14437.*x(:,8).^1.8812.*x(:,12).^0.3424....
		//     .*x(:,10).*x(:,14).^(-1).*x(:,1).^2.*x(:,7).*x(:,9).^(-1)+20470.2.*x(:,7).^(2.893).*x(:,11).^0.316.*x(:,1).^2;

		return 63098.88 * x2 * x4 * x12 + 5441.5 * x2 * x2 * x12 + 115055.5 * pow(x2, 1.664) * x6 + 6172.27 * x2 * x2 * x6
			   + 63098.88 * x1 * x3 * x11 + 5441.5 * x1 * x1 * x11 + 115055.5 * pow(x1, 1.664) * x5 + 6172.27 * x1 * x1 * x5
			   + 140.53 * x1 * x11 + 281.29 * x3 * x11 + 70.26 * x1 * x1 + 281.29 * x1 * x3 + 281.29 * x3 * x3
			   + 14437.0 * pow(x8, 1.8812) * pow(x12, 0.3424) * x10 * pow(x14, -1) * x1 * x1 * x7 * pow(x9, -1)
			   + 20470.2 * pow(x7, 2.893) * pow(x11, 0.316) * x1 * x1;
	}

};
