using UnityEngine;
using UnityEngine.UI;

public class View : MonoBehaviour
{
    [SerializeField] private Image HP;
    [SerializeField] private Image MP;


    public void HpMpAmount(float hp,float mp)
    {
        HP.fillAmount = hp;
        MP.fillAmount = mp;
    }

   
}
