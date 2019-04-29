///
/// This file is auto generated based on the Space Engineers content directory.
/// Space Engineers version: 1.0.0.0, date: 2019-04-15 15:33:34
/// 

using System.Collections.Generic;
using VRageMath;

namespace IngameScript.Mockups.Base
{
	public partial class MockTextSurface
	{

		static readonly IReadOnlyDictionary<string, string> _fonts = new Dictionary<string, string>
		{
			{ "Debug", "white_shadow" },
			{ "Red", "white_shadow" },
			{ "Green", "white" },
			{ "Blue", "white" },
			{ "White", "white" },
			{ "DarkBlue", "white" },
			{ "UrlNormal", "white" },
			{ "UrlHighlight", "white" },
			{ "ErrorMessageBoxCaption", "white" },
			{ "ErrorMessageBoxText", "white" },
			{ "InfoMessageBoxCaption", "white" },
			{ "InfoMessageBoxText", "white" },
			{ "ScreenCaption", "white" },
			{ "GameCredits", "white" },
			{ "LoadingScreen", "white_shadow" },
			{ "BuildInfo", "white" },
			{ "BuildInfoHighlight", "white" },
			{ "Monospace", "monospace" }
		};

		static readonly IReadOnlyDictionary<string, IReadOnlyDictionary<Vector2, string>> _charMap = new Dictionary<string, IReadOnlyDictionary<Vector2, string>>()
		{
			{
				"monospace",
				new Dictionary<Vector2, string>()
				{
					{ new Vector2(30, 42), " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~ ¡¢£¤¥¦§¨©ª«¬­®¯°±²³´µ¶·¸¹º»¼½¾¿ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖ×ØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõö÷øùúûüýþÿĀāĂăĄąĆćĈĉĊċČčĎďĐđĒēĔĕĖėĘęĚěĜĝĞğĠġĢģĤĥĦħĨĩĪīĬĭĮįİıĲĳĴĵĶķĸĹĺĻļĽľĿŀŁłŃńŅņŇňŉŊŋŌōŎŏŐőŒœŔŕŖŗŘřŚśŜŝŞşŠšŢţŤťŦŧŨũŪūŬŭŮůŰűŲųŴŵŶŷŸŹźŻżŽžſƒơƷǺǻǼǽǾǿȘșȚțɑɸˆˇˉ˘˙˚˛˜˝;΄΅Ά·ΈΉΊΌΎΏΐΑΒΓΔΕΖΗΘΙΚΛΜΝΞΟΠΡΣΤΥΦΧΨΩΪΫάέήίΰαβγδεζηθικλμνξοπρςστυφχψωϊϋόύώϐϴЀЁЂЃЄЅІЇЈЉЊЋЌЍЎЏАБВГДЕЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдежзийклмнопрстуфхцчшщъыьэюяѐёђѓєѕіїјљњћќѝўџҐґ־אבגדהוזחטיךכלםמןנסעףפץצקרשתװױײ׳״ᴛᴦᴨẀẁẂẃẄẅẟỲỳ‐‒–—―‗‘’‚‛“”„‟†‡•…‧‰′″‵‹›‼‾‿⁀⁄⁔⁴⁵⁶⁷⁸⁹⁺⁻ⁿ₁₂₃₄₅₆₇₈₉₊₋₣₤₧₪€℅ℓ№™Ω℮⅐⅑⅓⅔⅕⅖⅗⅘⅙⅚⅛⅜⅝⅞←↑→↓↔↕↨∂∅∆∈∏∑−∕∙√∞∟∩∫≈≠≡≤≥⊙⌀⌂⌐⌠⌡─│┌┐└┘├┤┬┴┼═║╒╓╔╕╖╗╘╙╚╛╜╝╞╟╠╡╢╣╤╥╦╧╨╩╪╫╬▀▁▄█▌▐░▒▓■□▪▫▬▲►▼◄◊○●◘◙◦☺☻☼♀♂♠♣♥♦♪♫✓ﬁﬂ�" },
					{ new Vector2(53, 45), "" },
					{ new Vector2(1, 1), "" },
					{ new Vector2(37, 37), "" }
				}
			},
			{
				"white",
				new Dictionary<Vector2, string>()
				{
					{ new Vector2(15, 45), " " },
					{ new Vector2(24, 45), "!()1If¡ÌÍÎÏĨĪĮİІЇ‹›∙" },
					{ new Vector2(25, 45), "\",-.:;[]rt{}·ŕŗřţťŧț" },
					{ new Vector2(35, 45), "#&05689CHPVXZ¤¥ÇÞĀĆĈĊČĤĦŹŻŽƒАБВДИОСТУХЪЯ€" },
					{ new Vector2(36, 45), "$GUY§ÙÚÛÜĜĞĠĢŨŪŬŮŰŲФЦжы†‡" },
					{ new Vector2(39, 45), "%ю" },
					{ new Vector2(22, 45), "'|¦ˉј‘’‚" },
					{ new Vector2(26, 45), "*ªºŀ" },
					{ new Vector2(34, 45), "+24<=>E^~¬±¶ÈÉÊË×ß÷ĒĔĖĘĚЁЄЌЕЙПРЬЭ−" },
					{ new Vector2(30, 45), "/Lv«»ĹĻĽĿŁГгзлтэєҐ–" },
					{ new Vector2(33, 45), "3Kabdeghnopqsuy£µÝàáâãäåèéêëðñòóôõöøùúûüýþÿāăąďđēĕėęěĝğġģĥħĶńņňŉōŏőśŝşšũūŭůűųŶŷŸșЏЗКЛНбдеуцёђћўџ" },
					{ new Vector2(31, 45), "7?J_xz¿ĴźżžЈвнхчъьѓќ•" },
					{ new Vector2(40, 45), "@©®ĲЫ" },
					{ new Vector2(37, 45), "ABDNOQRSÀÁÂÃÄÅÐÑÒÓÔÕÖØĂĄĎĐŃŅŇŌŎŐŔŖŘŚŜŞŠȘЅЊЖф□" },
					{ new Vector2(32, 45), "FTck¢çćĉċčķŢŤŦȚЃЎЧаийкопрсяѕ" },
					{ new Vector2(42, 45), "MmwŵМШЮщ" },
					{ new Vector2(47, 45), "WÆŒŴ…‰" },
					{ new Vector2(28, 45), "\\" },
					{ new Vector2(23, 45), "`ijl ¨¯´¸ìíîïĩīįıĵĺļľłˆˇ˘˙˚˛˜˝ії" },
					{ new Vector2(14, 8), "­" },
					{ new Vector2(27, 45), "°²³¹“”„" },
					{ new Vector2(43, 45), "¼¾Љ" },
					{ new Vector2(45, 45), "½Щ" },
					{ new Vector2(44, 45), "æœ" },
					{ new Vector2(29, 45), "ĳґ" },
					{ new Vector2(41, 45), "мшњ" },
					{ new Vector2(38, 45), "љ" },
					{ new Vector2(46, 45), "—™" },
					{ new Vector2(53, 52), "" }
				}
			},
			{
				"white_shadow",
				new Dictionary<Vector2, string>()
				{
					{ new Vector2(15, 45), " " },
					{ new Vector2(24, 45), "!()1If¡ÌÍÎÏĨĪĮİІЇ‹›∙" },
					{ new Vector2(25, 45), "\",-.:;[]rt{}·ŕŗřţťŧț" },
					{ new Vector2(35, 45), "#&05689CHPVXZ¤¥ÇÞĀĆĈĊČĤĦŹŻŽƒАБВДИОСТУХЪЯ€" },
					{ new Vector2(36, 45), "$GUY§ÙÚÛÜĜĞĠĢŨŪŬŮŰŲФЦжы†‡" },
					{ new Vector2(39, 45), "%ю" },
					{ new Vector2(22, 45), "'|¦ˉј‘’‚" },
					{ new Vector2(26, 45), "*ªºŀ" },
					{ new Vector2(34, 45), "+24<=>E^~¬±¶ÈÉÊË×ß÷ĒĔĖĘĚЁЄЌЕЙПРЬЭ−" },
					{ new Vector2(30, 45), "/Lv«»ĹĻĽĿŁГгзлтэєҐ–" },
					{ new Vector2(33, 45), "3Kabdeghnopqsuy£µÝàáâãäåèéêëðñòóôõöøùúûüýþÿāăąďđēĕėęěĝğġģĥħĶńņňŉōŏőśŝşšũūŭůűųŶŷŸșЏЗКЛНбдеуцёђћўџ" },
					{ new Vector2(31, 45), "7?J_xz¿ĴźżžЈвнхчъьѓќ•" },
					{ new Vector2(40, 45), "@©®ĲЫ" },
					{ new Vector2(37, 45), "ABDNOQRSÀÁÂÃÄÅÐÑÒÓÔÕÖØĂĄĎĐŃŅŇŌŎŐŔŖŘŚŜŞŠȘЅЊЖф□" },
					{ new Vector2(32, 45), "FTck¢çćĉċčķŢŤŦȚЃЎЧаийкопрсяѕ" },
					{ new Vector2(42, 45), "MmwŵМШЮщ" },
					{ new Vector2(47, 45), "WÆŒŴ…‰" },
					{ new Vector2(28, 45), "\\" },
					{ new Vector2(23, 45), "`ijl ¨¯´¸ìíîïĩīįıĵĺļľłˆˇ˘˙˚˛˜˝ії" },
					{ new Vector2(14, 8), "­" },
					{ new Vector2(27, 45), "°²³¹“”„" },
					{ new Vector2(43, 45), "¼¾Љ" },
					{ new Vector2(45, 45), "½Щ" },
					{ new Vector2(44, 45), "æœ" },
					{ new Vector2(29, 45), "ĳґ" },
					{ new Vector2(41, 45), "мшњ" },
					{ new Vector2(38, 45), "љ" },
					{ new Vector2(46, 45), "—™" },
					{ new Vector2(53, 52), "" }
				}
			}
		};

		static readonly IEnumerable<string> _textures = new List<string>
		{
			"Offline",
			"Online",
			"Arrow",
			"Cross",
			"Danger",
			"No Entry",
			"Construction",
			"White screen",
			"Grid"
		};

		static readonly IEnumerable<string> _sprites = new List<string>
		{
			"Arrow",
			"Blank",
			"Circle",
			"Circle_Hollow",
			"Cross",
			"DangerZone",
			"default_offline",
			"default_online",
			"LCD_Grid",
			"Left_Bracket",
			"NoEntry",
			"Right_Bracket",
			"Right_Triangle",
			"Semi_Circle",
			"Square_Hollow",
			"Triangle",
			"UnderConstruction",
			"WhiteSprite"
		};

		static readonly IEnumerable<string> _scripts = new List<string>
		{
			"ClockAnalog",
			"ArtificialHorizon",
			"EnergyHydrogen",
			"Gravity",
			"Velocity",
			"ClockDigital"
		};
	}
}
