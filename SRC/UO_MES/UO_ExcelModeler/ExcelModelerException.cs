using System;
using System.Runtime.InteropServices;

namespace UO_ExcelModeler
{
    [ComVisible(false)]
    public class ExcelModelerException : ApplicationException
    {
        public ExcelModelerException(string message) : base(message)
        { 
        }
    }
}
