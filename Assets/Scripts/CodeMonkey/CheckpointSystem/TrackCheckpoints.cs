using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCheckpoints : MonoBehaviour
{
    private List<CheckpointSingle> checkpointSingleList;
    private List<int> nextCheckpointSingleIndexList;
    [SerializeField] private List<Transform> carTransformList;
    private void Awake()
    {
        Transform checkpointTransform = transform.Find("Checkpoints");
        checkpointSingleList = new List<CheckpointSingle>();
        foreach (Transform checkpointSingleTransform in checkpointTransform)
        {
            CheckpointSingle checkpointSingle = checkpointSingleTransform.GetComponent<CheckpointSingle>();
            checkpointSingle.SetTrackCheckpoints(this);
            checkpointSingleList.Add(checkpointSingle);
        }
        nextCheckpointSingleIndexList = new List<int>();
        foreach (Transform carTransform in carTransformList)
        {
            nextCheckpointSingleIndexList.Add(0); //when we start game, bot need go to checkpoint with index 0
        }
    }

    public void BotThroughCheckpoint(CheckpointSingle checkpointSingle, Transform carTransform) //checkpointSingle = checkpoint for current bot, carTransform = transform current bot
    {
        int nextCheckpointSingleIndex = nextCheckpointSingleIndexList[carTransformList.IndexOf(carTransform)]; //find transform current bot car
        if (checkpointSingleList.IndexOf(checkpointSingle) == nextCheckpointSingleIndex) //if bot hits in need checkpoint, then:
        {
            nextCheckpointSingleIndexList[carTransformList.IndexOf(carTransform)] = (nextCheckpointSingleIndex + 1) % checkpointSingleList.Count; //прибавляем до тех пор, пока не достигнем последнего индекса, после чего идём заново (когда прошли первый круг - отправляемся на второй)
        }
    }
}
