using UnityEngine;

namespace Runtime.Data.Original
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObject/PlayerData", order = 0)]
    public class PlayerData : ScriptableObject
    {
        public Quarter quarter;
        public Vector3 position;
    }
}