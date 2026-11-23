
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MarineQuizManager : MonoBehaviour
{
    [System.Serializable]
    public class QuizQuestion
    {
        public string question;
        public string[] answers;
        public int correctAnswerIndex;
        public string explanation;
    }

    [Header("Quiz UI")]
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private TMP_Text feedbackText;
    [SerializeField] private TMP_Text scoreText;

    [Header("Answer Buttons")]
    [SerializeField] private Button[] answerButtons;

    [Header("Quiz Settings")]
    [SerializeField] private float nextQuestionDelay = 2.5f;

    private QuizQuestion[] questions;
    private int currentQuestionIndex;
    private int score;
    private bool acceptingAnswers;
    private Coroutine nextQuestionCoroutine;

    private void Start()
    {
        CreateQuestions();
        StartQuiz();
    }

    private void CreateQuestions()
    {
        questions = new QuizQuestion[]
        {
            new QuizQuestion
            {
                question = "Which marine animal has eight arms?",
                answers = new[]
                {
                    "Octopus", "Shark", "Dolphin", "Jellyfish"
                },
                correctAnswerIndex = 0,
                explanation = "Octopuses have eight arms!"
            },
            new QuizQuestion
            {
                question = "What do clownfish commonly live among?",
                answers = new[]
                {
                    "Coral rocks", "Sea anemones",
                    "Seaweed only", "Open ocean"
                },
                correctAnswerIndex = 1,
                explanation = "Clownfish live among sea anemones!"
            },
            new QuizQuestion
            {
                question = "What is the largest fish in the ocean?",
                answers = new[]
                {
                    "Great white shark", "Blue whale",
                    "Whale shark", "Manta ray"
                },
                correctAnswerIndex = 2,
                explanation = "The whale shark is the largest fish!"
            },
            new QuizQuestion
            {
                question = "What is a major threat to coral reefs?",
                answers = new[]
                {
                    "Clean water", "Healthy fish",
                    "Sunlight", "Ocean warming"
                },
                correctAnswerIndex = 3,
                explanation = "Ocean warming can cause coral bleaching!"
            },
            new QuizQuestion
            {
                question = "How can people help protect marine life?",
                answers = new[]
                {
                    "Reduce plastic waste", "Litter beaches",
                    "Damage coral", "Leave fishing nets"
                },
                correctAnswerIndex = 0,
                explanation = "Reducing plastic waste protects marine life!"
            }
        };
    }

    private void StartQuiz()
    {
        currentQuestionIndex = 0;
        score = 0;

        if (nextQuestionCoroutine != null)
        {
            StopCoroutine(nextQuestionCoroutine);
            nextQuestionCoroutine = null;
        }

        ShowQuestion();
    }

    private void ShowQuestion()
    {
        if (currentQuestionIndex >= questions.Length)
        {
            FinishQuiz();
            return;
        }

        QuizQuestion current = questions[currentQuestionIndex];

        questionText.text =
            $"Question {currentQuestionIndex + 1}/{questions.Length}\n" +
            current.question;

        feedbackText.text = "Select an answer!";
        feedbackText.color = Color.cyan;

        scoreText.text = $"Score: {score}/{questions.Length}";

        acceptingAnswers = true;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            Button button = answerButtons[i];

            if (button == null)
                continue;

            button.interactable = true;

            TMP_Text buttonText =
                button.GetComponentInChildren<TMP_Text>();

            if (buttonText != null)
            {
                buttonText.text =
                    $"{(char)('A' + i)}. {current.answers[i]}";
            }
        }
    }

    public void SelectAnswer(int answerIndex)
    {
        if (!acceptingAnswers)
            return;

        if (answerIndex < 0 || answerIndex >= 4)
            return;

        acceptingAnswers = false;

        QuizQuestion current = questions[currentQuestionIndex];

        bool correct =
            answerIndex == current.correctAnswerIndex;

        if (correct)
        {
            score++;
            feedbackText.text =
                "CORRECT!\n" + current.explanation;

            feedbackText.color = Color.green;
        }
        else
        {
            feedbackText.text =
                "INCORRECT!\n" + current.explanation;

            feedbackText.color = new Color(1f, 0.45f, 0.35f);
        }

        scoreText.text =
            $"Score: {score}/{questions.Length}";

        foreach (Button button in answerButtons)
        {
            if (button != null)
                button.interactable = false;
        }

        nextQuestionCoroutine =
            StartCoroutine(AdvanceAfterDelay());
    }

    private IEnumerator AdvanceAfterDelay()
    {
        yield return new WaitForSeconds(nextQuestionDelay);

        currentQuestionIndex++;
        nextQuestionCoroutine = null;

        ShowQuestion();
    }

    private void FinishQuiz()
    {
        acceptingAnswers = false;

        questionText.text = "QUIZ COMPLETE!";

        scoreText.text =
            $"Final Score: {score}/{questions.Length}";

        float percentage =
            (float)score / questions.Length * 100f;

        if (percentage >= 80f)
        {
            feedbackText.text =
                "EXCELLENT!\nYou're a Marine Life Expert!";
            feedbackText.color = Color.green;
        }
        else if (percentage >= 60f)
        {
            feedbackText.text =
                "GOOD JOB!\nKeep learning about marine life!";
            feedbackText.color = Color.cyan;
        }
        else
        {
            feedbackText.text =
                "KEEP EXPLORING!\nEvery discovery helps you learn!";
            feedbackText.color = Color.yellow;
        }

        foreach (Button button in answerButtons)
        {
            if (button != null)
                button.gameObject.SetActive(false);
        }

        Debug.Log(
            $"AquaDiscover Quiz Completed! Score: {score}/{questions.Length}"
        );
    }
}
