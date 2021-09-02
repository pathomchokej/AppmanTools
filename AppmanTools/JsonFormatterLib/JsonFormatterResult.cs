using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonFormatterLib
{
    public class JsonFormatterResult
    {
        public enum Status
        {
            Unknown,
            Success,
            Failed
        }

        private Status _status = Status.Unknown;
        private JsonItem _root = null;

        public Status FormatStatus => _status;
        public JsonItem Root => _root;

        internal JsonFormatterResult(Status status, JsonItem root)
        {
            _status = status;
            _root = root;
        }
    }
}
