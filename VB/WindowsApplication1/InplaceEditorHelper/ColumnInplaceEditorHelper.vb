Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraGrid.Drawing
Imports DevExpress.Utils
Imports System.Runtime.InteropServices

Namespace WindowsApplication1

    Public Class ColumnInplaceEditorHelper

        Private _Item As RepositoryItem

        Private _Column As GridColumn

        Private view As GridView

        Public Sub New(ByVal column As GridColumn, ByVal inplaceEditor As RepositoryItem)
            _Column = column
            _Item = inplaceEditor
            view = TryCast(column.View, GridView)
            AddHandler view.CustomDrawColumnHeader, AddressOf view_CustomDrawColumnHeader
            AddHandler view.MouseDown, AddressOf view_MouseDown
            AddHandler view.Layout, New EventHandler(AddressOf Me.view_Layout)
        End Sub

        Private Sub view_Layout(ByVal sender As Object, ByVal e As EventArgs)
            CloseEditor()
        End Sub

        Private _EditValue As Object

        Public Property EditValue As Object
            Get
                Return _EditValue
            End Get

            Set(ByVal value As Object)
                _EditValue = value
            End Set
        End Property

        Private _ActiveEditor As BaseEdit

        Public Property ActiveEditor As BaseEdit
            Get
                Return _ActiveEditor
            End Get

            Set(ByVal value As BaseEdit)
                _ActiveEditor = value
            End Set
        End Property

        Private Sub view_CustomDrawColumnHeader(ByVal sender As Object, ByVal e As ColumnHeaderCustomDrawEventArgs)
            If e.Column Is _Column Then
                e.Info.Caption = String.Empty
                e.Painter.DrawObject(e.Info)
                e.Handled = True
                DrawColumnInplaceEditor(e, _Item, EditValue, GetRightIndent())
            End If
        End Sub

        Public Function GetRightIndent() As Integer
            Return If(_Column.OptionsColumn.AllowSort <> DefaultBoolean.False OrElse _Column.OptionsFilter.AllowFilter, 25, 0)
        End Function

        Private Sub view_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs)
            CloseEditor()
            Dim editorBounds As Rectangle
            If ClickInEditor(e, editorBounds) Then
                ShowEditor(editorBounds)
                DXMouseEventArgs.GetMouseArgs(e).Handled = True
            End If
        End Sub

        Private Function ClickInEditor(ByVal e As MouseEventArgs, <Out> ByRef editorBounds As Rectangle) As Boolean
            editorBounds = Rectangle.Empty
            Dim vi As GridViewInfo = TryCast(view.GetViewInfo(), GridViewInfo)
            Dim columnInfo As GridColumnInfoArgs = CalcColumnHitInfo(e.Location, vi.ColumnsInfo)
            If columnInfo IsNot Nothing AndAlso columnInfo.Column Is _Column Then
                editorBounds = GetEditorBounds(columnInfo.Bounds, GetRightIndent())
                Return editorBounds.Contains(e.Location)
            End If

            Return False
        End Function

        Protected Overridable Function CalcColumnHitInfo(ByVal pt As Point, ByVal cols As GridColumnsInfo) As GridColumnInfoArgs
            For Each ci As GridColumnInfoArgs In cols
                If Not ci.Bounds.IsEmpty AndAlso IntInRange(pt.X, ci.Bounds.Left, ci.Bounds.Right) Then
                    If ci.Type = GridColumnInfoType.EmptyColumn Then Continue For
                    Return ci
                End If
            Next

            Return Nothing
        End Function

        Protected Function IntInRange(ByVal x As Integer, ByVal left As Integer, ByVal right As Integer) As Boolean
            If right < left Then
                Dim temp As Integer = left
                left = right
                right = temp
            End If

            Return x >= left AndAlso x < right
        End Function

        Private Sub ShowEditor(ByVal bounds As Rectangle)
            ActiveEditor = _Item.CreateEditor()
            ActiveEditor.Properties.LockEvents()
            ActiveEditor.Parent = view.GridControl
            ActiveEditor.Properties.Assign(_Item)
            ActiveEditor.Properties.AutoHeight = False
            ActiveEditor.Location = bounds.Location
            ActiveEditor.Size = bounds.Size
            ActiveEditor.CreateControl()
            ActiveEditor.EditValue = EditValue
            ActiveEditor.SendMouse(ActiveEditor.PointToClient(Control.MousePosition), Control.MouseButtons)
            ActiveEditor.Properties.UnLockEvents()
        End Sub

        Private Sub CloseEditor()
            If ActiveEditor IsNot Nothing Then
                EditValue = ActiveEditor.EditValue
                ActiveEditor.Dispose()
                ActiveEditor = Nothing
            End If
        End Sub

        Private Sub editor_Leave(ByVal sender As Object, ByVal e As EventArgs)
            CloseEditor()
        End Sub
    End Class
End Namespace
