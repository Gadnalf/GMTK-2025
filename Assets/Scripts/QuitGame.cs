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
            // commented this out since it won't build with this line
            // Uncomment it if you are testing this in editor
            //EditorApplication.ExitPlaymode();
        }
        else
        {
            Application.Quit();
        }
    }
}
