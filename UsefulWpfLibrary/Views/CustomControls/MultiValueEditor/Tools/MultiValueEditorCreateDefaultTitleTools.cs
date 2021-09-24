using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using UsefulWpfLibrary.Views.CustomControls.MultiValueEditor.Attributes;
using UsefulWpfLibrary.Views.CustomControls.MultiValueEditor.ContextInfos;
using UsefulWpfLibrary.Views.CustomControls.MultiValueEditor.Options;
using UsefulWpfLibrary.Views.CustomControls.MultiValueEditor.Options.Args;

namespace UsefulWpfLibrary.Views.CustomControls.MultiValueEditor.Tools
{
    internal static class MultiValueEditorCreateDefaultTitleTools
    {
        internal static TextBlock CreateDefaultTitleControl(PropertyInfo propertyInfo,
            SourceInstanceContext sourceInstanceContext,
            MultiValueEditorOptions? options)
        {
            var args = new FindPropertyNameAttributeArgs
            {
                PropertyInfo = propertyInfo,
                SourceInstance = sourceInstanceContext.Instance,
                SourceInstanceType = sourceInstanceContext.Type
            };
            return new TextBlock
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Text =
                    propertyInfo.GetCustomAttribute<PropertyNameAttribute>()
                        ?.PropertyName ??
                    sourceInstanceContext.ViewModelControlOptions
                        ?.FindPropertyNameAttribute?.Invoke(args)
                        ?.PropertyName ??
                    options?.FindPropertyNameAttribute?.Invoke(args)?.PropertyName ??
                    propertyInfo.Name
            };
        }
        internal static void TitleAddMenuItem(PropertyInfo propertyInfo,
                     SourceInstanceContext sourceInstanceContext,
            TextBlock titleControl,
            MultiValueEditorOptions? controlOptions)
        {
            ApplyTitleMenuItemAttribute(propertyInfo,
                sourceInstanceContext,
                titleControl,
                controlOptions);
            ApplyTitleMenuItemCommandAttribute(propertyInfo,
                sourceInstanceContext,
                titleControl,
                controlOptions);
        }

        private static void ApplyTitleMenuItemAttribute(PropertyInfo propertyInfo,
            SourceInstanceContext sourceInstanceContext,
            TextBlock titleControl,
            MultiValueEditorOptions? controlOptions)
        {
            var titleMenuItemAttributes = new List<TitleMenuItemUseCommandNameAttribute>();
            titleMenuItemAttributes.AddRange(propertyInfo
                .GetCustomAttributes<TitleMenuItemUseCommandNameAttribute>());
            var args = new FindTitleMenuItemUseCommandNameAttributesArgs()
            {
                PropertyInfo = propertyInfo,
                SourceInstance = sourceInstanceContext.Instance,
                SourceInstanceType = sourceInstanceContext.Type
            };
            titleMenuItemAttributes.AddRange(
                sourceInstanceContext.ViewModelControlOptions?.FindTitleMenuItemUseCommandNameAttributes
                    ?.Invoke(args) ??
                Enumerable.Empty<TitleMenuItemUseCommandNameAttribute>());
            titleMenuItemAttributes.AddRange(
                controlOptions?.FindTitleMenuItemUseCommandNameAttributes?.Invoke(args) ??
                Enumerable.Empty<TitleMenuItemUseCommandNameAttribute>());
            foreach (TitleMenuItemUseCommandNameAttribute menuItemAttribute in
                titleMenuItemAttributes)
            {
                titleControl.ContextMenu ??= new ContextMenu();
                var menuItem = new MenuItem { Header = menuItemAttribute.Header };
                _ = menuItem.SetBinding(MenuItem.CommandProperty,
                    new Binding(menuItemAttribute.CommandName));
                _ = titleControl.ContextMenu.Items.Add(menuItem);
            }
        }
        private static void ApplyTitleMenuItemCommandAttribute(
                    PropertyInfo propertyInfo,
            SourceInstanceContext sourceInstanceContext,
            TextBlock titleControl,
            MultiValueEditorOptions? controlOptions)
        {
            var titleMenuItemCommandAttributes =
                new List<TitleMenuItemAttribute>();
            titleMenuItemCommandAttributes.AddRange(propertyInfo
                .GetCustomAttributes<TitleMenuItemAttribute>());
            var args = new FindTitleMenuItemAttributesArgs()
            {
                PropertyInfo = propertyInfo,
                SourceInstance = sourceInstanceContext.Instance,
                SourceInstanceType = sourceInstanceContext.Type
            };
            titleMenuItemCommandAttributes.AddRange(
                sourceInstanceContext.ViewModelControlOptions
                    ?.FindTitleMenuItemAttributes?.Invoke(args) ??
                Enumerable.Empty<TitleMenuItemAttribute>());
            titleMenuItemCommandAttributes.AddRange(
                controlOptions?.FindTitleMenuItemAttributes?.Invoke(args) ??
                Enumerable.Empty<TitleMenuItemAttribute>());
            foreach (TitleMenuItemAttribute titleMenuItemCommandAttribute in
                titleMenuItemCommandAttributes)
            {
                titleControl.ContextMenu ??= new ContextMenu();
                var menuItem = new MenuItem
                {
                    Header = titleMenuItemCommandAttribute.Header,
                    Command = titleMenuItemCommandAttribute.GetCommand(propertyInfo,
                        new GetCommandArgs
                        {
                            Instance = sourceInstanceContext.Instance,
                            PropertyInfo = propertyInfo,
                            Type = sourceInstanceContext.Type
                        })
                };
                _ = titleControl.ContextMenu.Items.Add(menuItem);
            }
        }
    }
}