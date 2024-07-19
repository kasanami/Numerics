// See https://aka.ms/new-console-template for more information
using Ksnm.Numerics;
using System.Numerics;
using Float16 = System.Half;
using Float32 = float;
using Float64 = double;

Console.WriteLine();
{
    Console.WriteLine("Exp");
    Console.WriteLine("decimal");
    Console.WriteLine("base:2");
    for (decimal i = 1; i <= 10; i += 0.1m)
    {
        var p = Ksnm.Math.Exp(i);
        var p2 = System.Math.Exp((double)i);
        Console.WriteLine($"exp={i}\n{p}\n{p2}");
    }

    Console.WriteLine("Log");
    Console.WriteLine("decimal");
    for (decimal i = 1; i <= 10; i += 0.1m)
    {
        var p = Ksnm.Math.NewtonRaphsonLog(i, 0.00000_00000_00000_00000_000001m);
        var p2 = System.Math.Log((double)i);
        Console.WriteLine($"value={i}\n{p}\n{p2}");
    }

    Console.WriteLine("Pow");
    Console.WriteLine("decimal");
    Console.WriteLine("base:2");
    for (decimal i = 1; i <= 10; i += 0.1m)
    {
        var p = Ksnm.Math.Pow(2m, i);
        var p2 = System.Math.Pow(2, (double)i);
        Console.WriteLine($"exp={i}\n{p}\n{p2}");
    }
    Console.WriteLine("base:10");
    for (decimal i = 1; i <= 10; i += 0.1m)
    {
        var p = Ksnm.Math.Pow(10, i);
        var p2 = System.Math.Pow(10, (double)i);
        Console.WriteLine($"exp={i}\n{p}\n{p2}");
    }

    Console.WriteLine("BigDecimal");
    Console.WriteLine("base:2");
    for (decimal i = 1; i <= 10; i += 0.1m)
    {
        var p = BigDecimal.Pow(2, i, 100);
        var p2 = System.Math.Pow(2, (double)i);
        Console.WriteLine($"exp={i}\n{p}\n{p2}");
    }
    Console.WriteLine("base:10");
    for (decimal i = 1; i <= 10; i += 0.1m)
    {
        var p = BigDecimal.Pow(10, i, 100);
        var p2 = System.Math.Pow(10, (double)i);
        Console.WriteLine($"exp={i}\n{p}\n{p2}");
    }
}

Console.WriteLine();
Console.WriteLine("素数の計算");

for (int i = 1; i < 100; i++)
{
    var p = Ksnm.Math.Prime(i);
    Console.WriteLine($"{i}:{p}");
}

#if false

Console.WriteLine("√の計算");

for (int i = 0; i < 10; i++)
{
    var result = Ksnm.Numerics.Math.Sqrt<BigFraction>(2, i);
    var resultD = (BigDecimal)result;
    Console.WriteLine($"{i}:\n√{2}\n={result}\n={resultD}");
}

#endif

Console.WriteLine();
Console.WriteLine("定数のネイピア数");
{
    Console.WriteLine($"{nameof(Float16)}={Float16.E.ToString()}");
    Console.WriteLine($"{nameof(Float32)}={Float32.E.ToString()}");
    Console.WriteLine($"{nameof(Float64)}={Float64.E.ToString()}");
    var bigDecimal = (BigDecimal)BigFraction.E;
    Console.WriteLine($"{nameof(BigFraction)}={bigDecimal.ToString()}");
}
Console.WriteLine("定数の円周率");
{
    Console.WriteLine($"{nameof(Float16)}={Float16.Pi.ToString()}");
    Console.WriteLine($"{nameof(Float32)}={Float32.Pi.ToString()}");
    Console.WriteLine($"{nameof(Float64)}={Float64.Pi.ToString()}");
    var bigDecimal = (BigDecimal)BigFraction.Pi;
    Console.WriteLine($"{nameof(BigFraction)}={bigDecimal.ToString()}");
}
{
    Console.WriteLine($"{nameof(Float16)}={Float16.Tau.ToString()}");
    Console.WriteLine($"{nameof(Float32)}={Float32.Tau.ToString()}");
    Console.WriteLine($"{nameof(Float64)}={Float64.Tau.ToString()}");
    var bigDecimal = (BigDecimal)BigFraction.Tau;
    Console.WriteLine($"{nameof(BigFraction)}={bigDecimal.ToString()}");
}

Console.WriteLine();
Console.WriteLine("円周率の計算");

Console.WriteLine("マチンの公式");

for (int i = 0; i < 10; i++)
{
    var result = Ksnm.Math.MachinsFormula<BigFraction>(i) * 4;
    var resultD = (BigDecimal)result;
    Console.WriteLine($"{i}:PI\n={result.ToString()}\n={resultD}");
}


#if false
BigDecimal a = BigDecimal.Parse("4951760157141521");
BigDecimal b = BigDecimal.Parse("4951760157141521099596496896");

var c = a / b;
Console.WriteLine($"{a}/{b}={c}");

a.MinExponent = -50;
c = a / b;
Console.WriteLine($"{a}/{b}={c}");

a.MinExponent = -100;
c = a / b;
Console.WriteLine($"{a}/{b}={c}");
#endif