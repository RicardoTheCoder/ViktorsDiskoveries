﻿using System.Collections.Generic;
using UnityEngine;

namespace Guu.Utils
{
	/// <summary>
	/// An utility class to help with Colors
	/// </summary>
	public static class ColorUtils
	{
		/// <summary>
		/// Gets a color from a Hexadecimal Code
		/// </summary>
		/// <param name="hex">Hexa Code (without the #)</param>
		/// <returns>The color or white if invalid hexa</returns>
		public static Color FromHex(string hex)
		{
			ColorUtility.TryParseHtmlString("#" + hex.ToUpper(), out Color color);

			return color;
		}
		
		/// <summary>
		/// Gets colors from an array of Hexadecimal Codes
		/// </summary>
		/// <param name="hexas">Hexa Codes (without the #)</param>
		/// <returns>The array of colors (invalid hexas will be white)</returns>
		public static Color[] FromHexArray(params string[] hexas)
		{
			List<Color> colors = new List<Color>();

			foreach (string hex in hexas)
			{
				ColorUtility.TryParseHtmlString("#" + hex.ToUpper(), out Color color);
				colors.Add(color);
			}

			return colors.ToArray();
		}

		/// <summary>
		/// Turns a color into a Hexadecimal Code
		/// </summary>
		/// <param name="color">Color to turn</param>
		/// <returns>Hexadecimal code without the #</returns>
		public static string ToHexRGB(Color color)
		{
			return ColorUtility.ToHtmlStringRGB(color);
		}

		/// <summary>
		/// Turns a color into a Hexadecimal Code with alpha
		/// </summary>
		/// <param name="color">Color to turn</param>
		/// <returns>Hexadecimal code without the #</returns>
		public static string ToHexRGBA(Color color)
		{
			return ColorUtility.ToHtmlStringRGB(color);
		}
	}
}
