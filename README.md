# openddr-vb
OpenDDR VB.NET Client

CLS (ISO/IEC 23271:2012) compliant .NET version of OpenDDR Classifier

[![Build status](https://ci.appveyor.com/api/projects/status/2gruv2nrmyvrge4r?svg=true)](https://ci.appveyor.com/project/keilw/openddr-vb)

This client is used to classify browser User-Agent strings.

The client requires openddr-data. Data can be loaded via:

 * URL
 * Filesystem

# Code

//get client using configured data source 
//see "Configuration" below for further details on how to configure the data source
Classifier cl = new Classifier();

String userAgent = "Mozilla/5.0 (Linux; U; Android 2.2; en; HTC Aria A6380 Build/ERE27) AppleWebKit/540.13+ (KHTML, like Gecko) Version/3.1 Mobile Safari/524.15.0";

//classify the userAgent
IDictionary<string, string> devices = cl.Map(userAgent);

//iterate thru the devices (here we already got a Dictionary of Device model objects)
For Each device As Device In devices.Values()
{
    Console.WriteLine(device);
}
	
## Compile
OpenDDR VB.NET Classifier Client and Console have been tested with .NET Framework 4.0 or higher.

To compile the sources you may use

- Visual Studio (ClassifierClient.sln file works with VS 2010 or above, Community Edition is sufficient)
- Mono Project (http://www.mono-project.com/) and MonoDevelop (http://www.monodevelop.com/)

We currently don't provide a MonoDevelop solution, but the project layout should be compatible with it. And you're more than welcome to contribute solution files if you use MonDevelop (please create an issue under https://github.com/OpenDDRmobi/openddr-vb/issues and attach your files or propose a Pull Request)

# Run

## Configuration
ClassifierConsole comes with a config file ClassifierConsole.exe.config, the runtime version of App.config in the Visual Studio project.
It contains the connection string for the OpenDDR data source: "http://openddr.mobi/data/"
This connection URL points to the latest available OpenDDR Data version online. 
If you prefer a different version or a local copy of the OpenDDR data repository, you may override ClassifierConsole by pointing to a different source. 
Either a URL or the location of a folder in the file system.
This is currently also backward-compatible with Apache DeviceMap and its URLs.

The config file also contains the supported .NET runtime version.
<supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.0"/>
You should normally not have to change this. Doing so is at your own risk. OpenDDR VB.NET client does not support .NET below 4.0, but in case you have multiple versions of the .NET Framework installed, you may chose a particular one, e.g. if you face problems running ClassifierConsole.