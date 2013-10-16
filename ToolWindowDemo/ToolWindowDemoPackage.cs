using CoreLogic.Service;
using CoreLogic.Views;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Utkarsh.ToolWindowDemo
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // This attribute is used to register the information needed to show this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [Guid(GuidList.guidToolWindowDemoPkgString)]
    [ProvideToolWindow(typeof(MyToolWindow), MultiInstances = false, Style = VsDockStyle.MDI, Transient = true)]
    [ProvideService(typeof(SToolWindowManager))]
    public sealed class ToolWindowDemoPackage : Package, IToolWindowManager, SToolWindowManager
    {
        /// <summary>
        /// Default constructor of the package.
        /// Inside this method you can place any initialization code that does not require
        /// any Visual Studio service because at this point the package object is created but
        /// not sited yet inside Visual Studio environment. The place to do all the other
        /// initialization is the Initialize method.
        /// </summary>
        public ToolWindowDemoPackage()
        {
            IServiceContainer serviceContainer = this;
            ServiceCreatorCallback creationCallback = CreateService;
            serviceContainer.AddService(typeof(SToolWindowManager), creationCallback, true);
        }

        private object CreateService(IServiceContainer container, Type serviceType)
        {
            if (container != this)
            {
                return null;
            }

            if (typeof(SToolWindowManager) == serviceType)
            {
                return this;
            }

            return null;
        }

        /////////////////////////////////////////////////////////////////////////////
        // Overridden Package Implementation

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this.ToString()));
            base.Initialize();
        }

        #endregion Package Members

        public void PassNameAndOpenToolWindow(string name)
        {
            ToolWindowPane windowPane = FindToolWindow(typeof(MyToolWindow), 0, true);
            var control = windowPane.Content as MyToolWindowContent;
            if (control != null)
            {
                var frame = windowPane.Frame as IVsWindowFrame;
                if (frame != null)
                {
                    frame.Show();
                }
                control.ClickedName = name;
            }
        }
    }
}