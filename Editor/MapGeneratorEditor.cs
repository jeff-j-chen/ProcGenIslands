using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(ChunkGenerator))]
// specify to unity that we are creating a custom editor for mapgenerator classes
public class ChunkGeneratorEditor : Editor {
    // inherit from editor
    public override void OnInspectorGUI() {
        // override method for when the inspector's gui is opened
        ChunkGenerator mapGen = (ChunkGenerator)target;
        // create mapgen, target is the object that is being inspected and we want to cast it to be a mapgenerator
        if (DrawDefaultInspector()) {
            // draw the default inspector (if any value was changed)
            if (mapGen.autoUpdate) {
                mapGen.GenerateChunkAt(mapGen.center, true);
                // mapGen.ChunkTest(mapGen.center);
            }
        }
        if (GUILayout.Button("Generate")) {
            // if player clicked the "generate" button
            mapGen.GenerateChunkAt(mapGen.center,true);
            // generate the map
            // mapGen.ChunkTest(mapGen.center);
        }
    }
}
