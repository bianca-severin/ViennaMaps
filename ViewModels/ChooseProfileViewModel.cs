using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using ViennaMaps.Commands;
using System.Collections.ObjectModel;
using ViennaMaps.Models;

namespace ViennaMaps.ViewModels
{
    /// <summary>
    /// Viewmodel for <c>ChooseProfileWindow</c> 
    /// The class fills two ComboBoxes with the database list of locations and profiles
    /// The user chooses one profile and one location. 
    /// The user can open the Main Window only if a profile and location are selected
    /// </summary>
    internal class ChooseProfileViewModel : BaseViewModel
    {
        // Events
        public event EventHandler OnRequestOpenMainWindow;
        public event EventHandler OnRequestClose;

        // Commands
        public ICommand ExitCmd { get; set; }
        public ICommand ViewMainWindowCmd { get; set; }

        // Properties
        public List<string> DistrictName { get; set; }
        public List<string> ProjectName { get; set; }
        public string SelectedLocation { get; set; }
        public string SelectedProject { get; set; }

        // Constructor
        public ChooseProfileViewModel()
        {
            // create district and project list
            DistrictName = new List<string>();
            ProjectName = new List<string>();

            // bind command between the ViewModel and UI elements
            ExitCmd = new RelayCommand(Exit);
            ViewMainWindowCmd = new RelayCommand(ViewMainWindow);

            // fill ComboBoxes
            FillLocationList();
            FillProfileList();
        }

        // Exit the current window
        private void Exit()
        {
            if (OnRequestClose != null)
                OnRequestClose(this, new EventArgs());
        }

        // ViewMainWindow opens the MainWindow 
        private void ViewMainWindow()
        {
            // error/message box if no project or location was selected by the user
            if (SelectedLocation == null || SelectedProject == null)
            {
                DialogResult box = MessageBox.Show("Please select a profile and district for your project", "Error", MessageBoxButtons.OK);
                return;
            }
            // open Main Window
            if (OnRequestOpenMainWindow != null)
                OnRequestOpenMainWindow(this, new EventArgs());
        }

        #region Fill Dropdowns

        // fill Location ComboBox with the list of available districts in the database
        public void FillLocationList()
        {
            // empty districts list
            DistrictName.Clear();            

            using (UrbanAnalysisContext context = new UrbanAnalysisContext())
            {
                // show location ordered by district number
                var locations = context.Location.OrderBy(l => l.DistrictId);
                // add locations to the list
                foreach (Location loc in locations)
                    DistrictName.Add(loc.DistrictName);
            }
        }

        // fill Profile ComboBox with the list of available profiles in he database
        public void FillProfileList()
        {
            // empty projects list
            ProjectName.Clear();

            using (UrbanAnalysisContext context = new UrbanAnalysisContext())
            {
                // show projects ordered by project name
                var project = context.Project.OrderBy(l => l.ProjectName);
                // add projects bame to the list
                foreach (Project p in project)
                    ProjectName.Add(p.ProjectName);
            }
        }

        #endregion
    }
}
