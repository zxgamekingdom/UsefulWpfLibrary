using System;

namespace UsefulWpfLibrary.Views.CustomControls.MultiValueEditor.Attributes
{
    /// <summary>
    /// 属性在控件上显示的名字
    /// </summary>
    [AttributeUsage(AttributeTargets.Property,
        Inherited = false,
        AllowMultiple = false)]
    public sealed class PropertyNameAttribute : Attribute
    {
        public PropertyNameAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }

        /// <summary>
        /// 属性名
        /// </summary>
        public string PropertyName { get; }
    }
}