namespace TSCP.Server

open System
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.DependencyInjection
open Microsoft.AspNetCore.Http
open TSCP.Core
open TSCP.Core.Domain

module Program =

    [<EntryPoint>]
    let main args =
        let builder = WebApplication.CreateBuilder(args)
        
        // Add services to the container.
        // On active CORS pour permettre à une future UI Web (JS/React) d'interroger le serveur
        builder.Services.AddCors() |> ignore

        let app = builder.Build()

        // Configure the HTTP request pipeline.
        app.UseCors(fun b -> b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader() |> ignore) |> ignore

        // --- 1. STATUS ROUTE ---
        app.MapGet("/", Func<string>(fun () -> 
            "TSCP Server v5.2 | Status: NOMINAL | Vector: |S>"
        )) |> ignore

        // --- 2. CATALOG ROUTE ---
        // Retourne la liste brute de tous les concepts (M1, M2, M3)
        app.MapGet("/api/concepts", Func<IResult>(fun () ->
            let concepts = Catalog.loadAll()
            Results.Ok(concepts)
        )) |> ignore

        // --- 3. SEMANTIC PROFILE ROUTE (NOUVEAU) ---
        // Retourne les voisins sémantiques (Archétypes et Pairs)
        // Exemple: /api/profile/MY_BANK
        app.MapGet("/api/profile/{id}", Func<string, IResult>(fun id ->
            let concepts = Catalog.loadAll()
            
            match concepts |> List.tryFind (fun c -> c.Id = id) with
            | Some target ->
                // Utilisation de la logique du moteur pour trouver les voisins
                let neighbors = Engine.Physics.findNearestNeighbors target concepts 20
                
                // Projection en objet simple pour le JSON de réponse
                let response = 
                    neighbors 
                    |> List.map (fun (c, sim) -> 
                        {| 
                           Id = c.Id
                           Name = c.Name
                           Similarity = Math.Round(sim * 100.0, 1) // Arrondi pour lecture facile
                           // Distinction Nature (M2) vs Voisin (M1)
                           Type = if c.Id.StartsWith("M3_") || c.Id.StartsWith("M2_") || c.Tags |> List.contains "Ontology" then "Archetype" else "Peer"
                        |})
                
                Results.Ok(response)
            | None -> Results.NotFound(sprintf "Concept '%s' not found." id)
        )) |> ignore

        // --- 4. SIMULATION ROUTE (BONUS) ---
        // Permet de calculer une interaction via HTTP
        // Exemple: /api/simulate/M2_AGENT/M2_SYSTEM
        app.MapGet("/api/simulate/{sourceId}/{targetId}", Func<string, string, IResult>(fun sourceId targetId ->
            let concepts = Catalog.loadAll()
            let sourceOpt = concepts |> List.tryFind (fun c -> c.Id = sourceId)
            let targetOpt = concepts |> List.tryFind (fun c -> c.Id = targetId)

            match sourceOpt, targetOpt with
            | Some s, Some t ->
                let result = Engine.Physics.computeInteraction s t
                Results.Ok(result)
            | _ -> Results.BadRequest("Source ID or Target ID not found.")
        )) |> ignore

        // Démarrage du serveur
        app.Run()
        0 // Exit code