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
Imports System.Configuration
Imports System.IO
''' <summary>
'''  Load Device and Pattern data from StreamReader
''' </summary>
''' <author>eberhard speer jr.</author>
''' <remarks>-</remarks>
Friend NotInheritable Class Loader
    '
    Private Property deviceList As IDictionary(Of String, Device)

#Region "Properties"
    ''' <summary>
    '''  Returns Device data dictionary
    ''' </summary>
    ''' <returns>IDictionary(Of String, Device)</returns>
    ''' <remarks>-</remarks>
    Public ReadOnly Property Devices() As IDictionary(Of String, Device)
        Get
            Return deviceList
        End Get
    End Property
#End Region ' Properties

#Region "Constructor"
    ''' <summary>
    '''  Default new Device data Loader
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
        deviceList = New Dictionary(Of String, Device)
        Try
            Dim folder As String = ConfigurationManager.ConnectionStrings(Constants.APP_NAME).ToString.Trim.ToLowerInvariant
            If folder.StartsWith(Constants.HTTP_PREFIX) Then
                folder = folder.TrimEnd(CChar("/"))
                ' Devices
                For Each xmlFile In {Constants.DEVICE_DATA, Constants.DEVICE_DATA_PATCH}
                    LoadDeviceData(New UrlLoader(String.Format("{0}/{1}", folder, xmlFile)).Reader)
                Next
                ' Patterns
                For Each xmlFile In {Constants.BUILDER_DATA, Constants.BUILDER_DATA_PATCH}
                    LoadDevicePatterns(New UrlLoader(String.Format("{0}/{1}", folder, xmlFile)).Reader)
                Next
            Else
                folder = folder.TrimEnd(CChar("\"))
                ' Devices
                For Each xmlFile In {Constants.DEVICE_DATA, Constants.DEVICE_DATA_PATCH}
                    LoadDeviceData(New FileLoader(String.Format("{0}/{1}", folder, xmlFile)).Reader)
                Next
                ' Patterns
                For Each xmlFile In {Constants.BUILDER_DATA, Constants.BUILDER_DATA_PATCH}
                    LoadDevicePatterns(New FileLoader(String.Format("{0}/{1}", folder, xmlFile)).Reader)
                Next
            End If
        Catch ex As NullReferenceException
            Throw New OpenDDRException(String.Format(Constants.CONFIG_ERROR_CONN_FORMAT, AppDomain.CurrentDomain.SetupInformation.ConfigurationFile), ex)
        Catch ex As System.Net.WebException
            Throw New OpenDDRException(String.Format(Constants.WEB_ERROR_FORMAT, ex.Message), ex)
        Catch ex As ArgumentException
            Throw New OpenDDRException(ex.Message, ex)
        Catch ex As Exception
            Throw New OpenDDRException(ex.Message, ex)
        End Try
    End Sub
#End Region ' Constructor

#Region "Methods"
    ''' <summary>
    '''  Load Device data from StreamReader
    ''' </summary>
    ''' <param name="inSteam">StreamReader</param>
    ''' <remarks>-</remarks>
    Private Sub LoadDeviceData(inSteam As IO.StreamReader)
        Dim parser As New XmlParser(inSteam)
        Dim tag As String = String.Empty
        Try
            Dim device As New Device()
            Dim attributes As New Dictionary(Of String, String)
            While (Util.InlineAssignHelper(tag, parser.NextTag)).Length > 0
                If tag.StartsWith("<device ") Then
                    device.Id = XmlParser.getAttribute(tag, "id")
                    device.ParentId = XmlParser.getAttribute(tag, "parentId")
                ElseIf tag.Equals("</device>") Then
                    If Not String.IsNullOrEmpty(device.Id) Then
                        attributes("id") = device.Id
                        device.Attributes = attributes
                        If Devices.ContainsKey(device.Id) Then
                            Devices(device.Id) = device
                        Else
                            Devices.Add(device.Id, device)
                        End If
                    End If
                    ' reset
                    device = New Device()
                    attributes = New Dictionary(Of String, String)
                ElseIf tag.StartsWith("<property ") Then
                    Dim key As String = XmlParser.getAttribute(tag, "name")
                    Dim value As String = XmlParser.getAttribute(tag, "value")
                    attributes(key) = value
                End If
            End While
        Catch ex As Exception
            Throw New ArgumentException(String.Format("loadDeviceData : {0}", ex.Message), ex)
        End Try
    End Sub
    ''' <summary>
    '''  Load Device Pattern data from StreamReader
    ''' </summary>
    ''' <param name="inStream">StreamReader</param>
    ''' <remarks></remarks>
    Private Sub LoadDevicePatterns(inStream As IO.StreamReader)
        Dim parser As New XmlParser(inStream)
        Dim tag As String = ""
        Try
            Dim builder As String = ""
            Dim device As Device = Nothing
            Dim id As String = ""
            Dim patterns As New List(Of String)
            While (Util.InlineAssignHelper(tag, parser.NextTag)).Length > 0
                If tag.StartsWith("<builder ") Then
                    builder = XmlParser.getAttribute(tag, "class")
                    If builder.LastIndexOf(".") >= 0 Then
                        builder = builder.Substring(builder.LastIndexOf(".") + 1)
                    End If
                ElseIf tag.StartsWith("<device ") Then
                    device = Devices(XmlParser.getAttribute(tag, "id"))
                ElseIf tag.Equals("</device>") Then
                    If device IsNot Nothing Then
                        If builder.Equals("TwoStepDeviceBuilder") Then
                            device.Patterns.AndPattern = patterns
                            Dim unigram As String = ""
                            For Each pattern As String In patterns
                                If pattern.Contains(unigram) Then
                                    unigram = pattern
                                Else
                                    unigram &= pattern
                                End If
                            Next
                            device.Patterns.Pattern = unigram
                        Else
                            device.Patterns.OrPattern = patterns
                        End If
                        If builder.Equals("SimpleDeviceBuilder") Then
                            device.Type = "simple"
                        Else
                            device.Type = "weak"
                        End If
                    Else
                        Util.Log("ERROR: device not found: '" & id & "'")
                    End If
                    ' reset
                    device = Nothing
                    id = ""
                    patterns = New List(Of String)()
                ElseIf tag.Equals("<value>") Then
                    Dim pattern As String = Util.Normalize(parser.TagValue)
                    If String.IsNullOrEmpty(pattern) Then
                        Continue While
                    End If
                    patterns.Add(pattern)
                End If
            End While
        Catch ex As Exception
            Throw New ArgumentException(String.Format("loadDevicePatterns : {0}", ex.Message), ex)
        End Try
    End Sub
    ''' <summary>
    '''  Recursively add Device's Parent Attributes
    ''' </summary>
    ''' <param name="device">Device</param>
    ''' <remarks>-</remarks>
    Private Sub MergeParent(device As Device)
        Dim parentId As String = device.ParentId
        If String.IsNullOrEmpty(parentId) Then
            Return
        End If
        Dim parent As Device = Nothing
        If Not deviceList.TryGetValue(parentId, parent) Then
            Return
        End If
        MergeParent(parent)
        For Each key As String In parent.Attributes.Keys
            If Not device.Attributes.ContainsKey(key) Then
                device.Attributes(key) = parent.Attributes(key)
            End If
        Next
    End Sub
    ''' <summary>
    '''  Sets Parent device attributes
    ''' </summary>
    ''' <remarks>-</remarks>
    Private Sub setParentAttributes()
        For Each device As Device In deviceList.Values
            MergeParent(device)
        Next
    End Sub
#End Region ' Methods
End Class