using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoroutineApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start");

            RoutineManager.StartCoroutine(Test());

            Thread.Sleep(5000);
            Console.WriteLine("End");
            RoutineManager.Stop();
        }

        static IEnumerator<IYieldInstance> Test()
        {
            for(int i = 0; i < 3; i++)
            {
                Console.WriteLine("nice");
                yield return new WaitFor(100);
            }

            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine("nice");
                yield return new WaitFor(500);
            }

            yield break;
        }
    }
}
