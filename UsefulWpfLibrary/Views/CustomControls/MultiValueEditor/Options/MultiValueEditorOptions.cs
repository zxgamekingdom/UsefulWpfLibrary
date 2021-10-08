using System;
using System.Collections.Generic;
using System.Windows;
using UsefulWpfLibrary.Views.CustomControls.MultiValueEditor.Attributes;
using UsefulWpfLibrary.Views.CustomControls.MultiValueEditor.Options.Args;

namespace UsefulWpfLibrary.Views.CustomControls.MultiValueEditor.Options
{
    public class MultiValueEditorOptions
    {
        public Action<AfterCreateDefaultTitleControlArgs>?
            AfterCreateDefaultTitleControl { get; set; }

        public Action<AfterCreateDefaultSingleValueEditorArgs>?
            AfterCreateDefaultSingleValueEditor { get; set; }

        public Func<FindCustomControlAttributeArgs, CustomSingleValueEditorAttribute?>?
            FindCustomControlAttribute { get; set; }

        public Func<FindIgnoreAttributeArgs, IgnoreAttribute?>? FindIgnoreAttribute
        {
            get;
            set;
        }

        public Func<FindOrderAttributeArgs, OrderAttribute?>? FindOrderAttribute
        {
            get;
            set;
        }

        public Func<FindPropertyNameAttributeArgs, PropertyNameAttribute?>?
            FindPropertyNameAttribute { get; set; }

        public Func<FindTitleMenuItemUseCommandNameAttributesArgs,
                IEnumerable<TitleMenuItemUseCommandNameAttribute>>?
            FindTitleMenuItemUseCommandNameAttributes { get; set; }

        public Func<FindTitleMenuItemAttributesArgs,
            IEnumerable<TitleMenuItemAttribute>>? FindTitleMenuItemAttributes
        {
            get;
            set;
        }

        public Func<UseCustomSingleValueEditorArgs, (bool isHideTitle, UIElement
            uiElement)?>? UseCustomSingleValueEditor { get; set; }
    }
}
