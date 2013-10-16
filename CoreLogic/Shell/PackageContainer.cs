using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace CoreLogic.Shell
{
    /// <devdoc>
    ///     This class derives from container to provide a service provider
    ///     connection to the package.
    /// </devdoc>
    internal sealed class PackageContainer : Container
    {
        private readonly IServiceProvider _provider;
        private AmbientProperties _ambientProperties;
        private IUIService _uiService;

        //Creates a new container using the given service provider.
        internal PackageContainer(IServiceProvider provider)
        {
            _provider = provider;
        }

        //     Override to GetService so we can route requests
        //     to the package's service provider.
        protected override object GetService(Type serviceType)
        {
            if (serviceType == null)
                throw new ArgumentNullException("serviceType");

            if (_provider != null)
            {
                if (serviceType == typeof(AmbientProperties))
                {
                    if (_uiService == null)
                        _uiService = (IUIService)_provider.GetService(typeof(IUIService));

                    if (_ambientProperties == null)
                        _ambientProperties = new AmbientProperties();

                    if (_uiService != null)
                        // update the _ambientProperties in case the styles have changed
                        // since last time.
                        _ambientProperties.Font = (Font)_uiService.Styles["DialogFont"];

                    return _ambientProperties;
                }
                object service = _provider.GetService(serviceType);

                if (service != null)
                    return service;
            }

            return base.GetService(serviceType);
        }
    }
}