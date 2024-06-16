using UnityEngine;

namespace HexaFortress.Game
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Game Property")]
    public class GeneralConfig : ScriptableObject
    {
        public GameObject BonusPar;
        [Header("Tile Icons")] public Sprite TowerIcon;
        public Sprite MortarIcon;
        public Sprite CannonIcon;

        /// <summary>
        /// Play bonus partical when a tile give bonus to other one.
        /// </summary>
        /// <param name="pos"></param>
        public void PlayBonusPartical(Vector3 pos)
        {
            pos += Vector3.up * 0.2f;
            string objID = "BonusPar";
            var par = ObjectPoolingManager.Instance.SpawnObject(objID, BonusPar, pos, Quaternion.identity);
            par.GetComponent<ParticleCallBack>().OnStop = delegate
            {
                ObjectPoolingManager.Instance.ReturnObject(objID, par);
            };
        }
    }
}