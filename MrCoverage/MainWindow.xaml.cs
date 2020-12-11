using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using System.Windows.Threading;

namespace MrCoverage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public IterationItemSet ItemSet { get; set; }

        private IterationItemColumn CurrentMovingColumn;
        private int MouseStartPosionX;
        private IInputElement MouseStartPositionButton;

        private DispatcherTimer BottomItemSelectionTimer = new DispatcherTimer();
        private ListView targetListView;
        private int targetIndex;

        public MainWindow()
        {
            InitializeComponent();
            ItemSet = new IterationItemSet();
            this.DataContext = ItemSet;

            BottomItemSelectionTimer.Interval = TimeSpan.FromMilliseconds(10);
            BottomItemSelectionTimer.Tick += BottomItemSelectionTimer_Tick;
        }

        #region 列のサイズ変更処理
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

        private void Window_PreviewMouseMove(object sender, MouseEventArgs e)
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

        /// <summary>
        /// 一番下を選択するタイマー処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BottomItemSelectionTimer_Tick(object sender, EventArgs e)
        {
            BottomItemSelectionTimer.Stop();
            try
            {
                var gen = targetListView.ItemContainerGenerator;
                ListViewItem vi = gen.ContainerFromIndex(targetIndex) as ListViewItem;
                vi.Focus();
            }
            catch
            {
                targetListView = null;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ItemSet.AddNewColumn();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ListView_LostFocus(object sender, RoutedEventArgs e)
        {
            (sender as ListView).SelectedItem = null;
        }

        private void ListView_KeyDown(object sender, KeyEventArgs e)
        {
            IterationItemColumn column = (sender as ListView).DataContext as IterationItemColumn;
            if (column == null) { return; }

            if (e.Key == Key.Down)
            {
                // 下に新しいセルを作成する
                if (column.SelectedIndex == column.Items.Count -1) {
                    var item = new IterationItem();
                    column.Items.Add(item);
                }

                // カーソルを下に動かす
                targetListView = sender as ListView;
                targetIndex = column.SelectedIndex + 1;
                BottomItemSelectionTimer.Start();
                return;
            }

            if (e.Key == Key.Right)
            {
                return;
            }

            if (e.Key == Key.Left)
            {
                return;
            }

            if (e.Key == Key.Return)
            {
                TextInputWindow frm = new TextInputWindow();
                frm.Text = column.SelectedItem.Text;
                frm.ShowDialog();
                column.SelectedItem.Text = frm.Text;

                // 下に新しいセルを作成する
                IterationItem iter = new IterationItem();
                if (column.SelectedIndex == column.Items.Count - 1)
                {
                    column.Items.Add(iter);
                }

                // カーソルを下に動かす
                targetListView = sender as ListView;
                targetIndex = column.SelectedIndex + 1;
                BottomItemSelectionTimer.Start();
                return;
            }
        }

        //private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    IterationItemColumn column = (sender as ListView)?.DataContext as IterationItemColumn;
        //    if (column == null) { return; }
        //    TextInputWindow frm = new TextInputWindow();
        //    frm.Text = column.SelectedItem.Text;
        //    frm.ShowDialog();
        //    column.SelectedItem.Text = frm.Text;
        //}

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var item = ItemSet.Colmuns;
        }

        private void Datagrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //IterationItemColumn column = (sender as DataGrid)?.DataContext as IterationItemColumn;
            //if (column == null) { return; }
            //TextInputWindow frm = new TextInputWindow();
            //frm.Text = column.SelectedItem?.Text;
            //frm.ShowDialog();
            //if (column.SelectedItem != null)
            //{
            //    column.SelectedItem.Text = frm.Text;
            //}
            
        }

        private void Datagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtEdit.DataContext = (sender as DataGrid).SelectedItem;
        }

        private void Datagrid_LostFocus(object sender, RoutedEventArgs e)
        {
            //(sender as DataGrid).SelectedItem = null;
        }

        private void Datagrid_KeyDown(object sender, KeyEventArgs e)
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

                //// カーソルを下に動かす
                //targetListView = sender as DataGrid;
                //targetIndex = column.SelectedIndex + 1;
                //BottomItemSelectionTimer.Start();
                //return;
            }

            if (e.Key == Key.Right)
            {
                return;
            }

            if (e.Key == Key.Left)
            {
                return;
            }

            if (e.Key == Key.Return)
            {
                TextInputWindow frm = new TextInputWindow();
                frm.Text = column.SelectedItem.Text;
                frm.ShowDialog();
                column.SelectedItem.Text = frm.Text;

                // 下に新しいセルを作成する
                IterationItem iter = new IterationItem();
                if (column.SelectedIndex == column.Items.Count - 1)
                {
                    column.Items.Add(iter);
                }

                //// カーソルを下に動かす
                //targetListView = sender as ListView;
                //targetIndex = column.SelectedIndex + 1;
                //BottomItemSelectionTimer.Start();
                return;
            }
        }

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

        private void DataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            txtEdit.DataContext = (sender as DataGrid).SelectedItem;
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
            
            DataGridCellsPresenter presenter = FindVisualChild<DataGridCellsPresenter>(rowContainer);
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

    }
}
