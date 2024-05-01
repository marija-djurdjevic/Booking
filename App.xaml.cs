using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using BookingApp;
using System.Windows.Automation;

namespace BookingApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public GlobalVariables globalVariables;
        public static GlobalVariables StaticGlobalVariables => ((App)Current).globalVariables;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            globalVariables = new GlobalVariables();
        }
    }

}
