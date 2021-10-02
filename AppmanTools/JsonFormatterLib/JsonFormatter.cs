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
        private const char StartAndEndString = '\"';
        private const char IgnoreNextChar = '\\';
        private const char SeparatorNameValue = ':';
        private const char SeparatorElement = ',';

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

        public JsonFormatterResult Format(string jsonString)
        {
            JsonFormatterResult.Status status = JsonFormatterResult.Status.Failed;
            JsonCompositionItem root = new JsonCompositionItem("");
            string desc = string.Empty;
            try
            {
                ProcessJSONString(root, jsonString, 0);
                status = JsonFormatterResult.Status.Success;
            }
            catch(Exception ex)
            {
                desc = ex.Message;
                status = JsonFormatterResult.Status.Failed;
            }

            return new JsonFormatterResult(status, root, desc);
        }

        private int ProcessJSONString(JsonCompositionItem parent, string jsonString, int index)
        {
            // Find {
            int nextIndex = index;
            try
            {
                nextIndex = GetStartCompositionIndex(jsonString, index);
                if (IsInRange(jsonString, nextIndex))
                    nextIndex++; // move next position
                else
                    throw new Exception($"Can't find start composition from {index}");
            }
            catch(Exception ex)
            {
                throw new Exception($"Find start composition from {index} got error : {ex.Message}", ex);
            }

            bool isEndElement = false;
            while(!isEndElement)
            {
                nextIndex = ProcessJSONElement(parent, jsonString, nextIndex);

                // Find , or }
                int previousIndex = nextIndex;
                try
                {
                    NextEvents nextEvent;
                    nextIndex = GetNextEvent(jsonString, previousIndex, out nextEvent);
                    switch(nextEvent)
                    {
                        case NextEvents.GetNextElement:
                            break;

                        case NextEvents.CloseComposition:
                            isEndElement = true;
                            break;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Find }} or , from {previousIndex} got error : {ex.Message}", ex);
                }
            }

            return nextIndex;
        }

        private int ProcessJSONElement(JsonCompositionItem parent, string jsonString, int index)
        {
            // [name] : [value] or Empty
            int startCompositIndex = index;
            int separatorIndex = startCompositIndex;
            try
            {
                separatorIndex = GetSeparatorIndex(jsonString, startCompositIndex);
                if (!IsInRange(jsonString, separatorIndex))
                    return index;   // is empty or missing
            }
            catch (Exception ex)
            {
                throw new Exception($"Find separator from {startCompositIndex} got error : {ex.Message}", ex);
            }

            // build name
            string name = string.Empty;
            try
            {
                name = jsonString.Substring(startCompositIndex, (separatorIndex - startCompositIndex));
                name = name.Trim(' ', '\n', '\r', '\t');
            }
            catch (Exception ex)
            {
                throw new Exception($"Build name from {startCompositIndex} to {separatorIndex} got error : {ex.Message}", ex);
            }

            // get type of value
            int startValueIndex = separatorIndex;
            ValueType valueType;
            try
            {
                startValueIndex = GetTypeOfValue(jsonString, separatorIndex, out valueType);
            }
            catch (Exception ex)
            {
                throw new Exception($"Build name from {startCompositIndex} to {separatorIndex} got error : {ex.Message}", ex);
            }

            // build value
            switch(valueType)
            {
                case ValueType.String:
                    string value;
                    index = GetStringValue(jsonString, startValueIndex, out value);
                    JsonValueItem valueItem = new JsonValueItem(name, value);
                    parent.Items.Add(valueItem);
                    break;

                case ValueType.Array:
                    break;

                case ValueType.Composite:
                    break;
            }

            return index;
        }

        private int GetStartCompositionIndex(string jsonString, int offset) => jsonString.IndexOf(StartComposition, offset);
        private int GetEndCompositionIndex(string jsonString, int offset) => jsonString.IndexOf(EndComposition, offset);
        private int GetSeparatorIndex(string jsonString, int offset) => jsonString.IndexOf(SeparatorNameValue, offset);

        private enum ValueType
        {
            String,
            Array,
            Composite,
        }

        private int GetTypeOfValue(string jsonString, int index, out ValueType valueType)
        {
            valueType = ValueType.String;
            bool isDone = false;
            while (IsInRange(jsonString, index) && !isDone)
            {
                char current = jsonString[index];
                switch (current)
                {
                    case StartAndEndString:
                        valueType = ValueType.String;
                        return index;

                    case StartComposition:
                        valueType = ValueType.Composite;
                        return index;

                    case StartArrry:
                        valueType = ValueType.Array;
                        return index;

                    case IgnoreNextChar:
                        index++;
                        break;
                }
                index++;
            }

            return index;
        }


        private enum NextEvents
        {
            Unknown,
            GetNextElement,
            CloseComposition,
        }

        private int GetNextEvent(string jsonString, int index, out NextEvents nextEvent)
        {
            nextEvent = NextEvents.Unknown;
            bool isDone = false;
            while (IsInRange(jsonString, index) && !isDone)
            {
                char current = jsonString[index++];
                switch (current)
                {
                   case EndComposition:
                        nextEvent = NextEvents.CloseComposition;
                        return index;

                    case SeparatorElement:
                        nextEvent = NextEvents.GetNextElement;
                        return index;
                }
            }

            return index;
        }

        private int GetStringValue(string jsonString, int index, out string value)
        {
            value = string.Empty;
            bool isDone = false;
            bool isStart = true;
            int startIndex = index;

            while (IsInRange(jsonString, index) && !isDone)
            {
                char current = jsonString[index];
                switch (current)
                {
                    case StartAndEndString:
                        {
                            if (isStart)
                            {
                                isStart = false;
                                startIndex = index;
                            }
                            else
                            {
                                isDone = true;
                                value = jsonString.Substring(startIndex, (index - startIndex) + 1);
                            }
                        }
                        break;

                    case IgnoreNextChar:
                        index++;
                        break;
                }
                index++;
            }

            return index;
        }
    }
}
