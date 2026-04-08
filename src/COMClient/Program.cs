namespace COMClient
{
    class Program
    {
        static void Main()
        {
            Type t = Type.GetTypeFromCLSID(
                new Guid("22222222-2222-2222-2222-222222222222"));

            object obj = Activator.CreateInstance(t);

            ICalculator calc = (ICalculator)obj;

            Console.WriteLine(calc.Add(10, 5));
            Console.WriteLine(calc.Multiply(10, 5));
        }
    }
}
