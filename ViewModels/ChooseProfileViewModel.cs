using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ViennaMaps.Commands;

namespace ViennaMaps.ViewModels
{
    internal class ChooseProfileViewModel : BaseViewModel
    {
        //Events - open additional windows
        public event EventHandler OnRequestOpenMainWindow;


        //Commands - view additional windows
        public ICommand ViewMainWindowCmd{ get; set; }

        public ChooseProfileViewModel()
        {
            ViewMainWindowCmd = new RelayCommand(ViewMainWindow);
        }

        private void ViewMainWindow()
        {
            if (OnRequestOpenMainWindow != null)
                OnRequestOpenMainWindow(this, new EventArgs());
        }
    }
}
