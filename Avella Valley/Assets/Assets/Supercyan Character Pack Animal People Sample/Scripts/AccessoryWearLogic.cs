using UnityEngine;

namespace Supercyan.AnimalPeopleSample
{
    public class AccessoryWearLogic : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer m_characterRenderer;

        [SerializeField] private AccessoryLogic[] m_accessoriesToAttach = null;

        private Transform[] m_characterBones;

        private bool m_initialized = false;

        private void Initialize(GameObject character)
        {
            if (m_characterRenderer == null)
            {
                m_characterRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

                if (m_characterRenderer == null)
                {
                    Debug.LogWarning("Missing character components.");
                    return;
                }
            }
            if (m_characterRenderer.rootBone == null)
            {
                Debug.LogWarning("Missing character root bone.");
                return;
            }

            m_characterBones = m_characterRenderer.bones;
            m_initialized = true;
        }

        private void Awake()
        {
            Initialize(gameObject);
            foreach (AccessoryLogic a in m_accessoriesToAttach) { Attach(a); }
        }

        public void Attach(AccessoryLogic accessory)
        {
            if (!m_initialized)
            {
                Initialize(gameObject);
                if (!m_initialized)
                {
                    Debug.LogWarning("AccessoryWearLogic not initialized correctly, can't attach accessories.");
                    return;
                }
            }
            else if (accessory == null)
            {
                Debug.LogWarning("Trying to attach null accessory.");
                return;
            }
            else if (accessory.Renderer == null)
            {
                Debug.LogWarning("Trying to attach accessory with missing renderer.");
                return;
            }
            else if (accessory.Renderer.rootBone == null)
            {
                Debug.LogWarning("Trying to attach accessory with missing root bone.");
                return;
            }

            Transform[] newBones = GetBones(accessory.Renderer.bones, m_characterBones);
            if (newBones == null)
            {
                Debug.LogWarning("Trying to attach accessory with incompatible rig.");
                return;
            }

            accessory.Renderer.bones = newBones;
            accessory.Renderer.rootBone = m_characterRenderer.rootBone;
        }

        private Transform[] GetBones(Transform[] accessoryBones, Transform[] characterBones)
        {
            Transform[] newBones = new Transform[accessoryBones.Length];

            for (int i = 0; i < accessoryBones.Length; i++)
            {
                Transform bone = FindBone(m_characterRenderer.rootBone, accessoryBones[i].name);
                if (bone == null) { return null; }
                newBones[i] = bone;
            }

            return newBones;
        }

        private Transform FindBone(Transform rootBone, string name)
        {
            if (rootBone.name == name) { return rootBone; }
            else
            {
                Transform found = null;
                for (int i = 0; i < rootBone.childCount; i++)
                {
                    found = FindBone(rootBone.GetChild(i), name);
                    if (found != null) { return found; }
                }
                return null;
            }
        }
    }
}