using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DummyScript : NetworkBehaviour
{
    public int MaxHP;


    [SerializeField] public int CurrentHP;

    public Material FlashMaterial;

    private SkinnedMeshRenderer Render;
    private Material OwnMaterial;
    private void Start()
    {
        CurrentHP = MaxHP;

        Render = GetComponentInChildren<SkinnedMeshRenderer>();
        OwnMaterial = Render.material;

    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Damage"))
        {
            OnHitFeedback();
            CurrentHP -= collision.gameObject.GetComponent<BulletBehaviour>().BulletDamage.Value;
            CheckLife();
        }
        if (collision.gameObject.CompareTag("Explosion"))
        {
            OnHitFeedback();
            CurrentHP--;
            CheckLife();
        }
    }

    public void OnHitFeedback()
    {
        SizeChange();
        StartCoroutine(Flash());
    }

    public void SizeChange()
    {
        transform.DOPunchScale(transform.localScale * Random.Range(1, 1.5f), 0.5f);
    }

    public IEnumerator Flash()
    {
        Render.material = FlashMaterial;
        yield return new WaitForSeconds(0.5f);
        Render.material = OwnMaterial;
    }

    void CheckLife()
    {
        if (CurrentHP <= 0)
        {
            Destroy(gameObject);
            
        }
    }
}
