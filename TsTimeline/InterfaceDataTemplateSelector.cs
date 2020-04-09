using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace TsTimeline
{
    /// <summary>
    /// interface が アサイン可能なテンプレートを選びます。
    /// </summary>
    public class InterfaceTemplateSelector : DataTemplateSelector
    {
        private static DataTemplate NotFoundDataTemplate;

        private DataTemplate TryGetNotFoundDataTemplate()
        {
            if (NotFoundDataTemplate is null)
            {
                NotFoundDataTemplate = new DataTemplate();

                var visualTree = new FrameworkElementFactory(typeof(TextBlock));
                visualTree.SetBinding(TextBlock.TextProperty , new Binding());
                
                NotFoundDataTemplate.VisualTree = visualTree;
            }

            return NotFoundDataTemplate;
        }
        
        
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var c = (FrameworkElement) container;
            
            if(item is null)
                return TryGetNotFoundDataTemplate();
            
            var interfaces = item.GetType().GetInterfaces();
            
            // step 1 実タイプと一致するデータテンプレートを見つけた場合
            var type = item.GetType();
            {
                if (c.TryFindResource(new DataTemplateKey(type)) is DataTemplate template)
                {
                    return template;
                }                
            }

            // step 2 インターフェースに一致するテンプレートを検索する
            foreach (var @interface in interfaces)
            {
                if (c.TryFindResource(new DataTemplateKey(@interface)) is DataTemplate template)
                {
                    return template;
                }
            }

            // step 3 基底タイプを検索し一致するテンプレートを検索する
            while (type.BaseType != null && type.BaseType != typeof(System.Object))
            {
                type = type.BaseType;
                if (c.TryFindResource(new DataTemplateKey(type)) is DataTemplate template)
                {
                    return template;
                }
            }
            
            return TryGetNotFoundDataTemplate();
        }
    }
}