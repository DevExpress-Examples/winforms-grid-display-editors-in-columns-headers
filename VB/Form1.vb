Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraEditors

Namespace WindowsApplication1
	Partial Public Class Form1
		Inherits Form
				Private Function CreateTable(ByVal RowCount As Integer) As DataTable
			Dim tbl As New DataTable()
			tbl.Columns.Add("Name", GetType(String))
			tbl.Columns.Add("ID", GetType(Integer))
			tbl.Columns.Add("Check", GetType(Boolean))
			tbl.Columns.Add("Date", GetType(DateTime))
			For i As Integer = 0 To RowCount - 1
				tbl.Rows.Add(New Object() { String.Format("Name{0}", i), i, False, DateTime.Now.AddDays(i) })
			Next i
			Return tbl
				End Function


		Public Sub New()
			InitializeComponent()
			gridControl1.DataSource = CreateTable(20)
            Dim helper As ColumnInplaceEditorHelper = New ColumnInplaceEditorHelper(gridColumn1, New RepositoryItemSpinEdit())
			gridColumn1.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False
			gridColumn1.OptionsFilter.AllowFilter = False
            helper = New ColumnInplaceEditorHelper(gridColumn2, New RepositoryItemSpinEdit())
			Dim riCheckEdit As New RepositoryItemCheckEdit()
			riCheckEdit.AllowFocused = False
			AddHandler riCheckEdit.EditValueChanged, AddressOf riCheckEdit_EditValueChanged
            helper = New ColumnInplaceEditorHelper(gridColumn3, riCheckEdit)
            helper = New ColumnInplaceEditorHelper(gridColumn4, New RepositoryItemProgressBar())
            helper.EditValue = 50
		End Sub

		Private Sub riCheckEdit_EditValueChanged(ByVal sender As Object, ByVal e As EventArgs)
			Try
				gridView1.BeginDataUpdate()
				For i As Integer = 0 To gridView1.DataRowCount - 1
					gridView1.SetRowCellValue(i, "Check", (TryCast(sender, CheckEdit)).Checked)
				Next i
			Catch ex As Exception

			Finally
				gridView1.EndDataUpdate()
			End Try
		End Sub
	End Class
End Namespace