using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;


/// <summary>
/// Contains general Game data for the React gametype.
/// </summary>
public class ReactDataRed : GameData
{
	const string ATTRIBUTE_GUESS_TIMELIMIT = "guessTimeLimit";
	const string ATTRIBUTE_RESPONSE_TIMELIMIT = "responseTimeLimit";
	public const string ATTRIBUTE_DURATION = "duration";
    public const string ATTRIBUTE_DELAY = "delay";
    public const string ATTRIBUTE_ISRED = "isRed";
    public const string ATTRIBUTE_ISRANDOMPOS = "isRandomPos";
    public const string ATTRIBUTE_MINX = "minX";
    public const string ATTRIBUTE_MAXX = "maxX";
    public const string ATTRIBUTE_MINY = "minY";
    public const string ATTRIBUTE_MAXY = "maxY";
    public const string ATTRIBUTE_FIXEDX = "fixedX";
    public const string ATTRIBUTE_FIXEDY = "fixedY";



    /// <summary>
    /// The amount of time that needs to pass before the player can respond without being penalized.
    /// </summary>
    private float guessTimeLimit = 0;
	/// <summary>
	/// The amount of time that the user has to respond; 
	/// Starts when input becomes enabled during a Trial. 
	/// Responses that fall within this time constraint will be marked as Successful.
	/// </summary>
	private float responseTimeLimit = 0;
	/// <summary>
	/// The visibility Duration for the Stimulus.
	/// </summary>
	private float duration = 0;
    /// <summary>
    /// If the stimulus is Red or not.
    /// </summary>
    private bool isRed = false;
    /// <summary>
    /// If the trial requires stimulus to be at random position or not.
    /// </summary>
    private bool isRandomPos = false;
    /// <summary>
    /// Minimum X coordinate value of the stimulus if its position is random.
    /// </summary>
    private int minX = 0;
    /// <summary>
    /// Maximum X coordinate value of the stimulus if its position is random.
    /// </summary>
    private int maxX = 0;
    /// <summary>
    /// Minimum Y coordinate value of the stimulus if its position is random.
    /// </summary>
    private int minY = 0;
    /// <summary>
    /// Maximum Y coordinate value of the stimulus if its position is random.
    /// </summary>
    private int maxY = 0;
    /// <summary>
    /// X coordinate value of the stimulus if its position is not random.
    /// </summary>
    private int fixedX = 0;
    /// <summary>
    /// Y coordinate value of the stimulus if its position is not random.
    /// </summary>
    private int fixedY = 0;

    #region ACCESSORS

    public float GuessTimeLimit
	{
		get
		{
			return guessTimeLimit;
		}
	}
	public float ResponseTimeLimit
	{
		get
		{
			return responseTimeLimit;
		}
	}
	public float GeneratedDuration
	{
		get
		{
			return duration;
		}
	}

    public bool IsRed
    {
        get
        {
            return isRed;
        }
    }

    public bool IsRandomPos
    {
        get
        {
            return isRandomPos;
        }
    }

    public int MinX
    {
        get
        {
            return minX;
        }
    }

    public int MaxX
    {
        get
        {
            return maxX;
        }
    }

    public int MinY
    {
        get
        {
            return minY;
        }
    }

    public int MaxY
    {
        get
        {
            return maxY;
        }
    }

    public int FixedX
    {
        get
        {
            return fixedX;
        }
    }

    public int FixedY
    {
        get
        {
            return fixedY;
        }
    }

    #endregion


    public ReactDataRed(XmlElement elem) 
		: base(elem)
	{
	}


	public override void ParseElement(XmlElement elem)
	{
		base.ParseElement(elem);
		XMLUtil.ParseAttribute(elem, ATTRIBUTE_DURATION, ref duration);
		XMLUtil.ParseAttribute(elem, ATTRIBUTE_RESPONSE_TIMELIMIT, ref responseTimeLimit);
		XMLUtil.ParseAttribute(elem, ATTRIBUTE_GUESS_TIMELIMIT, ref guessTimeLimit);
	}


	public override void WriteOutputData(ref XElement elem)
	{
		base.WriteOutputData(ref elem);
		XMLUtil.CreateAttribute(ATTRIBUTE_GUESS_TIMELIMIT, guessTimeLimit.ToString(), ref elem);
		XMLUtil.CreateAttribute(ATTRIBUTE_RESPONSE_TIMELIMIT, responseTimeLimit.ToString(), ref elem);
		XMLUtil.CreateAttribute(ATTRIBUTE_DURATION, duration.ToString(), ref elem);
	}
}
