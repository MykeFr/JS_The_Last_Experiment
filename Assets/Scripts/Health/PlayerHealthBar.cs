using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealthBar : Health
{
    [SerializeField] private StopAnimations stopAnimations;
    [SerializeField] private PhysicsGun physicsGun;
    [SerializeField] private GrapplingHook grapplingHook;
    [SerializeField] private MouseLook mouseLook;
    [SerializeField] private GameObject player;
    public Slider slider;
    [SerializeField] private PauseMenu pauseMenu;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        slider.value = health / maxHealth;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        slider.value = health / maxHealth;
    }

    public override void Die(RaycastHit hit){
        //show screen
        stopAnimations.stop = true;
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<PlayerShooter>().enabled = false;
        physicsGun.enabled = false;
        grapplingHook.enabled = false;
        mouseLook.enabled = false;
        this.enabled = false;
        pauseMenu.GameOver();
        //StatesManager.Instance.GameOver(this.gameObject);
    }

    public override bool TakeDamage(float damage, RaycastHit hit){
        return base.TakeDamage(damage, hit);
    }

    public override void RestoreHealth(){
        base.RestoreHealth();
    }
}
