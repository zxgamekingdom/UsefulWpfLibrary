using System;

namespace UsefulWpfLibrary.Views.CustomControls.MultiValueEditor.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public sealed class TitleMenuItemUseCommandNameAttribute : Attribute
    {
        public TitleMenuItemUseCommandNameAttribute(string header, string commandName)
        {
            Header = header;
            CommandName = commandName;
        }

        public string CommandName { get; }
        public string Header { get; }
    }
}
