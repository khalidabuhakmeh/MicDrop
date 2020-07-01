using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MicDrop
{
    public partial class MainPage : ContentPage
    {
        private static readonly Random Rng = new Random();
        private bool _holding = true;

        public string OpenTitleText => Application.Current.Resources[nameof(OpenTitleText)] as string;
        public string CloseTitleText => Application.Current.Resources[nameof(CloseTitleText)] as string;
        public string OpenHandEmoji => Application.Current.Resources[nameof(OpenHandEmoji)] as string;
        public string CloseHandEmoji => Application.Current.Resources[nameof(CloseHandEmoji)] as string;
        public double TitleScale => Application.Current.Resources[nameof(TitleScale)] as double? ?? 0;
        public double Bottom => Application.Current.Resources[nameof(Bottom)] as double? ?? 0;
        public uint AnimationLength => Application.Current.Resources[nameof(AnimationLength)] as uint? ?? 250;
        public uint MaxRotation => Application.Current.Resources[nameof(MaxRotation)] as uint? ?? 720;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnMicDrop(object sender, EventArgs e)
        {
            if (!_holding) return;
            
            _holding = false;
            hand.Text = OpenHandEmoji;
            title.Text = OpenTitleText;

            canvas.RaiseChild(mic);

            await Task.WhenAll(
                title.ScaleTo(title.Scale * TitleScale, easing: Easing.CubicInOut),
                mic.RotateTo(Rng.Next(1, (int) MaxRotation), AnimationLength, easing: Easing.SinInOut),
                mic.TranslateTo(0, (Height/ 2) - Bottom, AnimationLength, easing: Easing.BounceOut)
            );

            await mic.FadeTo(0);
            
            hand.Text = CloseHandEmoji;
            title.Text = CloseTitleText;
            
            // reset location
            await Task.WhenAll(
                title.ScaleTo(title.Scale / TitleScale, easing: Easing.CubicInOut),
                mic.TranslateTo(0, 0, 0, Easing.Linear),
                mic.RotateTo(45, 0, Easing.Linear),
                mic.FadeTo(1, 0)
            );

            canvas.RaiseChild(hand);
            
            _holding = true;
        }
    }
}