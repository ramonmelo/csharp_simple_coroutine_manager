﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoroutineApp
{
    class WaitFor : IYieldInstance
    {
        private long waitTime;
        private Stopwatch stopwatch;

        public WaitFor(long waitMillis)
        {
            this.waitTime = waitMillis;
            this.stopwatch = new Stopwatch();
            this.stopwatch.Start();
        }

        public bool CanRun()
        {
            return this.stopwatch.ElapsedMilliseconds > waitTime;
        }
    }
}
