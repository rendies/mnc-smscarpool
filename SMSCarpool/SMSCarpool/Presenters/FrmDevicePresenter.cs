using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMSCarpool.Views;
using SMSCarpool.Services;

namespace SMSCarpool.Presenters
{
    public class FrmDevicePresenter
    {
        IFrmDevice frmDevice;
        IDBConnectionService DBConnService;

        public FrmDevicePresenter(IFrmDevice FrmDeviceView)
        {
            frmDevice = FrmDeviceView;
        }

        public void chekcDBConnection()
        {
            if (frmDevice.frmKonfigurasi.Presenter.chekcDBConnection())
            {
                //frmDevice.ShowSelf();
                //frmDevice.frmKonfigurasi.HideSelf();
            } else
            {
                //frmDevice.HideSelf();
                //frmDevice.frmKonfigurasi.ShowSelf();
            }
        }

    }
}
