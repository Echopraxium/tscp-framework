// uuid: f1e2d3c4-b5a6-4789-8b9c-0d1e2f3a4b5c
namespace TSCP.Tests.Session

open Xunit
open TSCP.Core
open TSCP.Session

module StateTests =
    [<Fact>]
    let ``Initial state should start at layer zero on the continuum`` () =
        Assert.Equal(0.0, SessionManager.initialStore.ActiveLayer)

    [<Fact>]
    let ``Session should increment layer via Engine sync`` () =
        let state = SessionManager.initialStore
        let newState, _ = Engine.executeCommand "sync" state
        Assert.True(newState.ActiveLayer > state.ActiveLayer)

    [<Fact>]
    let ``ResetSession should return the continuum to zero`` () =
        let resetState = SessionManager.resetSession ()
        Assert.Equal(0.0, resetState.ActiveLayer)