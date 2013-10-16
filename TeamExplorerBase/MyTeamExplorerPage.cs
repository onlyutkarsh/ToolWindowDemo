using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Controls;
using TeamExplorerIntegration.Base;

namespace TeamExplorerIntegration
{
    [TeamExplorerPage(GuidList.MY_PAGE_GUID)]
    public class MyTeamExplorerPage : TeamExplorerBasePage
    {
        public MyTeamExplorerPage()
        {
            Title = "ToolWindow Demo";
        }
    }
}
