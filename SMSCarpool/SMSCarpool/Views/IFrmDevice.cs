﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMSCarpool.Presenters;

namespace SMSCarpool.Views
{
    public interface IFrmDevice
    {
        FrmDevicePresenter Presenter { get; set; }
        IFrmKonfigurasi frmKonfigurasi { get; set; }
        void HideSelf();
        void ShowSelf();
    }
}
