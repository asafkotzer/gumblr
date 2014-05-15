using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gumblr.Storage
{
    public class StorageProviderException : Exception
    {
        public StorageProviderException() { }
    }

    public class ItemAlreadyExistsException : StorageProviderException
    {
        public ItemAlreadyExistsException() { }
    }

    public class ItemDoesNotExitException : StorageProviderException
    {
        public ItemDoesNotExitException()  { }
    }

}