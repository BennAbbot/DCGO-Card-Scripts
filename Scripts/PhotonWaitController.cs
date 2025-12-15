using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PhotonWaitController 
{
    public void SetWaiting(string key, bool isGo, bool isAdd)
    {
        
       
    }
    public IEnumerator Wait(string key)
    {
        yield break;
        //StartCoroutine(SetWaitingText("Syncing"));

        //Debug.Log($"Start Waiting:{key}");

        //while (isWaiting(key, PhotonNetwork.LocalPlayer))
        //{
        //    if (PhotonNetwork.IsMasterClient)
        //    {
        //        if (AllIsWaiting(key))
        //        {
        //            yield return new WaitForSeconds(Time.deltaTime);

        //            SetRoomTrueKey(key);
        //        }
        //    }

        //    if (RoomHasTrueKey(key))
        //    {
        //        SetWaiting(key, true, false);
        //        break;
        //    }

        //    yield return null;
        //}

        //yield return null;

        //OffSyncText();

        //Debug.Log($"End Waiting:{key}");
    }

    public Coroutine StartWait(string key)
    {
        return null;
    }
}
