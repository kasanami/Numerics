// See https://aka.ms/new-console-template for more information
using Ksnm.Numerics;
using System.Numerics;

Console.WriteLine("Sqrt");

for (int i = 0; i < 10; i++)
{
    var result = Ksnm.Numerics.Math.Sqrt<BigFraction>(2, i);
    var resultD = (BigDecimal)result;
    Console.WriteLine($"{i}:\n√{2}\n={result}\n={resultD}");
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