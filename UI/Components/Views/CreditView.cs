using MonoCoC.UI.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoCoC.UI.Components.Views
{
    public class CreditView : HjsonView
    {
        protected override string ScreenFile => "Views\\CreditView.hjson";
        protected override UIViewContract Contract => CreditContract.Value;
    }

    public static class CreditContract
    {
        public static readonly UIViewContract Value = new(
            "CreditView",
            [
                new("credit.text", typeof(TextArea))
            ]);
    }
}
