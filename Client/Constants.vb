Public Class Constants
    Public Const APP_NAME As String = "ClassifierClient"
    ' data files
    Public Const BUILDER_DATA As String = "BuilderDataSource.xml"
    Public Const BUILDER_DATA_PATCH As String = "BuilderDataSourcePatch.xml"
    Public Const DEVICE_DATA As String = "DeviceDataSource.xml"
    Public Const DEVICE_DATA_PATCH As String = "DeviceDataSourcePatch.xml"

    Public Const DEVICE_UA As String = "mobi.openddr.classifier.client"

    Public Const DEVICE_TOSTRING_FORMAT As String = "Id='{0}', ParentId='{1}', Type='{2}', Pattern={3}, Attributes={4}"
    Public Const HTTP_PREFIX As String = "http"
    Public Const RELEASE_VERSION As String = "1.0.4-SNAPSHOT" ' TODO could we crop this for the assembly info?
    Public Const SIMPLE As String = "simple"
    Public Const USER_AGENT_SPLIT As String = " |-|_|/|\\|\[|\]|\(|\)|;"
    Public Const VERSION_FORMAT As String = "Version : {0}, Build : {1}"

    Public Const CONFIG_ERROR_FORMAT As String = "Error in configuration file '{0}' : {1}."
    Public Const CONFIG_ERROR_CONN_FORMAT As String = "Error in configuration file '{0}' : ConnectionString entry for OpenDDR is missing."
    Public Const FILE_ERROR_FORMAT As String = "File '{0}' not found."
    Public Const WEB_ERROR_FORMAT As String = "Web exception : {0}."
End Class