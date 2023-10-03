open System;
open Tensorflow;
open Tensorflow.NumPy;
open Tensorflow.Keras.Engine;
open Tensorflow.Keras.Layers;


let CreateModel (height: int) (width: int) (depth: int): Tensorflow.Keras.Engine.IModel =
    
    //Tensorflow.Keras.Layers
    let inputShape: Tensorflow.Shape = new Tensorflow.Shape([| height; width; depth |]);
    let layerArguments: Tensorflow.Keras.ArgsDefinition.LayerArgs = new Tensorflow.Keras.ArgsDefinition.LayerArgs();
    layerArguments.InputShape <- inputShape;

    let inputLayer = Tensorflow.Keras.Engine
    
    let model: Tensorflow.Keras.Engine.IModel = Tensorflow.Keras.Engine.IModel()



    model;