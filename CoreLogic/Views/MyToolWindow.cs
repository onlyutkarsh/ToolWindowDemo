using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using System.Runtime.InteropServices;

namespace CoreLogic.Views
{
    [Guid(GuidList.MY_TOOL_WINDOW_GUID)]
    public class MyToolWindow : ToolWindowPane
    {
        public MyToolWindow()
        {
            Caption = Resources.MyToolWindowTitle;
        }

        protected override void Initialize()
        {
            base.Initialize();
            base.Content = new MyToolWindowContent(this);
        }
    }
}
