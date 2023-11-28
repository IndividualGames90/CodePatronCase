using System.Collections;
using UnityEngine;

namespace GamePatron.IndividualGames.Player
{
    public class Jump : MonoBehaviour
    {
        private void Awake()
        {
            StartCoroutine(asd());
        }

        private IEnumerator asd()
        {
            for (; ; )
            {
                Debug.Log("asd");
                yield return new WaitForEndOfFrame();
            }
        }
    }
}