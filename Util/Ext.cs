using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Stratus.Util
{
    public static class Ext
    {
        public static CoreCursor DefaultCoreCursor = Window.Current.CoreWindow.PointerCursor;

        public static readonly DependencyProperty CursorProperty = DependencyProperty.RegisterAttached(
            "Cursor",
            typeof(CoreCursorType),
            typeof(UIElement),
            new PropertyMetadata(false)
        );

        public static void SetCursor(UIElement element, CoreCursorType value)
        {
            var cursor = new CoreCursor(CoreCursorType.Hand, 1);
            element.SetValue(CursorProperty, value);
            element.PointerEntered += (sender, args) => Window.Current.CoreWindow.PointerCursor = cursor;
            element.PointerExited += (sender, args) => Window.Current.CoreWindow.PointerCursor = DefaultCoreCursor;
        }

        public static CoreCursorType GetCursor(UIElement element)
        {
            return (CoreCursorType)element.GetValue(CursorProperty);
        }
    }
}
