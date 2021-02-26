using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlmSharp;

namespace Globe3DLight
{
    //public class GameObject
    //{
    //    public GameObject()
    //        : this(null, null, null) { }

    //    public GameObject(IGraphicsComponent graphics, IPhysicsComponent physics, IInputComponent input)
    //    {
    //        this.Graphics = graphics;
    //        this.Physics = physics;
    //        this.Input = input;
    //        modelMatrix = dmat4.Identity;
    //    }

    //    public virtual void Render(Context context, SceneState scene)
    //    {
    //        if (Graphics != null)
    //            Graphics.Render(this, context, scene);
    //    }

    //    public virtual void Update(double t)
    //    {
    //        if (Physics != null)
    //            Physics.Update(this, t);
    //    }

    //    public virtual void UpdateInput()
    //    {
    //        if (Input != null)
    //            Input.Update(this);
    //    }

    //    public dvec3 Position
    //    {
    //        get
    //        {
    //            return new dvec3(ModelMatrix.Column3);
    //        }
    //    }

    //    public IGraphicsComponent Graphics { get; set; }
    //    public IPhysicsComponent Physics { get; set; }
    //    public IInputComponent Input { get; set; }

    //    public GameObject Parent { get; set; }

    //    public dmat4 ModelMatrix
    //    {
    //        get
    //        {
    //            if (Parent == null)
    //                return modelMatrix;
    //            return Parent.ModelMatrix * modelMatrix;
    //        }
    //        set
    //        {
    //            modelMatrix = value;
    //        }
    //    }

    //    private dmat4 modelMatrix;
    //}

}
