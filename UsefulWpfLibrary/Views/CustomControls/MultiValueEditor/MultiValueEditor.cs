using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using UsefulWpfLibrary.Views.CustomControls.MultiValueEditor.Attributes;
using UsefulWpfLibrary.Views.CustomControls.MultiValueEditor.ContextInfos;
using UsefulWpfLibrary.Views.CustomControls.MultiValueEditor.Options;
using UsefulWpfLibrary.Views.CustomControls.MultiValueEditor.Options.Args;
using UsefulWpfLibrary.Views.CustomControls.MultiValueEditor.Tools;

namespace UsefulWpfLibrary.Views.CustomControls.MultiValueEditor
{
    [TemplatePart(Name = "PART_ContentPresenter", Type = typeof(ContentPresenter))]
    public class MultiValueEditor : Control
    {
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(nameof(Header),
                typeof(string),
                typeof(MultiValueEditor),
                new PropertyMetadata(default(string)));

        static MultiValueEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MultiValueEditor),
                new FrameworkPropertyMetadata(typeof(MultiValueEditor)));
        }

        public MultiValueEditor()
        {
            DataContextChanged += OnDataContextChanged;
            Unloaded += (_, _) => DataContextChanged -= OnDataContextChanged;
        }

        [SuppressMessage("CodeQuality", "IDE0079:请删除不必要的忽略", Justification = "<挂起>")]
        public MultiValueEditor(MultiValueEditorOptions options) : this()
        {
            Options = options;
        }

        public Grid Grid { get; } = new();

        public string Header
        {
            get => (string)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        public MultiValueEditorOptions? Options { get; }

        public override void OnApplyTemplate()
        {
            var contentPresenter =
                (ContentPresenter)Template.FindName("PART_ContentPresenter", this);
            contentPresenter.Content = Grid;
            base.OnApplyTemplate();
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            Grid.Arrange(new Rect(arrangeBounds));
            return base.ArrangeOverride(arrangeBounds);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            Grid.Measure(constraint);
            return base.MeasureOverride(constraint);
        }

        /// <summary>
        /// 自动生成没有指定 <seealso cref="CustomSingleValueEditorAttribute" /> 的属性的显示控制
        /// </summary>
        /// <param name="propertyInfo"> </param>
        /// <param name="sourceInstanceContext"> </param>
        private (bool isHideTitle, FrameworkElement frameworkElement)
            DefaultCreateSingleValueEditorControl(PropertyInfo propertyInfo,
                SourceInstanceContext sourceInstanceContext)
        {
            FrameworkElement contentControl =
                MultiValueEditorCreateDefaultSinaValueEditorTools
                    .CreateDefaultSingleValueEditor(propertyInfo);
            var args = new AfterCreateDefaultSingleValueEditorArgs
            {
                PropertyInfo = propertyInfo,
                SourceInstance = sourceInstanceContext.Instance,
                SourceInstanceType = sourceInstanceContext.Type,
                DefaultSingleValueEditor = contentControl
            };
            if (sourceInstanceContext.ViewModelControlOptions != null)
            {
                sourceInstanceContext.ViewModelControlOptions
                   .AfterCreateDefaultSingleValueEditor?.Invoke(args);
            }
            else
            {
                Options?.AfterCreateDefaultSingleValueEditor?.Invoke(args);
            }

            return (false, contentControl);
        }

        /// <summary>
        /// 创建用于显示属性的控件
        /// </summary>
        /// <param name="propertyInfo"> </param>
        /// <param name="customControlAttribute"> </param>
        /// <param name="sourceInstanceContext"> </param>
        private (bool isHideTitle, UIElement uiElement) CreateSingleValueEditor(
            PropertyInfo propertyInfo,
            CustomSingleValueEditorAttribute? customControlAttribute,
            SourceInstanceContext sourceInstanceContext)
        {
            var args = new UseCustomSingleValueEditorArgs
            {
                PropertyInfo = propertyInfo,
                SourceInstance = sourceInstanceContext.Instance,
                SourceInstanceType = sourceInstanceContext.Type
            };
            return FindSingleValueEditorFromAttribute(customControlAttribute,
                    propertyInfo,
                    sourceInstanceContext) ??
                sourceInstanceContext.ViewModelControlOptions
                    ?.UseCustomSingleValueEditor
                    ?.Invoke(args) ??
                Options?.UseCustomSingleValueEditor?.Invoke(args) ??
                DefaultCreateSingleValueEditorControl(propertyInfo,
                    sourceInstanceContext);
        }

        private (bool isHideTitle, FrameworkElement frameworkElement)?
            FindSingleValueEditorFromAttribute(
                CustomSingleValueEditorAttribute? customSingleValueEditorAttribute,
                PropertyInfo propertyInfo,
                SourceInstanceContext sourceInstanceContext)
        {
            return customSingleValueEditorAttribute is null ?
                null :
                (customSingleValueEditorAttribute.IsHidePropertyTitle,
                    customSingleValueEditorAttribute.UseCustomSingleValueEditor(
                        new UseCustomSingleValueEditorAttributeArgs()
                        {
                            PropertyInfo = propertyInfo,
                            SourceInstance = sourceInstanceContext.Instance,
                            SourceInstanceType = sourceInstanceContext.Type
                        }) ??
                    throw new NullReferenceException($@"特性""{
                        customSingleValueEditorAttribute.GetType().FullName}""的{
                            nameof(customSingleValueEditorAttribute
                                .UseCustomSingleValueEditor)}()得到的结果为null"));
        }

        private void CreateTitleControl(int index,
            PropertyInfo propertyInfo,
            bool isHideTitle,
            SourceInstanceContext sourceInstanceContext)
        {
            if (isHideTitle is false)
            {
                TextBlock titleControl =
                    MultiValueEditorCreateDefaultTitleTools.CreateDefaultTitleControl(
                        propertyInfo,
                        sourceInstanceContext,
                        Options);
                var args = new AfterCreateDefaultTitleControlArgs
                {
                    PropertyInfo = propertyInfo,
                    SourceInstance = sourceInstanceContext.Instance,
                    SourceInstanceType = sourceInstanceContext.Type,
                    DefaultTitleControl = titleControl
                };
                if (sourceInstanceContext.ViewModelControlOptions != null)
                {
                    sourceInstanceContext.ViewModelControlOptions
                       .AfterCreateDefaultTitleControl?.Invoke(args);
                }
                else
                {
                    Options?.AfterCreateDefaultTitleControl?.Invoke(args);
                }

                MultiValueEditorCreateDefaultTitleTools.TitleAddMenuItem(propertyInfo,
                    sourceInstanceContext,
                    titleControl,
                    Options);
                Grid.SetColumn(titleControl, 0);
                Grid.SetRow(titleControl, index);
                _ = Grid.Children.Add(titleControl);
            }
        }

        private void InitGrid()
        {
            Grid.Children.Clear();
            Grid.RowDefinitions.Clear();
            Grid.ColumnDefinitions.Clear();
        }

        /// <summary>
        /// 生成并布局 <seealso cref="Grid" /> 的所有子控件
        /// </summary>
        /// <param name="orderPropertyInfos"> </param>
        /// <param name="sourceInstanceContext"> </param>
        private void InitGridChildren(PropertyInfo[] orderPropertyInfos,
            SourceInstanceContext sourceInstanceContext)
        {
            for (var index = 0; index < orderPropertyInfos.Length; index++)
            {
                PropertyInfo propertyInfo = orderPropertyInfos[index];
                var args = new FindCustomControlAttributeArgs
                {
                    PropertyInfo = propertyInfo,
                    SourceInstance = sourceInstanceContext.Instance,
                    SourceInstanceType = sourceInstanceContext.Type,
                };
                var customControlAttribute =
                    propertyInfo
                        .GetCustomAttribute<CustomSingleValueEditorAttribute>() ??
                    sourceInstanceContext.ViewModelControlOptions
                        ?.FindCustomControlAttribute?.Invoke(args) ??
                    Options?.FindCustomControlAttribute?.Invoke(args);
                (var isHideTitle, var editorElement) =
                    CreateSingleValueEditor(propertyInfo,
                        customControlAttribute,
                        sourceInstanceContext);
                switch (isHideTitle)
                {
                    case true:
                        Grid.SetColumn(editorElement, 0);
                        Grid.SetColumnSpan(editorElement, 2);
                        break;
                    case false:
                        Grid.SetColumn(editorElement, 1);
                        break;
                }

                Grid.SetRow(editorElement, index);
                _ = Grid.Children.Add(editorElement);
                CreateTitleControl(index,
                    propertyInfo,
                    isHideTitle,
                    sourceInstanceContext);
            }
        }

        /// <summary>
        /// 初始化 <seealso cref="Grid" /> 的布局
        /// </summary>
        /// <param name="valueTuples"> </param>
        private void InitGridLayout(PropertyInfo[] valueTuples)
        {
            Grid.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = GridLength.Auto
            });
            Grid.ColumnDefinitions.Add(new ColumnDefinition());
            var count = valueTuples.Length;
            for (var i = 0; i < count; i++)
            {
                Grid.RowDefinitions.Add(new RowDefinition());
            }
        }

        private void OnDataContextChanged(object sender,
            DependencyPropertyChangedEventArgs e)
        {
            object value = e.NewValue;
            Type type = value.GetType();
            var instanceContextInfo = new SourceInstanceContext
            {
                Instance = value, Type = type
            };
            if (value is IGetMultiValueEditorOptions o)
                instanceContextInfo.ViewModelControlOptions = o.GetOptions();
            InitGrid();
            var selectPropertyInfos = SelectPropertyInfos(type, instanceContextInfo);
            PropertyInfo[] orderPropertyInfos =
                OrderPropertyInfos(selectPropertyInfos, instanceContextInfo);
            InitGridLayout(selectPropertyInfos);
            InitGridChildren(orderPropertyInfos, instanceContextInfo);
        }

        /// <summary>
        /// 将ViewModel对象的属性进行排序 <remarks> 首先将有顺序特性的属性插入到没有顺序特性的属性集合中 </remarks>
        /// </summary>
        /// <param name="propertyInfos"> </param>
        /// <param name="sourceInstanceContext"> </param>
        private PropertyInfo[] OrderPropertyInfos(PropertyInfo[] propertyInfos,
            SourceInstanceContext sourceInstanceContext)
        {
            (PropertyInfo propertyInfo, OrderAttribute orderAttribute)[] array =
            (
                from propertyInfo in propertyInfos
                let args =
                    new FindOrderAttributeArgs
                    {
                        PropertyInfo = propertyInfo,
                        SourceInstance = sourceInstanceContext.Instance,
                        SourceInstanceType = sourceInstanceContext.Type
                    }
                let orderAttribute =
                    propertyInfo.GetCustomAttribute<OrderAttribute>() ??
                    sourceInstanceContext.ViewModelControlOptions?.FindOrderAttribute
                        ?.Invoke(args) ??
                    Options?.FindOrderAttribute?.Invoke(args)
                select (propertyInfo, orderAttribute)).ToArray();
            List<PropertyInfo> noOrderList = (
                from item in array
                where item.orderAttribute is null
                select item.propertyInfo).ToList();
            (PropertyInfo propertyInfo, int order)[] orderArray =
            (
                from item in array
                let attribute = item.orderAttribute
                where attribute != null
                let order = attribute.Order
                select (item.propertyInfo, order)).ToArray();
            foreach (var (propertyInfo, order) in orderArray)
            {
                if (order < 0)
                {
                    throw new ArgumentException(
                        $"属性 {propertyInfo.Name} 序号不能小于零,当前的序号为{order}");
                }

                if (order < noOrderList.Count)
                    noOrderList.Insert(order, propertyInfo);
                else
                    noOrderList.Add(propertyInfo);
            }

            return noOrderList.ToArray();
        }

        private PropertyInfo[] SelectPropertyInfos(Type type,
            SourceInstanceContext sourceInstanceContext)
        {
            PropertyInfo[] propertyInfos =
                type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            return propertyInfos.Where(info =>
                {
                    var args = new FindIgnoreAttributeArgs
                    {
                        PropertyInfo = info,
                        SourceInstance = sourceInstanceContext.Instance,
                        SourceInstanceType = sourceInstanceContext.Type
                    };
                    var attribute =
                        info.GetCustomAttribute<IgnoreAttribute>() ??
                        sourceInstanceContext.ViewModelControlOptions
                            ?.FindIgnoreAttribute
                            ?.Invoke(args) ??
                        Options?.FindIgnoreAttribute?.Invoke(args);
                    return attribute is null;
                })
                .ToArray();
        }
    }
}
