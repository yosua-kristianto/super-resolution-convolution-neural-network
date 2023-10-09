namespace SupportiveFunctions

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

module ResizeImage (imageArray, factor) = 

module ConvertImageToArray (imageArray, factor) = 

module DownsizeUpsizeImage (imageArray, factor) = 

module TightCropImage (imageArray, factor) = 

module CropInput (imageArray, factor) = 

module CropOutput (imageArray, factor) = 
  