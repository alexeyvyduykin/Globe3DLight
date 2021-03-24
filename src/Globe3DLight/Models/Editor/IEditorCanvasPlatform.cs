using System;
using System.Collections.Generic;
using System.Text;

namespace Globe3DLight.Models.Editor
{
    public interface IEditorCanvasPlatform
    {
        Action InvalidateControl { get; set; }

    }
}
