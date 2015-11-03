namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("HDAS-ui")>]
[<assembly: AssemblyProductAttribute("HDAS-ui")>]
[<assembly: AssemblyDescriptionAttribute("HDAS-ui")>]
[<assembly: AssemblyVersionAttribute("1.0")>]
[<assembly: AssemblyFileVersionAttribute("1.0")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "1.0"
