using System;

namespace UO_Service
{
    class UOServiceException : ApplicationException
    {
        public UOServiceException(string message) : base(message)
        { 
        }
    }
}
