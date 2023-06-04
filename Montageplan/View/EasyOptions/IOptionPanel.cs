using System.Windows;

namespace Montageplan.EasyOptions
{
    public interface IOptionPanel
    {
        string PanelName { get; }
        string PanelDescription { get;  }
        UIElement View { get; }
    }
}
