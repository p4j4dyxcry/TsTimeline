using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TsTimeline
{
    public class ClipSelectorService
    {
        private readonly List<ISelectable> _selectedItems = new List<ISelectable>();

        public IReadOnlyList<ISelectable> SelectedItems => _selectedItems;

        public ClipSelectorService()
        {
            
        }

        public void UpdateSelectedItems(ISelectable selectable)
        {
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
            ClearSelect();
            selectable.IsSelected = true;
        }

        public void ClearSelect()
        {
            foreach (var selectedItem in SelectedItems.ToArray())
            {
                // falseにすれば SelectionChanged経由で SetSelectedが呼ばれるので 
                selectedItem.IsSelected = false;
            }
        }
    }
}