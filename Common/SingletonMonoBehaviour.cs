using System;
using UnityEngine;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static bool IsExist => instance != null;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                Type t = typeof(T);

                instance = (T)FindObjectOfType(t);
                if (instance == null)
                {
                    Debug.LogError(t + " をアタッチしているGameObjectはありません");
                }
            }

            return instance;
        }
    }

    virtual protected void Awake()
    {
        // 他のゲームオブジェクトにアタッチされているか調べる
        // アタッチされている場合は破棄する。
        CheckInstance();
    }
    protected bool CheckInstance()
    {
        //インスタンスがnullなら自身を登録
        if (instance == null)
        {
            instance = this as T;
            return true;
        }
        //インスタンスが自分ならtrue
        else if (Instance == this)
        {
            return true;
        }
        //上記のif文に引っかからなかったら自身を削除
        Destroy(this);
        return false;
    }
}
