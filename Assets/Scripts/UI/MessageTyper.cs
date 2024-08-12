using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageTyper : MonoBehaviour
{
    [SerializeField] private float charPerSec = 2f;
    [SerializeField] private float startTypingAfterSec = 0f;

    TMP_Text text;
    string message;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();
        message = text.text;
        text.text = " ";
        StartCoroutine(typeOut());
    }

    IEnumerator typeOut()
    {
        yield return new WaitForSeconds(startTypingAfterSec);
        for (int i = 0; i < message.Length; i++)
        {
            text.text = text.text.Substring(0, text.text.Length - 1);
            string cursor = i % 2 == 0 ? "|" : " ";
            text.text += message[i] + cursor;
            yield return new WaitForSeconds(1f / charPerSec);
        }
        if (text.text.Length % 2 == 0)
        {
            text.text = text.text.Substring(0, text.text.Length - 1);
        }
    }

}
