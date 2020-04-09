using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace TsTimeline
{
    public class InterfaceTemplateSelector : DataTemplateSelector
    {
        readonly string[] _resourceKeys;

        public InterfaceTemplateSelector(string[] resourceKeys)
        {
            this._resourceKeys = resourceKeys;
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var c = (FrameworkElement) container;
            var dataTemplates = _resourceKeys
                .Select(resourceKey => c.TryFindResource(resourceKey))
                .OfType<DataTemplate>()
                .Where(x => x.DataType is Type);

            var result = dataTemplates
                .FirstOrDefault(x => ((Type) x.DataType).IsInstanceOfType(item));

            return result ?? base.SelectTemplate(item, container);
        }
    }
}