// uuid: f1e2d3c4-b5a6-7890-1234-567890abcdef
module TSCP.Server.App

open System
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Hosting
open Microsoft.AspNetCore.Http
open TSCP.Core

[<EntryPoint>]
let main args =
    let builder = WebApplication.CreateBuilder(args)
    let app = builder.Build()

    // 1. Root / Health Check
    app.MapGet("/", Func<string>(fun () -> 
        "TSCP Systemic Server Online")) |> ignore

    // 2. Catalog listing
    app.MapGet("/catalog", Func<IResult>(fun () ->
        let items = 
            Catalog.loadFromDisk() 
            |> List.mapi (fun i (m, _) -> {| Index = i + 1; Id = m.Id; Description = m.Description |})
        Results.Ok(items))) |> ignore

    // 3. Systemic Analysis
    app.MapGet("/analyse/{id}", Func<string, IResult>(fun (id: string) ->
        let items = Catalog.loadFromDisk()
        let target = items |> List.tryFind (fun (m, _) -> m.Id.Equals(id, StringComparison.OrdinalIgnoreCase))
        
        match target with
        | Some (model, _) ->
            let state = { ActiveLayer = 0.0; History = [] }
            let stateLoaded = Engine.loadModel model state
            let _, msg = Engine.executeCommand "analyse" stateLoaded
            match msg with
            | Some m -> Results.Ok({| Model = model.Id; Analysis = m |})
            | None -> Results.BadRequest("Analysis failed.")
        | None -> Results.NotFound({| Error = sprintf "Model '%s' not found." id |})
    )) |> ignore

    app.Run()
    0