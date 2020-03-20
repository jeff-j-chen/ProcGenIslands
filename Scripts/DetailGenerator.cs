using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DetailGenerator : MonoBehaviour {
    
    public int[,] GenerateCoral(int chunkSize, float[,] hueMap) {
        int[,] intMap = new int[chunkSize, chunkSize];
        // generate chunks of coral (squares? circles?) with one color, all across the ocean where the huemap makes it green
        // run completely random filters over the chunks, more filters where there is less green, several times, generating blocks of coral with the same color and increasing density towards the center
        // use the int[,] in chunkgenerator to get the color from the array and apply it to that point
        // apply noise filter to the coral as well?
        return intMap;
    }

}