using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Drawing;
using DevExpress.Utils;

namespace WindowsApplication1
{
    public class ColumnInplaceEditorHelper
    {
        private RepositoryItem _Item;
        private GridColumn _Column;
        GridView view;
        public ColumnInplaceEditorHelper(GridColumn column, RepositoryItem inplaceEditor)
        {
            _Column = column;
            _Item = inplaceEditor;
            view = column.View as GridView;
            view.CustomDrawColumnHeader += view_CustomDrawColumnHeader;
            view.MouseDown += view_MouseDown;
            view.Layout += new EventHandler(view_Layout);
        }

        void view_Layout(object sender, EventArgs e)
        {
            CloseEditor();
        }

        private object _EditValue;
        public object EditValue
        {
            get { return _EditValue; }
            set
            {
                _EditValue = value;
            }
        }

        private BaseEdit _ActiveEditor;
        public BaseEdit ActiveEditor
        {
            get { return _ActiveEditor; }
            set { _ActiveEditor = value; }
        }

        void view_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {
            if (e.Column == _Column)
            {
                e.Info.Caption = string.Empty;
                e.Painter.DrawObject(e.Info);
                e.Handled = true;
                DrawEditorHelper.DrawColumnInplaceEditor(e, _Item, EditValue, GetRightIndent());
            }
        }

        public int GetRightIndent()
        {
            return _Column.OptionsColumn.AllowSort != DevExpress.Utils.DefaultBoolean.False || _Column.OptionsFilter.AllowFilter ? 25 : 0;
        }
        void view_MouseDown(object sender, MouseEventArgs e)
        {
            CloseEditor();
            Rectangle editorBounds;
            if (ClickInEditor(e, out editorBounds)) 
            {
                ShowEditor(editorBounds);
                DXMouseEventArgs.GetMouseArgs(e).Handled = true;
            }
        }
        private bool ClickInEditor(MouseEventArgs e, out Rectangle editorBounds)
        {
            editorBounds = Rectangle.Empty;
            GridViewInfo vi = view.GetViewInfo() as GridViewInfo;
            GridColumnInfoArgs columnInfo = CalcColumnHitInfo(e.Location, vi.ColumnsInfo);

            if (columnInfo != null && columnInfo.Column == _Column)
            {
                editorBounds = DrawEditorHelper.GetEditorBounds(columnInfo.Bounds, GetRightIndent());
                return editorBounds.Contains(e.Location);
            }
            return false;
        }

        protected virtual GridColumnInfoArgs CalcColumnHitInfo(Point pt, GridColumnsInfo cols)
        {
            foreach (GridColumnInfoArgs ci in cols)
            {
                if (!ci.Bounds.IsEmpty && IntInRange(pt.X, ci.Bounds.Left, ci.Bounds.Right))
                {
                    if (ci.Type == GridColumnInfoType.EmptyColumn) continue;
                    return ci;
                }
            }
            return null;
        }

        protected bool IntInRange(int x, int left, int right)
        {
            if (right < left)
            {
                int temp = left;
                left = right;
                right = temp;
            }
            return (x >= left && x < right);
        }

        private void ShowEditor(Rectangle bounds)
        {
            ActiveEditor = _Item.CreateEditor();
            ActiveEditor.Properties.LockEvents();
            ActiveEditor.Parent = view.GridControl;
            ActiveEditor.Properties.Assign(_Item);
            ActiveEditor.Properties.AutoHeight = false;
            ActiveEditor.Location = bounds.Location;
            ActiveEditor.Size = bounds.Size;
            ActiveEditor.CreateControl();
            ActiveEditor.EditValue = EditValue;
            ActiveEditor.SendMouse(ActiveEditor.PointToClient(Control.MousePosition), Control.MouseButtons);
            ActiveEditor.Properties.UnLockEvents();
        }

        private void CloseEditor()
        {
            if (ActiveEditor != null)
            {
                EditValue = ActiveEditor.EditValue;
                ActiveEditor.Dispose();
                ActiveEditor = null;
            }
        }
        void editor_Leave(object sender, EventArgs e)
        {
            CloseEditor();
        }
    }
}