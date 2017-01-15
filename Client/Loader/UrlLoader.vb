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
Imports System.Net
''' <summary>
'''  Load XML resources from URL
''' </summary>
''' <remarks>-</remarks>
Friend NotInheritable Class UrlLoader : Implements ILoader
    '
    Private Property resLength As Long = -1
    Private Property resReader As IO.StreamReader = Nothing
    Private Property resUrl As String = String.Empty
    '
#Region "Properties"
    ''' <summary>
    '''  Resource URI
    ''' </summary>
    ''' <returns>String</returns>
    ''' <remarks>-</remarks>
    Public ReadOnly Property Path As String Implements ILoader.Path
        Get
            Return resUrl
        End Get
    End Property
    ''' <summary>
    '''  Response Lenght
    ''' </summary>
    ''' <returns>Long</returns>
    ''' <remarks>-</remarks>
    Public ReadOnly Property ResponseLenght As Long Implements ILoader.ResponseLength
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
    '''  Load resource for url string
    ''' </summary>
    ''' <param name="url">String</param>
    ''' <exception cref="ArgumentException">thrown when HTTP Status-code is not 200 [ok]</exception>
    ''' <remarks>-</remarks>
    Public Sub New(url As String)
        resUrl = url.Trim
        Dim resStatus As Integer = 0
        Dim ddrRequest As HttpWebRequest = CType(WebRequest.Create(New Uri(resUrl)), HttpWebRequest)
        ddrRequest.UserAgent = String.Format("{0} {1}", Constants.DEVICE_UA, Constants.RELEASE_VERSION)
        ddrRequest.AllowAutoRedirect = False
        Dim ddrResponse As WebResponse = ddrRequest.GetResponse()
        resStatus = CType(ddrResponse, HttpWebResponse).StatusCode
        Select Case resStatus
            Case 200
                resLength = ddrResponse.ContentLength
                resReader = New System.IO.StreamReader(ddrResponse.GetResponseStream())
            Case Is > 299
                Throw New ArgumentException(String.Format("HTTP Status code : {0}", resStatus.ToString))
            Case Else
                Throw New ArgumentException(String.Format("Weird HTTP Status code : {0}", resStatus.ToString))
        End Select
    End Sub
#End Region ' Constructor
End Class