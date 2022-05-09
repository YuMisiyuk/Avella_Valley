using UnityEngine;

namespace Supercyan.AnimalPeopleSample
{
    public class AccessoryLogic : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer m_renderer = null;
        public SkinnedMeshRenderer Renderer { get { return m_renderer; } }

        [SerializeField] private GameObject m_rig = null;

        private void Awake() { Destroy(m_rig); }
    }
}