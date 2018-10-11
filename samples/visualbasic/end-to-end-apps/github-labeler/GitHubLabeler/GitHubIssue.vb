﻿Imports Microsoft.ML.Runtime.Api

#Disable Warning IDE0044 ' We don't care about unsused fields here, because they are mapped with the input file.

Namespace GitHubLabeler
	Friend Class GitHubIssue
        <Column("0")>
        Public ID As String

        <Column("1")>
        Public Area As String ' This is an issue label, for example "area-System.Threading"

        <Column("2")>
        Public Title As String

        <Column("3")>
        Public Description As String
    End Class
End Namespace