using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    public float Speed = 0;
    public Vector3 RespawnPosition = Vector3.zero;
    public float XPositionToTriggerRespawn = 0;
    public GameObject[] spritesToChangeColor = { };
    public GameObject[] lifeIcons = { };
    public AudioClip AudioHeal = null;
    public AudioClip AudioDamage = null;
    public GameObject explosion = null;

    private Color[] colors = { };
    private int health = 1;
    private int maxHealth = 3;
    private int colorIndex = 0;
    private AudioSource audioSource;
    private bool dead = false; // Garante que parem de ocorrer os casos onde o Alien some mas fica aparecendo o primeiro LifeIcon

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Respawn(true);
    }

    // Update is called once per frame
    void Update()
    {
        // Caminhar
        transform.Translate(Vector3.left * Speed * Time.deltaTime);

        // Respawn
        if (transform.position.x <= XPositionToTriggerRespawn)
		{
            Respawn(false);
        }
    }

    void ChangeColor()
	{
        // Recupera cores para a troca
        LaserBubble laserBubble = FindObjectOfType<LaserBubble>();
        colors = laserBubble.Colors;

        // Troca a cor das sprites do corpo do alien
        colorIndex = Random.Range(0, colors.Length);

        for (int i = 0; i < spritesToChangeColor.Length; i++)
        {
            SpriteRenderer spriteRenderer = spritesToChangeColor[i].GetComponent<SpriteRenderer>();
            Color tmpColor = colors[colorIndex];
            spriteRenderer.color = new Color(tmpColor.r, tmpColor.g, tmpColor.b, 1);
        }
    }

    public void Heal(int qtd)
    {
        if (health < maxHealth)
		{
            audioSource.clip = AudioHeal;
            audioSource.Play();
            health++;
            UpdateLifeIcons();
        }
    }

    public void Damage(int qtd)
    {
        audioSource.clip = AudioDamage;
        audioSource.Play();
        health--;
        UpdateLifeIcons();

        if (health == 0)
		{
            Die();
		}
    }

    public int GetHealth()
	{
        return health;
	}

    void Die()
	{
        // Destroy(gameObject);
        Color explosionColor = new Color(colors[colorIndex].r, colors[colorIndex].g, colors[colorIndex].b, 10);
        ParticleSystem.MainModule explosionSettings = explosion.GetComponent<ParticleSystem>().main;
        explosionSettings.startColor = new ParticleSystem.MinMaxGradient(explosionColor);
        // Instantiate(explosion, transform.position, transform.rotation);
        ToggleAlienVisibility(false);
        dead = true;
    }

    void UpdateLifeIcons()
	{
        if (dead == true)
		{
            return;
		}

		for (int i = 0; i < lifeIcons.Length; i++)
		{
            if (i < health)
			{
                lifeIcons[i].SetActive(true);
            } else
			{
                lifeIcons[i].SetActive(false);
            }
		}
	}

    void Respawn(bool first)
	{
        if (first == false)
		{
            transform.position = RespawnPosition;
        }
        
        ChangeColor();
        UpdateLifeIcons();
        ToggleAlienVisibility(true);
        dead = false;
        health = 1;
        UpdateLifeIcons();
    }

    public int GetColorIndex()
    {
        return colorIndex;
    }

    void ToggleAlienVisibility(bool enabled)
	{
        SpriteRenderer[] sprites = transform.GetComponentsInChildren<SpriteRenderer>();
        GetComponent<Collider2D>().enabled = enabled;

        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].enabled = enabled;
        }
    }
}
