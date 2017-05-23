using System.Collections;
using UnityEngine;

public class RandomColor : MonoBehaviour
{
    private Material mat;
    public float waitTime;

	private void Awake ()
	{
	    mat = GetComponent<Renderer>().material;
	    StartCoroutine(ColorSwap());
	}

    private IEnumerator ColorSwap()
    {
        while (true)
        {
            var r = Random.Range(-.1f, .1f);
            var g = Random.Range(-.1f, .1f);
            var b = Random.Range(-.1f, .1f);

            var newColor = new Color((mat.color.r + r) % 1, (mat.color.g + g) % 1, (mat.color.b + b) % 1);
            mat.color = newColor;
            yield return new WaitForSeconds(waitTime);
        }
    }
}
