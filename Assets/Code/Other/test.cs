using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;

public class test : MonoBehaviour
{
    [SerializeField] private SphericalHarmonicsL2[] _type;
    [SerializeField] private SphericalHarmonicsL2[] _type2;
    [SerializeField] private LightProbeGroup _g;
    
    
    [Button]
    public void Get()
    {
        SphericalHarmonicsL2[] s = LightmapSettings.lightProbes.bakedProbes;
        _type = new SphericalHarmonicsL2[147];
        for (int i = 0; i < 147; i++)
        {
            _type[i] = s[i];
        }
    }

    [Button]
    public void Set()
    {
        SphericalHarmonicsL2[] s = LightmapSettings.lightProbes.bakedProbes;
        for (int i = 0; i < 147; i++)
        {
            s[i] = _type[i];
        }
        LightmapSettings.lightProbes.bakedProbes = s;
    }
}
