using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(MapGenerator))]
// specify to unity that we are creating a custom editor for mapgenerator classes
public class MapGeneratorEditor : Editor {
    // inherit from editor
    public override void OnInspectorGUI() {
        // override method for when the inspector's gui is opened
        MapGenerator mapGen = (MapGenerator)target;
        // create mapgen, target is the object that is being inspected and we want to cast it to be a mapgenerator
        if (DrawDefaultInspector()) {
            // draw the default inspector (if any value was changed)
            if (mapGen.autoUpdate) {
                // if we want the map to automatically update
                mapGen.GenerateMap();
                // do so
            }
        }
        if (GUILayout.Button("Generate")) {
            // if player clicked the "generate" button
            mapGen.GenerateMap();
            // generate the map
        }
    }
}
