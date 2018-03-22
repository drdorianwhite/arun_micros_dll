using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using customerdisplay;
using System.Collections.Generic;


namespace customerdisplay
{
    public class DLLFormMgr
    {
        private Form1 displayWindow = null;
        private ApplicationContext appContext;

        private delegate void SetDisplayMode(CustomerDisplay.DisplayMode mode);
        private delegate void UpdateOrderDisplay ();
        private delegate void UpdateDisplayModeMethod();
        private delegate void DllUnloadHandler();

        private void OnMessageLoopEnd()
        {

        }

        private void OnThreadExit(object sender, EventArgs e)
        {
            if(appContext.MainForm != null)
            { 
                appContext.MainForm.Dispose();
                appContext.MainForm = null;
            }
            displayWindow = null;
            appContext.Dispose();
            appContext = null;
        }

        public DLLFormMgr()
        {
            Thread t;

            if (appContext == null)
            {
       
                t = new Thread(new ThreadStart(this.ThreadProc));
                t.IsBackground = true;
                t.SetApartmentState(ApartmentState.STA);
                t.Start();
            }
        }

        internal void UpdateDisplayMode()
        {
            Thread.Sleep(50);

            if (appContext == null)
                MessageBox.Show("calling updatedisplay before display window launched");
            else
                appContext.MainForm.Invoke(new UpdateDisplayModeMethod(DispatchUpdateDisplayMode));
        }

        private void DispatchUpdateDisplayMode()
        {
            displayWindow.UpdateDisplayMode();   
        }

        private void ThreadProc()
        {
            
            displayWindow = new Form1();
            displayWindow.Show();
            appContext = new ApplicationContext(displayWindow);
            appContext.MainForm.Activate();
            Application.ThreadExit += new EventHandler(OnThreadExit);
            Application.Run(appContext);
        }


        public void UpdateOrder()
        {
            Thread.Sleep(50);
            UpdateOrderDisplay function = new UpdateOrderDisplay(DispatchUpdateOrderDisplay);
            appContext.MainForm.Invoke(function);
        }

        private void DispatchUpdateOrderDisplay()
        {
            displayWindow.UpdateDisplayOrder();
        }
    }
}