using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ChoixResto.Models;
using ChoixResto.Controllers;
using ChoixResto.ViewModels;
using System.Web.Mvc;
using System.Collections.Generic;
using Moq;
using System.Collections.Specialized;
using ChoixResto.Filters;

namespace ChoixResto.Tests
{
    [TestClass]
    public class AjaxFilterAttributeTests
    {
        [TestMethod]
        public void AjaxFilterOnActionExecuting_AvecAjaxHeader_LaissePasserRequete()
        {
            NameValueCollection fausseCollection = new NameValueCollection { { "X-Requested-With", "XMLHttpRequest" } };
            Mock<ActionExecutingContext> context = new Mock<ActionExecutingContext>();
            context.Setup(r => r.HttpContext.Request.Headers).Returns(fausseCollection);

            AjaxFilterAttribute filtre = new AjaxFilterAttribute();
            filtre.OnActionExecuting(context.Object);

            Assert.IsNull(context.Object.Result);
        }

        [TestMethod]
        public void AjaxFilterOnActionExecuting_SansAjaxHeader_RenvoiHttpNotFoudResult()
        {
            NameValueCollection fausseCollection = new NameValueCollection();
            Mock<ActionExecutingContext> context = new Mock<ActionExecutingContext>();
            context.Setup(r => r.HttpContext.Request.Headers).Returns(fausseCollection);

            AjaxFilterAttribute filtre = new AjaxFilterAttribute();
            filtre.OnActionExecuting(context.Object);

            Assert.IsInstanceOfType(context.Object.Result, typeof(HttpNotFoundResult));
        }
    }
}
