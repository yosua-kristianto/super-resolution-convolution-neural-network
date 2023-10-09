namespace Model

open Tensorflow.Keras.Engine;
open type Tensorflow.Binding;

module ModelImporter = 
  
  let LoadModel: IModel = 
    let path = "../path-to-model";
    tf.keras.models.load_model(path);