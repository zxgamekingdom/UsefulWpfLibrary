using System;
using System.Reflection;

namespace UsefulWpfLibrary.Views.CustomControls.MultiValueEditor.Options.Args
{
    public class FindCustomControlAttributeArgs : IGetSourceInstanceInfoArgs
    {
        public PropertyInfo? PropertyInfo { get; init; }
        public object? SourceInstance { get; init; }
        public Type? SourceInstanceType { get; init; }
    }
}