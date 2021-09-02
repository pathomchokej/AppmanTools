using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonFormatterLib
{
    public abstract class JsonItem
    {
        public enum Type
        {
            Unknown,
            Error,
            Value,
            Array,
            Composition,
        }

        private Type _type = Type.Unknown;
        private string _name = string.Empty;

        public Type ItemType => _type;
        public string Name => _name;

        protected JsonItem(Type type, string name)
        {
            _type = type;
            _name = name;
        }
    }
}
