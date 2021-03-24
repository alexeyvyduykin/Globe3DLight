using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Globe3DLight.Models.Style;

namespace Globe3DLight.ViewModels.Style
{
    public class ArgbColor : ViewModelBase, IArgbColor
    {
        private byte _a;
        private byte _r;
        private byte _g;
        private byte _b;

        /// <inheritdoc/>
        public byte A
        {
            get => _a;
            set => RaiseAndSetIfChanged(ref _a, value);
        }

        /// <inheritdoc/>
        public byte R
        {
            get => _r;
            set => RaiseAndSetIfChanged(ref _r, value);
        }

        /// <inheritdoc/>
        public byte G
        {
            get => _g;
            set => RaiseAndSetIfChanged(ref _g, value);
        }

        /// <inheritdoc/>
        public byte B
        {
            get => _b;
            set => RaiseAndSetIfChanged(ref _b, value);
        }

        /// <inheritdoc/>
        public override object Copy(IDictionary<object, object> shared)
        {
            return new ArgbColor()
            {
                A = this.A,
                R = this.R,
                G = this.G,
                B = this.B
            };
        }

        /// <inheritdoc/>
        public override bool IsDirty()
        {
            var isDirty = base.IsDirty();
            return isDirty;
        }

        /// <inheritdoc/>
        public override void Invalidate()
        {
            base.Invalidate();
        }

        /// <inheritdoc/>
        public string ToXamlString() => ToXamlHex(this);

        /// <inheritdoc/>
        public string ToSvgString() => ToSvgHex(this);

        /// <summary>
        /// Creates a <see cref="ArgbColor"/> from an integer.
        /// </summary>
        /// <param name="value">The integer value.</param>
        /// <returns>The color.</returns>
        public static IArgbColor FromUInt32(uint value)
        {
            return new ArgbColor
            {
                A = (byte)((value >> 24) & 0xff),
                R = (byte)((value >> 16) & 0xff),
                G = (byte)((value >> 8) & 0xff),
                B = (byte)(value & 0xff),
            };
        }

        public static IArgbColor FromArgb(byte r, byte g, byte b, byte a)
        {
            return new ArgbColor
            {
                A = a,
                R = r,
                G = g,
                B = b,
            };
        }

        /// <summary>
        /// Parses a color string.
        /// </summary>
        /// <param name="s">The color string.</param>
        /// <returns>The new instance of the <see cref="ArgbColor"/> class.</returns>
        public static IArgbColor Parse(string s)
        {
            if (s[0] == '#')
            {
                var or = 0u;

                if (s.Length == 7)
                {
                    or = 0xff000000;
                }
                else if (s.Length != 9)
                {
                    throw new FormatException($"Invalid color string: '{s}'.");
                }

                return FromUInt32(uint.Parse(s.Substring(1), NumberStyles.HexNumber, CultureInfo.InvariantCulture) | or);
            }
            else
            {
                var upper = s.ToUpperInvariant();
                var member = typeof(Colors).GetTypeInfo().DeclaredProperties.FirstOrDefault(x => x.Name.ToUpperInvariant() == upper);
                if (member != null)
                {
                    return (ArgbColor)member.GetValue(null);
                }
                else
                {
                    throw new FormatException($"Invalid color string: '{s}'.");
                }
            }
        }

        /// <summary>
        /// Converts a color to xaml hex string.
        /// </summary>
        /// <param name="c">The color instance.</param>
        /// <returns>The color string.</returns>
        public static string ToXamlHex(IArgbColor c)
        {
            return string.Concat('#', c.A.ToString("X2"), c.R.ToString("X2"), c.G.ToString("X2"), c.B.ToString("X2"));
        }

        /// <summary>
        /// Converts a color to svg hex string.
        /// </summary>
        /// <param name="c">The color instance.</param>
        /// <returns>The color string.</returns>
        public static string ToSvgHex(IArgbColor c)
        {
            return string.Concat('#', c.R.ToString("X2"), c.G.ToString("X2"), c.B.ToString("X2")); // NOTE: Not using c.A.ToString("X2")
        }
    }
}
