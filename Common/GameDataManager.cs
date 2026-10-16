using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Scripts.Enum.SoundType;

public class GameDataManager : MonoBehaviour
{
    [SerializeField] private SoundDatasInfo _soundDatasInfo;
    //[SerializeField] private FruitDatasInfo _fruitDatasInfo;
    private static List<SoundData> _soundDatas;
    //private static List<FruitData> _fruitDatas;

    protected void Awake()
    {
        _soundDatas = _soundDatasInfo.SoundDatas;
        //_fruitDatas = _fruitDatasInfo.FruitDatas;
    }

    public static AudioClip GetSoundData(BGMType soundType)
    {
        var data = _soundDatas.FirstOrDefault(x => x.BGMType == soundType);
        if (data == null)
        {
            Debug.LogError($"データがありません。Key => {soundType}");
        }
        return data.AudioClip;
    }

    //public static FruitData GetFruitData(FruitType fruitType)
    //{
    //    var data = _fruitDatas.FirstOrDefault(x => x.FruitType == fruitType);
    //    if (data == null)
    //    {
    //        Debug.LogError($"データがありません。Key => {fruitType}");
    //    }
    //    return data;
    //}

    public static int GetEnumLength<T>() => Enum.GetValues(typeof(T)).Length;

    public static T GetRandomEnumType<T>() => (T)Enum.ToObject(typeof(T), UnityEngine.Random.Range(0, GetEnumLength<T>()));
}
