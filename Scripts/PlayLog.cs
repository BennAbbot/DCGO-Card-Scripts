using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayLog
{
    #region Events
    public static Action<string> OnAddLog;
    public static Action<string> OnLinkPressed;
    #endregion

    public string GetLogString()
    {
        string logString = "";

        foreach (string log in _logList)
        {
            logString += log;
        }

        return logString;
    }

    List<string> _logList = new List<string>();

    //16250, 13000
    int _maxLogCharacterLength = 11000;


    bool _first = false;

    public PlayLog()
    {
        _logList = new List<string>();

        OnAddLog = null;
        OnLinkPressed = null;

        OnAddLog += AddLogString;
        OnLinkPressed += ShowCard;
    }

    public void AddLogString(string logText)
    {
        AddLogStringCoroutine(DataBase.ReplaceToASCII(logText));
    }

    void AddLogStringCoroutine(string log)
    {
        Debug.Log($"{log}");

        _logList.Add(AddLink(log));

        while (GetLogString().Length >= _maxLogCharacterLength)
        {
            if (_logList.Count >= 1)
            {
                _logList.RemoveAt(0);
            }
        }
    }

    string AddLink(string log)
    {
        List<int> startIndex = AllIndexesOf(log, "(");
        List<int> endIndex = AllIndexesOf(log, ")");
        List<string> subStrings = new List<string>();

        for(int i = 0; i < startIndex.Count; i++)
        {
            if (startIndex[i] < 0 && endIndex[i] < 0)
                continue;

            startIndex[i] += 1;

            string str = log.Substring(startIndex[i], endIndex[i] - startIndex[i]);

            if(!subStrings.Contains(str) && !String.IsNullOrEmpty(str))
                subStrings.Add(str);
        }

        foreach(string str in subStrings)
            log = log.Replace(str, $"<link={str}><color=#92F6FF><u>{str}</u></color></link>");

        return log;
    }

    void ShowCard(string cardID)
    {
        CardSource founcdCardSource = GManager.instance.turnStateMachine.gameContext.ActiveCardList
        .Find(cardSource1 => cardSource1.CardID == cardID);

        if (founcdCardSource != null)
        {
            //GManager.instance.cardDetail.OpenCardDetail(founcdCardSource, true);
        }
    }

    //Might need to move this more relavent
    List<int> AllIndexesOf(string str, string value)
    {
        if (String.IsNullOrEmpty(value))
            throw new ArgumentException("the string to find may not be empty", "value");
        List<int> indexes = new List<int>();
        for (int index = 0; ; index += value.Length)
        {
            index = str.IndexOf(value, index);
            if (index == -1)
                return indexes;
            indexes.Add(index);
        }
    }
}
