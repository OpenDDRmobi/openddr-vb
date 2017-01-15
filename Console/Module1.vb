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
Imports ClassifierClient
''' <summary>
'''  Console app
''' </summary>
''' <remarks>-</remarks>
Module Module1
    '
    Sub Main()
        ' Test Data Directory and File
        Dim testDataDirectory As String = Util.Home
        Dim testDataFile As String = "ua_strings.txt"
        ' Load
        Dim stopWatch As New Diagnostics.Stopwatch()
        stopWatch.Start()
        Dim client As Classifier = Nothing
        Console.WriteLine("Loading...")
        Try
            client = New Classifier()
        Catch ex As OpenDDRException
            '' Util.WriteEntry("OpenDDR", ex.Message, EventLogEntryType.Error, ex)
            Console.WriteLine(ex.Message)
        End Try
        stopWatch.Stop()
        Console.Clear()
        If Not IsNothing(client) Then
            Console.WriteLine("Loaded !")
            Console.WriteLine(String.Format("OpenDDR Classifier Client : {0}", client.Version))
            Console.WriteLine(String.Format("Loaded {0} devices with {1} patterns in {2} ms", client.DeviceCount.ToString, client.PatternCount.ToString, stopWatch.Elapsed.TotalMilliseconds.ToString))
            stopWatch.Restart()
            ' cold run
            Console.WriteLine("Cold run")
            Map(client, "Mozilla/5.0 (Linux; U; Android 2.2; en; HTC Aria A6380 Build/ERE27) AppleWebKit/540.13+ (KHTML, like Gecko) Version/3.1 Mobile Safari/524.15.0")
            Map(client, "Mozilla/5.0 (iPad; U; CPU OS 4_3_5 like Mac OS X; en-us) AppleWebKit/533.17.9 (KHTML, like Gecko) Mobile/8L1")
            Map(client, "Mozilla/5.0 (BlackBerry; U; BlackBerry 9810; en-US) AppleWebKit/534.11+ (KHTML, like Gecko) Version/7.0.0.261 Mobile Safari/534.11+")
            Map(client, "Mozilla/5.0 (iPhone; CPU iPhone OS 6_0 like Mac OS X; en-us) AppleWebKit/536.26 (KHTML, like Gecko) CriOS/23.0.1271.91 Mobile/10A403 Safari/8536.25")
            stopWatch.Stop()
            Console.WriteLine(String.Format("End cold run : {0} ms", stopWatch.Elapsed.TotalMilliseconds.ToString))
            ' test data path and file
            Dim data As String = String.Format("{0}\{1}", testDataDirectory.TrimEnd(CChar("\")), testDataFile)
            Console.WriteLine(String.Format("Press any key to run test file : {0}", data))
            Console.ReadKey()
            ' test
            If IO.File.Exists(data) Then
                Dim lines As New List(Of String)(IO.File.ReadAllLines(data).ToList)
                Dim cleanLines As List(Of String) = (From l In lines Select l Where Not String.IsNullOrWhiteSpace(l)).ToList
                stopWatch.Restart()
                Dim i As Integer = 0
                For Each ua In cleanLines
                    Map(client, ua.Trim)
                    i = i + 1
                Next
                stopWatch.Stop()
                Console.WriteLine(String.Format("Tested {0} User-Agent strings in {1} ms.", i.ToString, stopWatch.Elapsed.TotalMilliseconds.ToString))
            Else
                Console.WriteLine(String.Format("Test file {0} not found.", data))
            End If
            Console.WriteLine("Press any key to finish")
            Console.ReadKey()
        Else
            Console.WriteLine("OpenDDR Client or Data not found.")
            Console.WriteLine()
            Console.WriteLine("You may need to check your configuration '" + Process.GetCurrentProcess().ProcessName + ".exe.config'.")
        End If
    End Sub
    ''' <summary>
    '''  Maps User-Agent String to Device
    ''' </summary>
    ''' <param name="client">ClassifierClient</param>
    ''' <param name="text">User-Agent String</param>
    ''' <remarks>-</remarks>
    Private Sub Map(client As Classifier, text As String)
        Dim stopWatch As New Diagnostics.Stopwatch()
        stopWatch.Start()
        Dim ret As IDictionary(Of String, String) = client.Map(text)
        stopWatch.Stop()
        Dim deviceId As String = "unknown"
        If ret IsNot Nothing Then
            deviceId = ret("id")
        End If
        Console.WriteLine("Result: " & deviceId & " took " & stopWatch.Elapsed.TotalMilliseconds.ToString & " ms")
    End Sub
End Module