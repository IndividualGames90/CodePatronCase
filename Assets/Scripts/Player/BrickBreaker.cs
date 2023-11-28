using UnityEngine;

public class BrickBreaker : MonoBehaviour
{
    private readonly string _brickTag = "KirilacakObje";

    /*
     * CEM: Neden oncollisionenterla yapmadim, bir scriptin ontriggerenter yapmasi
     * bir suru kirilacakobjenin oncollisionenter yapmasindan daha performant.
     * oncollisionenterda fizik checkleri var, ontriggerenterda yok.
     */
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_brickTag))
        {
            other.GetComponentInParent<KirilacakObje>().BrickBroken();
        }
    }
}
