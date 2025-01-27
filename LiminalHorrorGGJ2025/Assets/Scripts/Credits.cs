using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Credits : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI message;
    [SerializeField] private GameObject[] credits;
    [SerializeField] private float messageTime = 5.0f;
    private int creditsRevealed = 1;

    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(ShowMessage());
        SC_FPSController.Instance.enabled = false;
        AudioManager.Instance.PostEvent("Stop_Walk");
    }

    IEnumerator ShowMessage()
    {
        yield return new WaitForSeconds(messageTime);

        credits[0].SetActive(true);
        AudioManager.Instance.PostEvent("Stop_Ambience");
        AkSoundEngine.PostEvent("Play_Credits", AudioManager.Instance.gameObject, (uint)AkCallbackType.AK_MusicSyncUserCue, RevealCredit, null);
    }

    void RevealCredit(object in_cookie, AkCallbackType in_type, object in_info)
    {
        if (in_type == AkCallbackType.AK_MusicSyncUserCue)
        {
            if (creditsRevealed < credits.Length)
            {
                credits[creditsRevealed].SetActive(true);
                creditsRevealed++;
            }
        }
    }
}
