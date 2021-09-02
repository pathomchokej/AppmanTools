using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonFormatterLib
{
    public class JsonValueItem : JsonItem
    {
        private string _value = string.Empty;

        public string Value => _value;

        public JsonValueItem(string name, string jsonValue) : base(Type.Value, name)
        {
            _value = jsonValue;
        }
    }
}
