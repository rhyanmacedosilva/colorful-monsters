using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBubble : MonoBehaviour
{
    public Color[] Colors = { };
    public float TimeToChangeColor = 0;

    private int colorIndex = 0;
    private int correctColorIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Muda de cor de tempos em tempos
        StartCoroutine(ChangeColor());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ChangeColor()
	{
        while (true)
		{
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            Color tmpColor = Colors[colorIndex];
            spriteRenderer.color = new Color(tmpColor.r, tmpColor.g, tmpColor.b, 1);
            correctColorIndex = colorIndex;

            colorIndex++;

            if (colorIndex >= Colors.Length)
            {
                colorIndex = 0;
            }

            yield return new WaitForSeconds(TimeToChangeColor);
        }
    }

    public int GetColorIndex()
	{
        return correctColorIndex;
	}
}
