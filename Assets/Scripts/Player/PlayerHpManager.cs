using System;
using UnityEngine;
using UnityEngine.Events;

public enum HealthOperation { Inc, Dec }

public class PlayerHpManager : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    private int currentHealth = 3;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private AudioClip deathClip;

    [Header("Sound Variation")]
    [SerializeField, Range(0.1f, 3f)] private float minPitch = 0.9f;
    [SerializeField, Range(0.1f, 3f)] private float maxPitch = 1.1f;

    public UnityEvent OnHealthChange;
    public UnityEvent OnDeath;

    private AudioSource audioSource;

    void Start()
    {
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public void IncrementMaxHealth()
    {
        if (maxHealth == 7) return;

        maxHealth++;
        currentHealth++;

        OnHealthChange.Invoke();
    }

    public void UpdateCurrentHealth(HealthOperation type)
    {
        int previousHealth = currentHealth;

        switch (type)
        {
            case HealthOperation.Inc:
                currentHealth = Math.Min(currentHealth + 1, maxHealth);
                break;
            case HealthOperation.Dec:
                currentHealth = Math.Max(currentHealth - 1, 0);
                break;
        }

        if (previousHealth != currentHealth)
        {
            OnHealthChange.Invoke();
            audioSource.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
            audioSource.PlayOneShot(hitClip);
        }

        if (currentHealth == 0)
        {
            audioSource.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
            audioSource.PlayOneShot(deathClip);
            OnDeath.Invoke();
        }
    }
}
