using System;
using System.ComponentModel.Composition;
using Avanade.TeamExplorerIntegration.Base;
using Microsoft.TeamFoundation.Controls;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace Avanade.TeamExplorerIntegration
{
    [TeamExplorerNavigationLink(LINK_ID, "523D0B83-3894-45E1-B252-0BE24FC653D1", 100)]
    public class TeamProjectPermissionsLink : TeamExplorerBaseNavigationLink
    {
        public const string LINK_ID = "5447A654-D328-4606-ADD6-2CBED21B3443";

        /// <summary>
        /// Constructor.
        /// </summary>
        [ImportingConstructor]
        public TeamProjectPermissionsLink([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            Text = "Team Project Permissions";
            IsVisible = true;
            IsEnabled = true;

        }
        /// <summary>
        /// Execute the link action.
        /// </summary>
        public override void Execute()
        {
            // Navigate to the recent changes page
            ITeamExplorer teamExplorer = GetService<ITeamExplorer>();

            if (teamExplorer != null)
            {
                var service = GetService<IVsUIShell>();

                IVsWindowFrame winFrame;

                var guidNo = new Guid("71db4cd7-5a90-41fb-8b12-fa308df3a328");

                if (service.FindToolWindowEx(0x80000, ref guidNo, 0, out winFrame) >= 0 && winFrame != null)
                {
                    winFrame.Show();
                }
            }
        }

        public override void Invalidate()
        {
            base.Invalidate();
            IsEnabled = true;
            IsVisible = true;
        }
    }
}
