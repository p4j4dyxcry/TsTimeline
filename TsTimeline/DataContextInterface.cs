namespace TsTimeline
{
    public interface IHoldClipDataContext
    {
        double StartFrame { get; set; }
        double EndFrame { get; set; }
    }

    public interface ITriggerClipDataContext
    {
        double Frame { get; set; }
    }
}