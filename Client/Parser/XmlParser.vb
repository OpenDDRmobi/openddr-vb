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
'''  XML Parser
''' </summary>
''' <author>eberhard speer jr.</author>
''' <remarks>OpenDDR Project VB .Net version<br /> 
'''          ported from OpenDDR Classifier XMLParser.java</remarks>
Friend NotInheritable Class XmlParser
    '
    Private inStream As IO.StreamReader
    Private pre As Char = ChrW(0)

#Region "Properties"
    ''' <summary>
    '''  Returns next XML tag in StreamReader
    ''' </summary>
    ''' <returns>String</returns>
    ''' <remarks>-</remarks>
    Public ReadOnly Property NextTag() As String
        Get
            Dim localBuilder As New StringBuilder()

            Dim i As Integer
            Dim start As Boolean = False

            If pre = "<"c Then
                localBuilder.Append(pre)
                pre = ChrW(0)
                start = True
            End If

            While (Util.InlineAssignHelper(i, inStream.Read())) <> -1
                Dim c As Char = ChrW(i)
                If c = "<"c Then
                    start = True
                    localBuilder.Append(c)
                ElseIf c = ">"c Then
                    localBuilder.Append(c)
                    Exit While
                ElseIf start Then
                    localBuilder.Append(c)
                End If
            End While

            Return localBuilder.ToString()
        End Get
    End Property
    ''' <summary>
    '''  Returns XML tag value from StreamReader
    ''' </summary>
    ''' <returns>String</returns>
    ''' <remarks>-</remarks>
    Public ReadOnly Property TagValue() As String
        Get
            Dim localBuilder As New StringBuilder()
            Dim i As Integer
            While (Util.InlineAssignHelper(i, inStream.Read())) <> -1
                Dim c As Char = ChrW(i)
                If c = "<"c Then
                    pre = "<"c
                    Exit While
                Else
                    localBuilder.Append(c)
                End If
            End While
            Return localBuilder.ToString.Trim()
        End Get
    End Property
#End Region ' Properties

#Region "Constructor"
    ''' <summary>
    '''  Prevent parameterless new
    ''' </summary>
    ''' <remarks>-</remarks>
    Private Sub New()
        ' Nice !
    End Sub
    ''' <summary>
    '''  New XmlParser for StreamReader
    ''' </summary>
    ''' <param name="stream">StreamReader</param>
    ''' <remarks>-</remarks>
    Public Sub New(stream As IO.StreamReader)
        inStream = stream
    End Sub
#End Region ' Constructor

#Region "Functions"
    ''' <summary>
    '''  Returns Attribute (Device property) value of tag with name
    ''' </summary>
    ''' <param name="tag">XML tag</param>
    ''' <param name="name">Attribute name</param>
    ''' <returns>String</returns>
    ''' <remarks>-</remarks>
    Public Shared Function getAttribute(tag As String, name As String) As String
        Dim retpos As Integer = tag.ToLower.IndexOf(name.ToLower() & "=")
        If retpos = -1 Then
            Return ""
        End If
        Dim result As String = tag.Substring(retpos + name.Length + 1)
        If result.StartsWith("""") Then
            result = result.Substring(1)
            Dim endpos As Integer = result.IndexOf("""")
            If endpos = -1 Then
                Return ""
            End If
            result = result.Substring(0, endpos)
        Else
            Dim endpos As Integer = result.IndexOf(" ")
            If endpos = -1 Then
                Return ""
            End If
            result = result.Substring(0, endpos)
        End If
        Return result
    End Function
#End Region ' Functions
End Class