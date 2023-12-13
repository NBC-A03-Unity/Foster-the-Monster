using UnityEngine;
using System;
using System.Collections;
using System.Text;
using System.Security.Cryptography;

public static class Utility
{
    public static StringBuilder sb = new StringBuilder();

    private static RNGCryptoServiceProvider rng = rng = new RNGCryptoServiceProvider();
    public static void Invoke(this MonoBehaviour mb, Action f, float delay)
    {
        mb.StartCoroutine(InvokeRoutine(f, delay));
    }

    private static IEnumerator InvokeRoutine(System.Action f, float delay)
    {
        yield return new WaitForSeconds(delay);
        f();
    }

    public static int RandomInt()
    {
        byte[] randomNumber = new byte[4];
        rng.GetBytes(randomNumber);
        int value = BitConverter.ToInt32(randomNumber, 0);

        return value;
    }

    public static void CheckPopUp(int e, int k)
    {
        int singlePopupKey = GlobalSettings.CurrentLocale == "en-US" ? e : k;

        UIManager.Instance.OpenSingleConfirmationPopup(
            singlePopupKey,
            () => {
                UIManager.Instance.CloseUI<SingleConfirmationPopup>();
            }
        );

    }
}
