using System;
using System.Reflection;
using System.Windows;

namespace UsefulWpfLibrary.Views.CustomControls.MultiValueEditor.Options.Args
{
    public class AfterCreateDefaultSingleValueEditorArgs : IGetSourceInstanceInfoArgs
    {
        public PropertyInfo? PropertyInfo { get; init; }
        public object? SourceInstance { get; init; }
        public Type? SourceInstanceType { get; init; }
        public UIElement? DefaultSingleValueEditor { get; init; }
    }
}
