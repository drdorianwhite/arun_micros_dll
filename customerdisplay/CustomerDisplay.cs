
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

            MessageBox.Show(mode.ToString());
            displayMode = (DisplayMode)mode;

            if(formMgr == null)
            {
                formMgr = new DLLFormMgr();
            }
           

            return true;
        }

        [DllExport("cdsetdisplaymode")]
        public static void CDSetDisplayMode(int mode)
        {
            if (mode == 1)
            {
                displayMode = DisplayMode.OpenOrder;
            }
                else //if (mode == 2)
            {
                displayMode = DisplayMode.PaidOrder;
            }

            formMgr.UpdateDisplayMode();
        }


        [DllExport("cdsenddata")]
        public static void CDSendOrderData(int message, int microsCheckItemID, [MarshalAs(UnmanagedType.LPStr)]  string title, int quanitity, [MarshalAs(UnmanagedType.LPStr)] string priceString, [MarshalAs(UnmanagedType.LPStr)] string taxTotalString, [MarshalAs(UnmanagedType.LPStr)] string extraVal)
        {
            float price = float.Parse(priceString);
            float taxTotal = float.Parse(taxTotalString);
            
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
                formMgr.UpdateOrder();

            }
            else if (message == 2) //clear order display (cancel order)
            {
                orderData.orderItems.Clear();
                orderData.tax = 0;
                orderData.discount = 0;
                orderData.amountPaid = 0;
                formMgr.UpdateOrder();
            }
            else if (message == 3) //update discount
            {
                orderData.discount = float.Parse(extraVal);
                formMgr.UpdateOrder();
            }
            else if (message == 4) //update amount paid
            {
                orderData.amountPaid = float.Parse(extraVal);
                formMgr.UpdateOrder();
            }
            else if(message == 5) //void item
            {

            }
            else
            {
                MessageBox.Show("isl script called cdsenddata in dll with bad message value");
            }
        }
    }
}
