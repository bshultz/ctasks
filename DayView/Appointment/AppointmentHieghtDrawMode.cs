using System;
using System.Collections.Generic;
using System.Text;

namespace Calendar
{
	public enum AppHeightDrawMode
	{
		TrueHeightAll = 0, // all appointments have height proportional to true length
		FullHalfHourBlocksAll = 1, // all appts drawn in half hour blocks
		EndHalfHourBlocksAll = 2, // all appts boxes start at actual time, end in half hour blocks
		FullHalfHourBlocksShort = 3, // short (< 30 mins) appts drawn in half hour blocks
		EndHalfHourBlocksShort = 4, // short appts boxes start at actual time, end in half hour blocks
	}
}
