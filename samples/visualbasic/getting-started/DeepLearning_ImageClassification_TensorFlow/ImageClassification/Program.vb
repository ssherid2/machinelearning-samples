﻿Imports ImageClassification.ModelScorer
Imports System.IO

Namespace ImageClassification
    Module Program
        Sub Main(args() As String)
            Dim assetsRelativePath As String = "../../../assets"
            Dim assetsPath As String = GetAbsolutePath(assetsRelativePath)

            Dim tagsTsv = Path.Combine(assetsPath, "inputs", "images", "tags.tsv")
            Dim imagesFolder = Path.Combine(assetsPath, "inputs", "images")
            Dim inceptionPb = Path.Combine(assetsPath, "inputs", "inception", "tensorflow_inception_graph.pb")
            Dim labelsTxt = Path.Combine(assetsPath, "inputs", "inception", "imagenet_comp_graph_label_strings.txt")

            Dim customInceptionPb = Path.Combine(assetsPath, "inputs", "inception_custom", "model_tf.pb")
            Dim customLabelsTxt = Path.Combine(assetsPath, "inputs", "inception_custom", "labels.txt")

            Try
                Dim modelScorer = New TFModelScorer(tagsTsv, imagesFolder, inceptionPb, labelsTxt)
                modelScorer.Score()

            Catch ex As Exception
                ConsoleHelpers.ConsoleWriteException(ex.Message)
            End Try

            ConsoleHelpers.ConsolePressAnyKey()
        End Sub

        Public Function GetAbsolutePath(relativePath As String) As String
            Dim _dataRoot As New FileInfo(GetType(Program).Assembly.Location)
            Dim assemblyFolderPath As String = _dataRoot.Directory.FullName

            Dim fullPath As String = Path.Combine(assemblyFolderPath, relativePath)

            Return fullPath
        End Function
    End Module
End Namespace
