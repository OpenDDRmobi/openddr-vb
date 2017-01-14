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