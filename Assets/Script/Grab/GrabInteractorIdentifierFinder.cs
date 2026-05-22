using System;
using System.Reflection;
using UnityEngine;

public class GrabInteractorIdentifierFinder
{
    public struct PointerEventIdentifier
    {
        public string gameObjectName;
        public string componentName;
        public long identifier;
    }

    /// <summary>
    ///  オブジェクト名からすべてのMonoBehaviourを調べて Identifier プロパティを探す
    /// </summary>
    /// <param name="mbArray">走査対象のMonoBehaviour配列</param>
    /// <param name="objName">探すオブジェクトの名前</param>
    /// <returns></returns>
    public PointerEventIdentifier FindIdentifierFromObjName(MonoBehaviour[] mbArray, string objName)
    {
        PointerEventIdentifier result = new PointerEventIdentifier();

        foreach (var mb in mbArray)
        {
            Type t = mb.GetType();
            PropertyInfo prop = t.GetProperty("Identifier", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);// public/protected/private プロパティを含めて Identifier を探す

            if (prop == null || prop.PropertyType == null)
            {// プロパティが取得できない場合は次へ
                Debug.LogWarning($"IdentifierFinder: {t.Name} does not have an Identifier property", mb);
                continue;
            }

            try
            {
                var val = prop.GetValue(mb);
                if (val == null)
                {
                    Debug.LogWarning($"IdentifierFinder: Identifier property on {t.Name} is null", mb);
                    continue;
                }

                long id = Convert.ToInt64(val);
                Debug.Log($"[{mb.gameObject.name}] {t.Name}.Identifier = {id}");

                //結果の格納
                result.gameObjectName = mb.gameObject.name;
                result.componentName = t.FullName;
                result.identifier = id;

                if (result.gameObjectName == objName)
                {
                    //発見
                    Debug.Log($"MATCH: GameObject='{mb.gameObject.name}', Component='{t.FullName}', Identifier={id}", mb.gameObject);
                    return result;
                }

            }
            catch (Exception ex)
            {
                // 取得失敗は無視
                Debug.LogWarning($"IdentifierFinder: failed to read Identifier on {t.Name}: {ex.Message}");
                return default;
            }
        }

        //未発見時
        Debug.LogError($"IdentifierFinder: no interactor found for id={objName}");
        return result;
    }

    /// <summary>
    /// 指定したMonoBehaviourからIdentifierプロパティを探す
    /// </summary>
    /// <param name="mono"></param>
    /// <returns></returns>
    public PointerEventIdentifier FindId(MonoBehaviour mono)
    {
        PointerEventIdentifier result = new PointerEventIdentifier();

        Type t = mono.GetType();
        PropertyInfo prop = t.GetProperty("Identifier", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);// public/protected/private プロパティを含めて Identifier を探す

        if (prop == null || prop.PropertyType == null)
        {// プロパティが取得できない場合は次へ
            Debug.LogWarning($"IdentifierFinder: {t.Name} does not have an Identifier property", mono);
            return default;
        }
        try
        {
            var val = prop.GetValue(mono);
            if (val == null)
            {
                Debug.LogWarning($"IdentifierFinder: Identifier property on {t.Name} is null", mono);
                return default;
            }

            long id = Convert.ToInt64(val);
            Debug.Log($"[{mono.gameObject.name}] {t.Name}.Identifier = {id}");

            //結果の格納
            result.gameObjectName = mono.gameObject.name;
            result.componentName = t.FullName;
            result.identifier = id;

            Debug.Log($"Found Identifier: GameObject='{mono.gameObject.name}', Component='{t.FullName}', Identifier={id}", mono.gameObject);
        }
        catch (Exception ex)
        {
            // 取得失敗は無視
            Debug.LogWarning($"IdentifierFinder: failed to read Identifier on {t.Name}: {ex.Message}");
            return default;
        }

        //未発見時
        if (result.gameObjectName == null)
        {
            Debug.LogError($"IdentifierFinder: no interactor found for id={mono.gameObject.name}");
        }

        return result;
    }
}