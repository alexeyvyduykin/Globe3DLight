using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using A = OpenTK.Graphics.OpenGL;
using GlmSharp;

namespace Globe3DLight.Renderer.OpenTK.Core
{

    internal class UniformBool : Uniform<bool>, ICleanable
    {
        internal UniformBool(string name, int location, ICleanableObserver observer)
            : base(name, A.ActiveUniformType.Bool)
        {
            this.location = location;
            this.dirty = true;
            this.observer = observer;
            this.observer.NotifyDirty(this);
        }

        #region Uniform<> Members

        public override bool Value
        {
            set
            {
                if (!dirty && (this.value != value))
                {
                    dirty = true;
                    observer.NotifyDirty(this);
                }

                this.value = value;
            }

            get { return value; }
        }

        #endregion

        #region ICleanable Members

        public void Clean()
        {
            A.GL.Uniform1(location, value ? 1 : 0);
            dirty = false;
        }

        #endregion

        private int location;
        private bool value;
        private bool dirty;
        private readonly ICleanableObserver observer;
    }

    internal class UniformFloat : Uniform<float>, ICleanable
    {
        internal UniformFloat(string name, int location, ICleanableObserver observer)
            : base(name, A.ActiveUniformType.Float)
        {
            this.location = location;
            this.dirty = true;
            this.observer = observer;
            this.observer.NotifyDirty(this);
        }

        #region Uniform<> Members

        public override float Value
        {
            set
            {
                if (!dirty && (this.value != value))
                {
                    dirty = true;
                    observer.NotifyDirty(this);
                }

                this.value = value;
            }

            get { return value; }
        }

        #endregion

        #region ICleanable Members

        public void Clean()
        {
            A.GL.Uniform1(location, value);
            dirty = false;
        }

        #endregion

        private int location;
        private float value;
        private bool dirty;
        private readonly ICleanableObserver observer;
    }

    internal class UniformFloatMatrix33 : Uniform<mat3>, ICleanable
    {
        internal UniformFloatMatrix33(string name, int location, ICleanableObserver observer)
            : base(name, A.ActiveUniformType.FloatMat3)
        {
            this.location = location;
            this.value = new mat3();
            this.dirty = true;
            this.observer = observer;
            this.observer.NotifyDirty(this);
        }

        #region Uniform<> Members

        public override mat3 Value
        {
            set
            {
                if (!dirty && (this.value != value))
                {
                    dirty = true;
                    observer.NotifyDirty(this);
                }

                this.value = value;
            }

            get { return value; }
        }

        #endregion

        #region ICleanable Members

        public void Clean()
        {
            A.GL.UniformMatrix3(location, 1, false, value.Values1D);
            dirty = false;
        }

        #endregion

        private int location;
        private mat3 value;
        private bool dirty;
        private readonly ICleanableObserver observer;
    }

    internal class UniformFloatMatrix44 : Uniform<mat4>, ICleanable
    {
        internal UniformFloatMatrix44(string name, int location, ICleanableObserver observer)
            : base(name, A.ActiveUniformType.FloatMat4)
        {
            this.location = location;
            this.value = new mat4();
            this.dirty = true;
            this.observer = observer;
            this.observer.NotifyDirty(this);
        }

        #region Uniform<> Members

        public override mat4 Value
        {
            set
            {
                if (!this.dirty && (this.value != value))
                {
                    dirty = true;
                    observer.NotifyDirty(this);
                }

                this.value = value;
            }

            get { return value; }
        }

        #endregion

        #region ICleanable Members

        public void Clean()
        {
            A.GL.UniformMatrix4(location, 1, false, value.Values1D);
            dirty = false;
        }

        #endregion

        private int location;
        private mat4 value;
        private bool dirty;
        private readonly ICleanableObserver observer;
    }

    internal class UniformFloatVector2 : Uniform<vec2>, ICleanable
    {
        internal UniformFloatVector2(string name, int location, ICleanableObserver observer)
            : base(name, A.ActiveUniformType.FloatVec2)
        {
            this.location = location;
            this.dirty = true;
            this.observer = observer;
            this.observer.NotifyDirty(this);
        }

