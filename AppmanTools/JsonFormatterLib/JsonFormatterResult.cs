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
        private string _description = string.Empty;

        public Status FormatStatus => _status;
        public JsonItem Root => _root;
        public string Description => _description;

        internal JsonFormatterResult(Status status, JsonItem root, string description = "")
        {
            _status = status;
            _root = root;
            _description = description;
        }
    }
}
