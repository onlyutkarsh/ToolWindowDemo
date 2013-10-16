using CoreLogic.Views;
using Microsoft.TeamFoundation.Controls;
using TeamExplorerIntegration.Base;

namespace CoreLogic
{
    [TeamExplorerSection(GuidList.MY_SECTION_GUID, TeamExplorerIntegration.GuidList.MY_PAGE_GUID, 10)]
    public class MySection : TeamExplorerBaseSection
    {
        private MyToolWindow _window;

        public MySection()
        {
            Title = "User Names";
        }

        public override void Initialize(object sender, SectionInitializeEventArgs e)
        {
            base.Initialize(sender, e);

            SectionContent = new MySectionContent(ServiceProvider, CurrentContext);
        }
    }
}