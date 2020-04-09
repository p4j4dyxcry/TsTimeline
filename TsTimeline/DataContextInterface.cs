namespace TsTimeline
{
    public interface IHoldClipDataContext
    {
        double StartValue { get; set; }
        double EndValue { get; set; }
    }

    public interface ITriggerClipDataContext
    {
        double Value { get; set; }
    }
}