using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CopyButtons : MonoBehaviour
{
    public Text CopyText;

#if UNITY_EDITOR
    public void Copy()
    {
        GUIUtility.systemCopyBuffer = CopyText.text;
    }
#endif
}
