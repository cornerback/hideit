using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Runtime.InteropServices.ComTypes;
using DirectShowLib;
using System.Runtime.InteropServices;

namespace HideIt.Live
{
    public partial class SelectDevice : Form
    {
        private const string TITLE = "Select Device";

        public SelectDevice()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Get selected capture moniker instance
        /// </summary>
        public Device CaptureDevice { get; set; }

        /// <summary>
        /// Enumerate the devices and show them in ListView
        /// </summary>
        private void EnumerateDevices()
        {
            ICreateDevEnum deviceEnum = null;
            IEnumMoniker monikerEnum = null;
            IMoniker[] monikers = new IMoniker[1];

            int hr = 0;

            try
            {
                // create the device enumerator (COM) object
                Type t = Type.GetTypeFromCLSID(ComGuids.DevEnumGuid);
                deviceEnum = (ICreateDevEnum)Activator.CreateInstance(t);
                
                hr = deviceEnum.CreateClassEnumerator(ComGuids.VidCapGuid, out monikerEnum, CDef.None);
                if (hr != 0)
                {
                    UIMessage.Error("No devices of the category", TITLE);
                    return;
                }

                while (monikerEnum.Next(1, monikers, IntPtr.Zero) == 0)
                {
                    string displayName = this.GetFriendlyName(monikers[0]);

                    ListViewItem item = new ListViewItem(displayName);
                    item.Tag = new Device(displayName, monikers[0]);
                    this.lstDevices.Items.Add(item);
                }
            }
            catch (Exception exc)
            {
                UIMessage.Error(exc.Message, TITLE);
            }
            finally
            {
                if (deviceEnum != null)
                {
                    Marshal.ReleaseComObject(deviceEnum);
                }
                if (monikerEnum != null)
                {
                    Marshal.ReleaseComObject(monikerEnum);
                }
            }
        }

        /// <summary>
        /// Get friendly name of the device
        /// </summary>
        /// <param name="monikers"></param>
        /// <param name="displayName"></param>
        /// <returns></returns>
        private string GetFriendlyName(IMoniker moniker)
        {
            int hr = 0;
            string displayName = string.Empty;
            ///Get selected device name
            object bagObj = null;
            object propValue = null;
            IPropertyBag monikerProperty = null;

            Guid propertyBagGuid = typeof(IPropertyBag).GUID;
            moniker.BindToStorage(null, null, ref propertyBagGuid, out bagObj);
            monikerProperty = (IPropertyBag)bagObj;
            hr = monikerProperty.Read("FriendlyName", out propValue, null);

            displayName = propValue as string;
            if (hr != 0 || string.IsNullOrEmpty(displayName))
            {
                displayName = "Unknown";
            }

            if (bagObj != null)
            {
                Marshal.ReleaseComObject(bagObj);
            }

            return displayName;
        }

        /// <summary>
        /// Release all the resources used.
        /// </summary>
        private void Cleanup()
        {
            foreach (ListViewItem item in this.lstDevices.Items)
            {
                Device device = item.Tag as Device;
                device.Dispose();
            }

            this.lstDevices.Items.Clear();
        }

        private void SelectDevice_Load(object sender, EventArgs e)
        {
            this.EnumerateDevices();
        }

        private void select_Click(object sender, EventArgs e)
        {
            if (this.lstDevices.SelectedItems.Count > 0)
            {
                ListViewItem selected = this.lstDevices.SelectedItems[0];
                this.lstDevices.Items.RemoveAt(selected.Index);
                this.CaptureDevice = selected.Tag as Device;
            }

            this.Cleanup();
            this.Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.Cleanup();
            this.Close();
        }

        private void lstDevices_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.select_Click(sender, e);
        }
    }
}
