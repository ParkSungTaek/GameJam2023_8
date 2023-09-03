using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RecordDataHandler 
{
    public List<RecordData> recorddatas = new List<RecordData>();

    [Serializable]
    public class RecordData
    {
        public string name;
        public bool startA;
        public int[] StartSet = new int[4];

        public List<RecordDataPacket> SaveDataPackets = new List<RecordDataPacket>();

    }
    [Serializable]
    public class RecordDataPacket
    {
        public float Time;
        public int Protocol;
        public int Input;
    }

}
