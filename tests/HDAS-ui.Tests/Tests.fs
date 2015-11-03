module HDAS.Tests

open HDAS
open NUnit.Framework
open Suave
open Suave.Http.Successful
open Suave.Web
open Suave.Http
open Suave.Types
open Suave.Testing
open Suave.Http.Applicatives
open NUnit.Framework
open Swensen.Unquote
open HDAS.Program

[<Test>]
let ``hello returns 42`` () =
  let result = Library.hello 42
  printfn "%i" result
  Assert.AreEqual(42,result)

let startServer () =
    runWith defaultConfig hdasApp


[<Test>]
let ``Home page exists`` () =
  let home =
    startServer ()
    |> req HttpMethod.GET "/" None
  test <@ home = "Home" @>

[<Test>]
let ``Evidence search returns results`` () =
  let evidence =
    startServer ()
    |> req HttpMethod.GET "/evidencesearch/cancer" None
  test <@ evidence.ToLower().Contains("<b>cancer</b>") @>
