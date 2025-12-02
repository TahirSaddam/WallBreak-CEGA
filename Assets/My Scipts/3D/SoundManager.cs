using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip bgm, gameplay, gameover, uiButton;

    [Header("Bricks Hit")]
    public AudioClip[] brickHit;

    public static SoundManager instance;
    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        PlayBGM();
    }

    public void PlayBGM()
    {
        audioSource.clip = bgm;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayGameplayMusic()
    {
        audioSource.clip = gameplay;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayGameOverMusic()
    {
        audioSource.clip = gameover;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void BrickHit()
    {
        int randomIndex = Random.Range(0, brickHit.Length);
        audioSource.PlayOneShot(brickHit[randomIndex]);
    }

    public void UIButtonClick()
    {
        audioSource.PlayOneShot(uiButton);
    }
}
