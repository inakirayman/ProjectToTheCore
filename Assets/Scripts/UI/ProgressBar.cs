using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class ProgressBar : MonoBehaviour
{

#if UNITY_EDITOR
    //For ease of use
    [MenuItem("GameObject/UI/Linear Progress Bar")]
    public static void AddLinearProgressBar()
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>("UI/Linear Progress Bar"));
        obj.transform.SetParent(Selection.activeGameObject.transform, false);

    }
    [MenuItem("GameObject/UI/Radial Progress Bar")]
    public static void AddRadialProgressBar()
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>("UI/Radial Progress Bar"));
        obj.transform.SetParent(Selection.activeGameObject.transform, false);
    }
#endif

    public float Maximum;
    [SerializeField]
    private float _current;
   
    public float Current
    {
        get { return _current; }
        set
        {
            if (value < 0)
            {
                _current = 0;
            }
            else
            {
                _current = value;
            }
            SetCurrentFill();
        }
    }

    public Image _mask;

    private void SetCurrentFill()
    {
        float fillAmount = _current / Maximum;
        _mask.fillAmount = fillAmount;
    }


}
