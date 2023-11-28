using UnityEngine;

namespace GamePatron.IndividualGames.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Points", menuName = "IndividualGames/Points")]
    public class Points : ScriptableObject
    {
        public int Brick = 10;
    }
}