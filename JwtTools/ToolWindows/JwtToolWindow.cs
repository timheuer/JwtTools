using Microsoft.VisualStudio.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace JwtTools
{
    public class JwtToolWindow : BaseToolWindow<JwtToolWindow>
    {
        public override string GetTitle(int toolWindowId) => "JWT Tools";

        public override Type PaneType => typeof(Pane);

        public override Task<FrameworkElement> CreateAsync(int toolWindowId, CancellationToken cancellationToken)
        {
            return Task.FromResult<FrameworkElement>(new MyToolWindowControl());
        }

        [Guid("204a3f89-afc3-4957-9eba-e55daa1c6685")]
        internal class Pane : ToolWindowPane
        {
            public Pane()
            {
                BitmapImageMoniker = KnownMonikers.Certificate;
            }
        }
    }
}