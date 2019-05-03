//%HEADER%
using System.Collections.Generic;

namespace IngameScript.Mockups.Base
{
    public partial class MockTextSurface
	{
        static readonly IReadOnlyDictionary<string, string> _fonts = new Dictionary<string, string>
		{
//%FONTS%
        };
                
		static readonly IEnumerable<string> _textures = new List<string>
		{
//%TEXTURES%
        };
        
		static readonly IEnumerable<string> _sprites = new List<string>
		{
//%SPRITES%
        };
        
		static readonly IEnumerable<string> _scripts = new List<string>
		{
//%SCRIPTS%
        };
    }
}
