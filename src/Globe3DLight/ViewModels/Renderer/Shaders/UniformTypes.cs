using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;

namespace Globe3DLight.Renderer
{
    //internal class UniformBool : Uniform<bool>, ICleanable
    //{
    //    internal UniformBool(string name, int location, ICleanableObserver observer)
    //        : base(name, ActiveUniformType.Bool)
    //    {
    //        this.location = location;
    //        this.dirty = true;
    //        this.observer = observer;
    //        this.observer.NotifyDirty(this);
    //    }

    //    public override bool Value
    //    {
    //        set
    //        {
    //            if (!dirty && (this.value != value))
    //            {
    //                dirty = true;
    //                observer.NotifyDirty(this);
    //            }

    //            this.value = value;
    //        }

    //        get { return value; }
    //    }


    //    #region ICleanable Members

    //    public void Clean()
    //    {
    //        GL.Uniform1(location, value ? 1 : 0);
    //        dirty = false;
    //    }

    //    #endregion

    //    private int location;
    //    private bool value;
    //    private bool dirty;
    //    private readonly ICleanableObserver observer;
    //}

    //internal class UniformFloat : Uniform<float>, ICleanable
    //{
    //    internal UniformFloat(string name, int location, ICleanableObserver observer)
    //        : base(name, ActiveUniformType.Float)
    //    {
    //        this.location = location;
    //        this.dirty = true;
    //        this.observer = observer;
    //        this.observer.NotifyDirty(this);
    //    }

    //    #region Uniform<> Members

    //    public override float Value
    //    {
    //        set
    //        {
    //            if (!dirty && (this.value != value))
    //            {
    //                dirty = true;
    //                observer.NotifyDirty(this);
    //            }

    //            this.value = value;
    //        }

    //        get { return value; }
    //    }

    //    #endregion

    //    #region ICleanable Members

    //    public void Clean()
    //    {
    //        GL.Uniform1(location, value);
    //        dirty = false;
    //    }

    //    #endregion

    //    private int location;
    //    private float value;
    //    private bool dirty;
    //    private readonly ICleanableObserver observer;
    //}

    //internal class UniformFloatMatrix33 : Uniform<mat3>, ICleanable
    //{
    //    internal UniformFloatMatrix33(string name, int location, ICleanableObserver observer)
    //        : base(name, ActiveUniformType.FloatMat3)
    //    {
    //        this.location = location;
    //        this.value = new mat3();
    //        this.dirty = true;
    //        this.observer = observer;
    //        this.observer.NotifyDirty(this);
    //    }

    //    #region Uniform<> Members

    //    public override mat3 Value
    //    {
    //        set
    //        {
    //            if (!dirty && (this.value != value))
    //            {
    //                dirty = true;
    //                observer.NotifyDirty(this);
    //            }

    //            this.value = value;
    //        }

    //        get { return value; }
    //    }

    //    #endregion

    //    #region ICleanable Members

    //    public void Clean()
    //    {
    //        GL.UniformMatrix3(location, 1, false, value.Values1D);
    //        dirty = false;
    //    }

    //    #endregion

    //    private int location;
    //    private mat3 value;
    //    private bool dirty;
    //    private readonly ICleanableObserver observer;
    //}

    //internal class UniformFloatMatrix44 : Uniform<mat4>, ICleanable
    //{
    //    internal UniformFloatMatrix44(string name, int location, ICleanableObserver observer)
    //        : base(name, ActiveUniformType.FloatMat4)
    //    {
    //        this.location = location;
    //        this.value = new mat4();
    //        this.dirty = true;
    //        this.observer = observer;
    //        this.observer.NotifyDirty(this);
    //    }

    //    #region Uniform<> Members

    //    public override mat4 Value
    //    {
    //        set
    //        {
    //            if (!this.dirty && (this.value != value))
    //            {
    //                dirty = true;
    //                observer.NotifyDirty(this);
    //            }

    //            this.value = value;
    //        }

    //        get { return value; }
    //    }

    //    #endregion

    //    #region ICleanable Members

    //    public void Clean()
    //    {
    //        GL.UniformMatrix4(location, 1, false, value.Values1D);
    //        dirty = false;
    //    }

    //    #endregion

    //    private int location;
    //    private mat4 value;
    //    private bool dirty;
    //    private readonly ICleanableObserver observer;
    //}

    //internal class UniformFloatVector2 : Uniform<vec2>, ICleanable
    //{
    //    internal UniformFloatVector2(string name, int location, ICleanableObserver observer)
    //        : base(name, ActiveUniformType.FloatVec2)
    //    {
    //        this.location = location;
    //        this.dirty = true;
    //        this.observer = observer;
    //        this.observer.NotifyDirty(this);
    //    }

    //    #region Uniform<> Members

