using System;

namespace UsefulWpfLibrary.Views.CustomControls.MultiValueEditor.Attributes
{
    /// <summary>
    /// 是否忽略属性的显示
    /// </summary>
    [AttributeUsage(AttributeTargets.Property,
        Inherited = false,
        AllowMultiple = false)]
    public sealed class IgnoreAttribute : Attribute
    {
    }
}