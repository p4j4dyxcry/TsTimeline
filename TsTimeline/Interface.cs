namespace TsTimeline
{
    public interface IHoldClipDataContext
    {
        public double StartValue { get; set; }
        public double EndValue { get; set; }
    }

    public interface ITriggerClipDataContext
    {
        public double Value { get; set; }
    }
}