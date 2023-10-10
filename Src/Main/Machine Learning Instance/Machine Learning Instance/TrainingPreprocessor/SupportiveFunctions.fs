namespace TrainingPreprocessor

open Microsoft.FSharp.Core.Operators
open SixLabors.ImageSharp;
open SixLabors.ImageSharp.PixelFormats;
open SixLabors.ImageSharp.Processing;

module PythonLongerHand =
    
    (*
        ImageToArray

        As it name suggest, this function is a substitute of img_to_array keras utils in Python.
    *)
    let ImageToArray (imagePath: string): int[,] =
        use image = Image.Load<Rgba32>(imagePath);

        let width: int = image.Width;
        let height: int = image.Height;

        let imageArray = Array2D.zeroCreate<int> width height;

        for y in 0 .. height - 1 do
            for x in 0 .. width - 1 do
                let pixelValue = image.[x, y]
                let pixelAsInt = int pixelValue.A << 24 + int pixelValue.R << 16 + int pixelValue.G << 8 + int pixelValue.B

                imageArray.[y, x] <- image.[x, y];

        imageArray;

    (*
        StreamImageToArray

        As it name suggest, this function is a substitute of img_to_array keras utils in Python.
    *)
    let StreamImageToArray (image: Image<Rgba32>) =
        let width: int = image.Width;
        let height: int = image.Height;

        let imageArray = Array2D.create width height [||];

        for y in 0 .. height - 1 do
            for x in 0 .. width - 1 do
                let pixel = image.[x, y];
                imageArray.[y, x] <- [| float pixel.R; float pixel.G; float pixel.B |];

        imageArray;

    (*
        ArrayToImage

        As its name suggest, this function is a substitute of Image.fromArray since there are no PIL in F#.
    *)
    let ArrayToImage (imageArray: int[,]) =
        let height = imageArray.GetLength(0);
        let width = imageArray.GetLength(1);

        use image = new Image<Rgba32>(width, height);

        for y in 0 .. height - 1 do
            for x in 0 .. width - 1 do
                let value = imageArray.[y, x];
                let pixel = Rgba32(byte value, byte value, byte value);
                image.[x, y] <- pixel;
        
        image;

