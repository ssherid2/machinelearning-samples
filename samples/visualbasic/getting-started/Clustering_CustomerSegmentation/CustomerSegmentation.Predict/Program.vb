﻿Imports CustomerSegmentation.Model
Imports System.IO
Imports Microsoft.ML

Namespace CustomerSegmentation
    Public Class Program
        Shared Sub Main(args() As String)
            Dim assetsRelativePath = "../../../assets"
            Dim assetsPath As String = GetAbsolutePath(assetsRelativePath)

            Dim pivotCsv = Path.Combine(assetsPath, "inputs", "pivot.csv")
            Dim modelPath = Path.Combine(assetsPath, "inputs", "retailClustering.zip")
            Dim plotSvg = Path.Combine(assetsPath, "outputs", "customerSegmentation.svg")
            Dim plotCsv = Path.Combine(assetsPath, "outputs", "customerSegmentation.csv")

            Try
                Dim mlContext As MLContext = New MLContext 'Seed set to any number so you have a deterministic results

                'Create the clusters: Create data files and plot a chart
                Dim clusteringModelScorer = New ClusteringModelScorer(mlContext, pivotCsv, plotSvg, plotCsv)
                clusteringModelScorer.LoadModel(modelPath)

                clusteringModelScorer.CreateCustomerClusters()
            Catch ex As Exception
                Common.ConsoleHelper.ConsoleWriteException(ex.ToString())
            End Try

            Common.ConsoleHelper.ConsolePressAnyKey()
        End Sub

        Public Shared Function GetAbsolutePath(relativePath As String) As String
            Dim _dataRoot As New FileInfo(GetType(Program).Assembly.Location)
            Dim assemblyFolderPath As String = _dataRoot.Directory.FullName

            Dim fullPath As String = Path.Combine(assemblyFolderPath, relativePath)

            Return fullPath
        End Function
    End Class
End Namespace
