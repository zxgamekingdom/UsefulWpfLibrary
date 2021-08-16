using System;
using System.Reflection;

namespace UsefulWpfLibrary.Views.CustomControls.MultiValueEditor.Options.Args
{
    public class GetCommandArgs
    {
        public object Instance { get; init; } = null!;
        public PropertyInfo PropertyInfo { get; init; } = null!;
        public Type Type { get; init; } = null!;
    }
}