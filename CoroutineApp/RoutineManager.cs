using System.Collections.Generic;
using System.Threading;

namespace CoroutineApp
{
    public static class RoutineManager
    {
        private static List<IEnumerator<IYieldInstance>> methods;
        private static List<IEnumerator<IYieldInstance>> markRemove;

        private static Thread mainThread;

        static RoutineManager()
        {
            methods = new List<IEnumerator<IYieldInstance>>();
            markRemove = new List<IEnumerator<IYieldInstance>>();
        }

        /**
         * This will start a internal Thread
         * used to execute the Service methods without
         * need to call it directly
         * 
         * If you call this, make sure to call the Stop
         * method when Coroutines are note needed anymore
         */
        public static void StartRunner()
        {
            mainThread = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    RoutineManager.Service();
                }
            }));

            mainThread.Start();
        }

        /**
         * Stop the intenral Thread
         */ 
        public static void Stop()
        {
            mainThread.Abort();
        }

        /**
         * Register a new Coroutine to be executed
         */
        public static void StartCoroutine(IEnumerator<IYieldInstance> enumerator)
        {
            if (enumerator == null) { return; }

            lock (methods)
            {
                methods.Add(enumerator);
                enumerator.MoveNext();
            }
        }

        /**
         * Move the process of the registered coroutines 
         * this must be called often
         */
        public static void Service()
        {
            markRemove.Clear();

            lock (methods)
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

                foreach (IEnumerator<IYieldInstance> func in markRemove)
                {
                    methods.Remove(func);
                }
            }
        }
    }
}
