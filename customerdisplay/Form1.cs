using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace customerdisplay
{
    public partial class Form1 : Form
    {
        bool foundSecondDisplay = false;
        private DLLFormMgr formManager;
        public string[] picturePaths;
        private int displayedPictureIndex = 0;

        public void SetPictureDirectory()
        {
            
            picturePaths = Directory.GetFiles(CustomerDisplay.imagesDirectory, "*.jpg");
           
            displayedPictureIndex = 0;
        }


        internal void UpdateDisplayOrder()
        {
            listView1.Items.Clear();
            
            foreach(OrderData.OrderItem item in CustomerDisplay.orderData.orderItems)
            {
                ListViewItem i = new ListViewItem(item.quantity + "x " + item.itemName);
                i.SubItems.Add(item.price.ToString());
                listView1.Items.Add(i);
            }
            float subtotal = CustomerDisplay.orderData.getSubtotal();
            float tax = CustomerDisplay.orderData.tax;
            float discount = CustomerDisplay.orderData.discount;
            float total = subtotal + tax + discount;

            //listView2.Items[0].SubItems[1].Text = "$" + subtotal.ToString();
            //listView2.Items[1].SubItems[1].Text = "$" + tax.ToString();
            //listView2.Items[3].SubItems[1].Text = "$" + total;
            listView2.Items[0].SubItems[1].Text = String.Format("{0:C}", subtotal);
            listView2.Items[1].SubItems[1].Text = String.Format("{0:C}", tax);
            listView2.Items[2].SubItems[1].Text = String.Format("{0:C}", discount);
            listView2.Items[3].SubItems[1].Text = listView2.Items[1].SubItems[1].Text = String.Format("{0:C}", total);

        }

        internal void UpdateDisplayMode()
        {
            throw new NotImplementedException();
        }

        public Form1()
        {
            InitializeComponent();

            if (Screen.AllScreens.Length < 2)
            {
                //throw new Exception("second display not found");
                AutoClosingMessageBox.Show("Second Display not found (creating test window)", "Micros", 2000);
            }
            else
            {
                foundSecondDisplay = true;
                this.Location = Screen.AllScreens[1].WorkingArea.Location;
            }

            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            SetPictureDirectory();
            timer1.Start();
        }

        private void OnLoad(object sender, EventArgs e)
        {
           
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(picturePaths != null && picturePaths.Length != 0)
            {
                if (displayedPictureIndex == picturePaths.Length)
                    displayedPictureIndex = 0;
                    
                pictureBox1.Load(picturePaths[displayedPictureIndex]);
                displayedPictureIndex++;
            }
        }
    }
}
