﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace HatTASUI
{
    public class Frame
    {
        public int FrameNumber { get; set; }
        public IDictionary<string, float> Changes { get; set; }
		public string Random { get; set; }
        public string Comment { get; set; }

        public Frame(int frameNumber)
        {
            FrameNumber = frameNumber;
            Changes = new Dictionary<string, float>();
			Random = string.Empty;
            Comment = string.Empty;
        }

        public Frame Clone()
        {
            var newFrame = new Frame(FrameNumber);
            newFrame.Changes = Changes.ToDictionary(x => x.Key, x => x.Value);
			newFrame.Random = Random;
            newFrame.Comment = Comment;
            return newFrame;
        }

        public string ToListItem(Font font, int listWidth)
        {
			var result = FrameNumber.ToString();
			var defaults = new FrameState();

			while(TextRenderer.MeasureText(result, font).Width < listWidth - (listWidth - (listWidth / 7)))
			{
				result += ' ';
			}

			foreach(var change in Changes)
			{
				if(ValuesEqual(change.Value, defaults.Inputs[change.Key]))
				{
					result += " ~" + change.Key;
				}
				else if(change.Key == "SPEED")
				{
					result += " SPEED:" + change.Value;
				}
				else if(change.Key == "LX" || change.Key == "LY" || change.Key == "RX" || change.Key == "RY")
				{
					result += " " + change.Key + ":" + (int)change.Value;
				}
				else if((int)change.Value == 1)
				{
					result += " " + change.Key;
				}
			}

			if(Random != string.Empty)
			{
				if(Random == defaults.Random)
				{
					result += " ~RNG";
				}
				else
				{
					result += " RNG:" + Random;
				}
			}

			if (!string.IsNullOrEmpty(Comment))
            {
				while (TextRenderer.MeasureText(result, font).Width < listWidth - (int)(listWidth / 2.4f))
				{
					result += ' ';
				}

				result += ' ' + Comment;
            }

			return result;
        }

        public override string ToString()
        {
            var result = FrameNumber.ToString("000000") + ":";
            var defaults = new FrameState();
            foreach (var change in Changes)
            {               
                if (ValuesEqual(change.Value, defaults.Inputs[change.Key]))
                {
                    result += " ~" + change.Key;
                }
                else if (change.Key == "SPEED")
                {
                    result += " SPEED:" + change.Value;
                }
                else if (change.Key == "LX" || change.Key == "LY" || change.Key == "RX" || change.Key == "RY")
                {
                    result += " " + change.Key + ":" + (int)change.Value;
                }
                else if ((int)change.Value == 1)
                {
                    result += " " + change.Key;
                }
            }

			if(Random != string.Empty)
			{
				if(Random == defaults.Random)
				{
					result += " ~RNG";
				}
				else
				{
					result += " RNG:" + Random;
				}
			}

			if (!string.IsNullOrEmpty(Comment))
            {
                result += new string(' ', Math.Max(35 - result.Length, 0))
                          + " // " + Comment;
            }
            return result;
        }

        public static Frame FromString(string line)
        {
            var split = line.Split(new string[] { ": " }, 2, StringSplitOptions.None);
			if (split.Length == 1)
				return null;

            var frame = new Frame(int.Parse(split[0]));

            var commentSplit = split[1].Split(new string[] { "//" }, 2, StringSplitOptions.None);

            if (commentSplit.Length > 1)
                frame.Comment = commentSplit[1].Trim();

            var tokens = commentSplit[0].Trim().Split(' ');

			if(tokens.Length == 0)
				return null;

            var defaults = new FrameState();
            foreach (var token in tokens)
            {
                var tokenSplit = token.Split(':');
                if (tokenSplit[0] == "SPEED")
                {
                    frame.Changes.Add("SPEED", float.Parse(tokenSplit[1]));
                }
				else if (tokenSplit[0] == "RNG")
				{
					frame.Random = tokenSplit[1];
				}
				else if (token == "~RNG")
				{
					frame.Random = defaults.Random;
				}
                else if (tokenSplit.Length > 1)
                {
                    frame.Changes.Add(tokenSplit[0], int.Parse(tokenSplit[1]));
                }
                else if (token.StartsWith("~"))
                {
                    var input = token.Substring(1);
                    frame.Changes.Add(input, defaults.Inputs[input]);
                }
                else
                {
                    frame.Changes.Add(token, 1);
                }
            }

            return frame;
        }

        public static bool ValuesEqual(float value1, float value2)
        {
            return Math.Abs(value1 - value2) < 0.00001;
        }
    }
}
