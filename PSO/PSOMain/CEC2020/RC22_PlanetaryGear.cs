using System;
using PSOLib;

public class RC22_PlanetaryGear: Problem
{
    public override String name()
	{
		return "RC22_PlanetaryGear";
	}

    public RC22_PlanetaryGear()
    {
		// xmin22   = [16.51,13.51,13.51,16.51,13.51,47.51,0.51,0.51,0.51];
		// xmax22   = [96.49,54.49,51.49,46.49,51.49,124.49,3.49,6.49,6.49];
		x_l = new double[] {16.51,13.51,13.51,16.51,13.51,47.51,0.51,0.51,0.51};
		x_u = new double[] {96.49,54.49,51.49,46.49,51.49,124.49,3.49,6.49,6.49};
		setDims(x_u, x_l);
    }

	public override double GetFitness(PSOTuple pi)
	{
		int x1 = round(abs(pi.X[0]));
		int x2 = round(abs(pi.X[1]));
		int x3 = round(abs(pi.X[2]));
		int x4 = round(abs(pi.X[3]));
		int x5 = round(abs(pi.X[4]));
		int x6 = round(abs(pi.X[5]));
		int x7 = round(abs(pi.X[6]));
		int x8 = round(abs(pi.X[7]));
		int x9 = round(abs(pi.X[8]));
		
		// x = round(abs(x)); Pind = [3,4,5]; mind = [ 1.75, 2, 2.25, 2.5, 2.75, 3.0];
        double[] Pind = new double[] { 3.0, 4.0, 5.0 }; double[] mind = new double[] { 1.75, 2.0, 2.25, 2.5, 2.75, 3.0 };
		double N1 = x1; double N2 = x2; double N3 = x3; double N4 = x4; double N5 = x5; double N6 = x6;
		double p  = Pind[x7-1]; double m1 = mind[x8-1]; double m2 = mind[x9-1];
		// %% objective function
		double i1 = N6 / N4; double i01 = 3.11;
		double i2 = N6 * (N1 * N3 + N2 * N4) / (N1 * N3 * (N6 - N4)); double i02 = 1.84;
		double iR = -(N2 * N6 / (N1 * N3)); double i0R = -3.11;
		
		double ret = i1-i01;
		if (ret < i2-i02) ret = i2-i02;
		if (ret < iR-i0R) ret = iR-i0R;
		return ret;
	}

	//public override bool CheckParticle(PSOTuple pi) 
    public override ConstractResult GetConstraintResult(PSOTuple pi)
	{
		int x1 = round(abs(pi.X[0]));
		int x2 = round(abs(pi.X[1]));
		int x3 = round(abs(pi.X[2]));
		int x4 = round(abs(pi.X[3]));
		int x5 = round(abs(pi.X[4]));
		int x6 = round(abs(pi.X[5]));
		int x7 = round(abs(pi.X[6]));
		int x8 = round(abs(pi.X[7]));
		int x9 = round(abs(pi.X[8]));

		double[] Pind = new double[] {3.0, 4.0, 5.0}; double[] mind = new double[] {1.75, 2.0, 2.25, 2.5, 2.75, 3.0};
		double N1 = x1; double N2 = x2; double N3 = x3; double N4 = x4; double N5 = x5; double N6 = x6;
		double p  = Pind[x7-1]; double m1 = mind[x8-1]; double m2 = mind[x9-1];
		
		int gSize = 10;
        double[] g = new double[gSize];
		
		double Dmax = 220.0, dlt22 = 0.5, dlt33 = 0.5, dlt55 = 0.5, dlt35 = 0.5, dlt34 = 0.5, dlt56 = 0.5;
		double beta = acos((pow((N6-N3),2.0) + pow((N4+N5),2.0) - pow((N3+N5),2.0)) / (2.0 * (N6-N3) * (N4+N5) ) );
		g[0] = m2 * (N6+2.5) - Dmax;
		g[1] = m1 * (N1 + N2) + m1 * (N2 + 2.0) - Dmax;
		g[2] = m2 * (N4 + N5) + m2 * (N5 + 2.0) - Dmax;
		g[3] = abs(m1 * (N1+N2) - m2 * (N6-N3))-m1-m2;
		g[4] = -((N1+N2)*sin(PI/p)-N2-2.0-dlt22);
		g[5] = -((N6-N3)*sin(PI/p)-N3-2.0-dlt33);
		g[6] = -((N4+N5)*sin(PI/p)-N5-2.0-dlt55);
		g[7] = pow((N3+N5+2.0+dlt35),2.0)-(pow((N6-N3),2.0)+pow((N4+N5),2.0)-2.0*(N6-N3)*(N4+N5)*cos(2.0*PI/p-beta));
		// else
		   // g(:,8) = 1e6;
		// end
		g[8] = -(N6-2.0*N3-N4-4.0-2.0*dlt34);
		g[9] = -(N6-N4-2.0*N5-4.0-2.0*dlt56);

		//for(int i=0; i<gSize; i++)
		//	cout << g[i] << endl;

        double[] h = new double[1];
		double h1 = remainder((N6-N4), p);
		if(h1<0) h1+=p;
        h[0] = h1;

        return new ConstractResult(g, h);


		// cout << "h:" << h1 << endl;
		// cout << N6 <<" "<< N4 << " "<< N6 - N4 <<" "<< p << endl;

        //for(int i=0; i<gSize; i++)
        //    if(g[i] > 0) return false;
		
        //if (h1!=0) return false;
				
        //return true;
	}

};
