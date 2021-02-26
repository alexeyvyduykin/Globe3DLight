﻿using System;
using System.Collections.Immutable;
using System.Text;
using GlmSharp;
using Globe3DLight.Containers;
using Globe3DLight.Scene;

namespace Globe3DLight.ScenarioObjects
{
    public interface IScenarioObject : IObservableObject
    {


        ImmutableArray<IScenarioObject> Children { get; set; }
    }
}
