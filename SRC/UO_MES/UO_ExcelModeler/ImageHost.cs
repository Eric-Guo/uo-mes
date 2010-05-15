using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace UO_ExcelModeler
{
    [ComVisible(false)]
    internal class ImageHost : AxHost
    {
        public ImageHost()
            : base("59EE46BA-677D-4d20-BF10-8D8067CB8B33")
        {
        }
        public static stdole.IPictureDisp GettIPictureDispFromPicture(Image image)
        {
            return (stdole.IPictureDisp)AxHost.GetIPictureDispFromPicture(image);
        }
    }
}
