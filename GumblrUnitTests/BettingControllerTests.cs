using System;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gumblr.Controllers;
using System.Web.Mvc;
using Gumblr.Models;
using System.Linq;
using Gumblr.BusinessLogic;

namespace GumblrUnitTests
{
    [TestClass]
    public class BettingControllerTests
    {
        [TestMethod]
        public void PlaceBets_UserPlacedNoBets_ReturnsModelWithOneMatch()
        {
            var controller = CreateController();
            var result = controller.PlaceBets().Result as ViewResult;
            var model = result.Model as BettingModel;
            Assert.AreEqual(1, model.Matches.Count());
        }

        [TestMethod]
        public void Index_RedirectedToPlaceBets()
        {
            var controller = CreateController();
            var result = controller.Index();
            var actionName = (result as RedirectToRouteResult).RouteValues["action"];
            Assert.AreEqual("PlaceBets", actionName);
        }

        private BettingController CreateController()
        {
            return new BettingController(
                new FakeMatchRepository(), 
                new FakeMatchBetRepository(),
                new FakeUserRepository(), 
                new FakeIdentityManager(),
                new BettingModelValidator());
        }
    }
}
