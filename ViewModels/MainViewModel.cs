using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViennaMaps.Commands;
using System.Windows.Input;
using System.Windows.Forms;
using System.Collections.ObjectModel;

namespace ViennaMaps.ViewModels
{
    internal class MainViewModel: BaseViewModel
    {
        public event EventHandler OnRequestOpenMap;


        public ICommand ViewMapCmd { get; set; }

        public MainViewModel()
        {

            ViewMapCmd = new RelayCommand(ViewMap);
        }

        private void ViewMap()
        {
            if (OnRequestOpenMap != null)
                OnRequestOpenMap(this, new EventArgs());
        }
    }
}