    //    public override vec2 Value
    //    {
    //        set
    //        {
    //            if (!dirty && (this.value != value))
    //            {
    //                dirty = true;
    //                observer.NotifyDirty(this);
    //            }

    //            this.value = value;
    //        }

    //        get { return value; }
    //    }

    //    #endregion

    //    #region ICleanable Members

    //    public void Clean()
    //    {
    //        GL.Uniform2(location, value.x, value.y);
    //        dirty = false;
    //    }

    //    #endregion

    //    private int location;
    //    private vec2 value;
    //    private bool dirty;
    //    private readonly ICleanableObserver observer;
    //}

    //internal class UniformFloatVector3 : Uniform<vec3>, ICleanable
    //{
    //    internal UniformFloatVector3(string name, int location, ICleanableObserver observer)
    //        : base(name, ActiveUniformType.FloatVec3)
    //    {
    //        this.location = location;
    //        this.dirty = true;
    //        this.observer = observer;
    //        this.observer.NotifyDirty(this);
    //    }

    //    #region Uniform<> Members

    //    public override vec3 Value
    //    {
    //        set
    //        {
    //            if (!dirty && (this.value != value))
    //            {
    //                dirty = true;
    //                observer.NotifyDirty(this);
    //            }

    //            this.value = value;
    //        }

    //        get { return value; }
    //    }

    //    #endregion

    //    #region ICleanable Members

    //    public void Clean()
    //    {
    //        GL.Uniform3(location, value.x, value.y, value.z);
    //        dirty = false;
    //    }

    //    #endregion

    //    private int location;
    //    private vec3 value;
    //    private bool dirty;
    //    private readonly ICleanableObserver observer;
    //}

    //internal class UniformFloatVector4 : Uniform<vec4>, ICleanable
    //{
    //    internal UniformFloatVector4(string name, int location, ICleanableObserver observer)
    //        : base(name, ActiveUniformType.FloatVec4)
    //    {
    //        this.location = location;
    //        this.dirty = true;
    //        this.observer = observer;
    //        this.observer.NotifyDirty(this);
    //    }

    //    #region Uniform<> Members

    //    public override vec4 Value
    //    {
    //        set
    //        {
    //            if (!dirty && (this.value != value))
    //            {
    //                dirty = true;
    //                observer.NotifyDirty(this);
    //            }

    //            this.value = value;
    //        }

    //        get { return value; }
    //    }

    //    #endregion

    //    #region ICleanable Members

    //    public void Clean()
    //    {
    //        GL.Uniform4(location, value.x, value.y, value.z, value.w);
    //        dirty = false;
    //    }

    //    #endregion

    //    private int location;
    //    private vec4 value;
    //    private bool dirty;
    //    private readonly ICleanableObserver observer;
    //}

    //internal class UniformInt : Uniform<int>, ICleanable
    //{
    //    internal UniformInt(string name, int location, ActiveUniformType type, ICleanableObserver observer)
    //        : base(name, type)
    //    {
    //        this.location = location;
    //        this.dirty = true;
    //        this.observer = observer;
    //        this.observer.NotifyDirty(this);
    //    }


    //    #region Uniform<> Members

    //    public override int Value
    //    {
    //        set
    //        {
    //            if (!dirty && (this.value != value))
    //            {
    //                dirty = true;
    //                observer.NotifyDirty(this);
    //            }

    //            this.value = value;
    //        }

    //        get { return value; }
    //    }

    //    #endregion

    //    #region ICleanable Members

    //    public void Clean()
    //    {
    //        GL.Uniform1(location, value);
    //        dirty = false;
    //    }

    //    #endregion

    //    private int location;
    //    private int value;
    //    private bool dirty;
    //    private readonly ICleanableObserver observer;
    //}

    //internal class UniformFloatMatrix42 : Uniform<mat4x2>, ICleanable
    //{
    //    internal UniformFloatMatrix42(string name, int location, ICleanableObserver observer)
    //        : base(name, ActiveUniformType.FloatMat4x2)
    //    {
    //        this.location = location;
    //        this.value = new mat4x2();
    //        this.dirty = true;
    //        this.observer = observer;
    //        this.observer.NotifyDirty(this);
    //    }

    //    #region Uniform<> Members

    //    public override mat4x2 Value
    //    {
    //        set
    //        {
    //            if (!dirty && (this.value != value))
    //            {
    //                dirty = true;
    //                observer.NotifyDirty(this);
    //            }

    //            this.value = value;
    //        }

    //        get { return value; }
    //    }

    //    #endregion

    //    #region ICleanable Members

    //    public void Clean()
    //    {
    //        GL.UniformMatrix4x2(location, 1, false, value.Values1D);
    //        dirty = false;
    //    }

    //    #endregion

    //    private int location;
    //    private mat4x2 value;
    //    private bool dirty;
    //    private readonly ICleanableObserver observer;
    //}
}
