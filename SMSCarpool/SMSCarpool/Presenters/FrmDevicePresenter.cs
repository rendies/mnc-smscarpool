using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMSCarpool.Views;

namespace SMSCarpool.Presenters
{
    public class FrmDevicePresenter
    {
        IFrmDevice FrmDevice;

        public FrmDevicePresenter(IFrmDevice FrmDeviceView)
        {
            FrmDevice = FrmDeviceView;
            FrmDeviceView.Presenter = this;
        }
    }
}
