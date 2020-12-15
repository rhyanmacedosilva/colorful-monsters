using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Astronaut : MonoBehaviour
{
    public Transform FirePoint = null;
    public LineRenderer LaserLine = null;
    public float TimeToHideLaserLine = 0;
    public LaserBubble laserBubble = null;
    public Animator cameraAnimator = null;
    public SpriteRenderer[] laserLights = { };

    private Animator animator = null;
    private AudioSource audioSource = null;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) == true)
		{
            animator.SetTrigger("Shoot");
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
	{
        RaycastHit2D hitInfo = Physics2D.Raycast(FirePoint.position, FirePoint.right);
        Alien alien = null;

        // Pega cor do LaserBubble e coloca no LaserLine
        Color laserColor = laserBubble.GetComponent<SpriteRenderer>().color;
        int laserColorIndex = laserBubble.GetColorIndex();

        LaserLine.startColor = laserColor;
        LaserLine.endColor = laserColor;
        LaserLine.enabled = true;

        // Muda cor do das luzes do laser antes e dispara ShootAnimation e Sound
        for (int i = 0; i < laserLights.Length; i++)
        {
            laserLights[i].color = laserColor;
        }

        audioSource.Play();
        cameraAnimator.SetTrigger("ShootShake");
        LaserLine.SetPosition(0, FirePoint.position);

        if (hitInfo == true)
		{
            alien = hitInfo.transform.GetComponent<Alien>();
            if (alien != null)
			{
                LaserLine.SetPosition(1, hitInfo.point);
            }
		}
		else
		{
            LaserLine.SetPosition(1, new Vector3(FirePoint.position.x + 444, FirePoint.position.y, FirePoint.position.z));
        }

		yield return new WaitForSeconds(TimeToHideLaserLine);
        LaserLine.enabled = false;

        // Pega a cor do Alien e compara para decidir o que vai acontecer, faz aqui para destruir o alien ao mesmo tempo em que o laser some
        if (alien != null)
		{
            int alienColorIndex = alien.GetColorIndex();

            if (alienColorIndex == laserColorIndex)
            {
                alien.Damage(1);
            }
            else
            {
                alien.Heal(1);
            }
        }
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
        // Se colidir com um alien, vai para a tela de Game Over
        Alien tmpAlien = collision.transform.GetComponent<Alien>();

        if (tmpAlien != null)
		{
            SceneManager.LoadScene("GameOver");
		}
	}
}
