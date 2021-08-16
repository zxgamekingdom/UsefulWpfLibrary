using System;

using UsefulWpfLibrary.Views.CustomControls.MultiValueEditor.Options;

namespace UsefulWpfLibrary.Views.CustomControls.MultiValueEditor.ContextInfos
{
    internal class SourceInstanceContext
    {
        public object Instance { get; set; } = null!;
        public Type Type { get; set; } = null!;
        public MultiValueEditorOptions? ViewModelControlOptions { get; set; }
    }
}