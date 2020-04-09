using System;

namespace TsTimeline
{
    public class InterfaceTemplateSelectorExtension : System.Windows.Markup.MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new InterfaceTemplateSelector();
        }
    }
}