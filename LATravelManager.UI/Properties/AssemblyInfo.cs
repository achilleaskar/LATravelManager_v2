using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Windows;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("LATravelManager.UI")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Achilleas Karagiannis")]
[assembly: AssemblyProduct("LATravelManager.UI")]
[assembly: AssemblyCopyright("Copyright ©  2019")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

//In order to begin building localizable applications, set
//<UICulture>CultureYouAreCodingWith</UICulture> in your .csproj file
//inside a <PropertyGroup>.  For example, if you are using US english
//in your source files, set the <UICulture> to en-US.  Then uncomment
//the NeutralResourceLanguage attribute below.  Update the "en-US" in
//the line below to match the UICulture setting in the project file.

//[assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.Satellite)]

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
                                     //(used if a resource is not found in the page,
                                     // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
                                              //(used if a resource is not found in the page,
                                              // app, or any theme specific resource dictionaries)
)]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion("0.1.0")]

[assembly: NeutralResourcesLanguage("el-GR")]
[assembly: AssemblyVersion("0.10.85")]
[assembly: AssemblyFileVersion("0.10.85")]
/*
    cd "LATravelManager.UI\bin\Release"
    nuget pack .\LaTravelManager.nuspec
    squirrel --releasify .\LATravelManager.UI\bin\Release\LATravelManager.0.2.3.nupkg --no-msi
  Severity	Code	Description	Project	File	Line	Suppression State
Error		The command "nuget pack MyApp.nuspec -Version 0.10.0.0 -Properties Configuration=Release -OutputDirectory bin\Release\ -BasePath bin\Release\" exited with code 1.	LATravelManager.UI	C:\Users\achil\source\repos\LATravelManager_v2\LATravelManager.UI\LATravelManager.UI.csproj	920

*/
