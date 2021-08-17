using System;
using System.Reflection;
using System.Windows.Input;
using UsefulWpfLibrary.Views.CustomControls.MultiValueEditor.Options.Args;

namespace UsefulWpfLibrary.Views.CustomControls.MultiValueEditor.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public abstract class TitleMenuItemAttribute : Attribute
    {
        public abstract string Header { get; }

        public abstract ICommand GetCommand(PropertyInfo propertyInfo,
            GetCommandArgs getCommandArgs);
    }
}
