using Gumblr.BusinessLogic.Emails;
using Gumblr.DataAccess;
using Gumblr.Models;
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
    public class NewMatchEmailIntegrationTests
    {
        [TestMethod]
        public void Send()
        {
            AsyncContext.Run(async () =>
                {
                    var fakeConfigurationRetriever = new FakeConfigurationRetriever() { ReturnValues = new Dictionary<string, string> 
                    {
                        { "StorageConnectionString", "DefaultEndpointsProtocol=https;AccountName=gumblr;AccountKey=bqW4GHhllTUWjWNPsixtLIF7pFwU9Vf3Wr4gAzsuEJFqhQqpij3mBEFgyo+9ritvvvK5cKu5G9CSro6GFOpkUQ==" },
                        { "SendgridUsername", "gumblr" },
                        { "SendgridPassword", "HoneyPie0" }
                    }};

                    var provider = new BlobStorageProvider(new JsonSerializer(), fakeConfigurationRetriever);
                    var userRepository = new UserRepository(provider);

                    var model = new NewMatchesModel()
                    {
                        User = await userRepository.GetUser("ba3feb8d-20a9-4da8-9a59-c0f174f96b1c"),
                        NewMatches = new List<Match> { new Match { Host = "Brazil", Visitor = "Croatia", StartTime = new DateTime(2014, 06, 12, 17, 0, 0), Stage = MatchStage.Group } },
                    };

                    NewMatchesEmailGenerator generator = new NewMatchesEmailGenerator(model);

                    var emailProvider = new SendGridEmailProvider(fakeConfigurationRetriever);
                    await emailProvider.Send(generator.GetMessage());
                });
        }
    }
}
