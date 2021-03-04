using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Containers;

namespace Globe3DLight.Data
{
    public interface IDatabaseProvider 
    {
        IProjectContainer LoadProject();
        ScenarioData LoadScenarioData();
    }
}
