using System;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gumblr.Controllers;
using System.Web.Mvc;
using Gumblr.Models;
using System.Linq;

namespace GumblrUnitTests
{
    [TestClass]
    public class BettingControllerTests
    {
        [TestMethod]
        public void PlaceBets_ReturnsModelWithOneMatch()
        {
            var controller = new BettingController(new FakeMatchRepository(), new FakeMatchBetRepository());
            var result = controller.PlaceBets() as ViewResult;
            var model = result.Model as BettingModel;
            Assert.AreEqual(1, model.Matches.Count());
        }

        [TestMethod]
        public void Index_RedirectedToPlaceBets()
        {
            var controller = new BettingController(new FakeMatchRepository(), new FakeMatchBetRepository());
            var result = controller.Index();
            var actionName = (result as RedirectToRouteResult).RouteValues["action"];
            Assert.AreEqual("PlaceBets", actionName);
        }
    }
}
