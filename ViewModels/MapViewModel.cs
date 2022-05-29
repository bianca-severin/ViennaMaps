using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ViennaMaps.Commands;
using System.Windows.Forms;

namespace ViennaMaps.ViewModels
{
    internal class MapViewModel: BaseViewModel
    {
        public event EventHandler OnRequestClose;

        public ICommand VerlassenCmd { get; set; }

        public MapViewModel()
        {
            VerlassenCmd = new RelayCommand(Verlassen);
        }

        private void Verlassen()
        {
            if (OnRequestClose != null)
                OnRequestClose(this, new EventArgs());
        }
    }
}
