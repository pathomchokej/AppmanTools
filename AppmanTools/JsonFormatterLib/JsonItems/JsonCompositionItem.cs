using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonFormatterLib
{
    public class JsonCompositionItem : JsonItem
    {
        private List<JsonItem> _items = new List<JsonItem>();
        public List<JsonItem> Items => _items;

        public JsonCompositionItem(string name) : base(Type.Composition, name)
        {
        }
    }
}
