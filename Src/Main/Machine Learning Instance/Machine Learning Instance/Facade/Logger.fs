namespace Facade

open System.IO;
open System;
open System.Configuration;

module Log =

    (*
        WriteLog

        This function will write a log with this format below:
        [2023-10-01T00:00][INFO] Some message
        
        The generated log file name is current time session with format of [ml-instance-f#-yyyyMMdd]
    *)
    let WriteLog (logMessage: string) (logCategory: string) = 
        let currentTime: DateTime = DateTime.Now;
        let currentSession: string = currentTime.ToString("yyyyMMdd");
        
        let logPath: string = "D://storage/logs/" + "ml-instance-f#-" + currentSession + ".log";

        let currentSessionFull = currentTime.ToString("yyyy-MM-dd HH:mm:ss");
        let log: string = "[" + currentSessionFull + "]" + "[" + logCategory + "]" + logMessage + "\n";

        if File.Exists(logPath) then
            File.AppendAllText(logPath, log);
        else
            File.WriteAllText(logPath, log);

    (*
        I

        This function will help write a log with category of INFO
    *)
    let I (log: string) = 
        WriteLog log "INFO";

    (*
        E

        This function will help write a log with category of ERROR
    *)
    let E (log: string) = 
        WriteLog log "ERROR";

    (*
        V

        This function will help write a log with category of VERBOSE
    *)
    let V (log: string) = 
        WriteLog log "VERBOSE";

    (*
        D

        This function will help write a log with category of DEBUG
    *)
    let D (log: string) = 
        WriteLog log "DEBUG";

        

