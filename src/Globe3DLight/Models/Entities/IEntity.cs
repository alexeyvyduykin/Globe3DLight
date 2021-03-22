﻿using System;
using System.Collections.Immutable;
using System.Text;
using GlmSharp;
using Globe3DLight.Containers;
using Globe3DLight.Scene;

namespace Globe3DLight.Entities
{
    public interface IEntity : IObservableObject
    {
        bool IsVisible { get; set; }

        bool IsExpanded { get; set; }       
    }
}