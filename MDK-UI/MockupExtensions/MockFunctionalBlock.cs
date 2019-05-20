using System.Windows.Media;

namespace IngameScript.Mockups.Base
{
    public partial class MockFunctionalBlock
    {
        public virtual Brush Preview
        {
            get
            {
                if (!Enabled)
                {
                    return new SolidColorBrush(new Color
                    {
                        R = 255,
                        G = 0,
                        B = 0,
                        A = 255
                    });
                }

                return new SolidColorBrush(new Color
                {
                    A = 0
                });
            }
        }
    }
}
