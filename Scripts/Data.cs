using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Data
{
    public float[] playerposition;
    public float[] chunckposition;

    public Data (Player player)
    {
        playerposition = new float[3];
        playerposition[0] = player.transform.position.x;
        playerposition[1] = player.transform.position.y;
        playerposition[2] = player.transform.position.z;
    }
    public Data (ChunkLoader chunck)
    {
        chunckposition = new float[3];
        chunckposition[0] = chunck.transform.position.x;
        chunckposition[1] = chunck.transform.position.y;
        chunckposition[2] = chunck.transform.position.z;
    }
}