using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class QuitGame : MonoBehaviour
{
	void Start () {
		Button btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}
    void TaskOnClick()
    {
        if (Application.isEditor)
        {
            EditorApplication.ExitPlaymode();
        }
        else
        {
            Application.Quit();
        }
    }
}
