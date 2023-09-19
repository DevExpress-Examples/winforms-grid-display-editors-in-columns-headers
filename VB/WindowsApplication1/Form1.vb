Imports System
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraEditors

Namespace WindowsApplication1

    Public Partial Class Form1
        Inherits Form

        Private Function CreateTable(ByVal RowCount As Integer) As DataTable
            Dim tbl As DataTable = New DataTable()
            tbl.Columns.Add("Name", GetType(String))
            tbl.Columns.Add("ID", GetType(Integer))
            tbl.Columns.Add("Check", GetType(Boolean))
            tbl.Columns.Add("Date", GetType(Date))
            For i As Integer = 0 To RowCount - 1
                tbl.Rows.Add(New Object() {String.Format("Name{0}", i), i, False, Date.Now.AddDays(i)})
            Next

            Return tbl
        End Function

        Public Sub New()
            InitializeComponent()
            gridControl1.DataSource = CreateTable(20)
            Dim tmp_ColumnInplaceEditorHelper = New ColumnInplaceEditorHelper(gridColumn1, New RepositoryItemSpinEdit())
            gridColumn1.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False
            gridColumn1.OptionsFilter.AllowFilter = False
            Dim tmp_ColumnInplaceEditorHelper = New ColumnInplaceEditorHelper(gridColumn2, New RepositoryItemSpinEdit())
            Dim riCheckEdit As RepositoryItemCheckEdit = New RepositoryItemCheckEdit()
            riCheckEdit.AllowFocused = False
            AddHandler riCheckEdit.EditValueChanged, New EventHandler(AddressOf riCheckEdit_EditValueChanged)
            Dim tmp_ColumnInplaceEditorHelper = New ColumnInplaceEditorHelper(gridColumn3, riCheckEdit)
            New ColumnInplaceEditorHelper(gridColumn4, New RepositoryItemProgressBar()).EditValue = 50
        End Sub

        Private Sub riCheckEdit_EditValueChanged(ByVal sender As Object, ByVal e As EventArgs)
            Try
                gridView1.BeginDataUpdate()
                For i As Integer = 0 To gridView1.DataRowCount - 1
                    gridView1.SetRowCellValue(i, "Check", TryCast(sender, CheckEdit).Checked)
                Next
            Catch ex As Exception
            Finally
                gridView1.EndDataUpdate()
            End Try
        End Sub
    End Class
End Namespace
