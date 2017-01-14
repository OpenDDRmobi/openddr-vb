#Region "Header"
'   Licensed to the Apache Software Foundation (ASF) under one
'   or more contributor license agreements.  See the NOTICE file
'   distributed with this work for additional information
'   regarding copyright ownership.  The ASF licenses this file
'   to you under the Apache License, Version 2.0 (the
'   "License"); you may not use this file except in compliance
'   with the License.  You may obtain a copy of the License at
'
'     http://www.apache.org/licenses/LICENSE-2.0
'
'   Unless required by applicable law or agreed to in writing,
'   software distributed under the License is distributed on an
'   "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
'   KIND, either express or implied.  See the License for the
'   specific language governing permissions and limitations
'   under the License.
#End Region ' Header
'
Imports System.Text
''' <summary>
'''  Device Pattern data
''' </summary>
''' <author>eberhard speer jr.</author>
''' <remarks>OpenDDR VB .Net version<br />
'''          ported from OpenDDR Classifier Pattern.java</remarks>
Public NotInheritable Class Pattern
    '
    Private patternList As IList(Of IList(Of String))

#Region "Properties"
    ''' <summary>
    '''  List of Patterns which <em>all</em> must occur in User-Agent string for a match
    ''' </summary>
    ''' <remarks>-</remarks>
    Public WriteOnly Property AndPattern() As IList(Of String)
        Set(value As IList(Of String))
            patternList.Add(value)
        End Set
    End Property
    ''' <summary>
    '''  List of Patterns of which <em>at least one</em> must occur in User-Agent string for a match
    ''' </summary>
    ''' <remarks>-</remarks>
    Public WriteOnly Property OrPattern() As IList(Of String)
        Set(value As IList(Of String))
            For Each patternString As String In value
                Pattern = patternString
            Next
        End Set
    End Property
    ''' <summary>
    '''  List of Patterns to match with User-Agent string
    ''' </summary>
    ''' <remarks>-</remarks>
    Public WriteOnly Property Pattern() As String
        Set(value As String)
            Dim subList As New List(Of String)
            subList.Add(value)
            patternList.Add(subList)
        End Set
    End Property
    ''' <summary>
    '''  List of Patterns Lists to match with User-Agent string
    ''' </summary>
    ''' <returns>IList(Of IList(Of String))</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Patterns() As IList(Of IList(Of String))
        Get
            Return patternList
        End Get
    End Property
#End Region ' Properties

#Region "Constructor"
    ''' <summary>
    '''  Default new Device Pattern data
    ''' </summary>
    ''' <remarks>-</remarks>
    Public Sub New()
        patternList = New List(Of IList(Of String))
    End Sub
#End Region ' Constructor

#Region "Functions"
    ''' <summary>
    '''  Returns true if one of the patterns in patternList occurs in Device Pattern data
    ''' </summary>
    ''' <param name="patternList">List(Of String)</param>
    ''' <returns>Boolean</returns>
    ''' <remarks>-</remarks>
    Public Function isValid(patternList As List(Of String)) As Boolean
        Dim found As Boolean = False
        For Each patternset As IList(Of String) In Patterns
            For Each pattern As String In patternset
                If Not patternList.Contains(pattern) Then
                    GoTo patternsContinue
                End If
            Next
            found = True
            Exit For
patternsContinue:
        Next
        Return found
    End Function
    ''' <summary>
    '''  ToString override
    ''' </summary>
    ''' <returns>String</returns>
    ''' <remarks>-</remarks>
    Public Overrides Function ToString() As String
        Dim builder As New StringBuilder
        For Each sublist As List(Of String) In patternList
            builder.AppendFormat("'{0}',", String.Join(",", sublist.ToArray()))
        Next
        Return builder.ToString.TrimEnd(CChar(","))
    End Function
#End Region ' Functions
End Class