(*
### 1.1 Making support functions
This part is making support function to actually reducing the image quality for the testing data. 
*)
module SupportFunctions =

    (*
        ResizeImage

        As its name suggests, this function will resize the image by the specified factor. 

        Some algorithm change in this code compare to dzlab's one:

        1. Since I cannot use NumPy, and ImageSharp has their Height and Width separated as separated values, 
           I decided to seperate, and resizing it in directly in binary form. Which here using Mutate.

        2. Since I kind of enforcing data type, I standardize the factor to float32 and then convert the image 
           size into int as the result
        
        3. Also, I can't use img_to_array so I make them own instead.

        ```python
        def resize_image(image_array, factor):
            original_image = Image.fromarray(image_array)

            new_size = numpy.array(original_image.size) * factor
            new_size = new_size.astype(numpy.int32)
            new_size = tuple(new_size)

            resized = original_image.resize(new_size)
            resized = img_to_array(resized)
            resized = resized.astype(numpy.uint8)
    
            return resized
        ```

        This function will return the resized image's array2d
        @return array2d
    *)
    let ResizeImage (imageArray: int[,]) (factor: float32) =
        let originalImage: Image<Rgba32> = PythonLongerHand.ArrayToImage(imageArray);

        // Image size in ImageSharp is not in array form. Instead, they're one by one.
        let newHeight = int(float32(originalImage.Size.Height) * factor);
        let newWidth = int(float32(originalImage.Size.Width) * factor);

        // Resizing image using ImageSharp
        originalImage.Mutate(fun x -> 
            x.Resize(ResizeOptions(
                Size = Size(newWidth, newHeight),
                Mode = ResizeMode.Stretch,
                Sampler = KnownResamplers.Bicubic
            )) |> ignore
        )
        
        PythonLongerHand.StreamImageToArray originalImage;

    (*
        DownUpSizeImage

        As its name suggest, this function will down scale the image by 1 / scale first, 
        and then up scaling it by scale. 

        I also believe that this action will able to destroy image quality in first place.

        Original function from dzlab:

        ```python
        def downsize_upsize_image(image, scale):
        scaled = resize_image(image, 1.0 / scale)
        scaled = resize_image(scaled, scale) # In the reference, the scale is divided by 1.0. What changes over it?

        return scaled
        ```

        This function will return the resized image's array2d
        @return array2d
    *)
    let DownUpSizeImage (imageArray: int[,]) (scale: float32) =
        // Downsize
        let mutable  scaledImage = ResizeImage imageArray (1.0f / scale);
        
        // Upsize
        scaledImage <- ResizeImage imageArray scale;
        
        scaledImage;

    (*
        @onhold
        This function is deleting the whole training purpose of an object

        TightCropImage

        This function will crop the image by taking its array. I still don't understand why this code is needed

        Original function from dzlab:
        
        ```python
        def tight_crop_image(image, scale):
            height, width = image.shape[:2]

            width -= int(width % scale)
            height -= int(height % scale)

            return image[:height, :width]
        ```

        This function will return the cropped image's array2d
        @return array2d
    *)
    let TightCropImage (imageArray: int[,]) (scale: float32) =
        let height, width = imageArray.GetLength(0), imageArray.GetLength(1);

        let newWidth = width - int(float32(width) * scale);
        let newHeight = height- int(float32(height) * scale);

        let croppedImageArray = Array2D.zeroCreate<int> newHeight newWidth;

        for y in 0 .. newHeight - 1 do
            for x in 0 .. newWidth - 1 do
                croppedImageArray.[y, x] <- imageArray.[y, x];

        croppedImageArray;

    (*
        CropInput

        This function will slice through the input image to the destinated dimension.

        The original code from dzlab:
        ```python
        def crop_input(image, x, y):
            x_slice = slice(x, x + INPUT_DIM)
            y_slice = slice(y, y + INPUT_DIM)
            return image[y_slice, x_slice]
        ```

        This function will return the cropped image's array2d
        @return array2d
    *)
    let CropInput (image: int[,]) (x: int) (y: int) =
        let newWidth = TrainingPreprocessor.HyperParameters.INPUT_DIM;
        let newHeight = TrainingPreprocessor.HyperParameters.INPUT_DIM;
        let croppedImage = Array2D.zeroCreate<int> newHeight newWidth;
    
        for j in 0 .. newHeight - 1 do
            for i in 0 .. newWidth - 1 do
                croppedImage.[j, i] <- image.[y + j, x + i];
    
        croppedImage;

    (*
        CropOutput

        This function will slice through the target image to the destinated dimension.

        The original code from dzlab:
        ```python
        def crop_output(image, x, y):
            x_slice = slice(x + PAD, x + PAD + LABEL_SIZE)
            y_slice = slice(y + PAD, y + PAD + LABEL_SIZE)
    
            return image[y_slice, x_slice]
        ```

        This function will return the cropped image's array2d
        @return array2d
    *)
    let CropOutput (image: int[,]) (x: int) (y: int) =
        let newWidth = TrainingPreprocessor.HyperParameters.LABEL_SIZE;
        let newHeight = TrainingPreprocessor.HyperParameters.LABEL_SIZE;
        let croppedImage = Array2D.zeroCreate<int> newHeight newWidth;
    
        for j in 0 .. newHeight - 1 do
            for i in 0 .. newWidth - 1 do
                croppedImage.[j, i] <- image.[y + j + TrainingPreprocessor.HyperParameters.PAD, x + i + TrainingPreprocessor.HyperParameters.PAD];
    
        croppedImage;
