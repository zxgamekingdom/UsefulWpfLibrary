using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using UsefulWpfLibrary.Views.CustomControls.MultiValueEditor.Attributes;
using UsefulWpfLibrary.Views.CustomControls.MultiValueEditor.Options.Args;

namespace UsefulWpfLibrary.Views.CustomControls.MultiValueEditor.Options
{
    public class MultiValueEditorOptions
    {
        public Action<AfterCreateDefaultTitleControlArgs>?
            AfterCreateDefaultTitleControl;

        public Action<AfterCreateDefaultSingleValueEditorArgs>?
            AfterCreateDefaultSingleValueEditor;

        public Func<FindCustomControlAttributeArgs, CustomSingleValueEditorAttribute?>?
            FindCustomControlAttribute;

        public Func<FindIgnoreAttributeArgs, IgnoreAttribute?>? FindIgnoreAttribute;
        public Func<FindOrderAttributeArgs, OrderAttribute?>? FindOrderAttribute;

        public Func<FindPropertyNameAttributeArgs, PropertyNameAttribute?>?
            FindPropertyNameAttribute;

        public Func<FindTitleMenuItemUseCommandNameAttributesArgs,
                IEnumerable<TitleMenuItemUseCommandNameAttribute>>?
            FindTitleMenuItemUseCommandNameAttributes;

        public Func<FindTitleMenuItemAttributesArgs,
            IEnumerable<TitleMenuItemAttribute>>? FindTitleMenuItemAttributes;

        public Func<UseCustomSingleValueEditorArgs, (bool isHideTitle, UIElement
            uiElement)?>? UseCustomSingleValueEditor;
    }
}
