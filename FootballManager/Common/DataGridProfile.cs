using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace FootballManager.Common
{
    public static class DataGridInterface
    {
        public static TContainer GetContainerFromIndex<TContainer>
  (this ItemsControl itemsControl, int index)
    where TContainer : DependencyObject
        {
            return (TContainer)
              itemsControl.ItemContainerGenerator.ContainerFromIndex(index);
        }

        public static bool IsEditing(this DataGrid dataGrid)
        {
            return dataGrid.GetEditingRow() != null;
        }

        public static DataGridRow GetEditingRow(this DataGrid dataGrid)
        {
            var sIndex = dataGrid.SelectedIndex;
            if (sIndex >= 0)
            {
                var selected = dataGrid.GetContainerFromIndex<DataGridRow>(sIndex);
                if (selected.IsEditing) return selected;
            }

            for (int i = 0; i < dataGrid.Items.Count; i++)
            {
                if (i == sIndex) continue;
                var item = dataGrid.GetContainerFromIndex<DataGridRow>(i);
                if (item.IsEditing) return item;
            }

            return null;
        }
    }
}
