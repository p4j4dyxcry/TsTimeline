using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace TsTimeline
{
    public class InterfaceTemplateSelectorExtension : System.Windows.Markup.MarkupExtension
    {
        public InterfaceTemplateSelectorExtension()
        {
            ResourceKeysSeparator = ",";
        }
 
        public string ResourceKeysSeparator { get; set; }
 
        public InterfaceTemplateSelectorExtension(string resourceKeysCSV) : this()
        {
            ResourceKeys = resourceKeysCSV;
        }
 
        public InterfaceTemplateSelectorExtension(string resourceKeys, string separator)
        {
            ResourceKeys = resourceKeys;
            ResourceKeysSeparator = separator;
        }
 
 
        /// 

        /// Comma separated resource keys specifying keys of DataTemplates that binds to interface
        /// 
        public string ResourceKeys { get; set; }
 
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new InterfaceTemplateSelector(ResourceKeys.Split(new string[]{ResourceKeysSeparator}, StringSplitOptions.RemoveEmptyEntries));
        }
 
        public class InterfaceTemplateSelector:DataTemplateSelector
        {
            string[] resourceKeys;
            public InterfaceTemplateSelector(string[] resourceKeys)
            {
                this.resourceKeys = resourceKeys;
            }
 
            public override DataTemplate SelectTemplate(object item, DependencyObject container)
            {
                var c = (FrameworkElement)container;
                var dataTemplates = (from rk in resourceKeys
                                let resource = c.TryFindResource(rk)
                                where resource is DataTemplate
                                where (resource as DataTemplate).DataType is Type
                                select resource).Cast<DataTemplate>();
 
                var itemType = item.GetType();
 
                var result = dataTemplates.FirstOrDefault(dt => 
                    (dt.DataType as Type).IsInstanceOfType(item));
 
                
                return result??base.SelectTemplate(item, container);
                
            }
        }
        
    }
}