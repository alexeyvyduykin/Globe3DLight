using TimeDataViewer.Spatial;

namespace TimeDataViewer.Core
{
    public class HitTestResult
    {
        public HitTestResult(UIElement element, ScreenPoint nearestHitPoint, object item = null, double index = 0)
        {
            Element = element;
            NearestHitPoint = nearestHitPoint;
            Item = item;
            Index = index;
        }

        public double Index { get; private set; }

        public object Item { get; private set; }

        public UIElement Element { get; private set; }

        public ScreenPoint NearestHitPoint { get; private set; }
    }
}
