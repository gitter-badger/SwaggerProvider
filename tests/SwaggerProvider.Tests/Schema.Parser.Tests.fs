﻿module SwaggerProvider.Tests

open SwaggerProvider.Internal.Schema
open SwaggerProvider.Internal.Schema.Parsers
open FSharp.Data
open NUnit.Framework
open FsUnit
open System.IO

[<Test>]
let ``Schema parse of PetStore.Swagger.json sample`` () =
    let schema =
        "Schemas/PetStore.Swagger.json"
        |> File.ReadAllText
        |> JsonValue.Parse
        |> JsonParser.parseSwaggerObject
    schema.Definitions |> should haveLength 6

    schema.Info
    |> should equal {
        Title = "Swagger Petstore"
        Version = "1.0.0"
        Description = "This is a sample server Petstore server.  You can find out more about Swagger at [http://swagger.io](http://swagger.io) or on [irc.freenode.net, #swagger](http://swagger.io/irc/).  For this sample, you can use the api key `special-key` to test the authorization filters."
    }


// Test that provider can parse real-word Swagger 2.0 schemas
// https://github.com/APIs-guru/api-models/blob/master/API.md
type ApisGuru = FSharp.Data.JsonProvider<"http://apis-guru.github.io/api-models/api/v1/list.json">

let ApisGuruSchemaUrls =
    ApisGuru.GetSample().JsonValue.Properties()
    |> Array.choose (fun (name, obj)->
        obj.TryGetProperty("versions")
        |> Option.bind (fun v->
            v.Properties()
            |> Array.choose (fun (_,x)-> x.TryGetProperty("swaggerUrl"))
            |> Some
        )
       )
    |> Array.concat
    |> Array.map (fun x->x.AsString())

let ManualSchemaUrls =
    [|"http://netflix.github.io/genie/docs/rest/swagger.json"
      //"https://www.expedia.com/static/mobile/swaggerui/swagger.json" // This schema is incorrect
      "https://graphhopper.com/api/1/vrp/swagger.json"|]

let SchemaUrls =
    Array.concat [ManualSchemaUrls; ApisGuruSchemaUrls]

[<Test; TestCaseSource("SchemaUrls")>]
let ``Schema Parse`` url =
    let json =
        try
            Http.RequestString url
        with
        | :? System.Net.WebException ->
            printfn "Schema is unaccessible %s" url
            ""
    if not <| System.String.IsNullOrEmpty(json) then
        let schema = json |> JsonValue.Parse |> Parsers.JsonParser.parseSwaggerObject
        schema.Paths.Length + schema.Definitions.Length |> should be (greaterThan 0)