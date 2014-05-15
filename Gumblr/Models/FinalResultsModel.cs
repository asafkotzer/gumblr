using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gumblr.Models
{
    public class FinalResultsModel : TableEntity
    {
        public string Winner { get; set; }
    }
}