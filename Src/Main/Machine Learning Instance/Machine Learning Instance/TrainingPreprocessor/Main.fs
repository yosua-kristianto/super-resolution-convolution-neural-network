namespace TrainingPreprocessor

open System;
open System.IO;
open TrainingPreprocessor.PythonLongerHand;
open TrainingPreprocessor.SupportFunctions;
open TrainingPreprocessor.HyperParameters;

module LoadImage =
    let Main = 
        let currentTime: DateTime = DateTime.Now;

        let imageDirectory: string = "D:\\storage\\training-dataset";
        let currentSession: string = currentTime.ToString("yyyyMMdd HHmm");

        // This flag used for making a sequence for the processed image file 
        let mutable i: int = 0;

        for file in Directory.GetFiles(imageDirectory) do
            printfn "%s" file;

            // Limit process for 1 image only for debugging
            if i <> 0 then
                Console.WriteLine("Processing 1 data complete");
                ()

            // Prepare folder for the destinated file
            let folderName: string = currentSession + "_" + string(i);
            //Console.WriteLine(folderName);

            printfn "Proposed folder name is: %s" folderName;

            let imageArray = ImageToArray file;
            let scaledImage = DownUpSizeImage imageArray SCALE;

            i <- i+1;

            
            // Get file full path
            //let fileFullPath: string = imageDirectory + file;

