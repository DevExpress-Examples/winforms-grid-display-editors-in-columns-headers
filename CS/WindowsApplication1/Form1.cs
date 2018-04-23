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

namespace WindowsApplication1
{
    public partial class Form1 : Form
    {
                private DataTable CreateTable(int RowCount)
        {
            DataTable tbl = new DataTable();
            tbl.Columns.Add("Name", typeof(string));
            tbl.Columns.Add("ID", typeof(int));
            tbl.Columns.Add("Check", typeof(bool));
            tbl.Columns.Add("Date", typeof(DateTime));
            for (int i = 0; i < RowCount; i++)
                tbl.Rows.Add(new object[] { String.Format("Name{0}", i), i, false, DateTime.Now.AddDays(i) });
            return tbl;
        }


        public Form1()
        {
            InitializeComponent();
            gridControl1.DataSource = CreateTable(20);
            new ColumnInplaceEditorHelper(gridColumn1, new RepositoryItemSpinEdit());
            gridColumn1.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            gridColumn1.OptionsFilter.AllowFilter = false;
            new ColumnInplaceEditorHelper(gridColumn2, new RepositoryItemSpinEdit());
            RepositoryItemCheckEdit riCheckEdit = new RepositoryItemCheckEdit();
            riCheckEdit.AllowFocused = false;
            riCheckEdit.EditValueChanged += new EventHandler(riCheckEdit_EditValueChanged);
            new ColumnInplaceEditorHelper(gridColumn3, riCheckEdit);
            new ColumnInplaceEditorHelper(gridColumn4, new RepositoryItemProgressBar()).EditValue = 50;
        }

        void riCheckEdit_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                gridView1.BeginDataUpdate();
                for (int i = 0; i < gridView1.DataRowCount; i++)
                {
                    gridView1.SetRowCellValue(i, "Check", (sender as CheckEdit).Checked);
                }
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                gridView1.EndDataUpdate();
            }
        }
    }
}