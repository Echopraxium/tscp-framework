// uuid: e1f2-g3h4-cli-program-v3
namespace TSCP.CLI

open System
open TSCP.Session

module Program =

    [<EntryPoint>]
    let main argv =
        Console.Clear()
        printfn "=== TSCP Framework (Phase 2 - .NET 10.0) ==="
        printfn "Type 'help' for a list of commands or 'exit' to quit."

        let mutable session = SessionManager.loadSession()
        let mutable running = true
        
        while running do
            printf "\nTSCP> "
            let input = Console.ReadLine()
            
            if String.IsNullOrWhiteSpace(input) then
                ()
            elif input.ToLower().Trim() = "exit" then
                running <- false
                printfn "Saving session and shutting down..."
                SessionManager.saveSession session
            else
                try
                    // Sync with CommandHandler.executeCommand
                    session <- CommandHandler.executeCommand input session
                with
                | ex -> printfn "Error: %s" ex.Message

        0