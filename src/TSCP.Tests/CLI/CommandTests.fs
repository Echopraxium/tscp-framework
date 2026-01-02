// uuid: e8d9c0b1-a2b3-4c4d-9e0f-1a2b3c4d5e6f
namespace TSCP.Tests.CLI

open Xunit
open TSCP.Core
open TSCP.Session
open TSCP.CLI

module CommandTests =

    [<Theory>]
    [<InlineData("catalog")>]
    [<InlineData("load")>]
    [<InlineData("list")>]
    [<InlineData("analyse")>]
    [<InlineData("compare")>]
    [<InlineData("graph")>]
    [<InlineData("export")>]
    [<InlineData("layer++")>]
    [<InlineData("reset")>]
    [<InlineData("pwd")>]
    [<InlineData("help")>]
    let ``CommandHandler must support mandated command`` (commandName: string) =
        let state = SessionManager.initialStore
        try
            let result = CommandHandler.executeCommand commandName state
            Assert.NotNull(result)
        with
        | ex -> Assert.Fail(sprintf "Command '%s' integrity failure: %s" commandName ex.Message)