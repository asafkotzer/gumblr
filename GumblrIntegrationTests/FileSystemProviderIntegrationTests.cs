using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nito.AsyncEx;
using Gumblr.Storage;
using System.IO;

namespace GumblrIntegrationTests
{
    [TestClass]
    public class FileSystemProviderIntegrationTests
    {
        //TODO: write cleanup code to run before each test

        [TestMethod]
        public void FileSystemProvider_CreateFile_FileExists()
        {
            AsyncContext.Run(async () =>
            {
                var key = Guid.NewGuid().ToString();
                var provider = new FileSystemProvider(new JsonSerializer());
                await provider.Create("TestContainer", key, new TestItem { TextProperty = "SomeText", NumberProperty = 1 });

                var path = Path.Combine(@"c:\temp\gumblr\storage\TestContainer\", key);
                Assert.IsTrue(File.Exists(path));
                Assert.AreNotEqual(0, new FileInfo(path).Length);
            });
        }

        [TestMethod]
        public void FileSystemProvider_WriteToFileAndReadFromFile_DeserializedCorrectly()
        {
            AsyncContext.Run(async () =>
            {
                var key = Guid.NewGuid().ToString();
                var provider = new FileSystemProvider(new JsonSerializer());
                await provider.Create("TestContainer", key, new TestItem { TextProperty = "SomeText", NumberProperty = 1 });
                var result = await provider.Read<TestItem>("TestContainer", key);

                Assert.AreEqual("SomeText", result.TextProperty);
                Assert.AreEqual(1, result.NumberProperty);
            });
        }

        [TestMethod]
        public void FileSystemProvider_WriteToFileAndUpdateFileAndReadFromFile_DeserializedCorrectly()
        {
            AsyncContext.Run(async () =>
            {
                var key = Guid.NewGuid().ToString();
                var provider = new FileSystemProvider(new JsonSerializer());

                await provider.Create("TestContainer", key, new TestItem { TextProperty = "SomeText", NumberProperty = 1 });
                await provider.Update("TestContainer", key, new TestItem { TextProperty = "SomeOtherText", NumberProperty = 2 });
                var result = await provider.Read<TestItem>("TestContainer", key);

                Assert.AreEqual("SomeOtherText", result.TextProperty);
                Assert.AreEqual(2, result.NumberProperty);
            });
        }

        [TestMethod]
        public void FileSystemProvider_CreateFileAndDeleteIt_FileDoesNotExist()
        {
            AsyncContext.Run(async () =>
            {
                var key = Guid.NewGuid().ToString();
                var provider = new FileSystemProvider(new JsonSerializer());

                await provider.Create("TestContainer", key, new TestItem { TextProperty = "SomeText", NumberProperty = 1 });
                //await provider.Delete("TestContainer", key);

                var path = Path.Combine(@"c:\temp\gumblr\storage\TestContainer\", key);
                Assert.IsFalse(File.Exists(path));
            });
        }

    }

    public class TestItem
    {
        public string TextProperty { get; set; }
        public int NumberProperty { get; set; }
    }
}
