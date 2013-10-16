using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace CoreLogic.Shell
{
    public class PackageAssist : IVsPackage, IDisposable
    {
        //FROM: http://tytannet.googlecode.com/svn/trunk/src/TytanAddInSolution/TytanCore/Data/PackageAssist.cs

        private readonly IServiceProvider serviceProvider;
        private Hashtable toolWindows;          // this is the list of all toolwindows
        private Container componentToolWindows; // this is the toolwindows that implement IComponent

        public PackageAssist(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        /// <include file='doc\Package.uex' path='docs/doc[@for="Package.CreateToolWindow"]/*' />
        /// <devdoc>
        /// Create a tool window of the specified type with the specified ID.
        /// </devdoc>
        /// <param name="toolWindowType">Type of the window to be created</param>
        /// <param name="id">Instance ID</param>
        /// <returns>An instance of a class derived from ToolWindowPane</returns>
        /// <param name="transient">Should creation of this window be forced ?</param>
        /// <param name="multiInstances">Is this window multiinstanced ?</param>
        protected ToolWindowPane CreateToolWindow(Type toolWindowType, int id, bool transient, bool multiInstances)
        {
            if (toolWindowType == null)
                throw new ArgumentNullException("toolWindowType");
            if (id < 0)
                throw new ArgumentOutOfRangeException("id");
            if (!toolWindowType.IsSubclassOf(typeof(ToolWindowPane)))
                throw new ArgumentException("toolWindowType");

            ///////////////////////////////////////////////////////////////////////////////////////
            //
            // this is the only method that should be calling IVsUiShell.CreateToolWindow()


            // First create an instance of the ToolWindowPane
            ToolWindowPane window = (ToolWindowPane)Activator.CreateInstance(toolWindowType);

            // Check if this window has a ToolBar
            bool hasToolBar = (window.ToolBar != null);

            uint flags = (uint)__VSCREATETOOLWIN.CTW_fInitNew;
            if (!transient)
                flags |= (uint)__VSCREATETOOLWIN.CTW_fForceCreate;
            if (hasToolBar)
                flags |= (uint)__VSCREATETOOLWIN.CTW_fToolbarHost;
            if (multiInstances)
                flags |= (uint)__VSCREATETOOLWIN.CTW_fMultiInstance;
            Guid emptyGuid = Guid.Empty;
            Guid toolClsid = window.ToolClsid;
            IVsWindowPane windowPane = null;
            if (toolClsid.CompareTo(Guid.Empty) == 0)
            {
                // If a tool CLSID is not specified, then host the IVsWindowPane
                windowPane = window.GetIVsWindowPane() as IVsWindowPane;
            }
            Guid persistenceGuid = toolWindowType.GUID;
            IVsWindowFrame windowFrame;
            // Use IVsUIShell to create frame.
            IVsUIShell vsUiShell = (IVsUIShell)serviceProvider.GetService(typeof(SVsUIShell));
            if (vsUiShell == null)
                throw new Exception(string.Format("Missing service: {0}", typeof(SVsUIShell).FullName));

            int hr = vsUiShell.CreateToolWindow(flags, // flags
                                                (uint)id, // instance ID
                                                windowPane,
                // IVsWindowPane to host in the toolwindow (null if toolClsid is specified)
                                                ref toolClsid,
                // toolClsid to host in the toolwindow (Guid.Empty if windowPane is not null)
                                                ref persistenceGuid, // persistence Guid
                                                ref emptyGuid, // auto activate Guid
                                                null, // service provider
                                                window.Caption, // Window title
                                                null,
                                                out windowFrame);
            //ErrorHandler.ThrowOnFailure(hr);

            window.Package = null; // this;

            // If the toolwindow is a component, site it.
            IComponent component = null;
            if (window.Window is IComponent)
                component = (IComponent)window.Window;
            else if (windowPane is IComponent)
                component = (IComponent)windowPane;
            if (component != null)
            {
                if (componentToolWindows == null)
                    componentToolWindows = new PackageContainer(window);
                componentToolWindows.Add(component);
            }

            // This generates the OnToolWindowCreated event on the ToolWindowPane
            window.Frame = windowFrame;

            if (hasToolBar && windowFrame != null)
            {
                // Set the toolbar
                IVsToolWindowToolbarHost toolBarHost;
                object obj;
                ErrorHandler.ThrowOnFailure(windowFrame.GetProperty((int)__VSFPROPID.VSFPROPID_ToolbarHost, out obj));
                toolBarHost = (IVsToolWindowToolbarHost)obj;
                if (toolBarHost != null)
                {
                    Guid toolBarCommandSet = window.ToolBar.Guid;
                    ErrorHandler.ThrowOnFailure(
                        toolBarHost.AddToolbar((VSTWT_LOCATION)window.ToolBarLocation, ref toolBarCommandSet,
                                               (uint)window.ToolBar.ID));
                }
            }

            window.OnToolBarAdded();

            // keep track of created window:
            if (toolWindows == null)
                toolWindows = new Hashtable();
            toolWindows.Add(GetHash(toolWindowType.GUID, id), window);

            return window;
        }


        /// <include file='doc\Package.uex' path='docs/doc[@for="Package.0lWindow"]/*' />
        /// <devdoc>
        /// Return the tool window corresponding to the specified type and ID.
        /// If it does not exist, it returns creates one if create is true,
        /// or null if create is false.
        /// </devdoc>
        /// <param name="toolWindowType">Type of the window to be created</param>
        /// <param name="id">Instance ID</param>
        /// <param name="create">Create if none exist?</param>
        /// <param name="transient">Should creation of this window be forced ?</param>
        /// <param name="multiInstances">Is this window multiinstanced ?</param>
        /// <returns>An instance of a class derived from ToolWindowPane</returns>
        public ToolWindowPane FindToolWindow(Type toolWindowType, int id, bool create, bool transient, bool multiInstances)
        {
            if (toolWindowType == null)
                throw new ArgumentNullException("toolWindowType");

            ToolWindowPane window = null;

            int hash = GetHash(toolWindowType.GUID, id);
            if (toolWindows != null && toolWindows.ContainsKey(hash))
                window = (ToolWindowPane)toolWindows[hash];
            else if (create)
                window = CreateToolWindow(toolWindowType, id, transient, multiInstances);

            return window;
        }

        /// <summary>
        /// Shows (or creates if needed) the specified tool window.
        /// </summary>
        public static void ShowToolWindow(Type toolWindowType, PackageAssist assist, bool transient, bool multiInstances)
        {
            // Get the instance number 0 of given tool window. Silent assumption is that this window
            // is single instance so this instance is actually the only one.
            // The last flag is set to true so that if the tool window does not exists it will be created.
            ToolWindowPane window = assist.FindToolWindow(toolWindowType, 0, true, transient, multiInstances);
            if (window == null || window.Frame == null || !(window.Frame is IVsWindowFrame))
                throw new COMException("Can not create IVsWindowFrame object");

            ErrorHandler.ThrowOnFailure(((IVsWindowFrame)window.Frame).Show());
        }

        private static int GetHash(Guid guid, int id)
        {
            return guid.ToString("N").GetHashCode() ^ id;
        }

        #region IDisposable Members

        /// <summary>
        /// Dispose used resources.
        /// </summary>
        public void Dispose()
        {
            // Dispose all IComponent ToolWindows
            //
            if (componentToolWindows != null)
            {
                Container ctw = componentToolWindows;
                componentToolWindows = null;
                try
                {
                    ctw.Dispose();
                }
                catch (Exception e)
                {
                    Debug.Fail(String.Format("Failed to dispose component toolwindows for package {0}\n{1}", GetType().FullName, e.Message));
                }

                GC.SuppressFinalize(this);
            }
        }

        #endregion

        #region IVsPackage Members

        public int SetSite(Microsoft.VisualStudio.OLE.Interop.IServiceProvider psp)
        {
            return 0;
        }

        public int QueryClose(out int pfCanClose)
        {
            pfCanClose = 0;
            return 0;
        }

        public int Close()
        {
            return 0;
        }

        public int GetAutomationObject(string pszPropName, out object ppDisp)
        {
            ppDisp = null;
            return 0;
        }

        public int CreateTool(ref Guid rguidPersistenceSlot)
        {
            return 0;
        }

        public int ResetDefaults(uint grfFlags)
        {
            return 0;
        }

        public int GetPropertyPage(ref Guid rguidPage, VSPROPSHEETPAGE[] ppage)
        {
            return 0;
        }

        #endregion
    }
}
