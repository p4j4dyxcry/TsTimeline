using System;

namespace TsTimeline
{
    public class InterfaceTemplateSelectorExtension : System.Windows.Markup.MarkupExtension
    {
        private string ResourceKeys { get; }

        private string ResourceKeysSeparator { get; }

        public InterfaceTemplateSelectorExtension(string resourceKeysCsv) : this( resourceKeysCsv , "," )
        {
        }

        private InterfaceTemplateSelectorExtension(string resourceKeys, string separator)
        {
            ResourceKeys = resourceKeys;
            ResourceKeysSeparator = separator;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new InterfaceTemplateSelector(ResourceKeys.Split(new [] {ResourceKeysSeparator}, StringSplitOptions.RemoveEmptyEntries));
        }
    }
}