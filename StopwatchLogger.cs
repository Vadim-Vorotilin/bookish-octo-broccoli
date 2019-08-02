using System;
using System.Collections.Generic;
using System.Diagnostics;
using Serilog;

namespace BookishOctoBroccoli
{
    public static class StopwatchLogger
    {
        private static readonly Dictionary<int, Stopwatch> _stopwatches = new Dictionary<int, Stopwatch>();
        private static readonly Dictionary<int, TimeSpan> _lastElapsed = new Dictionary<int, TimeSpan>();

        public static void Start(int id)
        {
            var stopwatch = new Stopwatch();
            
            _stopwatches.Add(id, stopwatch);
            _lastElapsed.Add(id, TimeSpan.Zero);
            
            stopwatch.Start();
        }

        public static TimeSpan CheckPoint(int id)
        {
            var delta = _stopwatches[id].Elapsed - _lastElapsed[id];

            _lastElapsed[id] = _stopwatches[id].Elapsed;

            return delta;
        }

        public static TimeSpan GetTotal(int id)
        {
            return _stopwatches[id].Elapsed;
        }

        public static TimeSpan Stop(int id)
        {
            var stopwatch = _stopwatches[id]; 
            
            stopwatch.Stop();

            _stopwatches.Remove(id);
            _lastElapsed.Remove(id);

            return stopwatch.Elapsed;
        }

        public static void LogDelta(string message, int id)
        {
            Log.Information($"=============== {message}. Delta: {CheckPoint(id).TotalMilliseconds} ms. ===============");
        }
    }
}
