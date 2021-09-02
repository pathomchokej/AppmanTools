using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonFormatterLib
{
    public class JsonFormatter
    {
        private enum State
        {
            InitializeRoot,
            FindName,
            FindContent,
            BuildJsonItem,
            End,
        }
        private const char StartComposition = '{';
        private const char EndComposition = '}';
        private const char StartArrry = '[';
        private const char EndArry = ']';
        private const char Separator = ':';

        private static JsonFormatter _instance = new JsonFormatter();
        public static JsonFormatter Instance => _instance;

        private bool IsInRange(string jsonString, int index) => (index >= 0 && index < jsonString.Length);

        private bool FindName(string jsonString, ref int index, ref string name)
        {
            int separatorIndex = GetSeparatorIndex(jsonString, index);
            if (IsInRange(jsonString, separatorIndex))
            {
                int lenght = separatorIndex - index;
                name = jsonString.Substring(index, lenght);

                index = separatorIndex + 1;

                return true;
            }
            else
                return false;
        }


        private JsonFormatterResult.Status FindCompositionContent(JsonCompositionItem parent, string jsonString, ref int index)
        {
            throw new NotImplementedException();
            /*
            bool isExit = false;
            string name = string.Empty;
            if (!FindName(jsonString, ref index, ref name))
                return JsonFormatterResult.Status.Failed;

            // find type of value
            while (!isExit)
            {
                if (!IsInRange(jsonString, index++))
                    break;

            }

            return JsonFormatterResult.Status.Success;
            //*/
        }

        public JsonFormatterResult Format(string jsonString)
        {
            JsonFormatterResult.Status status = JsonFormatterResult.Status.Failed;
            JsonCompositionItem root = new JsonCompositionItem("");
            int index = GetStartCompositionIndex(jsonString, 0);
            if (IsInRange(jsonString, index))
                status = FindCompositionContent(root, jsonString, ref index);

            return new JsonFormatterResult(status, root);
        }

        private int GetStartCompositionIndex(string jsonString, int offset) => jsonString.IndexOf(StartComposition, offset);
        private int GetEndCompositionIndex(string jsonString, int offset) => jsonString.IndexOf(EndComposition, offset);
        private int GetSeparatorIndex(string jsonString, int offset) => jsonString.IndexOf(Separator, offset);


    }
}
