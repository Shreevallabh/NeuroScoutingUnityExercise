using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;


/// <summary>
/// Contains Trial data for the React gameType.
/// </summary>
public class ReactTrialRed : Trial
{
	/// <summary>
	/// The duration the stimulus will be shown for.
	/// </summary>
	public float duration = 0;
    /// <summary>
    /// If the stimulus is red or not.
    /// </summary>
    public bool isRed = false;
    /// <summary>
    /// If the position of stimulus is random or not.
    /// </summary>
    public bool isRandomPos = false;
    /// <summary>
    /// Minimum X coordinate value of the stimulus if its position is random.
    /// </summary>
    public int minX = 0;
    /// <summary>
    /// Maximum X coordinate value of the stimulus if its position is random.
    /// </summary>
    public int maxX = 0;
    /// <summary>
    /// Minimum Y coordinate value of the stimulus if its position is random.
    /// </summary>
    public int minY = 0;
    /// <summary>
    /// Maximum Y coordinate value of the stimulus if its position is random.
    /// </summary>
    public int maxY = 0;
    /// <summary>
    /// X coordinate value of the stimulus if its position is not random.
    /// </summary>
    public int fixedX = 0;
    /// <summary>
    /// Y coordinate value of the stimulus if its position is not random.
    /// </summary>
    public int fixedY = 0;




    #region ACCESSORS

    public float Duration
	{
		get
		{
			return duration;
		}
	}

	#endregion


	public ReactTrialRed(SessionData data, XmlElement n = null) 
		: base(data, n)
	{
	}
	

	/// <summary>
	/// Parses Game specific variables for this Trial from the given XmlElement.
	/// If no parsable attributes are found, or fail, then it will generate some from the given GameData.
	/// Used when parsing a Trial that IS defined in the Session file.
	/// </summary>
	public override void ParseGameSpecificVars(XmlNode n, SessionData session)
	{
		base.ParseGameSpecificVars(n, session);

		ReactDataRed data = (ReactDataRed)(session.gameData);
		if (!XMLUtil.ParseAttribute(n, ReactDataRed.ATTRIBUTE_DURATION, ref duration, true))
		{
			duration = data.GeneratedDuration;
		}

        if (!XMLUtil.ParseAttribute(n, ReactDataRed.ATTRIBUTE_ISRED, ref isRed, true))
        {
            isRed = data.IsRed;
        }

        if (!XMLUtil.ParseAttribute(n, ReactDataRed.ATTRIBUTE_ISRANDOMPOS, ref isRandomPos, true))
        {
            isRandomPos = data.IsRandomPos;
        }

        if (!XMLUtil.ParseAttribute(n, ReactDataRed.ATTRIBUTE_MINX, ref minX, true))
        {
            minX = data.MinX;
        }

        if (!XMLUtil.ParseAttribute(n, ReactDataRed.ATTRIBUTE_MAXX, ref maxX, true))
        {
            maxX = data.MaxX;
        }

        if (!XMLUtil.ParseAttribute(n, ReactDataRed.ATTRIBUTE_MINY, ref minY, true))
        {
            minY = data.MinY;
        }

        if (!XMLUtil.ParseAttribute(n, ReactDataRed.ATTRIBUTE_MAXY, ref maxY, true))
        {
            maxY = data.MaxY;
        }

        if (!XMLUtil.ParseAttribute(n, ReactDataRed.ATTRIBUTE_FIXEDX, ref fixedX, true))
        {
            fixedX = data.FixedX;
        }

        if (!XMLUtil.ParseAttribute(n, ReactDataRed.ATTRIBUTE_FIXEDY, ref fixedY, true))
        {
            fixedX = data.FixedY;
        }


    }


	/// <summary>
	/// Writes any tracked variables to the given XElement.
	/// </summary>
	public override void WriteOutputData(ref XElement elem)
	{
		base.WriteOutputData(ref elem);
		XMLUtil.CreateAttribute(ReactDataRed.ATTRIBUTE_DURATION, duration.ToString(), ref elem);
	}
}
