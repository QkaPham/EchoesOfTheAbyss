using UnityEngine;
using UnityEngine.Pool;

public class FragmentPool : MonoBehaviour
{
    [SerializeField] 
    private GameObject fragmentPrefab;
    public ObjectPool<Fragment> pool;
    void Awake()
    {
        pool = new ObjectPool<Fragment>(CreateFragment, OnGetFragment, OnReleaseFragment);
    }

    private Fragment CreateFragment()
    {
        var fragment = Instantiate(fragmentPrefab, this.transform).GetComponent<Fragment>();
        fragment.SetPool(pool);
        return fragment;
    }

    private void OnGetFragment(Fragment fragment)
    {
        fragment.gameObject.SetActive(true);
    }

    private void OnReleaseFragment(Fragment fragment)
    {
        fragment.gameObject.SetActive(false);
    }

}
