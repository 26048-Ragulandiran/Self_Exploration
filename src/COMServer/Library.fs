namespace FSharpComServer
 
open System
open System.Runtime.InteropServices
 
// Interface GUID
[<Guid("11111111-1111-1111-1111-111111111111")>]
[<InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>]
type ICalculator =
    abstract Add : int * int -> int
    abstract Multiply : int * int -> int
 
// Class GUID
[<Guid("22222222-2222-2222-2222-222222222222")>]
[<ClassInterface(ClassInterfaceType.None)>]
[<ComVisible(true)>]
type Calculator() =
    interface ICalculator with
        member _.Add(a, b) = a + b
        member _.Multiply(a, b) = a * b
 