
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace customerdisplay
{
    public class CustomerDisplay
    {
        public enum DisplayMode
        {
            BeforeOrder = 0,
            OpenOrder = 1,
            PaidOrder = 2
        }

        public static string lastError = null;
        public static DLLFormMgr formMgr;
        public static OrderData orderData = null;
        public static DisplayMode displayMode = DisplayMode.BeforeOrder;
        public static string imagesDirectory = "C:\\Micros\\images";
        
        [DllExport("cdgeterror")]
        [return: MarshalAs(UnmanagedType.LPStr)]
        public static  string CDGetLastError()
        {
            return lastError;
        }

        [DllExport("cdshowdisplay")]
        public static bool CDShowCustomerDisplay(int mode)
        {
            displayMode = (DisplayMode)mode;

            if(formMgr == null)
            {
                formMgr = new DLLFormMgr();
            }

            formMgr.UpdateDisplayMode();

            return true;
        }

        [DllExport("cdsetimagedir")]
        public static void CDSetImageDirectory([MarshalAs(UnmanagedType.LPStr)]string path)
        {
            //MessageBox.Show("1");
              // imagesDirectory = path;
            //MessageBox.Show("2");
            //formMgr.SetImagesDirectory();
            
        }

        [DllExport("cdsenddata")]
        public static void CDSendOrderData(int message, int microsCheckItemID, [MarshalAs(UnmanagedType.LPStr)]  string title, int quanitity, [MarshalAs(UnmanagedType.LPStr)] string priceString, [MarshalAs(UnmanagedType.LPStr)] string taxTotalString, [MarshalAs(UnmanagedType.LPStr)] string discountString)
        {
            float price = float.Parse(priceString);
            float taxTotal = float.Parse(taxTotalString);
            float discount = float.Parse(discountString);


            if (orderData == null)
            {
                orderData = new OrderData();
                displayMode = DisplayMode.OpenOrder;

            }

            if (message == 0) //add new item
            {
                orderData.addItem(title, quanitity, price, microsCheckItemID);
                orderData.tax = taxTotal;
                formMgr.UpdateOrder();
            }
            else if (message == 1) //add condement...
            {
                orderData.addCondement(title, price, microsCheckItemID);
                
            }
            else if (message == 2) //clear order display
            {
                orderData.orderItems.Clear();
                orderData.tax = 0;
                orderData.discount = 0;
                formMgr.UpdateOrder();
            }
            else if (message == 3) //add discount
            {
                orderData.discount = discount;
                formMgr.UpdateOrder();
            }
            else if (message == 4) //change to other screen
            {

            }
        }
    }
}
