Imports System.ComponentModel
Imports System.Drawing
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraEditors.ViewInfo
Imports DevExpress.XtraEditors.Drawing
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraGrid.Views.Grid

Namespace WindowsApplication1

    Public Module DrawEditorHelper

        Public Sub DrawEdit(ByVal g As Graphics, ByVal edit As RepositoryItem, ByVal r As Rectangle, ByVal value As Object)
            Dim info As BaseEditViewInfo = edit.CreateViewInfo()
            info.EditValue = value
            info.Bounds = r
            info.CalcViewInfo(g)
            Dim args As ControlGraphicsInfoArgs = New ControlGraphicsInfoArgs(info, New GraphicsCache(g), r)
            edit.CreatePainter().Draw(args)
            args.Cache.Dispose()
        End Sub

        Public Function GetEditorBounds(ByVal totalBounds As Rectangle, ByVal rightIdent As Integer) As Rectangle
            Dim leftIndent As Integer = 2
            Dim topIndent As Integer = 2
            Dim bounds As Rectangle = totalBounds
            bounds.Inflate(-leftIndent, -topIndent)
            bounds.Width -= rightIdent
            Return bounds
        End Function

        Public Sub DrawColumnInplaceEditor(ByVal e As ColumnHeaderCustomDrawEventArgs, ByVal item As RepositoryItem, ByVal value As Object, ByVal rightIndent As Integer)
            Dim targetRect As Rectangle = GetEditorBounds(e.Bounds, rightIndent)
            DrawEdit(e.Graphics, item, targetRect, value)
        End Sub
    End Module
End Namespace
