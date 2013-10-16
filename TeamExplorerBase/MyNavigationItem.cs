using System;
using System.ComponentModel.Composition;
using Microsoft.TeamFoundation.Controls;
using Microsoft.VisualStudio.Shell;
using TeamExplorerIntegration.Base;

namespace TeamExplorerIntegration
{
    [TeamExplorerNavigationItem(LINK_ID, 100)]
    public class MyNavigationItem : TeamExplorerBaseNavigationItem
    {
        public const string LINK_ID = "2075423C-9ECB-40D8-A7D7-7C0134263DB9";

        [ImportingConstructor]
        public MyNavigationItem([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            IsVisible = true;
            Text = "ToolWindow Demo";
            Image = Resources.Square;
        }

        public override void Execute()
        {
            var teamExplorer = GetService<ITeamExplorer>();

            if (teamExplorer != null)
            {
                teamExplorer.NavigateToPage(new Guid(GuidList.MY_PAGE_GUID), null);
            }
        }

        public override void Invalidate()
        {
            IsVisible = true;
        }
    }
}