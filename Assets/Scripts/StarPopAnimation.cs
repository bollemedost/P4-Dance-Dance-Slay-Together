using UnityEngine;

public class StarPopAnimation : MonoBehaviour
{
    public float popScale = 2f;
    public float popDuration = 0.4f;
    public ParticleSystem sparkleParticles;

    private Vector3 originalScale;
    private bool hasPopped = false;

    void Awake()
    {
        originalScale = transform.localScale;
    }

    public void PlayPop()
    {
        if (!hasPopped)
        {
            hasPopped = true;

            // Play particles
            if (sparkleParticles != null)
            {
                sparkleParticles.Play();
            }

            StartCoroutine(AnimatePop());
        }
    }

    private System.Collections.IEnumerator AnimatePop()
    {
        Vector3 targetScale = originalScale * popScale;
        float time = 0f;

        while (time < popDuration)
        {
            transform.localScale = Vector3.Lerp(originalScale, targetScale, time / popDuration);
            time += Time.deltaTime;
            yield return null;
        }

        time = 0f;
        while (time < popDuration)
        {
            transform.localScale = Vector3.Lerp(targetScale, originalScale, time / popDuration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale;
    }
}