        #region Uniform<> Members

        public override vec2 Value
        {
            set
            {
                if (!dirty && (this.value != value))
                {
                    dirty = true;
                    observer.NotifyDirty(this);
                }

                this.value = value;
            }

            get { return value; }
        }

        #endregion

        #region ICleanable Members

        public void Clean()
        {
            A.GL.Uniform2(location, value.x, value.y);
            dirty = false;
        }

        #endregion

        private int location;
        private vec2 value;
        private bool dirty;
        private readonly ICleanableObserver observer;
    }

    internal class UniformFloatVector3 : Uniform<vec3>, ICleanable
    {
        internal UniformFloatVector3(string name, int location, ICleanableObserver observer)
            : base(name, A.ActiveUniformType.FloatVec3)
        {
            this.location = location;
            this.dirty = true;
            this.observer = observer;
            this.observer.NotifyDirty(this);
        }

        #region Uniform<> Members

        public override vec3 Value
        {
            set
            {
                if (!dirty && (this.value != value))
                {
                    dirty = true;
                    observer.NotifyDirty(this);
                }

                this.value = value;
            }

            get { return value; }
        }

        #endregion

        #region ICleanable Members

        public void Clean()
        {
            A.GL.Uniform3(location, value.x, value.y, value.z);
            dirty = false;
        }

        #endregion

        private int location;
        private vec3 value;
        private bool dirty;
        private readonly ICleanableObserver observer;
    }

    internal class UniformFloatVector4 : Uniform<vec4>, ICleanable
    {
        internal UniformFloatVector4(string name, int location, ICleanableObserver observer)
            : base(name, A.ActiveUniformType.FloatVec4)
        {
            this.location = location;
            this.dirty = true;
            this.observer = observer;
            this.observer.NotifyDirty(this);
        }

        #region Uniform<> Members

        public override vec4 Value
        {
            set
            {
                if (!dirty && (this.value != value))
                {
                    dirty = true;
                    observer.NotifyDirty(this);
                }

                this.value = value;
            }

            get { return value; }
        }

        #endregion

        #region ICleanable Members

        public void Clean()
        {
            A.GL.Uniform4(location, value.x, value.y, value.z, value.w);
            dirty = false;
        }

        #endregion

        private int location;
        private vec4 value;
        private bool dirty;
        private readonly ICleanableObserver observer;
    }

    internal class UniformInt : Uniform<int>, ICleanable
    {
        internal UniformInt(string name, int location, A.ActiveUniformType type, ICleanableObserver observer)
            : base(name, type)
        {
            this.location = location;
            this.dirty = true;
            this.observer = observer;
            this.observer.NotifyDirty(this);
        }


        #region Uniform<> Members

        public override int Value
        {
            set
            {
                if (!dirty && (this.value != value))
                {
                    dirty = true;
                    observer.NotifyDirty(this);
                }

                this.value = value;
            }

            get { return value; }
        }

        #endregion

        #region ICleanable Members

        public void Clean()
        {
            A.GL.Uniform1(location, value);
            dirty = false;
        }

        #endregion

        private int location;
        private int value;
        private bool dirty;
        private readonly ICleanableObserver observer;
    }

    internal class UniformFloatMatrix42 : Uniform<mat4x2>, ICleanable
    {
        internal UniformFloatMatrix42(string name, int location, ICleanableObserver observer)
            : base(name, A.ActiveUniformType.FloatMat4x2)
        {
            this.location = location;
            this.value = new mat4x2();
            this.dirty = true;
            this.observer = observer;
            this.observer.NotifyDirty(this);
        }

        #region Uniform<> Members

        public override mat4x2 Value
        {
            set
            {
                if (!dirty && (this.value != value))
                {
                    dirty = true;
                    observer.NotifyDirty(this);
                }

                this.value = value;
            }

            get { return value; }
        }

        #endregion

        #region ICleanable Members

        public void Clean()
        {
            A.GL.UniformMatrix4x2(location, 1, false, value.Values1D);
            dirty = false;
        }

        #endregion

        private int location;
        private mat4x2 value;
        private bool dirty;
        private readonly ICleanableObserver observer;
    }

}
