using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace TsTimeline
{
    public class InterfaceTemplateSelectorExtension : System.Windows.Markup.MarkupExtension
    {
        public string ResourceKeys { get; }
        
        public string ResourceKeysSeparator { get; }
        
        public InterfaceTemplateSelectorExtension() : this(string.Empty , ",")
        {
        }

        public InterfaceTemplateSelectorExtension(string resourceKeysCsv) : this( resourceKeysCsv , "," )
        {
        }

        public InterfaceTemplateSelectorExtension(string resourceKeys, string separator)
        {
            ResourceKeys = resourceKeys;
            ResourceKeysSeparator = separator;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new InterfaceTemplateSelector(ResourceKeys.Split(new string[] {ResourceKeysSeparator},
                StringSplitOptions.RemoveEmptyEntries));
        }
    }
}