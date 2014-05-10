using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gumblr.BusinessLogic
{
    public class BettingModelValidationExcpetion : Exception
    {
        public BettingModelValidationExcpetion(string aMessage)
            : base(aMessage)
        {

        }
    }
}