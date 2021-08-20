namespace TimeDataViewer
{
    public class LinearAxis : Axis
    {
        public LinearAxis()
        {
            InternalAxis = new Core.LinearAxis();
        }

        public override Core.Axis CreateModel()
        {
            SynchronizeProperties();
            return InternalAxis;
        }
    }
}
