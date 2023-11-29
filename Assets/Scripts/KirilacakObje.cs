using UnityEngine;

public class KirilacakObje : MonoBehaviour
{
    [SerializeField] GameObject _disableOnBroken;
    [SerializeField] GameObject _enableOnBroken;

    /// <summary> Toggle breaking elements. </summary>
    public void Broken()
    {
        _disableOnBroken.SetActive(false);
        _enableOnBroken.SetActive(true);
    }
}
