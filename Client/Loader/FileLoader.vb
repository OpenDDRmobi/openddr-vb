#Region "Header"
' Copyright (c) 2011-2017 OpenDDR LLC and others. All rights reserved.
'
' Licensed under the Apache License, Version 2.0 (the "License");
' you may Not use this file except in compliance with the License.
' You may obtain a copy of the License at
' http://www.apache.org/licenses/LICENSE-2.0
'
'  Unless required by applicable law Or agreed to in writing,
'  software distributed under the License Is distributed on an
'  "AS IS" BASIS, WITHOUT WARRANTIES Or CONDITIONS OF ANY
'  KIND, either express Or implied.  See the License for the
'  specific language governing permissions And limitations
'  under the License.
#End Region ' Header
'
Imports System.IO
''' <summary>
'''  Load XML resources from local file
''' </summary>
''' <remarks>-</remarks>
Friend NotInheritable Class FileLoader : Implements ILoader
    '
    Private Property resLength As Long = -1
    Private Property resReader As IO.StreamReader = Nothing
    Private Property resUrl As String = String.Empty
    '
#Region "Properties"
    ''' <summary>
    '''  Resource path
    ''' </summary>
    ''' <returns>String</returns>
    ''' <remarks>-</remarks>
    Public ReadOnly Property Path As String Implements ILoader.Path
        Get
            Return resUrl
        End Get
    End Property
    ''' <summary>
    '''  Returns the resource file length
    ''' </summary>
    ''' <returns>Long</returns>
    ''' <remarks>file length</remarks>
    Public ReadOnly Property ResponseLength As Long Implements ILoader.ResponseLength
        Get
            Return resLength
        End Get
    End Property
    ''' <summary>
    '''  Reader
    ''' </summary>
    ''' <returns>StreamReader</returns>
    ''' <remarks>-</remarks>
    Public ReadOnly Property Reader() As IO.StreamReader Implements ILoader.Reader
        Get
            Return resReader
        End Get
    End Property
#End Region ' Properties

#Region "Constructor"
    ''' <summary>
    '''  Load resource for path string
    ''' </summary>
    ''' <param name="filePath">path and file name of resource file</param>
    ''' <exception cref="ArgumentException">thrown when file does not exist</exception>
    ''' <remarks>-</remarks>
    Public Sub New(filePath As String)
        resUrl = filePath.Trim
        If IO.File.Exists(resUrl) Then
            resLength = New IO.FileInfo(filePath).Length
            resReader = New IO.StreamReader(resUrl)
        Else
            Throw New ArgumentException(String.Format(Constants.FILE_ERROR_FORMAT, resUrl))
        End If
    End Sub
#End Region ' Constructor
End Class