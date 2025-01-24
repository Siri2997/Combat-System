using System;
using UnityEngine;

public class WeaponVisualController : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private Transform[] gunTransforms;
    [SerializeField] private Transform pistol;
    [SerializeField] private Transform revolver;
    [SerializeField] private Transform autoRifle;
    [SerializeField] private Transform shotGun;
    [SerializeField] private Transform Rifle;

    private Transform currentGun;

    private void Start()
    {
        anim = GetComponentInParent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchOn(pistol);
            SwitchAnimationLayer(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchOn(revolver);
            SwitchAnimationLayer(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchOn(autoRifle);
            SwitchAnimationLayer(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchOn(shotGun);
            SwitchAnimationLayer(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwitchOn(Rifle);
            SwitchAnimationLayer(3);
        }

    }

    private void SwitchOn(Transform gunTransform)
    {
        SwitchOffGuns();
        gunTransform.gameObject.SetActive(true);

        currentGun = gunTransform;

        AttachLeftHand();


    }

    private void AttachLeftHand()
    {
        //Transform targetTransform = currentGun.GetComponentInChildren<LeftHandTargeTransform>();
    }

    private void SwitchOffGuns()
    {
        for(int i = 0; i < gunTransforms.Length; i++)
        {
            gunTransforms[i].gameObject.SetActive(false);
        }
    }

    void SwitchAnimationLayer(int layerIndex)
    {
        for (int i = 0; i < anim.layerCount; i++)
        {
            anim.SetLayerWeight(i, 0);
        }

        anim.SetLayerWeight(layerIndex, 1);

    }


}
