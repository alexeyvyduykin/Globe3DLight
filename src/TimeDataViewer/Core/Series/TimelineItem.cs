namespace TimeDataViewer.Core
{
    public class TimelineItem : CategorizedItem
    {
        public TimelineItem()
        {

        }

        public TimelineItem(double begin, double end) : this()
        {
            Begin = begin;
            End = end;
        }

        public double End { get; set; }

        public double Begin { get; set; }
    }
}
