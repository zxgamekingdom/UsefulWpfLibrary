using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace UsefulWpfLibrary.Views.CustomControls.MultiValueEditor.Tools
{
    internal static class MultiValueEditorCreateDefaultSinaValueEditorTools
    {
        internal static FrameworkElement CreateDefaultSingleValueEditor(
            PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType == typeof(bool) ?
                CreateBoolControl(propertyInfo) :
                CreateDefaultControl(propertyInfo);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "U2U1011:Return types should be specific", Justification = "<挂起>")]
        private static FrameworkElement CreateBoolControl(PropertyInfo propertyInfo)
        {
            var contentControl = new CheckBox
            {
                VerticalContentAlignment = VerticalAlignment.Center
            };
            var rwAuth = GetPropertyReadWriterAuth(propertyInfo);
            _ = contentControl.SetBinding(ToggleButton.IsCheckedProperty,
                GetBinding(propertyInfo, rwAuth));
            if (rwAuth.set is false) contentControl.IsEnabled = false;
            return contentControl;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "U2U1011:Return types should be specific", Justification = "<挂起>")]
        private static FrameworkElement CreateDefaultControl(PropertyInfo propertyInfo)
        {
            var contentControl = new TextBox
            {
                VerticalContentAlignment = VerticalAlignment.Center
            };
            var rwAuth = GetPropertyReadWriterAuth(propertyInfo);
            _ = contentControl.SetBinding(TextBox.TextProperty,
                GetBinding(propertyInfo, rwAuth));
            if (rwAuth.set is false)
            {
                contentControl.IsReadOnly = true;
            }

            return contentControl;
        }

        /// <summary>
        /// 通过属性的读写权限获取绑定
        /// </summary>
        /// <param name="propertyInfo"> </param>
        /// <param name="tuple"> </param>
        private static Binding GetBinding(PropertyInfo propertyInfo,
            (bool get, bool set) tuple)
        {
            return new(propertyInfo.Name)
            {
                Mode = tuple switch
                {
                    (true, true) => BindingMode.TwoWay,
                    (true, false) => BindingMode.OneWay,
                    (false, true) => BindingMode.OneWayToSource,
                    _ => throw new ArgumentOutOfRangeException()
                }
            };
        }

        /// <summary>
        /// 获取属性的读写是否是公开的
        /// </summary>
        /// <param name="propertyInfo"> </param>
        private static (bool get, bool set) GetPropertyReadWriterAuth(
            PropertyInfo propertyInfo)
        {
            return (propertyInfo.GetMethod?.IsPublic ?? false,
                propertyInfo.SetMethod?.IsPublic ?? false);
        }
    }
}
