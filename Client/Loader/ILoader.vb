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
'
Public Interface ILoader
    ''' <summary>
    '''  Resource URI
    ''' </summary>
    ''' <returns>String</returns>
    ''' <remarks>-</remarks>
    ReadOnly Property Path As String
    ''' <summary>
    '''  Response Lenght
    ''' </summary>
    ''' <returns>Long</returns>
    ''' <remarks>-</remarks>
    ReadOnly Property ResponseLength As Long
    ''' <summary>
    '''  Reader
    ''' </summary>
    ''' <returns>StreamReader</returns>
    ''' <remarks>-</remarks>
    ReadOnly Property Reader As StreamReader
End Interface