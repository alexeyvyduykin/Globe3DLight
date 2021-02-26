using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Linq;


namespace Globe3DLight
{
    public struct UniqueName : IEquatable<UniqueName>
    {
        private readonly string _name;
        private readonly bool _validation;

        public UniqueName(string name)
        {
            this._name = name;

            this._validation = IsValid(name);
        }


        public static bool IsValid(string name)
        {
        
            if(string.IsNullOrWhiteSpace(name) == true)
            {
                return false;
            }

            if(name.Length != 10)
            {
                return false;
            }

            var type = name.Take(3);

            foreach (var item in type)
            {
                if ((item >= 'A' && item <= 'Z') == false)
                {
                    return false;
                }
            }

            var number = name.Skip(3);

            foreach (var item in number)
            {
                if ((item >= '0' && item <= '9') == false)
                {
                    return false;
                }
            }

            return true;
        }

        public string Type => (_validation == true) ? new string(_name.Take(3).ToArray()) : string.Empty;
       
        public string Number => (_validation == true) ? new string(_name.Skip(3).ToArray()) : string.Empty;

        public string Name => _name;


        public static bool operator ==(UniqueName lhs, UniqueName rhs) => lhs.Equals(rhs);

        public static bool operator !=(UniqueName lhs, UniqueName rhs) => !lhs.Equals(rhs);

        public bool Equals([AllowNull] UniqueName other)
        {
            return Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj) == true)
            {
                return false;
            }
           
            return obj is UniqueName && Equals((UniqueName)obj);
        }

        public override int GetHashCode()
        {                  
            return Name.GetHashCode();            
        }
    }
}
