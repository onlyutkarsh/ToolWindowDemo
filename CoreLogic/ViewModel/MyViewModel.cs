using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLogic.Common;
using CoreLogic.Service;
using Microsoft.TeamFoundation.Client;
using Microsoft.VisualStudio.Shell.Interop;

namespace CoreLogic.ViewModel
{
    public class MyViewModel : ViewModelBase
    {
        private IServiceProvider _serviceProvider;
        private ObservableCollection<string> _names = new ObservableCollection<string>
        {
            "John Doe",
            "James Franklin",
            "Henry Ipsum"
        };

        private DelegateCommand _clickOnNameCommand;

        public MyViewModel(IServiceProvider serviceProvider, ITeamFoundationContext currentContext)
        {
            _serviceProvider = serviceProvider;
        }

        public ObservableCollection<string> Names
        {
            get
            {
                return _names;
            }
            set
            {
                _names = value;
                RaisePropertyChanged(() => Names);
            }
        }

        public DelegateCommand ClickOnName
        {
            get
            {
                return _clickOnNameCommand ?? (_clickOnNameCommand = new DelegateCommand(OnNameClick));
            }
        }

        private void OnNameClick(object obj)
        {
            var name = (string) obj;

            //Just show the toolWindow
            //IVsUIShell service = _serviceProvider.GetService(typeof(IVsUIShell)) as IVsUIShell;

            //if (service != null)
            //{
            //    IVsWindowFrame winFrame;
            //    var guidNo = new Guid(GuidList.MY_TOOL_WINDOW_GUID);
            //    if (service.FindToolWindowEx(0x80000, ref guidNo, 0, out winFrame) >= 0 && winFrame != null)
            //    {
            //        winFrame.Show();
            //    }
            //}

            var toolWindowManager = _serviceProvider.GetService(typeof (SToolWindowManager)) as IToolWindowManager;
            if (toolWindowManager != null)
            {
                toolWindowManager.PassNameAndOpenToolWindow(name);
            }
        }
    }
}
