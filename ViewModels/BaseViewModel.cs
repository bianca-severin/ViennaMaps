using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;


namespace ViennaMaps.ViewModels
{
    /// <summary>
    /// Each of the ViewModel classes derives from BaseViewmodel. The BaseviewModel has an event
    /// 1) the view registers for the event by registering a delegate
    /// 2) when there is a change in data in the ViewModel, the change is reported by invoking the registered delegates
    /// 3) by calling a delegate, the Vsiew knows the changed data and updates the window
    /// </summary>
     internal class BaseViewModel : INotifyPropertyChanged
    {
        // Events
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                // tells the View (Window) that data has changedParameters: "this" is a reference to the ViewModel
                // propName is the name of the property whose value has changed
                // the registered delegates (reference to methods) are called;
                // redraw with the changed data in the window
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
