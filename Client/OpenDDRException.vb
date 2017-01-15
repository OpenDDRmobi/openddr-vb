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
''' <summary>
'''   OpenDDR Exception
''' </summary>
''' <remarks>-</remarks>
<Serializable>
Public NotInheritable Class OpenDDRException : Inherits ApplicationException
    '
#Region "Constructors"
    ''' <summary>
    '''  OpenDDR Exception
    ''' </summary>
    ''' <param name="msg">Message</param>
    ''' <remarks>-</remarks>
    Public Sub New(msg As String, ex As Exception)
        MyBase.New(msg, ex)
    End Sub
#End Region ' Constructors
End Class