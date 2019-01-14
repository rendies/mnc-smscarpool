using SMSCarpool.Presenters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SMSCarpool.Views
{
    public interface IFrmKonfigurasi
    {
        FrmKonfigurasiPresenter Presenter { get; set; }
        void HideSelf();
        void ShowSelf();
        DialogResult ShowDialogSelf();
    }
}
