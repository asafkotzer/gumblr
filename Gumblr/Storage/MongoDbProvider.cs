using Gumblr.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Gumblr.Storage
{
    public class MongoDbProvider : IStorageProvider
    {
        ISerializer mSerializer;
        string mBasePath;

        public MongoDbProvider(ISerializer aSerializer)
        {
            mSerializer = aSerializer;
            mBasePath = Settings.FileSystemProvider_BasePath;
        }

        public Task Create(string aContainer, string aKey, object aItem)
        {
            throw new NotImplementedException();
        }


        /* 
         * Example for implementation of MongoDB provider
         * TODO: async/await support (blocker), dependency injection
         * 
         * Nuget: Install-Package mongocsharpdriver
         * 
         * Requires the following line in Web.config (within <connectionStrings>)
         * <add name="MongoHQ" connectionString="mongodb://gumblrweb:QuickFox1@dharma.mongohq.com:10042/gumblrdb"/>
         * 
         * Usage:
         * var item = new Gumblr.Storage.MongoDbProvider(null).Read<Gumblr.Models.UserProfile>("Users", "b73cf365-3ffc-473c-9ced-233efed2013b");
         */
            public Task<T> Read<T>(string aContainer, string aKey)
        {
            throw new NotImplementedException();

            //var task = new Task<T>(() => 
            //{
            //    var connectionString = ConfigurationManager.ConnectionStrings["MongoHQ"].ConnectionString;
            //    var server = new MongoClient(connectionString).GetServer();
            //    var db = server.GetDatabase("gumblrdb");
            //    var collections = db.GetCollectionNames();

            //    var collection = db.GetCollection(aContainer)
            //                       .Find(new QueryDocument(new Dictionary<string, object> { { "_id", aKey } }))
            //                       .Select(x => new UserProfile
            //                       {
            //                           UserId = x["_id"].AsString,
            //                           UserName = x["Alias"].AsString,
            //                           UserEmail = x["Email"].AsString,
            //                       });

            //    var result = collection.FirstOrDefault();
            //    if (result == null)
            //    {
            //        throw new ItemDoesNotExitException();
            //    }

            //    return result;
            //});

            //return task;
        }

        public Task Update(string aContainer, string aKey, object aItem)
        {
            throw new NotImplementedException();
        }

        public Task CreateOrUpdate(string aContainer, string aKey, object aItem)
        {
            throw new NotImplementedException();
        }

        public void Delete(string aContainer, string aKey)
        {
            throw new NotImplementedException();
        }
    }
}