using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gumblr.Storage
{
    public interface IItemDescriptor
    {
        string Container { get; set; }
        string Key { get; set; }
    }

    public class ItemDescriptor : IItemDescriptor
    {
        public ItemDescriptor(string aContainer, string aKey)
        {
            Container = aContainer;
            Key = aKey;
        }

        public string Container { get; set; }
        public string Key { get; set; }
    }
}