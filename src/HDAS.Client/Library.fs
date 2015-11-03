namespace HDAS.Client

module public Search = 
  open System.Runtime.CompilerServices
  open HDAS.Client.Types
  open FSharp.Data
  open HDAS.Client.Authentication
  open HDAS.Client.JsonConverter
  open System.Web.Helpers
  
  let hello num = 42

  type proquestProvider = JsonProvider<"../../json/proquest2.json">
  let proquestSearch (term:string) = proquestProvider.GetSample()
  let result = box proquestSearch 


  type Person =
    {
      FirstName : string;
      LastName : string
    }

  let jon = {FirstName = "Jon"; LastName = "Russell"}

  type testDynamic() = 
      [<Dynamic([|true|])>]
        member this.DynamicObject : obj = box jon

  type internalDynamic(x) = 
      [<Dynamic([|true|])>]
        member this.DynamicObject  : obj = box x


//  let GetProquestSearchTyped term = box jon
//
//  let GetProquestSearchDynamic term = newDynamic({jon with FirstName = term})


//  let dynamic = GetProquestSearchDynamic "dsds"

//  let intDynamic = internalDynamic(dynamic)


  let testData = """{"FirstName":"John", "LastName":"Doe"}"""
  let testData2 = """{"employees":[{"firstName":"John", "lastName":"Doe"},{"firstName":"Anna", "lastName":"Smith"}]}"""

  //turn json string into f# dynamic type !!!!!!

  type newDynamic(x) = 
      [<Dynamic([|true|])>]
        member this.DynamicObject  : obj = box x

  //let actual = JsonConverter.unjson<Person> testData
 //let actual = JsonConverter.unjson<internalDynamic> testData

  //let dataObject = JsonConverter.unjson<obj> testData 
  
  //let dataObject2 = dataObject |> box

  //let dataObject = Json.Decode testData
  
  //let testDataObject = newDynamic testData

  //using the System.Web.Helpers.Json Json.Decode here to return a DynamicJsonObject
  //it sort of works, but really awkward to use, especially when you have nested structures

  let testDataObject = Json.Decode testData
  //let testDataObject2 = newDynamic testDataObject
  //let data = testDataObject2.DynamicObject

  let x = testDataObject

  