using System;
using System.Collections.Generic;
using System.Text;

namespace Globe3DLight.Editor
{
    public interface IEditorCanvasPlatform
    {

        Action InvalidateControl { get; set; }

    }
}
