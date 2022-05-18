using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingsController : MonoBehaviour
{
    [SerializeField] TaskSO endTask;
    [Header("Good ending")]
    [SerializeField] GameObject goodEnding;
    [SerializeField] int vmNeeded;
    [Header("Okay ending")]
    [SerializeField] GameObject okayEnding;
    [Header("Bad ending")]
    [SerializeField] GameObject badEnding;
    [SerializeField] int subNeeded;
    [Header("TOster ending")]
    [SerializeField] GameObject tosterEnding;
    //[SerializeField] int onWhatDayStarts;

    bool isEndingStarted = false;

    private void Awake()
    {
        EventAggregator.endGame.Subscribe(startEnding);
        EventAggregator.taskComplete.Subscribe(checkEnding);
        EventAggregator.DialogueEnded.Subscribe(afterCutscene);
    }

    void startEnding(Endings ending)
    {
        isEndingStarted = true;
        if (ending == Endings.TosterEnding) EventAggregator.DialogueStarted.Publish(tosterEnding);
        else if (GameInfo.suspicion < GameInfo.computingPower && GameInfo.computingPower >= vmNeeded)
            EventAggregator.DialogueStarted.Publish(goodEnding);
        else if (GameInfo.suspicion >= GameInfo.computingPower && GameInfo.suspicion >= subNeeded)
            EventAggregator.DialogueStarted.Publish(badEnding);
        else
            EventAggregator.DialogueStarted.Publish(okayEnding);
    }

    private void afterCutscene()
    {
        if (isEndingStarted)
        {
            Debug.Log("END CUTSCENE ENDED");
            EventAggregator.Reset();
            GameInfo.Reset();
            SceneManager.LoadScene("MainMenuScene");
        }
        //do something here
    }

    private void checkEnding(TaskSO obj)
    {
        Debug.Log("Checking endings after task " + obj);
        if (endTask == null) return;
        if (obj.taskInfo == endTask.taskInfo) EventAggregator.endGame.Publish(Endings.FinalEnding);
    }
}
