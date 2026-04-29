namespace DeadlockCondition
{
    public class Program
    {
        private static readonly object lockA = new object();
        private static readonly object lockB = new object();

        public static void Main()
        {
            Thread t1 = new Thread(Thread1Work);
            Thread t2 = new Thread(Thread2Work);

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();

            Console.WriteLine("Program finished [Wont execute because the deadlock occurred].");
        }

        public static void Thread1Work()
        {
            lock (lockA)
            {
                Console.WriteLine("Thread 1: Locked lockA");
                Thread.Sleep(100); // Simulate work

                Console.WriteLine("Thread 1: Waiting for lockB...");
                lock (lockB)
                {
                    Console.WriteLine("Thread 1: Locked lockB");
                }
            }
        }
        
        public static void Thread2Work()
        {
            lock (lockB)
            {
                Console.WriteLine("Thread 2: Locked lockB");
                Thread.Sleep(100); // Simulate work

                Console.WriteLine("Thread 2: Waiting for lockA...");
                lock (lockA)
                {
                    Console.WriteLine("Thread 2: Locked lockA");
                }
            }
        }
    }
}