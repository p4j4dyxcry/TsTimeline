using System;
using System.Collections.ObjectModel;
using System.Linq;
using TsTimeline;

namespace SandBox
{
    public class TriggerClipVm : Notification, ITriggerClipDataContext
    {
        public double Value { get; set; }
    }

    public class HoldClipVm : Notification, IHoldClipDataContext
    {
        public double StartValue { get; set; }
        public double EndValue { get; set; }
    }

    public class TrackVm : Notification
    {
        public ObservableCollection<Notification> Clips { get; } = new ObservableCollection<Notification>();
    }

    public class MainWindowVm : Notification
    {
        public ObservableCollection<TrackVm> Tracks { get; } = new ObservableCollection<TrackVm>();

        public MainWindowVm()
        {
            var maxinum = 4000;
            var rand = new Random();
            foreach (var track in Enumerable.Range(0, 500))
            {
                var start = rand.Next(maxinum);
                var end = start + rand.Next(maxinum - start);
                Tracks.Add(new TrackVm()
                {
                    Clips =
                    {
                        new HoldClipVm()
                        {
                            StartValue = start,
                            EndValue = end,
                        },
                        new TriggerClipVm()
                        {
                            Value = rand.Next(maxinum),
                        },
                        new TriggerClipVm()
                        {
                            Value = rand.Next(maxinum),
                        }
                    }
                });
            }
        }
    }
}