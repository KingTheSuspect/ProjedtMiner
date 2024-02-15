using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectionManager : MonoBehaviour
{
    public TextMeshPro BaseText;
    public int Money = 0;
    void Start()
    {
        BaseText = GetComponentInChildren<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        BaseText.transform.LookAt(Camera.main.transform);
        BaseText.transform.Rotate(0, 180, 0);
    }
}
