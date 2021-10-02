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

    }
}
