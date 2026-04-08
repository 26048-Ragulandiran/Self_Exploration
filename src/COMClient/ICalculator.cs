using System;
using System.Runtime.InteropServices;

[Guid("11111111-1111-1111-1111-111111111111")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
interface ICalculator
{
    int Add(int a, int b);
    int Multiply(int a, int b);
}
