using System;
using PSOLib;

public class RC27_10barTruss: Problem
{
    public override String name()
	{
		return "RC27_10barTruss";
	}

	int dims = 10;
    public RC27_10barTruss()
    {
		double[] x_l= new double[10];
        double[] x_u = new double[10];
        for (int i = 0; i < dims; i++)
        {
			x_l[i] = 0.00006450;
			x_u[i] = 0.0050;
		}		
		setDims(x_u, x_l);
    }

	double function_fitness(double[] section) {
	// function [Weight] = function_fitness(section)
		double E   = 6.98*1e10;
		//double A   = section;
		double rho = 2770;
	// %--------------------------------------------------------------------------
	// %           1         2       3       4       5     6                     
		double[] gcoord = new double[] {18.288,  18.288,  9.144,  9.144, 0, 
					       0, 9.144, 0, 9.144, 0, 9.144, 0};
		//double gcoord[] = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12};
        int[] element = new int[]{3, 1, 4, 2, 3, 1, 4, 3, 2, 1,
						 5, 3, 6, 4, 4, 2, 5, 6, 3, 4};
	// %--------------------------------------------------------------------------
	// % calculate Weight matrix
		double Weight = 0;
		double x1, x2, y1, y2;
		for(int i=0; i<10; i++) { // i=1:length(element)
			// double nd = element(:,i);
			// double x  = gcoord(1,nd), y = gcoord(2,nd);
			x1 = gcoord[element[i]-1]; 
			x2 = gcoord[element[i+10]-1];
			// cout << x1 << " X " << x2 << endl;
			y1 = gcoord[element[i]+6-1]; 
			y2 = gcoord[element[i+10]+6-1];
			//  cout << y1 << " Y " << y2 << endl;

			double le = sqrt(pow((x2-x1),2) + pow((y2-y1),2));
			//cout << rho << " le " << le << " A(i) " << section[i] << endl;
			Weight =  Weight + rho * le * section[i];
		};
	// end
		//cout << "Weight: " << Weight << endl;
		return Weight;
	}

	public override double GetFitness(PSOTuple pi)
	{
		return function_fitness(pi.X);
	}

	//public override bool CheckParticle(PSOTuple pi) 
    public override ConstractResult GetConstraintResult(PSOTuple pi)
	{
		// type = '2D';
		double E    = 6.98*1e10;
		double rho  = 2770;
		// %--------------------------------------------------------------------------
		// %           1        2        3       4       5     6          
		double[] gcoord  = {18.288,  18.288,  9.144,  9.144,      0,  0,
				   9.144,       0,  9.144,      0,  9.144,  0};
		// %          1  2  3  4  5  6  7  8  9  10
		double[] element = {3, 1, 4, 2, 3, 1, 4, 3, 2, 1,
				   5, 3, 6, 4, 4, 2, 5, 6, 3, 4};
		// nel     = length(element);    % total element
		// nnode   = length(gcoord);     % total node
		// ndof    = 2;                  % number of degree of freedom of one node
		// sdof    = nnode*ndof;         % total dgree of freedom of system
		// % plotModel( type,gcoord,element );
		// % calculate stiffness matrix 
		
		// [ K,M ] = Cal_K_and_M( type,gcoord,element,A,rho,E );
		// % add non-structural mass
		double addedMass = 454; // %kg
		// for idof = 1:sdof
			// M(idof,idof) = M(idof,idof) + addedMass;
		// end
		// % apply boundary
		// bcdof   = [(5:6)*2-1, (5:6)*2];     % boundary condition displacement
		// % Giai phuong trinh tim tri rieng va vector rieng
		// [omega_2,~]=eigens(K,M,bcdof); 
		// f=sqrt(omega_2)/2/pi;
		// % f(1:5)
		
		int gSize = 3;
		double[] g = new double [gSize];

        g[0] = 7 / (sqrt(1.0) / 2.0 / PI) - 1;
        g[1] = 15 / (sqrt(2.0) / 2.0 / PI) - 1;
        g[2] = 20 / (sqrt(3.0) / 2.0 / PI) - 1;

		//for(int i=0; i<gSize; i++)
		//	cout << g[i] << endl;

        return new ConstractResult(g, null);
        //for(int i=0; i<gSize; i++)
        //    if(g[i] > 0) return false;		
		
        //return true;
	}

};
