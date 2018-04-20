using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// In this game, ReactRed, we want to display a stimulus (rectangle) for a defined duration.
/// During that duration, the player needs to respond as quickly as possible.
/// Each Trial also has a defined delay to keep the player from guessing.
/// Some appropriate visual feedback is also displayed according to the player's response.
/// </summary>
public class ReactRed : GameBase
{
	const string INSTRUCTIONS = " Press <color=cyan>Spacebar</color> as soon as you see the square. Do not press <color=cyan>Spacebar</color> if the square is Red";
	const string FINISHED = "FINISHED!";
	const string RESPONSE_GUESS = "No Guessing!";
	const string RESPONSE_CORRECT = "Good!";
	const string RESPONSE_TIMEOUT = "Missed it!";
	const string RESPONSE_SLOW = "Too Slow!";
    const string RESPONSE_RED = "Responded to wrong color!";
    const string RESPONSE_RED_IGNORED = "Nice! Ignored red";

	Color RESPONSE_COLOR_GOOD = Color.green;
	Color RESPONSE_COLOR_BAD = Color.red;

	/// <summary>
	/// A reference to the UI canvas so we can instantiate the feedback text.
	/// </summary>
	public GameObject uiCanvas;
	/// <summary>
	/// The object that will be displayed briefly to the player.
	/// </summary>
	public GameObject stimulus;
	/// <summary>
	/// A prefab for an animated text label that appears when a trial fails/succeeds.
	/// </summary>
	public GameObject feedbackTextPrefab;
	/// <summary>
	/// The instructions text label.
	/// </summary>
	public Text instructionsText;

    Vector3 StimRandomPos = new Vector3(0, 0, 0);
    bool currentColorIsRed = false;



    /// <summary>
    /// Called when the game session has started.
    /// </summary>
    public override GameBase StartSession(TextAsset sessionFile)
	{
		base.StartSession(sessionFile);

		instructionsText.text = INSTRUCTIONS;
		StartCoroutine(RunTrials(SessionData));

		return this;
	}


	/// <summary>
	/// Iterates through all the trials, and calls the appropriate Start/End/Finished events.
	/// </summary>
	protected virtual IEnumerator RunTrials(SessionData data)
	{
		foreach (Trial t in data.trials)
		{
			StartTrial(t);
			yield return StartCoroutine(DisplayStimulus(t));
			EndTrial(t);
		}
		FinishedSession();
		yield break;
	}


	/// <summary>
	/// Displays the Stimulus for a specified duration.
	/// During that duration the player needs to respond as quickly as possible.
	/// </summary>
	protected virtual IEnumerator DisplayStimulus(Trial t)
	{
		GameObject stim = stimulus;
		stim.SetActive(false);

		yield return new WaitForSeconds(t.delay);

		StartInput();

        ReactTrialRed RTR = t as ReactTrialRed;

        if(RTR.isRed)
        {
            stim.GetComponent<Image>().color = Color.red;
            currentColorIsRed = true;

        }
        if (!RTR.isRed)
        {
            stim.GetComponent<Image>().color = Color.white;
            currentColorIsRed = false;
        }
        Debug.Log("IS random pos: + " +RTR.isRandomPos);
       
        if (RTR.isRandomPos)
        {
           
           

            StimRandomPos.x = GetRandomX(RTR.minX, RTR.maxX);
            StimRandomPos.y = GetRandomY(RTR.minY, RTR.maxY);

            stim.GetComponent<RectTransform>().anchoredPosition = StimRandomPos;
            Debug.Log(stim.GetComponent<RectTransform>().position);
        }

		stim.SetActive(true);

		yield return new WaitForSeconds(((ReactTrialRed)t).duration);
		stim.SetActive(false);
		EndInput();

		yield break;
	}


	/// <summary>
	/// Called when the game session is finished.
	/// e.g. All session trials have been completed.
	/// </summary>
	protected override void FinishedSession()
	{
		base.FinishedSession();
		instructionsText.text = FINISHED;
	}


	/// <summary>
	/// Called when the player makes a response during a Trial.
	/// StartInput needs to be called for this to execute, or override the function.
	/// </summary>
	public override void PlayerResponded(KeyCode key, float time)
	{
		if (!listenForInput)
		{
			return;
		}
		base.PlayerResponded(key, time);
		if (key == KeyCode.Space)
		{
			EndInput();
			AddResult(CurrentTrial, time);
		}
	}


