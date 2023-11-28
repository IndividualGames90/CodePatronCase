using UnityEngine;

public class KirilacakObje : MonoBehaviour
{
    [SerializeField] GameObject _disableOnBroken;
    [SerializeField] GameObject _enableOnBroken;

    public void BrickBroken()
    {
        _disableOnBroken.SetActive(false);
        _enableOnBroken.SetActive(true);
    }
}
