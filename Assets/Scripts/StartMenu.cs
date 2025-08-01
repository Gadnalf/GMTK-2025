using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    public string gameplayScene;

	void Start () {
		Button btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}
    void TaskOnClick()
    {
        SceneManager.LoadScene(gameplayScene);
    }
}
