using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace UsefulWpfLibrary.Views.CustomControls.MultiValueEditor.Options.Args
{
    public class AfterCreateDefaultTitleControlArgs : IGetSourceInstanceInfoArgs
    {
        public PropertyInfo? PropertyInfo { get; init; }
        public object? SourceInstance { get; init; }
        public Type? SourceInstanceType { get; init; }
        public UIElement? DefaultTitleControl { get; init; }
    }
}
