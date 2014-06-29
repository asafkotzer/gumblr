using Gumblr.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace Gumblr.Helpers
{
    public interface IRatioRetriever
    {
        Task<Ratio> GetRatio(Match aMatch);
    }

    public class RatioRetriever : IRatioRetriever
    {
        IHtmlGetter mRatioHtmlGetter;

        public RatioRetriever(IHtmlGetter aRatioHtmlGetter)
        {
            mRatioHtmlGetter = aRatioHtmlGetter;
        }

        public async Task<Ratio> GetRatio(Match aMatch)
        {
            var url = string.Format("http://www.predicttheworldcup.com/site/stats?fixtureid={0}", aMatch.Index);
            var html = await mRatioHtmlGetter.GetHtml(url);

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var predictionNodes = doc.DocumentNode
                .Descendants("table")
                .Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value == "prediction-stats")
                .SelectMany(x => x.Descendants("td"))
                .Take(3)
                .Select(x => TrimPredictionNodesHtml(x.InnerText))
                .Select(x => double.Parse(x))
                .ToList();

            return new Ratio(aMatch.Stage, predictionNodes[0], predictionNodes[2]);
        }

        private string TrimPredictionNodesHtml(string aNodeText)
        {
            var trimmedText = aNodeText.Trim();
            var result = trimmedText.Substring(1, trimmedText.IndexOf(']') - 2);
            return result;
        }
    }

    public interface IHtmlGetter
    {
        Task<string> GetHtml(string aUrl);
    }

    public class HtmlGetter : IHtmlGetter
    {
        public async Task<string> GetHtml(string aUrl)
        {
            var client = new WebClient();
            var response = await client.DownloadStringTaskAsync(aUrl);
            return response;
        }
    }

}