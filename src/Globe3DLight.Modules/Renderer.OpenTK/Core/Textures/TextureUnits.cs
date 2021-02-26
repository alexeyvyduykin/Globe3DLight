using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;



namespace Globe3DLight.Renderer.OpenTK.Core
{
    internal class TextureUnits : ICleanableObserver
    {
        public TextureUnits()
        {
            //
            // Device.NumberOfTextureUnits is not initialized yet.
            //
            int numberOfTextureUnits;
            GL.GetInteger(GetPName.MaxCombinedTextureImageUnits, out numberOfTextureUnits);

            textureUnits = new TextureUnit[numberOfTextureUnits];
            for (int i = 0; i < numberOfTextureUnits; ++i)
            {
                TextureUnit textureUnit = new TextureUnit(i, this);
                textureUnits[i] = textureUnit;
            }
            dirtyTextureUnits = new List<ICleanable>();
  //          lastTextureUnit = (TextureUnit)textureUnits[numberOfTextureUnits - 1];
        }

        public TextureUnit this[int index]
        {
            get { return textureUnits[index]; }
        }

        public int Count
        {
            get { return textureUnits.Length; }
        }

        public IEnumerator GetEnumerator()
        {
            return textureUnits.GetEnumerator();
        }

        public void Clean()
        {
            for (int i = 0; i < dirtyTextureUnits.Count; ++i)
            {
                dirtyTextureUnits[i].Clean();
            }
            dirtyTextureUnits.Clear();
  //          lastTextureUnit.CleanLastTextureUnit();
        }

        #region ICleanableObserver Members

        public void NotifyDirty(ICleanable value)
        {
            dirtyTextureUnits.Add(value);
        }

        #endregion

        private TextureUnit[] textureUnits;
        private IList<ICleanable> dirtyTextureUnits;
        //private TextureUnit lastTextureUnit;
    }

}
