using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Gumblr.Storage
{
    public class FileSystemProvider //: IStorageProvider
    {
        ISerializer mSerializer;
        string mBasePath;

        public FileSystemProvider(ISerializer aSerializer)
        {
            mSerializer = aSerializer;
            mBasePath = Settings.FileSystemProvider_BasePath;
        }

        public async Task Create(string aContainer, string aKey, object aItem)
        {
            var containerPath = Path.Combine(mBasePath, aContainer);
            if (!Directory.Exists(containerPath))
            {
                Directory.CreateDirectory(containerPath);
            }

            var filePath = Path.Combine(containerPath, aKey);
            if (File.Exists(filePath))
            {
                throw new ItemAlreadyExistsException();
            }

            var bytes = mSerializer.SerializeObject(aItem);
            await WriteToFile(filePath, bytes, false);
        }

        public async Task<T> Read<T>(string aContainer, string aKey)
        {
            var filePath = Path.Combine(mBasePath, aContainer, aKey);
            if (!File.Exists(filePath))
            {
                throw new ItemDoesNotExitException();
            }

            var bytes = await ReadFromFile(filePath);
            return mSerializer.DeserializeObject<T>(bytes);
        }


        public Task<IEnumerable<IItemDescriptor>> List(string aContainer)
        {
            var containerPath = Path.Combine(mBasePath, aContainer);
            if (!Directory.Exists(containerPath))
            {
                Directory.CreateDirectory(containerPath);
            }

            var files = Directory.GetFiles(containerPath).Select(x => new ItemDescriptor(aContainer, x) as IItemDescriptor);
            return Task.FromResult(files);
        }

        public async Task Update(string aContainer, string aKey, object aItem)
        {
            var filePath = Path.Combine(mBasePath, aContainer, aKey);
            if (!File.Exists(filePath))
            {
                throw new ItemDoesNotExitException();
            }

            var bytes = mSerializer.SerializeObject(aItem);
            await WriteToFile(filePath, bytes, true);
        }

        public async Task CreateOrReplace(string aContainer, string aKey, object aItem)
        {
            var filePath = Path.Combine(mBasePath, aContainer, aKey);
            if (File.Exists(filePath))
            {
                await Update(aContainer, aKey, aItem);
            }
            else
            {
                await Create(aContainer, aKey, aItem);
            }
        }

        public async Task Delete(string aContainer, string aKey)
        {
            var filePath = Path.Combine(mBasePath, aContainer, aKey);
            if (!File.Exists(filePath))
            {
                throw new ItemDoesNotExitException();
            }

            File.Delete(filePath);
        }

        private async Task WriteToFile(string aFilename, byte[] aData, bool aOverwrite)
        {
            var fileMode = aOverwrite ? FileMode.Open : FileMode.CreateNew;
            using (var stream = new FileStream(aFilename, fileMode))
            {
                await stream.WriteAsync(aData, 0, aData.Length);
            }
        }

        private async Task<byte[]> ReadFromFile(string aFilename)
        {
            var result = new List<byte>();

            using (var stream = new FileStream(aFilename, FileMode.Open))
            {
                byte[] buffer = new byte[4096];
                while (await stream.ReadAsync(buffer, 0, buffer.Length) > 0)
                {
                    result.AddRange(buffer);
                }

            }

            return result.ToArray();
        }


    }
}