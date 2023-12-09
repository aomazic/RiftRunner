using UnityEngine;
using UnityEngine.UI;

public class DasCdBar : MonoBehaviour
{

    [SerializeField] Dashing ds;
    private Slider slider;

    // Start is called before the first frame update
    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = ds.remaningDashes + ds.dashCdTimer / ds.dashCd;
    }
}
