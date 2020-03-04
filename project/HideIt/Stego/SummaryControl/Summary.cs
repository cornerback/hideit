using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HideIt.Stego.SummaryControl
{
    public class Summary : UserControl
    {
        /// <summary>
        /// Get or set message size label value
        /// </summary>
        public virtual string MessageSize
        {
            get { return string.Empty; }
            set { }
        }

        /// <summary>
        /// Get or set hiding capacity label value
        /// </summary>
        public virtual string HidingCapacity
        {
            get { return string.Empty; }
            set { }
        }
    }
}
