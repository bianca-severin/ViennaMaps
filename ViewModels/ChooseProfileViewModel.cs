using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ViennaMaps.Commands;
using ViennaMaps.Models;

namespace ViennaMaps.ViewModels
{
    internal class ChooseProfileViewModel : BaseViewModel
    {
        //Events - open additional windows
        public event EventHandler OnRequestOpenMainWindow;
        public event EventHandler OnRequestOpenNewProfile;


        //Commands
        public ICommand ExitCmd { get; set; }
        //Events
        public event EventHandler OnRequestClose;

        // Properties
        public List<string> DistrictName { get; set; }

        public List<string> ProjectName { get; set; }

        public string SelectedLocation { get; set; }
        public string SelectedProject { get; set; }


        //Commands - view additional windows
        public ICommand ViewMainWindowCmd{ get; set; }
        public ICommand ViewNewProfileCmd { get; set; }


        //Constructor
        public ChooseProfileViewModel()
        {
            DistrictName = new List<string>();
            ProjectName = new List<string>();
            ExitCmd = new RelayCommand(Exit);
            //open main window
            ViewMainWindowCmd = new RelayCommand(ViewMainWindow);
            ViewNewProfileCmd = new RelayCommand(ViewNewProfile);


            FillLocationList();
            FillProfileList();
        }
        private void Exit()
        {
            if (OnRequestClose != null)
                OnRequestClose(this, new EventArgs());
        }
        private void ViewMainWindow()
        {
            if (OnRequestOpenMainWindow != null)
                OnRequestOpenMainWindow(this, new EventArgs());
        }
        private void ViewNewProfile()
        {
            if (OnRequestOpenNewProfile != null)
                OnRequestOpenNewProfile(this, new EventArgs());
        }

        #region Fill Dropdowns
        public void FillLocationList()
        {
            //empty list
            //LocationList.Clear();            

            using (UrbanAnalysisContext context = new UrbanAnalysisContext())
            {

                //show location ordered by District Number
                var locations = context.Location.OrderBy(l => l.DistrictNumber);
                // fill location list
                foreach (Location loc in locations)
                {
                    DistrictName.Add(loc.DistrictName);
                }
            }
        }

        public void FillProfileList()
        {

            ProjectName.Clear();

            using (UrbanAnalysisContext context = new UrbanAnalysisContext())
            {

                var project = context.Project.OrderBy(l => l.ProjectName);

                foreach (Project p in project)
                {
                    ProjectName.Add(p.ProjectName);
                }
            }
        }
        #endregion
    }
}
