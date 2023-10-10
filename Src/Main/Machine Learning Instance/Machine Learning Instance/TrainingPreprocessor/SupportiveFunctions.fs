namespace TrainingPreprocessor

open SixLabors.ImageSharp;
open SixLabors.ImageSharp.PixelFormats;
open SixLabors.ImageSharp.Processing;

module PythonLongerHand =
    
    (*
        ImageToArray

        As it name suggest, this function is a substitute of img_to_array keras utils in Python.
    *)
    let ImageToArray (imagePath: string) =
        use image = Image.Load<Rgba32>(imagePath);

        let width: int = image.Width;
        let height: int = image.Height;

        let imageArray = Array2D.create width height [||];

        for y in 0 .. height - 1 do
            for x in 0 .. width - 1 do
                let pixel = image.[x, y];
                imageArray.[y, x] <- [| float pixel.R; float pixel.G; float pixel.B |];

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

List functions to be made:

1. `resize_image` 
As its name suggests, this function will resize the image by the specified factor. 

If you want to downsample the image, you can set the factor by 1 / x or x / 100.
If you want to upsample the image, you can set the factor by x

2. convert_image_to_array
As its name suggests, this function will convert a raw image data to a numpy array. You may notice that this function is only contains 1 line, but trust me, as a skilled-issue user (me), you may want to do this for better understanding of the function.

3. downsize_upsize_image
This function will downsample, and then upsample the image. It says, if this happen, for some reason the image will be start degraded on its quality.

4. tight_crop_image
This function will 

5. crop_input
This function will slice through the input image to the destinated dimension.

6. crop_output
This function will slice through the target image to the destinated dimension.
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



//module CropInput (imageArray, factor) = 

//module CropOutput (imageArray, factor) = 
