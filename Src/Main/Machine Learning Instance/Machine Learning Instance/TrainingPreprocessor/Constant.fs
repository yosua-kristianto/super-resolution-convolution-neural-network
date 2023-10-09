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

module SCALE =
  let value: float32 = 2.0f;

module INPUT_DIM =
  let value: int = 33;

module LABEL_SIZE =
  let value: int = 21;

module PAD =
  let value: int = int(float32(INPUT_DIM.value - LABEL_SIZE.value) / 2.0f);

module STRIDE =
  let value: int = 14;