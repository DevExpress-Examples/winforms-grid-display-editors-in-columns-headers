using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraEditors.Drawing;
using DevExpress.Utils.Drawing;
using DevExpress.XtraGrid.Views.Grid;

namespace WindowsApplication1
{
    public static class DrawEditorHelper
    {
        public static void DrawEdit(Graphics g, RepositoryItem edit, Rectangle r, object value)
        {
            BaseEditViewInfo info = edit.CreateViewInfo();
            info.EditValue = value;
            info.Bounds = r;
            info.CalcViewInfo(g);
            ControlGraphicsInfoArgs args = new ControlGraphicsInfoArgs(info, new GraphicsCache(g), r);
            edit.CreatePainter().Draw(args);
            args.Cache.Dispose();
        }

        public static Rectangle GetEditorBounds(Rectangle totalBounds, int rightIdent)
        {
            int leftIndent = 2;
            int topIndent = 2;
            Rectangle bounds = totalBounds;
            bounds.Inflate(-leftIndent, -topIndent);
            bounds.Width -= rightIdent;
            return bounds;
        }
        public static void DrawColumnInplaceEditor(ColumnHeaderCustomDrawEventArgs e, RepositoryItem item, object value, int rightIndent)
        {
            Rectangle targetRect = GetEditorBounds(e.Bounds, rightIndent);
            DrawEdit(e.Graphics, item, targetRect, value);
        }
    }
}
