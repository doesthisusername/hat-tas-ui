﻿using System.Collections.Generic;

namespace HatTASUI
{
    public class FrameState
    {
        public IDictionary<string, float> Inputs { get; set; }
		public string Random { get; set; }

        public FrameState()
        {
            Inputs = new Dictionary<string, float>();
            Inputs["LX"] = Editor.STICK_MID;
            Inputs["LY"] = Editor.STICK_MID;
            Inputs["RX"] = Editor.STICK_MID;
            Inputs["RY"] = Editor.STICK_MID;
            Inputs["A"] = 0;
            Inputs["B"] = 0;
            Inputs["X"] = 0;
            Inputs["Y"] = 0;
            Inputs["START"] = 0;
            Inputs["LB"] = 0;
            Inputs["RB"] = 0;
            Inputs["LT"] = 0;
            Inputs["RT"] = 0;
            Inputs["BACK"] = 0;
            Inputs["LEFT"] = 0;
            Inputs["DOWN"] = 0;
            Inputs["UP"] = 0;
            Inputs["RIGHT"] = 0;
			Inputs["L3"] = 0;
			Inputs["R3"] = 0;
            Inputs["SPEED"] = 1;

			Random = "0";
        }

        public void UpdateFromChanges(IDictionary<string, float> changes, string Random)
        {
            foreach (var key in changes.Keys)
            {
                Inputs[key] = changes[key];
            }

			if(Random == this.Random)
			{
				this.Random = string.Empty;
			}
			else
			{
				this.Random = Random;
			}
        }
    }
}
