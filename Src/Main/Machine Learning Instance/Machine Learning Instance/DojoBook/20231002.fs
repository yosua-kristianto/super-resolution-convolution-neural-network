module DojoBook_20231002

open Tensorflow;
open Tensorflow.Keras;
open Tensorflow.Keras.Engine;
open Tensorflow.Checkpoint;

open type Tensorflow.Binding;

// Model Creation Phase


(*
  THIS MODEL SETUP HAS BEEN TESTED AND TRAINED
  TEST: 20231001
  TRAINED: 20231002
*)
// The working chain from 20231002 
let CreateModel (height: int) (width: int) (depth: int): IModel =

    // Setting Input Layer
    let input = tf.keras.layers.Input(new Shape([| height; width; depth |]));

    // Setting Hidden Layer 1
    let x_l1 = tf.keras.layers.Conv2D(64, kernel_size=new Shape([| 9; 9 |]), kernel_initializer="he_normal", activation="relu");
    let x_l2 = tf.keras.layers.Conv2D(32, kernel_size=new Shape([| 1; 1 |]), kernel_initializer="he_normal", activation="relu");

    // Output Layer
    let output = tf.keras.layers.Conv2D(depth, kernel_size=new Shape([| 5; 5 |]), kernel_initializer="he_normal");

    // Chaining Tensors
    let mutable x: Tensors = x_l1.Apply(input); // Apply input to x_l1
    x <- x_l2.Apply(x); // Apply x to x_l2
    x <- output.Apply(x);

    tf.keras.Model(input, x);

// Optimizer Part

// Pre-training configuration
let epoch: float32 = 12f;
let inputDimension: int = 33;

let defaultLearningRate: float32 = float32(0.001f);
let optimizer = tf.keras.optimizers.AdamW(learning_rate=(defaultLearningRate), weight_decay=(defaultLearningRate / epoch));

let model: IModel = CreateModel inputDimension inputDimension 3;
model.compile(loss = tf.keras.losses.MeanSquaredError(), optimizer = optimizer);


// Model Summary
model.summary();

// Plot the model & Checkpoint (May not available)
