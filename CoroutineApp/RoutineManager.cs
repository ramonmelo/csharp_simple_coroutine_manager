using System.Collections.Generic;
using System.Threading;

namespace CoroutineApp
{
    static class RoutineManager
    {
        private static List<IEnumerator<IYieldInstance>> methods;
        private static List<IEnumerator<IYieldInstance>> markRemove;

        private static Thread mainThread;

        static RoutineManager()
        {
            methods = new List<IEnumerator<IYieldInstance>>();
            markRemove = new List<IEnumerator<IYieldInstance>>();

            ThreadStart start = new ThreadStart(RoutineManager.Service);

            mainThread = new Thread(start);
            mainThread.Start();
        }

        public static void StartCoroutine(IEnumerator<IYieldInstance> enumerator)
        {
            if (enumerator == null) { return; }

            lock(methods)
            {
                methods.Add(enumerator);
                enumerator.MoveNext();
            }
        }

        public static void Stop()
        {
            mainThread.Abort();
        }

        private static void Service()
        {
            while(true)
            {
                markRemove.Clear();

                lock(methods)
                {
                    foreach (IEnumerator<IYieldInstance> func in methods)
                    {
                        IYieldInstance yieldInstance = func.Current;

                        if (yieldInstance == null)
                        {
                            markRemove.Add(func);
                        }
                        else if (yieldInstance.CanRun())
                        {
                            if (!func.MoveNext())
                            {
                                markRemove.Add(func);
                            }
                        }
                    }
                }
            }
        }
    }
}
