using UnityEngine;

public class GameWin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.Player))
        {
            GameManagerScript.Instance.OnVictory();
        }
    }
}
