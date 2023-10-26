using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class BuildDebugger : MonoBehaviour
{
    [SerializeField] private int _maxLines = 8;

    [SerializeField] private TMP_Text _text;

    private Queue<string> queue = new Queue<string>();
    private string currentText = "";

    void OnEnable() {
        DontDestroyOnLoad(gameObject);

        Application.logMessageReceivedThreaded += HandleLog;
    }
    void OnDisable() {
        Application.logMessageReceivedThreaded -= HandleLog;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.KeypadPlus)) {
            Debug.LogError("test error");
        }
    }

    void HandleLog(string logString, string stackTrace, LogType type) {
        // Delete oldest message
        if (queue.Count >= _maxLines) queue.Dequeue();
        queue.Enqueue(logString);
        var builder = new StringBuilder();
        foreach (string st in queue) {
            builder.Append(st).Append("\n");
        }
        currentText = builder.ToString();

        _text.text = currentText;
    }
}
