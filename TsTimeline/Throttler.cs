using System;
using System.Threading.Tasks;

namespace TsTimeline
{
    public class Throttler
    {
        private readonly Action _action;
        private readonly TimeSpan _timeSpan;
        private DateTime _lastInvoked;
        private bool _invoked;
        public Throttler(TimeSpan timespan , Action action)
        {
            _action = action;
            _timeSpan = timespan;
        }

        public async void Invoke()
        {
            _lastInvoked = DateTime.Now;
            
            if(_invoked)
                return;
            
            _invoked = true;
            while (_lastInvoked + _timeSpan > DateTime.Now)
            {
                await Task.Delay(1);
            };

            _invoked = false;
            
            _action.Invoke();
        }
    }
}