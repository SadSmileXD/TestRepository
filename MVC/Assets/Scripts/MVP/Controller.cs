using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private View view;
    [SerializeField] private PlayerStatus Model;

    private void Awake()
    {
        Model.OnStatus += view.HpMpAmount;
    }
}

