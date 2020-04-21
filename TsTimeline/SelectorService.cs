using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Sandbox2.Models;

namespace TsTimeline
{
    public class SelectorService : Notification
    {
        public static SelectorService Default { get; } = new SelectorService();
        
        private readonly ObservableCollection<ISelectable> _selectedItems = new ObservableCollection<ISelectable>();

        public ReadOnlyObservableCollection<ISelectable> SelectedItems { get; }

        public ISelectable SelectedItem => SelectedItems.FirstOrDefault();

        public SelectorService()
        {
            SelectedItems = new ReadOnlyObservableCollection<ISelectable>(_selectedItems);
        }

        public void UpdateSelectedItems(ISelectable selectable)
        {
            if (selectable is null)
                return;
            
            var contain = _selectedItems.Contains(selectable);
            var selected = selectable.IsSelected;
            // 選択中のものを選択
            if (selected && contain)
            {
                return;
            }
            
            // 非選択中のものを非選択
            if (!selected && !contain)
            {
                return;
            }
            
            // 非選択中のものを選択
            if (selected)
            {
                _selectedItems.Add(selectable);
            }
            else
            {
                _selectedItems.Remove(selectable);
            }
        }

        public void SingleSelect(ISelectable selectable)
        {
            if (selectable is null)
                return;
            
            ClearSelect();
            selectable.IsSelected = true;
            RaiseSelectionChanged();
        }

        public void ClearSelect()
        {
            foreach (var selectedItem in SelectedItems.ToArray())
            {
                // falseにすれば SelectionChanged経由で SetSelectedが呼ばれるので 
                selectedItem.IsSelected = false;
            }
            RaiseSelectionChanged();
        }

        public void MouseDownSelectionChanged(ISelectable selectable)
        {
            // Add Select
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                selectable.IsSelected = true;
            }
            // Toggle Select
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                selectable.IsSelected ^= true;
            }
            // Single Select
            else
            {
                SingleSelect(selectable);
            }
            RaiseSelectionChanged();
        }

        public void RaiseSelectionChanged()
        {
            OnPropertyChanged(nameof(SelectedItem));
        }
    }
}