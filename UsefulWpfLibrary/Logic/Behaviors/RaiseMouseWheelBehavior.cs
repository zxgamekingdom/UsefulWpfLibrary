using System.Windows;
using Microsoft.Xaml.Behaviors;
using UsefulWpfLibrary.Logic.Tools;

namespace UsefulWpfLibrary.Logic.Behaviors
{
    public class RaiseMouseWheelBehavior : Behavior<FrameworkElement>
    {
        protected override void OnAttached()
        {
            AssociatedObject.PreviewMouseWheel +=
                RaiseMouseWheelTools.ElementOnPreviewMouseWheel;
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            AssociatedObject.PreviewMouseWheel -=
                RaiseMouseWheelTools.ElementOnPreviewMouseWheel;
            base.OnDetaching();
        }
    }
}
