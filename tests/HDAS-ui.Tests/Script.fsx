#I "bin/Release"
#r "bin/Release/nunit.framework.dll"
#r "bin/Release/Suave.dll"
#r "bin/Release/Suave.Testing.dll"
#r "bin/Release/HDAS.exe"
#r "bin/Release/Fuchu.dll"
#r "bin/Release/Unquote.dll"

open Suave
open Suave.Http
open Suave.Http.Applicatives
open Suave.Http.Successful
open Suave.Web
open Suave.Types
open NUnit.Framework
open Suave.Testing
open Swensen.Unquote

//Set pwd to the directory containing this script
System.Environment.CurrentDirectory <- __SOURCE_DIRECTORY__

let hdasApp =
  choose [
    GET >>= path "/" >>= OK "Home"
    ]


let defaultConfig = { defaultConfig with
                                    bindings = [ HttpBinding.mk' HTTP "0.0.0.0" 8083 ]
                                    homeFolder = Some (__SOURCE_DIRECTORY__ + "/web")
                      }

let startServer () =
  runWith defaultConfig hdasApp


[<Test>]
let ``Should do home`` () =
  let home =
    startServer ()
    |> req HttpMethod.GET "/" None 
  test <@ home = "KB - Home" @>

``Should do home`` () 
