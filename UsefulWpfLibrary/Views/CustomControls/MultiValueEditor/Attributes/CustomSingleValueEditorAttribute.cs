using System;
using System.Windows;

using UsefulWpfLibrary.Views.CustomControls.MultiValueEditor.Options.Args;

namespace UsefulWpfLibrary.Views.CustomControls.MultiValueEditor.Attributes
{
    /// <summary>
    /// 属性自定义显示控件
    /// <para />
    /// </summary>
    [AttributeUsage(AttributeTargets.Property,
        Inherited = false,
        AllowMultiple = false)]
    public abstract class CustomSingleValueEditorAttribute : Attribute
    {
        /// <summary>
        /// 是否隐藏自动生成的标题
        /// </summary>
        public abstract bool IsHidePropertyTitle { get; }

        /// <summary>
        /// 控件
        /// </summary>
        public abstract FrameworkElement? UseCustomSingleValueEditor(
            UseCustomSingleValueEditorAttributeArgs args);
    }
}