namespace TimeDataViewer.Core
{
    public class LinearAxis : Axis
    {
        public LinearAxis() { }

        public override bool IsXyAxis()
        {
            return true;
        }
    }
}
