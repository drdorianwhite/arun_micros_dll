
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;

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
        public static DLLFormMgr formMgr = null;
        public static OrderData orderData = new OrderData();
        public static DisplayMode displayMode = DisplayMode.OpenOrder;
        public static string imagesDirectory = @"C:\\Micros\\images";
        
        [DllExport("cdshowcustomerdisplay")]
        public static void CDShowCustomerDisplay()
        {
            if(formMgr == null)
            {
               

                if (!Directory.Exists(imagesDirectory))
                    MessageBox.Show("image directory: " + imagesDirectory + " doesn't exist.  Cusomer display won't work!");
                else
                {
                    formMgr = new DLLFormMgr();
                }
            }
          
  
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
        public static void CDSendOrderData(int message, int itemid, [MarshalAs(UnmanagedType.LPStr)]string title, int quantity, [MarshalAs(UnmanagedType.LPStr)] string price, [MarshalAs(UnmanagedType.LPStr)] string taxTotal, [MarshalAs(UnmanagedType.LPStr)] string extra)
        {  
            
            if (message == 0) //add new item
            {
                orderData.addItem(title, quantity, float.Parse(price), itemid);
                orderData.tax = float.Parse(taxTotal);
                formMgr.UpdateOrder();
            }
            else if (message == 1) //add condement...
            {
                orderData.addCondement("  " + title, float.Parse(price), itemid);
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
                orderData.tax = float.Parse(taxTotal);
                orderData.discount = float.Parse(extra.Substring(0,extra.Length - 1));
                formMgr.UpdateOrder();
            }
            else if (message == 4) //update amount paid
            {
                orderData.SetTotalAndPaid(float.Parse(price), float.Parse(extra));
                formMgr.UpdateOrder();
            }
            else if(message == 5) //void item
            {
                orderData.voidItem(itemid);
                formMgr.UpdateOrder();
            }
            else
            {
                MessageBox.Show("isl script called cdsenddata in dll with bad message value");
            }
            
        }

        [DllExport("cdsenddata2")]
        public static void CDSendOrderData2(int arrayLen, [MarshalAs(UnmanagedType.LPArray, SizeConst = 900)]string[] types, [MarshalAs(UnmanagedType.LPArray, SizeConst = 900)]string[] titles,  [MarshalAs(UnmanagedType.LPArray, SizeConst = 900)]string[] prices, [MarshalAs(UnmanagedType.LPArray, SizeConst = 900)]string[] quantities, [MarshalAs(UnmanagedType.LPStr, SizeConst = 900)] string taxTotal)
        {
            orderData.orderItems.Clear();
            orderData.tax = float.Parse(taxTotal);
            orderData.discount = 0;
            orderData.amountPaid = 0;
            string discountName = "";
            int discountId = 0;

            for (int i = 0; i < arrayLen; i++)
            {
                if (types[i] == "M")
                {
                    orderData.addItem(titles[i], int.Parse(quantities[i]), float.Parse(prices[i]), i);
                }
                else if (types[i] == "C")
                {
                    orderData.addCondement("  " + titles[i], float.Parse(prices[i]), i);
                }
                else if (types[i] == "D")
                {
                    if (discountId == 0)
                    {
                        discountId = i;
                        discountName = titles[i];
                    }
                    orderData.discount += float.Parse(prices[i]);
                }
            }

            if(discountId != 0)
                orderData.addItem(discountName, 1, orderData.discount, discountId);


            formMgr.UpdateOrder();

        }
    }
}
