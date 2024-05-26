using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Sahne yönetimi için gerekli


public class GameManager : MonoBehaviour
{
    public TMP_Text[] problemTexts; // TextMesh Pro metin referansları
    public InputField answerInput; // Legacy Input Field referansı
    public string nextSceneName = "son"; // Geçiş yapılacak sahnenin adı

    private int currentProblemIndex = 0;
    private int[] answers;
    private char[] operators = { '+', '-', '*', '/' }; // Operatörler dizisi

    void Start()
    {
        answers = new int[problemTexts.Length];
        for (int i = 0; i < problemTexts.Length; i++)
        {
            if (problemTexts[i] != null)
            {
                problemTexts[i].gameObject.SetActive(false); // Başlangıçta tüm metinleri pasif yap
            }
        }
        GenerateProblem(currentProblemIndex);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            CheckAnswer();
        }
    }

    void GenerateProblem(int index)
    {
        if (index < problemTexts.Length && problemTexts[index] != null)
        {
            int num1 = Random.Range(1, 10 * (index + 1));
            int num2 = Random.Range(1, 10 * (index + 1));
            char op = operators[Random.Range(0, operators.Length)];
            int answer = 0;

            switch (op)
            {
                case '+':
                    answer = num1 + num2;
                    break;
                case '-':
                    answer = num1 - num2;
                    break;
                case '*':
                    answer = num1 * num2;
                    break;
                case '/':
                    while (num2 == 0 || num1 % num2 != 0) // Bölme işlemi için tam sayı sonuç veren sayılar seç
                    {
                        num1 = Random.Range(1, 10 * (index + 1));
                        num2 = Random.Range(1, 10 * (index + 1));
                    }
                    answer = num1 / num2;
                    break;
            }

            answers[index] = answer;
            problemTexts[index].text = num1 + " " + op + " " + num2 + " = ?";
            problemTexts[index].gameObject.SetActive(true); // Şimdiki soruyu aktif yap
            answerInput.ActivateInputField(); // Yeni soruyu gösterirken input alanını aktif yap

            // Problem görünürlüğünü 5 saniye sonra kapatmak için Coroutine başlat
            StartCoroutine(HideProblemAfterTime(problemTexts[index], 5f));

            // Son soru ise sahne geçişini başlat
            if (index == problemTexts.Length - 1)
            {
                StartCoroutine(LoadNextSceneAfterDelay(2f)); // 2 saniye sonra sahne geçişini başlat
            }
        }
        else
        {
            Debug.LogError("Problem texts or index is out of range!");
        }
    }

    IEnumerator HideProblemAfterTime(TMP_Text problemText, float delay)
    {
        yield return new WaitForSeconds(delay);
        problemText.gameObject.SetActive(false);
    }

    IEnumerator LoadNextSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(nextSceneName); // Belirtilen sahneyi yükler
    }

    public void CheckAnswer()
    {
        int playerAnswer;
        if (int.TryParse(answerInput.text, out playerAnswer))
        {
            if (playerAnswer == answers[currentProblemIndex])
            {
                if (currentProblemIndex < problemTexts.Length && problemTexts[currentProblemIndex] != null)
                {
                    problemTexts[currentProblemIndex].gameObject.SetActive(false); // Doğru cevap verildiğinde mevcut soruyu pasif yap
                }
                currentProblemIndex++;
                if (currentProblemIndex < problemTexts.Length)
                {
                    GenerateProblem(currentProblemIndex);
                    answerInput.text = "";
                }
                else
                {
                    Debug.Log("Tebrikler, tüm soruları doğru cevapladınız!");
                    // Son soruya ulaşıldığında sahneyi değiştir
                    StartCoroutine(LoadNextSceneAfterDelay(2f));
                }
            }
            else
            {
                Debug.Log("Yanlış cevap, tekrar deneyin.");
                answerInput.ActivateInputField(); // Yanlış cevap girildiğinde input alanını tekrar aktif yap
            }
        }
        else
        {
            Debug.Log("Geçersiz cevap, bir sayı girin.");
            answerInput.ActivateInputField(); // Geçersiz cevap girildiğinde input alanını tekrar aktif yap
        }
    }
}
