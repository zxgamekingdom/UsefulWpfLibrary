using System;
using System.Reflection;

namespace UsefulWpfLibrary.Views.CustomControls.MultiValueEditor.Options.Args
{
    public interface IGetSourceInstanceInfoArgs
    {
        public PropertyInfo? PropertyInfo { get; }
        public object? SourceInstance { get; }
        public Type? SourceInstanceType { get; }
    }
}
