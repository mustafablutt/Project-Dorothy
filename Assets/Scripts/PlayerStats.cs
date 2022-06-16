using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;
    public PlayFabManager playfab;
    public HealthBar healthbar;
    [SerializeField] private AudioSource hurtSoundEffect;
    public int a;
    public GameOverScreen GameOverScreen;
    void Start()
    {
        currentHealth = maxHealth;
        healthbar.setMaxHealth(maxHealth);
        //FindObjectOfType<HealthBar>().setMaxHealth(maxHealth);
    }
    void Update()
    {      
        if(currentHealth <= 0)
        {
            die();          
        }
    }
    void die()
    {       
        Debug.Log("NO PROBLEMO2222");
        FindObjectOfType<PlayFabManager>().SendLeaderBord(FindObjectOfType<Score>().lateScore);
        GameOverScreen.Setup(FindObjectOfType<Score>().lateScore);
        SceneManager.LoadScene("GameOver");
    }
    public void Damage(int dmg)
    {
        currentHealth -= dmg;
        healthbar.setHealth(currentHealth);
        hurtSoundEffect.Play();
    }
    public void Heal(int heal)
    {
        currentHealth += heal;
        healthbar.setHealth(currentHealth);
    }
}

