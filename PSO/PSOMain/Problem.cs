using PSOLib;
using System;

public class Problem
{
	int dims = -1;
	public double[] x_u;
    public double[] x_l;
    public double[] v_u;
    public double[] v_l;
	
	int EvalueTimes = 0;
	public int MaxFES = 100000;
	double Fitness = 1000000000000;
	double[] logFES = new double[10];
	public const double PI = 3.141592653589793;

    public virtual double GetFitness(PSOTuple pi)
	{
		return 0;
	}

    public virtual ConstractResult GetConstraintResult(PSOTuple pi)
    {
        return new ConstractResult(null, null);
    }

	double Evaluate(PSOTuple pi)
	{
		EvalueTimes++;
		double ret = GetFitness(pi);
		if (Fitness > ret) Fitness = ret;
		
		return ret;
	}

	public virtual String name()
	{
		return "Undefined";
	}

    public void setDims(double[] ub, double[] lb)
	{
        dims = lb.Length;
        x_u = new double[dims];
		x_l = new double[dims];
        v_u = new double[dims];
        v_l = new double[dims];
        for (int i = 0; i < dims; i++)
		{
			x_u[i] = ub[i];
			x_l[i] = lb[i];
            v_u[i] = (ub[i] - lb[i]) / 10;
            v_l[i] = v_u[i] * -1;
        }
		if(dims > 10) MaxFES = 200000;
		if(dims > 30) MaxFES = 400000;
		if(dims > 50) MaxFES = 800000;
		if(dims > 150) MaxFES = 1000000;
	}	

    public Problem()
    {
        Console.WriteLine("*** Problem Constructor ***");
    }

	~Problem()
    {
        Console.WriteLine("*** Problem Destructor ***");
        if (dims > 0)
		{
            x_u = null;
            x_l = null;
            // delete x_u;
            // delete x_l;
		}
    }

    public static double pow(double x, double y)
    {
        return Math.Pow(x, y);
    }

    public static double sqrt(double d)
    {
        return Math.Sqrt(d);
    }

    public static int round(double d)
    {
        return (int)Math.Round(d);
    }

    public static double abs(double d)
    {
        return Math.Abs(d);
    }

    public static double sin(double d)
    {
        return Math.Sin(d);
    }

    public static double cos(double d)
    {
        return Math.Cos(d);
    }

    public static double acos(double d)
    {
        return Math.Acos(d);
    }

    public static double asin(double d)
    {
        return Math.Asin(d);
    }

    public static double log(double d)
    {
        return Math.Log(d);
    }

    public static double log10(double d)
    {
        return Math.Log10(d);
    }

    public static double exp(double d)
    {
        return Math.Exp(d);
    }

    public static double atan(double d)
    {
        return Math.Atan(d);
    }

    public static double remainder(double d1, double d2)
    {
        decimal ret = decimal.Remainder((decimal)d1, (decimal)d2);
        return (double)ret;
    }    

};
