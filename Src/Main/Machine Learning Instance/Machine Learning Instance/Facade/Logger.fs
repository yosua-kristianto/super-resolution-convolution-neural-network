namespace Facade

open System.Configuration;

module Log =

    (*
        WriteLog
    *)
    let WriteLog (logMessage: string) (logCategory: string) = 
        printfn "%s" logMessage

