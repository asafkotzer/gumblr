//using Gumblr.DataAccess;
//using Gumblr.Storage;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Nito.AsyncEx;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace GumblrIntegrationTests
//{
//    [TestClass]
//    public class Tools
//    {
//        [TestMethod]
//        public void FromCsvToJson()
//        {
//            AsyncContext.Run(async () =>
//            {
//                var staticMatchListPath = Path.Combine(@"C:\Users\Asaf\Documents\visual studio 2013\Projects\Gumblr\Gumblr\", @"Configuration\MatchList.csv");
//                var matchParser = new CsvMatchParser(File.ReadAllLines(staticMatchListPath));
//                var matches = matchParser.ParseMatches();

//                MatchRepository matchRepository = new MatchRepository(new FileSystemProvider(new JsonSerializer()));
//                foreach (var match in matches)
//                {
//                    await matchRepository.Update(match);
//                }
//            });
//        }
//    }
//}
