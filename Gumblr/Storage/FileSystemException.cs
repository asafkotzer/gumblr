using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gumblr.Storage
{
    public class StorageException : Exception
    {
        public StorageException() { }
    }

    public class ItemAlreadyExistsException : StorageException
    {
        public ItemAlreadyExistsException() { }
    }

    public class ItemDoesNotExitException : StorageException
    {
        public ItemDoesNotExitException()  { }
    }

}