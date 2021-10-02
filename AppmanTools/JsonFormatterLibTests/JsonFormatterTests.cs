using JsonFormatterLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace JsonFormatterLibTests
{
    [TestClass]
    public class JsonFormatterTests
    {
        [TestMethod]
        public void EmptyJson()
        {
            var result = JsonFormatter.Instance.Format("{}");
            Assert.IsNotNull(result);
            Assert.AreEqual(JsonFormatterResult.Status.Success, result.FormatStatus);
            Assert.IsNotNull(result.Root);
            Assert.AreEqual(JsonItem.Type.Composition, result.Root.ItemType);
            var item = result.Root as JsonCompositionItem;
            Assert.IsNotNull(item);
            Assert.AreEqual(string.Empty, item.Name);
            Assert.AreEqual(0, item.Items.Count);
        }

        private const string EndLine = "\n";
        [TestMethod]
        public void SingleStringElement()
        {
            string json = string.Empty;
            json += $"{{" + EndLine;
            json += $"\t \"testName\" : \"testValue\"" + EndLine;
            json += $"}}" + EndLine;

            System.Console.WriteLine(json);
            var result = JsonFormatter.Instance.Format(json);
            Assert.IsNotNull(result);
            Assert.AreEqual(JsonFormatterResult.Status.Success, result.FormatStatus);

            var item = result.Root as JsonCompositionItem;
            Assert.IsNotNull(item);
            Assert.AreEqual(1, item.Items.Count);

            var subItem1 = item.Items[0] as JsonValueItem;
            Assert.IsNotNull(subItem1);
            Assert.AreEqual("\"testName\"", subItem1.Name);
            Assert.AreEqual("\"testValue\"", subItem1.Value);
        }

        [TestMethod]
        public void ThreeStringElement()
        {
            string[] expectNames = { "\"testName1\"", "\"testName2\"", "\"testName3\"" };
            string[] expectValues = { "\"testValue1\"", "\"testValue2\"", "\"testValue3\"" };
            string json = string.Empty;
            json += $"{{" + EndLine;
            json += $"\t {expectNames[0]} :{expectValues[0]}," + EndLine;
            json += $"\t {expectNames[1]}: {expectValues[1]} ," + EndLine;
            json += $"\t {expectNames[2]}  :  {expectValues[2]} \t" + EndLine;
            json += $"}}" + EndLine;

            System.Console.WriteLine(json);
            var result = JsonFormatter.Instance.Format(json);
            Assert.IsNotNull(result);
            Assert.AreEqual(JsonFormatterResult.Status.Success, result.FormatStatus);

            var item = result.Root as JsonCompositionItem;
            Assert.IsNotNull(item);
            Assert.AreEqual(expectNames.Length, item.Items.Count);

            for(int i = 0; i < expectNames.Length; i++)
            {
                var subItem = item.Items[i] as JsonValueItem;
                Assert.IsNotNull(subItem);
                Assert.AreEqual(expectNames[i], subItem.Name);
                Assert.AreEqual(expectValues[i], subItem.Value);
            }
        }

        [TestMethod]
        public void SingleCompositeElement()
        {
            string json = string.Empty;
            json += $"{{" + EndLine;
            json += $"\t \"testComnposite\" : " + EndLine;
            json += $"\t {{" + EndLine;
            json += $"\t\t \"testName\" : \"testValue\"" + EndLine;
            json += $"\t }}" + EndLine;
            json += $"}}" + EndLine;

            System.Console.WriteLine(json);
            var result = JsonFormatter.Instance.Format(json);
            Assert.IsNotNull(result);
            Assert.AreEqual(JsonFormatterResult.Status.Success, result.FormatStatus);

            var item = result.Root as JsonCompositionItem;
            Assert.IsNotNull(item);
            Assert.AreEqual(1, item.Items.Count);

            item = item.Items[0] as JsonCompositionItem;
            Assert.IsNotNull(item);
            Assert.AreEqual("\"testComnposite\"", item.Name);

            var subItem1 = item.Items[0] as JsonValueItem;
            Assert.IsNotNull(subItem1);
            Assert.AreEqual("\"testName\"", subItem1.Name);
            Assert.AreEqual("\"testValue\"", subItem1.Value);
        }

        [TestMethod]
        public void ThreeCompositeElement()
        {
            string[] expectCompositeNames = { "\"testComnposite1\"", "\"testComnposite2\"", "\"testComnposite3\"" };
            string[] expectNames = { "\"testName1\"", "\"testName2\"", "\"testName3\"" };
            string[] expectValues = { "\"testValue1\"", "\"testValue2\"", "\"testValue3\"" };

            string json = string.Empty;
            json += $"{{" + EndLine;

            json += $"\t {expectCompositeNames[0]} : " + EndLine;
            json += $"\t {{" + EndLine;
            json += $"\t\t {expectNames[0]} :{expectValues[0]}" + EndLine;
            json += $"\t }}," + EndLine;

            json += $"\t {expectCompositeNames[1]} : \t {{" + EndLine;
            json += $"\t\t {expectNames[0]} :{expectValues[0]}," + EndLine;
            json += $"\t\t {expectNames[1]} :{expectValues[1]}  }}," + EndLine;

            json += $"\t {expectCompositeNames[2]} : \t {{" + EndLine;
            json += $"\t {expectNames[0]} :{expectValues[0]}," + EndLine;
            json += $"\t {expectNames[1]}: {expectValues[1]} ," + EndLine;
            json += $"\t {expectNames[2]}  :  {expectValues[2]} \t" + EndLine;
            json += $"\t }}" + EndLine;

            json += $"}}" + EndLine;


            System.Console.WriteLine(json);
            var result = JsonFormatter.Instance.Format(json);
            Assert.IsNotNull(result);
            Assert.AreEqual(JsonFormatterResult.Status.Success, result.FormatStatus);

            var root = result.Root as JsonCompositionItem;
            Assert.IsNotNull(root);
            Assert.AreEqual(expectCompositeNames.Length, root.Items.Count);

            for(int compositeIndex = 0; compositeIndex < expectCompositeNames.Length; compositeIndex++)
            {
                var item = root.Items[compositeIndex] as JsonCompositionItem;
                Assert.IsNotNull(item);
                Assert.AreEqual(expectCompositeNames[compositeIndex], item.Name);
                int length = compositeIndex + 1;
                for (int i = 0; i < length; i++)
                {
                    var subItem = item.Items[i] as JsonValueItem;
                    Assert.IsNotNull(subItem);
                    Assert.AreEqual(expectNames[i], subItem.Name);
                    Assert.AreEqual(expectValues[i], subItem.Value);
                }
            }

        }

        [TestMethod]
        public void CompositeStringElement()
        {
            string expectCompositeStringValue = string.Empty;
            expectCompositeStringValue += $"\t \"{{" + EndLine;
            expectCompositeStringValue += $"\t\t \\\" testName \\\" :\\\" testValue \\\"," + EndLine;
            expectCompositeStringValue += $"\t\t \\\" testName \\\" : \\\" testValue \\\"  }}\"" + EndLine;

            string[] expectCompositeNames = { "\"testComnposite1\"", "\"testComnposite2\"", "\"testComnposite3\"" };
            string[] expectNames = { "\"testName1\"", "\"testName2\"", "\"testName3\"" };
            string[] expectValues = { "\"testValue1\"", "\"testValue2\"", "\"testValue3\"" };

            string json = string.Empty;
            json += $"{{" + EndLine;

            json += $"\t {expectCompositeNames[0]} : " + EndLine;
            json += $"\t {{" + EndLine;
            json += $"\t\t {expectNames[0]} :{expectValues[0]}" + EndLine;
            json += $"\t }}," + EndLine;

            json += $"\t {expectCompositeNames[1]} : {expectCompositeStringValue}\t ," + EndLine;

            json += $"\t {expectCompositeNames[2]} : \t {{" + EndLine;
            json += $"\t {expectNames[0]} :{expectValues[0]}," + EndLine;
            json += $"\t {expectNames[1]}: {expectValues[1]} ," + EndLine;
            json += $"\t {expectNames[2]}  :  {expectValues[2]} \t" + EndLine;
            json += $"\t }}" + EndLine;

            json += $"}}" + EndLine;


            System.Console.WriteLine(json);
            var result = JsonFormatter.Instance.Format(json);
            Assert.IsNotNull(result);
            Assert.AreEqual(JsonFormatterResult.Status.Success, result.FormatStatus);

            var root = result.Root as JsonCompositionItem;
            Assert.IsNotNull(root);
            Assert.AreEqual(expectCompositeNames.Length, root.Items.Count);

            {
                int compositeIndex = 0;
                var item = root.Items[compositeIndex] as JsonCompositionItem;
                Assert.IsNotNull(item);
                Assert.AreEqual(expectCompositeNames[compositeIndex], item.Name);
                int length = compositeIndex + 1;
                for (int i = 0; i < length; i++)
                {
                    var subItem = item.Items[i] as JsonValueItem;
                    Assert.IsNotNull(subItem);
                    Assert.AreEqual(expectNames[i], subItem.Name);
                    Assert.AreEqual(expectValues[i], subItem.Value);
                }
            }

            {
                int compositeIndex = 1;
                var item = root.Items[compositeIndex] as JsonValueItem;
                Assert.IsNotNull(item);
                Assert.AreEqual(expectCompositeNames[compositeIndex], item.Name);
                Assert.AreEqual(expectCompositeStringValue.Trim(), item.Value);
            }

            {
                int compositeIndex = 2;
                var item = root.Items[compositeIndex] as JsonCompositionItem;
                Assert.IsNotNull(item);
                Assert.AreEqual(expectCompositeNames[compositeIndex], item.Name);
                int length = compositeIndex + 1;
                for (int i = 0; i < length; i++)
                {
                    var subItem = item.Items[i] as JsonValueItem;
                    Assert.IsNotNull(subItem);
                    Assert.AreEqual(expectNames[i], subItem.Name);
                    Assert.AreEqual(expectValues[i], subItem.Value);
                }
            }

        }
    }
}
