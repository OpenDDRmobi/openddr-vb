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
Imports System.Reflection
Imports System.Text.RegularExpressions
''' <summary>
'''  OpenDDR User-Agent Mapper
''' </summary>
''' <author>Eberhard Speer jr.</author>
''' <author>Werner Keil</author>
''' <remarks>OpenDDR Project VB .Net version<br /> 
'''          ported from OpenDDR Classifier</remarks>
Public Class Classifier
    '
    Public devices As IDictionary(Of String, Device)
    Friend patterns As IDictionary(Of String, List(Of Device))

#Region "Properties"
    ''' <summary>
    '''  Main Release and  build version
    ''' </summary>
    ''' <returns>String</returns>
    ''' <remarks>-</remarks>
    Public ReadOnly Property Version As String
        Get
            Return String.Format(Constants.VERSION_FORMAT, Constants.RELEASE_VERSION, Assembly.GetExecutingAssembly().GetName().Version.ToString)
        End Get
    End Property
    ''' <summary>
    '''  Returns number of Devices in Device data
    ''' </summary>
    ''' <returns>Integer</returns>
    ''' <remarks>-</remarks>
    Public ReadOnly Property DeviceCount() As Integer
        Get
            Return devices.Count
        End Get
    End Property
    ''' <summary>
    '''  Returns number of Device Patterns in Device pattern data
    ''' </summary>
    ''' <returns>Integer</returns>
    ''' <remarks>-</remarks>
    Public ReadOnly Property PatternCount() As Integer
        Get
            Return patterns.Count
        End Get
    End Property
#End Region ' Properties

#Region "Constructor"
    ''' <summary>
    ''' New Device User-Agent Resolver
    ''' </summary>
    ''' <exception cref="OpenDDRException">Thrown when (InnerException)<ul>
    '''                                                   <li>NullReferenceException : OpenDDR ConnectionStrings missing in config file</li>
    '''                                                   <li>WebException : URL Loader exception</li>
    '''                                                   <li>ArgumentException : File Loader exception</li> 
    '''                                                   <li>ArgumentException : Loader exception</li> 
    '''                                                  </ul>
    ''' </exception>
    ''' <remarks>-</remarks>
    Public Sub New()
        devices = New Dictionary(Of String, Device)
        patterns = New Dictionary(Of String, List(Of Device))
        devices = New Loader().Devices
        CreateIndex()
    End Sub
#End Region ' Constructor

#Region "Methods"
    ''' <summary>
    '''  Create Device and Pattern Index 
    ''' </summary>
    ''' <remarks>-</remarks>
    Private Sub CreateIndex()
        For Each device As Device In devices.Values()
            For Each patternset As IList(Of String) In device.Patterns.Patterns
                For i As Integer = 0 To patternset.Count - 1
                    Dim pattern As String = patternset(i)
                    ' deal with duplicates
                    If patterns.ContainsKey(pattern) Then
                        If i = (patternset.Count - 1) AndAlso Not patterns(pattern).Contains(device) Then
                            patterns(pattern).Add(device)
                        End If
                    Else
                        Dim subList As New List(Of Device)
                        subList.Add(device)
                        If patterns.ContainsKey(pattern) Then
                            patterns(pattern) = subList
                        Else
                            patterns.Add(pattern, subList)
                        End If
                    End If
                Next
            Next
        Next
    End Sub
#End Region ' Methods

#Region "Functions"
    ''' <summary>
    '''  Main Resolver function : Returns Attribute dictionary for device resolved from useragent
    ''' </summary>
    ''' <param name="useragent">user-agnet string to resolve</param>
    ''' <returns>IDictionary(Of String, String)</returns>
    ''' <remarks>-</remarks>
    Public Function Map(useragent As String) As IDictionary(Of String, String)
        '' added ;
        Dim parts As String() = Regex.Split(useragent, Constants.USER_AGENT_SPLIT)
        Dim hits As New Dictionary(Of String, IList(Of Device))
        For i As Integer = 0 To parts.Length - 1
            Dim pattern As String = ""
            Dim j As Integer = 0
            While j < 4 AndAlso (j + i) < parts.Length
                If Not String.IsNullOrEmpty(parts(i + j)) Then
                    pattern &= Util.Normalize(parts(i + j))
                    If patterns.ContainsKey(pattern) Then
                        hits(pattern) = patterns(pattern)
                    End If
                End If
                j += 1
            End While
        Next
        Dim winner As Device = Nothing
        Dim winnerStr As String = String.Empty
        For Each hit As String In hits.Keys
            For Each device As Device In hits(hit)
                If device.Patterns.isValid(hits.Keys.ToList) Then
                    If winner IsNot Nothing Then
                        If Constants.SIMPLE.Equals(winner.Type) AndAlso Not Constants.SIMPLE.Equals(device.Type) Then
                            winner = device
                            winnerStr = hit
                            ''ElseIf hit.Length > winnerStr.Length AndAlso Not "simple".Equals(device.Type) Then
                        ElseIf hit.Length > winnerStr.Length AndAlso (Not Constants.SIMPLE.Equals(device.Type) OrElse device.Type.Equals(winner.Type)) Then
                            winner = device
                            winnerStr = hit
                        End If
                    Else
                        winner = device
                        winnerStr = hit
                    End If
                End If
            Next
        Next
        If winner IsNot Nothing Then
            Return winner.Attributes
        Else
            Return Nothing
        End If
    End Function
#End Region ' Functions
End Class