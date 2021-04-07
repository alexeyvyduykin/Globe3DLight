#nullable enable
using Globe3DLight.Models.Data;
using Globe3DLight.ViewModels.Entities;

namespace Globe3DLight.ViewModels.Data
{
    public class DataUpdater : IDataUpdater
    {
        public void Update(double t, FrameViewModel frame)
        {
            if (frame.State is not null)
            {
                if (frame.State is IAnimator animator)
                {
                    animator.Animate(t);
                }

                foreach (var item in frame.Children)
                {
                    Update(t, item);
                }
            }
            else
            {
                foreach (var item in frame.Children)
                {
                    Update(t, item);
                }
            }
        }
    }
}
