using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour  // Bu sınıfa başka bir sınıftan statik olarak ulaşmak için
{
    public static T Instance { get; private set; } = null;

    public virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
        }
        else
        {
            Debug.LogError("UYARI! Birden fazla " + typeof(T) + "Sahnede. Ekstralar sonlandırılacak.");
            Destroy(gameObject);
        }

    }
}