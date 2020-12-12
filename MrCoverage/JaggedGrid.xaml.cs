using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MrCoverage
{
    /// <summary>
    /// JaggedGrid.xaml の相互作用ロジック
    /// </summary>
    public partial class JaggedGrid : UserControl
    {
        public IterationItemSet ItemSet { get; set; }

        public TextBox EditorTextBox { get; set; }

        private IterationItemColumn CurrentMovingColumn;
        private int MouseStartPosionX;
        private IInputElement MouseStartPositionButton;

        public JaggedGrid()
        {
            InitializeComponent();
        }

        #region "列のサイズ変更処理"
        private void Button_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // マウスが押されたら場所と基準となるコントロールを保存
            Button btn = sender as Button;
            CurrentMovingColumn = btn.DataContext as IterationItemColumn;
            MouseStartPositionButton = btn;
            MouseStartPosionX = (int)e.GetPosition(MouseStartPositionButton).X;
        }
        private void Button_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // マウスボタンが離されたら終了
            CurrentMovingColumn = null;
            MouseStartPositionButton = null;
        }

        public void Window_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (CurrentMovingColumn == null) { return; }
            // 基準位置からの差分さけサイズ変更する
            int widthDiff = (int)e.GetPosition(MouseStartPositionButton).X - MouseStartPosionX;
            if (CurrentMovingColumn.CurrentWidth + widthDiff > 0)
            {
                // 足して大丈夫なら足す
                CurrentMovingColumn.CurrentWidth += widthDiff;
            }
        }

        #endregion


        #region "セル選択処理"

        private void FocusCell(int rowIndex, int colIndex)
        {
            try
            {
                var gen = lstItems.ItemContainerGenerator.ContainerFromIndex(colIndex);
                var stackPanel = VisualTreeHelper.GetChild(gen, 0);
                var gridView = VisualTreeHelper.GetChild(stackPanel, 1) as DataGrid;
                var row = gridView.ItemContainerGenerator.ContainerFromIndex(rowIndex) as DataGridRow;
                DataGridCell cell = GetCell(gridView, row, 0);
                cell?.Focus();
            }
            catch { }
        }

        public static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            if (obj == null) { return null; }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T)
                {
                    return (T)child;
                }
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                    {
                        return childOfChild;
                    }
                }
            }
            return null;
        }
        public static DataGridCell GetCell(DataGrid dataGrid, DataGridRow rowContainer, int columnIndex)
        {
            if (dataGrid == null || rowContainer == null) { return null; }

            System.Windows.Controls.Primitives.DataGridCellsPresenter presenter = FindVisualChild<DataGridCellsPresenter>(rowContainer);
            if (presenter == null)
            {
                // ビュー仮想化しているときはこの処理が必要らしい
                rowContainer.ApplyTemplate();
                presenter = FindVisualChild<DataGridCellsPresenter>(rowContainer);
            }
            if (presenter != null)
            {
                DataGridCell cell = presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex) as DataGridCell;
                if (cell == null)
                {
                    // ビュー仮想化しているときはこの処理が必要らしい
                    dataGrid.ScrollIntoView(rowContainer, dataGrid.Columns[columnIndex]);
                    cell = presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex) as DataGridCell;
                }
                return cell;
            }
            return null;
        }

        #endregion

        private void DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            IterationItemColumn column = (sender as DataGrid).DataContext as IterationItemColumn;
            if (column == null) { return; }

            if (e.Key == Key.Down)
            {
                // 下に新しいセルを作成する
                if (column.SelectedIndex == column.Items.Count - 1)
                {
                    var item = new IterationItem();
                    column.Items.Add(item);
                }
            }

            if (e.Key == Key.Right)
            {
                int columnIndex = ItemSet.Colmuns.IndexOf(column);
                if (columnIndex >= ItemSet.Colmuns.Count - 1) { return; }

                int rowIndex = 0;
                IterationItemColumn target = ItemSet.Colmuns[columnIndex + 1];
                if (column.SelectedIndex >= target.Items.Count - 1)
                {
                    rowIndex = target.Items.Count - 1;
                }
                else
                {
                    rowIndex = column.SelectedIndex;
                }
                column.SelectedItem = null;
                target.SelectedIndex = rowIndex;

                FocusCell(rowIndex, columnIndex + 1);
                return;
            }

            if (e.Key == Key.Left)
            {
                int columnIndex = ItemSet.Colmuns.IndexOf(column);
                if (columnIndex <= 0) { return; }

                int rowIndex = 0;
                IterationItemColumn target = ItemSet.Colmuns[columnIndex - 1];
                if (column.SelectedIndex >= target.Items.Count - 1)
                {
                    rowIndex = target.Items.Count - 1;
                }
                else
                {
                    rowIndex = column.SelectedIndex;
                }
                column.SelectedItem = null;
                target.SelectedIndex = rowIndex;

                FocusCell(rowIndex, columnIndex - 1);
                return;
            }

        }

        private void TitleButton_Click(object sender, RoutedEventArgs e)
        {

        }


        private void gridIterator_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // 全てのセルの選択状態を解除する(１つだけ選ばれてほしいため)
            foreach (var col in ItemSet.Colmuns)
            {
                col.SelectedItem = null;
            }
        }

        private void gridIterator_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (e.AddedCells.Count < 1) { return; }
            EditorTextBox.DataContext = e.AddedCells[0].Item;
        }
    }
}
