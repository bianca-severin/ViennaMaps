using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ViennaMaps.Commands;
using System.Windows.Forms;
using ViennaMaps.Models;
using System.Collections.ObjectModel;


namespace ViennaMaps.ViewModels
{
    internal class Map2DViewModel : BaseViewModel
    {

        //Event
        public event EventHandler OnRequestClose;

        //Attribute
        private Layer _layer;

        public string Location {get; set;}
        public string Project { get; set;}

        //Commands
        public ICommand ExitCmd { get; set; }

        //Properties
        public ObservableCollection<Layer> LayerList { get; set; }
        private int _layerid;

        public Map2DViewModel( string project, string location)
        {
            ExitCmd = new RelayCommand(Exit);
            Location = location;
            Project = project;
        }

        private void Exit()
        {
            if (OnRequestClose != null)
                OnRequestClose(this, new EventArgs());
        }

        private void CreateLayers()
        {
            if(_layer == null)
            {
                Layer layer = new Layer();
                
            }
        }
    }
}
