﻿open Microsoft.Owin.Hosting
open Owin
open System
open System.Web.Http
open Swashbuckle.Application


let getAppBuilder() =
    let config = new HttpConfiguration()
    // Configure routes
    config.Routes
        .MapHttpRoute("default", "{controller}") |> ignore
    // Enable Swagger and Swagger UI
    config
        .EnableSwagger(fun c -> c.SingleApiVersion("v1", "Test Controllers for SwaggerProvider") |> ignore)
        .EnableSwaggerUi();

    fun (appBuilder:IAppBuilder) ->
        appBuilder.UseWebApi(config) |> ignore


[<EntryPoint>]
let main argv =
    try
        let hostAddress = "http://localhost:8735"
        use server = WebApp.Start(hostAddress, getAppBuilder())

        let swaggerUiUrl = sprintf "%s/swagger/ui/index" hostAddress
        printfn "Web server up and running on %s\n" hostAddress
        printfn "Swagger UI is running on %s" swaggerUiUrl
        printfn "Swagger Json Schema is available on %s/swagger/docs/v1" hostAddress

        printf  "\nPress Enter to open Swagger UI"
        Console.ReadLine() |> ignore
        System.Diagnostics.Process.Start(swaggerUiUrl) |> ignore

        printf  "\nPress Enter key to stop"
        Console.ReadLine() |> ignore
    with
    | e ->
        printfn "Exception %A" e
        raise e
    0 // return an integer exit code
