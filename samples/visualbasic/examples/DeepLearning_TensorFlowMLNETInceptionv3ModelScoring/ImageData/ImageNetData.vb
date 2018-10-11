﻿Imports System.IO
Imports Microsoft.ML.Runtime.Api

Namespace ImageData
    Public Class ImageNetData
        <Column("0")>
        Public ImagePath As String

        <Column("1")>
        Public Label As String

        Public Shared Function ReadFromCsv(file As String, folder As String) As IEnumerable(Of ImageNetData)
            Return From f In IO.File.ReadAllLines(file)
                   Let x = f.Split(vbTab)
                   Select New ImageNetData With {
                       .ImagePath = Path.Combine(folder, x(0)),
                       .Label = x(1)
                   }
        End Function
    End Class
End Namespace