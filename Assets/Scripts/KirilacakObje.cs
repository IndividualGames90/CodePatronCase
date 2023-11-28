using UnityEngine;

public class KirilacakObje : MonoBehaviour
{
    [SerializeField] GameObject _disableOnBroken;
    [SerializeField] GameObject _enableOnBroken;

    public void Broken()
    {
        _disableOnBroken.SetActive(false);
        _enableOnBroken.SetActive(true);
    }
}
