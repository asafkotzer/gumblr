using Gumblr.Helpers;
using Gumblr.Storage;
using Gumblr.Storage.Azure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GumblrIntegrationTests
{
    [TestClass]
    public class BlobStorageProviderIntegrationTests
    {
        [TestMethod]
        public void BlobStorageProvider_CreateBlob_BlobExists()
        {
            AsyncContext.Run(async () =>
            {

                var key = Guid.NewGuid().ToString();
                var fake = new FakeConfigurationRetriever() { ReturnValues = new Dictionary<string, string> { { "StorageConnectionString", "DefaultEndpointsProtocol=https;AccountName=gumblr;AccountKey=XLV5y947XhjJg4GaJYehiMx6My9n2Axw9/OdF+kSjtYTpbQGV6n9F8K+hPNjtHVtjmlRwCgssZ/5PM9LrftBIQ==" } } };
                var provider = new BlobStorageProvider(new JsonSerializer(), fake);
                await provider.Create("TestContainer", key, new TestItem { TextProperty = "SomeText", NumberProperty = 1 });

                var result = await provider.Read<TestItem>("TestContainer", key);
                Assert.IsNotNull(result);
                Assert.AreEqual("SomeText", result.TextProperty);
            });
        }

        [TestMethod]
        public void BlobStorageProvider_CreateBlobThenCreateItAgain_ThrowsAlreadyExistsException()
        {
            AsyncContext.Run(async () =>
            {

                var key = Guid.NewGuid().ToString();
                var fake = new FakeConfigurationRetriever() { ReturnValues = new Dictionary<string, string> { { "StorageConnectionString", "DefaultEndpointsProtocol=https;AccountName=gumblr;AccountKey=XLV5y947XhjJg4GaJYehiMx6My9n2Axw9/OdF+kSjtYTpbQGV6n9F8K+hPNjtHVtjmlRwCgssZ/5PM9LrftBIQ==" } } };
                var provider = new BlobStorageProvider(new JsonSerializer(), fake);
                await provider.Create("TestContainer", key, new TestItem { TextProperty = "SomeText", NumberProperty = 1 });

                Exception thrown = null;
                try
                {
                    await provider.Create("TestContainer", key, new TestItem { TextProperty = "SomeText", NumberProperty = 1 });
                }
                catch (Exception ex) { thrown = ex; }

                Assert.IsNotNull(thrown);
                Assert.AreEqual(typeof(ItemAlreadyExistsException), thrown.GetType());
            });
        }
    }

    public class FakeConfigurationRetriever : IConfigurationRetriever
    {
        public Dictionary<string,string> ReturnValues { get; set; }

        public string GetSetting(string aKey)
        {
            return ReturnValues[aKey];
        }

    }
}
