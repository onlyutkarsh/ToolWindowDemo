using System.Runtime.InteropServices;

namespace CoreLogic.Service
{
    [Guid("0C7C91F7-CA87-49D2-AE9E-BC1AEA81CC3C")]
    [ComVisible(true)]
    public interface IToolWindowManager
    {
        void PassNameAndOpenToolWindow(string name);
    }
}
