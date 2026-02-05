using UnityEngine;

public class ExitApp : MonoBehaviour
{
    public void QuitApplication()
    {
        Debug.Log("애플리케이션 종료 요청됨");

        // 에디터에서는 종료되지 않으므로 조건 분기
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
