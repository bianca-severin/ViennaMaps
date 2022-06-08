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
        public event EventHandler OnRequestOpen2DMap;
        public event EventHandler OnRequestOpenNewProfile;

        public ICommand ViewMapCmd { get; set; }
        public ICommand View2DMapCmd { get; set; }

        public ICommand ViewNewProfileCmd { get; set; }

        public MainViewModel()
        {

            ViewMapCmd = new RelayCommand(ViewMap);
            View2DMapCmd = new RelayCommand(View2DMap);
            ViewNewProfileCmd = new RelayCommand(ViewNewProfile);
        }

        private void ViewMap()
        {
            if (OnRequestOpenMap != null)
                OnRequestOpenMap(this, new EventArgs());
        }

        private void View2DMap()
        {
            if (OnRequestOpen2DMap != null)
                OnRequestOpen2DMap(this, new EventArgs());
        }

        private void ViewNewProfile()
        {
            if (OnRequestOpenNewProfile != null)
                OnRequestOpenNewProfile(this, new EventArgs());
        }
    }
}
