using System;

namespace UsefulWpfLibrary.Views.CustomControls.MultiValueEditor.Attributes
{
    /// <summary>
    /// 对要显示的属性进行排序
    /// </summary>
    [AttributeUsage(AttributeTargets.Property,
        Inherited = false,
        AllowMultiple = false)]
    public sealed class OrderAttribute : Attribute
    {
        ///<summary>
        ///ctor
        /// </summary>
        /// <param name="order">序号,从0开始</param>
        public OrderAttribute(int order)
        {
            Order = order;
        }

        public int Order { get; }
    }
}