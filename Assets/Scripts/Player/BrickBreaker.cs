using GamePatron.IndividualGames.Player;
using UnityEngine;

public class BrickBreaker : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

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
            playerController.InterruptJump();
            GameManagerScript.Instance.OnPointGained(GameManagerScript.Instance.Points.Brick);
            other.GetComponentInParent<KirilacakObje>().BrickBroken();
        }
    }
}
