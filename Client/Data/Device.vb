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
''' <summary>
'''  Device
''' </summary>
''' <author>eberhard speer jr.</author>
''' <remarks>Apache's DeviceMap Project .Net version<br /> 
'''          ported from Reza Naghibi's Device.java</remarks>
Public NotInheritable Class Device
    '
    Private builderType As String = String.Empty
    Private deviceId As String = String.Empty
    Private deviceParent As String = String.Empty
    Private pattern As Pattern
    Private properties As IDictionary(Of String, String)

#Region "Properties"
    ''' <summary>
    '''  Property dictionary
    ''' </summary>
    ''' <returns>IDictionary(Of String, String)</returns>
    ''' <remarks>-</remarks>
    Public Property Attributes() As IDictionary(Of String, String)
        Get
            Return properties
        End Get
        Set(value As IDictionary(Of String, String))
            properties = value
        End Set
    End Property
    ''' <summary>
    '''  Unique Id
    ''' </summary>
    ''' <returns>String</returns>
    ''' <remarks>-</remarks>
    Public Property Id() As String
        Get
            Return deviceId
        End Get
        Set(value As String)
            deviceId = value
        End Set
    End Property
    ''' <summary>
    '''  Unique Parent Id
    ''' </summary>
    ''' <returns>String</returns>
    ''' <remarks>-</remarks>
    Public Property ParentId() As String
        Get
            Return deviceParent
        End Get
        Set(value As String)
            deviceParent = value
        End Set
    End Property
    ''' <summary cref="Pattern">
    '''  Pattern collection
    ''' </summary>
    ''' <returns>Pattern</returns>
    ''' <remarks>Collection of patterns for 'matching' with User-Agent string</remarks>
    Public ReadOnly Property Patterns() As Pattern
        Get
            Return pattern
        End Get
    End Property
    ''' <summary>
    '''  Builder type
    ''' </summary>
    ''' <returns>String</returns>
    ''' <remarks>used to destinguish between 'simple' and 'two-step' device builders</remarks>
    Public Property Type() As String
        Get
            Return builderType
        End Get
        Set(value As String)
            builderType = value
        End Set
    End Property
#End Region ' Properties

#Region "Constructor"
    ''' <summary>
    '''  Default new Device
    ''' </summary>
    ''' <remarks>-</remarks>
    Public Sub New()
        pattern = New Pattern()
    End Sub
#End Region ' Constructor

#Region "Functions"
    ''' <summary>
    '''  ToString override
    ''' </summary>
    ''' <returns>String</returns>
    ''' <remarks>-</remarks>
    Public Overrides Function ToString() As String
        Return String.Format(Constants.DEVICE_TOSTRING_FORMAT, deviceId, deviceParent, builderType, pattern.ToString, properties.ToString)
    End Function
#End Region ' Functions
End Class