	/// <summary>
	/// Adds a result to the SessionData for the given trial.
	/// </summary>
	protected override void AddResult(Trial t, float time)
	{
     
        TrialResult r = new TrialResult(t);
		r.responseTime = time;
		if (time == 0)
		{
			// No response.
		

            if (IsValidStimulusColor(t))
            {
                GUILog.Log("Fail! No response!");
                DisplayFeedback(RESPONSE_TIMEOUT, RESPONSE_COLOR_BAD);
            }
            else
            {
                GUILog.Log("Nice! You ignored the red square!");
                DisplayFeedback(RESPONSE_RED_IGNORED, RESPONSE_COLOR_GOOD);
            }

            
		}
		else
		{
            Debug.Log(Time.deltaTime);
			if (IsGuessResponse(time))
			{
				// Responded before the guess limit, aka guessed.
				DisplayFeedback(RESPONSE_GUESS, RESPONSE_COLOR_BAD);
				GUILog.Log("Fail! Guess response! responseTime = {0}", time);
			}
			else if (IsValidResponse(time))
			{
                if (IsValidStimulusColor(t))
                {
                    // Responded correctly.
                    DisplayFeedback(RESPONSE_CORRECT, RESPONSE_COLOR_GOOD);
                    r.success = true;
                    r.accuracy = GetAccuracy(t, time);
                    GUILog.Log("Success! responseTime = {0}", time);
                }
                else
                {
                    DisplayFeedback(RESPONSE_RED, RESPONSE_COLOR_BAD);
                    r.success = false;
                    r.accuracy = GetAccuracy(t, time);
                    GUILog.Log("Fail! Responded to wrong color");
                }
			}
			else
			{
				// Responded too slow.
				DisplayFeedback(RESPONSE_SLOW, RESPONSE_COLOR_BAD);
				GUILog.Log("Fail! Slow response! responseTime = {0}", time);
			}

		}

        if(currentColorIsRed)
        {
            r.isRed = true;
        }
        else
        {
            r.isRed = false;
        }

        r.positionX = (int)StimRandomPos.x;
        r.positionY = (int)StimRandomPos.y;


        sessionData.results.Add(r);
	}


	/// <summary>
	/// Display visual feedback on whether the trial has been responded to correctly or incorrectly.
	/// </summary>
	private void DisplayFeedback(string text, Color color)
	{
		GameObject g = Instantiate(feedbackTextPrefab);
		g.transform.SetParent(uiCanvas.transform);
		g.transform.localPosition = feedbackTextPrefab.transform.localPosition;
		Text t = g.GetComponent<Text>();
		t.text = text;
		t.color = color;
	}


	/// <summary>
	/// Returns the players response accuracy.
	/// The perfect accuracy would be 1, most inaccuracy is 0.
	/// </summary>
	protected float GetAccuracy(Trial t, float time)
	{
		ReactDataRed data = sessionData.gameData as ReactDataRed;
		bool hasResponseTimeLimit =  data.ResponseTimeLimit > 0;

		float rTime = time - data.GuessTimeLimit;
		float totalTimeWindow = hasResponseTimeLimit ? 
			data.ResponseTimeLimit : (t as ReactTrialRed).duration;

		return 1f - (rTime / (totalTimeWindow - data.GuessTimeLimit));
	}


	/// <summary>
	/// Returns True if the given response time is considered a guess.
	/// </summary>
	protected bool IsGuessResponse(float time)
	{
        ReactDataRed data = sessionData.gameData as ReactDataRed;
        return data.GuessTimeLimit > 0 && time < data.GuessTimeLimit;
    }


	/// <summary>
	/// Returns True if the given response time is considered valid.
	/// </summary>
	protected bool IsValidResponse(float time)
	{
        ReactDataRed data = sessionData.gameData as ReactDataRed;
		return data.ResponseTimeLimit <= 0 || time < data.ResponseTimeLimit;
	}

    protected bool IsValidStimulusColor(Trial t)
    {
        ReactTrialRed RTR = t as ReactTrialRed;
        //ReactDataRed data = sessionData.gameData as ReactDataRed;
        Debug.Log(" print data color:" + RTR.isRed);
        if (RTR.isRed)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    protected int GetRandomX(int minX, int maxX)
    {
        return (int)Random.Range(minX, maxX);
    }

    protected int GetRandomY(int minY, int maxY)
    {
        return (int)Random.Range(minY, maxY);
    }



}
