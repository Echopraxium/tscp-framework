// uuid: c7b6a5f4-e3d2-4109-b018-d1c2b3a4f5e6
namespace TSCP.Server

open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open Giraffe
open TSCP.Core
open TSCP.Session

module ApiHandlers =
    let getSessionHandler : HttpHandler =
        fun (next : HttpFunc) (ctx : Microsoft.AspNetCore.Http.HttpContext) ->
            json (SessionManager.loadSession()) next ctx

    let commandHandler (cmd: string) : HttpHandler =
        fun (next : HttpFunc) (ctx : Microsoft.AspNetCore.Http.HttpContext) ->
            let currentState = SessionManager.loadSession()
            let newState, _ = Engine.executeCommand cmd currentState
            SessionManager.saveSession newState
            json newState next ctx

module App =
    let webApp =
        choose [
            GET  >=> route "/api/session" >=> ApiHandlers.getSessionHandler
            POST >=> routef "/api/execute/%s" ApiHandlers.commandHandler
            setStatusCode 404 >=> text "Not Found"
        ]

    [<EntryPoint>]
    let main args =
        let builder = WebApplication.CreateBuilder(args)
        builder.Services.AddGiraffe() |> ignore
        let app = builder.Build()
        app.UseGiraffe(webApp)
        app.Run()
        0