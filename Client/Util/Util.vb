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
Imports System.Text
''' <summary>
'''  Utilities
''' </summary>
''' <author>Eberhard Speer jr.</author>
''' <author>Werner Keil</author>
''' <remarks>OpenDDR Project VB .Net version<br /> 
'''          ported from OpenDDR Classifier Util.java</remarks>
Public NotInheritable Class Util
    ''' <summary>
    '''  Inline Assign Helper
    ''' </summary>
    ''' <remarks>-</remarks>
    Public Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
        target = value
        Return value
    End Function
    ''' <summary>
    '''  Console debug messages
    ''' </summary>
    ''' <param name="msg">message</param>
    ''' <remarks>-</remarks>
    Public Shared Sub Log(msg As String)
        Log(msg, Nothing)
    End Sub
    ''' <summary>
    '''  Console debug exception messages
    ''' </summary>
    ''' <param name="msg">message</param>
    ''' <param name="e">Exception</param>
    ''' <remarks>-</remarks>
    Public Shared Sub Log(msg As String, e As Exception)
        Console.WriteLine(String.Format("{0} - {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), msg))
        If e IsNot Nothing Then
            Console.WriteLine("Exception")
            Console.WriteLine(ExceptionToString("console", e))
        End If
    End Sub
    ''' <summary>
    '''  Normailze pattern to letters and digits only
    ''' </summary>
    ''' <param name="dirty">string to normalize</param>
    ''' <returns>String</returns>
    ''' <remarks>-</remarks>
    Public Shared Function Normalize(dirty As String) As String
        If String.IsNullOrEmpty(dirty) Then
            Return dirty
        End If
        dirty = dirty.ToLower.Trim.Replace("[bb]", "b")
        Dim builder As New StringBuilder()
        For i As Integer = 0 To dirty.Length - 1
            Dim c As System.Nullable(Of Char) = dirty(i)
            If Char.IsLetter(CChar(c)) OrElse Char.IsDigit(CChar(c)) Then
                builder.Append(c)
            End If
        Next
        Return builder.ToString()
    End Function
    '' <summary>
    ''  Log Object to Application Log
    '' </summary>
    '' <param name="entryStr">String</param>
    '' <param name="type">EventLogEntryType</param>
    '' <remarks></remarks>
    'Public Shared Sub WriteEntry(appName As String, entryStr As String, Optional type As EventLogEntryType = EventLogEntryType.Warning, Optional ex As Exception = Nothing)
    '    Using myLog As New Diagnostics.EventLog()
    '        myLog.Source = appName
    '        If Not Diagnostics.EventLog.SourceExists(myLog.Source) Then
    '            Diagnostics.EventLog.CreateEventSource(myLog.Source, "Application")
    '        End If
    '        If ex IsNot Nothing Then
    '            entryStr = String.Format("{0} : {1}", entryStr, ExceptionToString("ex", ex))
    '        End If
    '        Dim btText() As Byte = Encoding.UTF8.GetBytes(entryStr)
    '        If btText.Length > 32766 Then
    '            ' The message string is longer than 32766 bytes.
    '            entryStr = BitConverter.ToString(btText, 0, 32760)
    '        End If           
    '        myLog.WriteEntry(entryStr, type)
    '    End Using
    'End Sub
    ''' <summary>
    '''  Exception to string
    ''' </summary>
    ''' <param name="id">some id</param>
    ''' <param name="ex">Exception</param>
    ''' <returns>String</returns>
    ''' <remarks>-</remarks>
    Private Shared Function ExceptionToString(id As String, ex As Exception) As String
        Dim result As New StringBuilder()
        If Not String.IsNullOrEmpty(id) Then
            result.AppendLine(String.Format("Id : {0}", id))
        End If
        result.AppendLine(String.Format("Message : {0}", ex.Message))
        If Not ex.Data.Count = 0 Then
            result.AppendLine("Data : ")
            For Each de As DictionaryEntry In ex.Data
                result.AppendLine(de.Key.ToString)
            Next
        End If
        result.AppendLine(String.Format("Source : {0}", ex.Source))
        result.AppendLine(String.Format("TargetSite : {0}", ex.TargetSite.ToString))
        result.AppendLine(String.Format("StackTrace : {0}", ex.StackTrace))
        Return result.ToString
    End Function
    ''' <summary>
    '''  Returns directory of config file of running assembly
    ''' </summary>
    ''' <returns>String</returns>
    ''' <remarks>-</remarks>
    Public Shared Function Home() As String
        Return New IO.FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile).DirectoryName
    End Function
End Class