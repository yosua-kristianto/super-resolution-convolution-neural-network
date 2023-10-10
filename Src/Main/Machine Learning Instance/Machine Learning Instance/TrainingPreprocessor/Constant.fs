namespace Constant

(*
Here we define some parameters:

| Parameter Name | Description |
|---|---|
| `SCALE` | Resizing scale factor |
| `INPUT_DIM` | Input and Output patch sizes |
| `PAD` | Padding that need to be added to output patches |
| `STRIDE` | Larger STRIDE will result in higher pixel skipping. Which will reduce more image quality. On the dzlab's github, `STRIDE` explained as *"the stride which is the number of pixels we'll slide both in the horizontal and vertical axes to extract patches"* |
*)
module HyperParameters =
    let SCALE: float32 = 2.0f;
    let INPUT_DIM: int = 33;
    let LABEL_SIZE: int = 21;
    let PAD: int = int(float32(INPUT_DIM - LABEL_SIZE) / 2.0f);
    let STRIDE: int = 14;