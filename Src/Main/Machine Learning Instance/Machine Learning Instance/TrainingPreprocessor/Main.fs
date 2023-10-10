namespace Main

open System;
open System.IO;

module LoadImage =
    let currentTime: DateTime = DateTime.Now;

    let imageDirectory: string = "E:\\New folder\\TrainTest";
    let currentSession: string = currentTime.ToString("yyyyMMdd HHmm");

    for file in Directory.GetFiles(imageDirectory) do
        printfn "%s" file