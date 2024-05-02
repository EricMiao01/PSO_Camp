using System;
using PSOLib;

public class Rosenbrock : Problem
{
    public override String name()
    {
        return "Rosenbrock";
    }

    public Rosenbrock()
    {
		x_u = new double[] {1.5, 2.5};
        x_l = new double[] {-1.5, -0.5};
        // setDims(sizeof(x_u)/sizeof(x_u[0]), x_u, x_l);
        setDims(x_u, x_l);
    }

	public override double GetFitness(PSOTuple pi) 
	{
		double x1 = pi.X[0];
		double x2 = pi.X[1];
		
		return (1-x1) * (1-x1) + 100 * (x2 - x1 * x1) * (x2 - x1 * x1);
	}

    // public override bool CheckParticle(PSOTuple pi) 
    public override ConstractResult GetConstraintResult(PSOTuple pi)
	{
		double x1 = pi.X[0];
		double x2 = pi.X[1];
		double g1 = (x1 - 1) * (x1 - 1) * (x1 - 1) - x2 + 1;
		double g2 = (x1 + x2 - 2);
        double[] g = new double[2];
        g[0] = g1;
        g[1] = g2;
        
        return new ConstractResult(g, null);
		//return g1 <= 0 && g2 <=0;
	}

};
