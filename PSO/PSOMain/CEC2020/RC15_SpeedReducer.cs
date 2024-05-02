using System;
using PSOLib;

public class RC15_SpeedReducer: Problem
{
    public override String name()
	{
		return "RC15_SpeedReducer";
	}

    public RC15_SpeedReducer()
    {
		// xmin15   = [2.6, 0.7, 17, 7.3, 7.3, 2.9, 5];
		// xmax15   = [3.6, 0.8, 28, 8.3, 8.3, 3.9, 5.5];
		
		x_l = new double[] { 2.6, 0.7, 17, 7.3, 7.3, 2.9, 5.0 };
		x_u = new double[] { 3.6, 0.8, 28, 8.3, 8.3, 3.9, 5.5 };
		setDims(x_u, x_l);
    }

    //public override bool CheckParticle(PSOTuple pi) 
    //{
    //    double x1 = pi.X[0];
    //    double x2 = pi.X[1];
    //    double x3 = pi.X[2];
    //    double x4 = pi.X[3];
    //    double x5 = pi.X[4];
    //    double x6 = pi.X[5];
    //    double x7 = pi.X[6];

    //    int gSize = 11;
    //    double[] g = new double[gSize];

    //    // g(:,1) = -x(:,1).*x(:,2).^2.*x(:,3)+27;
    //    // g(:,2) = -x(:,1).*x(:,2).^2.*x(:,3).^2+397.5;
    //    // g(:,3) = -x(:,2).*x(:,6).^4.*x(:,3).*x(:,4).^(-3)+1.93;
    //    // g(:,4) = -x(:,2).*x(:,7).^4.*x(:,3)./x(:,5).^3+1.93;
    //    // g(:,5) = 10.*x(:,6).^(-3).*sqrt(16.91.*10^6+(745.*x(:,4)./(x(:,2).*x(:,3))).^2)-1100;
    //    // g(:,6) = 10.*x(:,7).^(-3).*sqrt(157.5.*10^6+(745.*x(:,5)./(x(:,2).*x(:,3))).^2)-850;
    //    // g(:,7) = x(:,2).*x(:,3)-40;
    //    // g(:,8) = -x(:,1)./x(:,2)+5;
    //    // g(:,9) = x(:,1)./x(:,2)-12;
    //    // g(:,10) = 1.5.*x(:,6)-x(:,4)+1.9;
    //    // g(:,11) = 1.1.*x(:,7)-x(:,5)+1.9;		

    //    //計算限制式
    //    g[0] = -x1 * x2 * x2 * x3 + 27.0;
    //    g[1] = -x1 * x2 * x2 * x3 * x3 + 397.5;
    //    g[2] = -x2 * pow(x6,4) * x3 * pow(x4,-3) + 1.93;
    //    g[3] = -x2 * pow(x7,4) * x3 * pow(x5,-3) + 1.93;
    //    g[4] = 10.0 * pow(x6,-3) * sqrt(16.91 * pow(10.0,6) + pow(745.0*x4*pow(x2,-1)*pow(x3,-1),2)) - 1100.0;
    //    g[5] = 10.0 * pow(x7,-3) * sqrt(157.5 * pow(10.0,6) + pow(745.0*x5*pow(x2,-1)*pow(x3,-1),2)) - 850.0;
    //    g[6] = x2 * x3 - 40.0;
    //    g[7] = -x1 * pow(x2, -1) + 5.0;
    //    g[8] = x1 * pow(x2, -1) - 12.0;
    //    g[9] = 1.5 * x6 - x4 + 1.9;
    //    g[10] = 1.1 * x7 - x5 + 1.9;
		
    //    for(int i=0; i<gSize; i++)
    //        if(g[i] > 0) return false;

    //    return true;
    //}

    public override ConstractResult GetConstraintResult(PSOTuple pi)
    {
        double x1 = pi.X[0];
        double x2 = pi.X[1];
        double x3 = pi.X[2];
        double x4 = pi.X[3];
        double x5 = pi.X[4];
        double x6 = pi.X[5];
        double x7 = pi.X[6];

        int gSize = 11;
        double[] g = new double[gSize];

        // g(:,1) = -x(:,1).*x(:,2).^2.*x(:,3)+27;
        // g(:,2) = -x(:,1).*x(:,2).^2.*x(:,3).^2+397.5;
        // g(:,3) = -x(:,2).*x(:,6).^4.*x(:,3).*x(:,4).^(-3)+1.93;
        // g(:,4) = -x(:,2).*x(:,7).^4.*x(:,3)./x(:,5).^3+1.93;
        // g(:,5) = 10.*x(:,6).^(-3).*sqrt(16.91.*10^6+(745.*x(:,4)./(x(:,2).*x(:,3))).^2)-1100;
        // g(:,6) = 10.*x(:,7).^(-3).*sqrt(157.5.*10^6+(745.*x(:,5)./(x(:,2).*x(:,3))).^2)-850;
        // g(:,7) = x(:,2).*x(:,3)-40;
        // g(:,8) = -x(:,1)./x(:,2)+5;
        // g(:,9) = x(:,1)./x(:,2)-12;
        // g(:,10) = 1.5.*x(:,6)-x(:,4)+1.9;
        // g(:,11) = 1.1.*x(:,7)-x(:,5)+1.9;		

        //計算限制式
        g[0] = -x1 * x2 * x2 * x3 + 27.0;
        g[1] = -x1 * x2 * x2 * x3 * x3 + 397.5;
        g[2] = -x2 * pow(x6, 4) * x3 * pow(x4, -3) + 1.93;
        g[3] = -x2 * pow(x7, 4) * x3 * pow(x5, -3) + 1.93;
        g[4] = 10.0 * pow(x6, -3) * sqrt(16.91 * pow(10.0, 6) + pow(745.0 * x4 * pow(x2, -1) * pow(x3, -1), 2)) - 1100.0;
        g[5] = 10.0 * pow(x7, -3) * sqrt(157.5 * pow(10.0, 6) + pow(745.0 * x5 * pow(x2, -1) * pow(x3, -1), 2)) - 850.0;
        g[6] = x2 * x3 - 40.0;
        g[7] = -x1 * pow(x2, -1) + 5.0;
        g[8] = x1 * pow(x2, -1) - 12.0;
        g[9] = 1.5 * x6 - x4 + 1.9;
        g[10] = 1.1 * x7 - x5 + 1.9; 
        
        return new ConstractResult(g, null);
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
		
		// f = 0.7854*x(:,1).*x(:,2).^2.*(3.3333.*x(:,3).^2+14.9334.*x(:,3)-43.0934)-1.508.*x(:,1).*(x(:,6).^2+x(:,7).^2).....
		//     +7.477.*(x(:,6).^3+x(:,7).^3)+0.7854.*(x(:,4).*x(:,6).^2+x(:,5).*x(:,7).^2);

		return 0.7854*x1*pow(x2,2)*(3.3333*pow(x3,2)+14.9334*x3-43.0934)-1.508*x1*(pow(x6,2)+pow(x7,2))
			   +7.477*(pow(x6,3)+pow(x7,3))+0.7854*(x4*pow(x6,2)+x5*pow(x7,2));

		// return 0.7854 * x2 * x2 * x1 * (14.9334 * x3 - 43.0934 + 3.3333 * x3 * x3)
			   // + 0.7854 * (x5 * x7 * x7 + x4 * x6 * x6)
			   // - 1.508 * x1 * (x7 * x7 + x6 * x6)
			   // + 7.477 * (x7 * x7 * x7 + x6 * x6 * x6);
	}

};
