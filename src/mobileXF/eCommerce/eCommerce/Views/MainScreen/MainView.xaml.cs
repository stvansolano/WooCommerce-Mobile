using System.Linq;
using System.Threading.Tasks;
using Plugin.SharedTransitions;
using WooCommerceNET.WooCommerce.v3;
using Xamarin.Forms;

namespace eCommerce.Views
{
    public partial class MainView
    {
        private double _height;
        private bool _isCartVisible;

        public MainView()
        {
            InitializeComponent();

            GridRow2.GestureRecognizers.Add(new SwipeGestureRecognizer { Direction = SwipeDirection.Up,
                Command = new Command(async () => await SwipeGestureRecognizer_SwipedUp()) });
            GridRow2.GestureRecognizers.Add(new SwipeGestureRecognizer { Direction = SwipeDirection.Down,
                Command = new Command(async () => await SwipeGestureRecognizer_SwipedDown())
            });

            _height = (Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Height / Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Density) - 80;

            //ProductOverview.SelectionChanged += ProductOverview_SelectionChanged;
        }

        async Task SwipeGestureRecognizer_SwipedUp()
        {
            if (_isCartVisible)
                return;

            _isCartVisible = true;

            MenuBarRow1.Height = 0;

            var animateGridRow1 = new Animation(v => GridRow1.HeightRequest = v, _height, 80);
            animateGridRow1.Commit(this, "GridRow1AnimationIn", 16, 200, Easing.CubicInOut);

            var animateGridRow2 = new Animation(v => GridRow2.HeightRequest = v, 80, _height);
            animateGridRow2.Commit(this, "GridRow2AnimationIn", 16, 200, Easing.CubicInOut);

            var animateCartSmall = new Animation(v => CartSmall.HeightRequest = v, 54, 0);
            animateCartSmall.Commit(this, "CartSmallAnimationIn", 16, 400, Easing.CubicInOut);

            await Task.WhenAll(
                MenuBarTitle.FadeTo(0, easing: Easing.CubicInOut),
                MenuBarIcon.FadeTo(0, easing: Easing.CubicInOut),
                TopGradient.FadeTo(0, easing: Easing.CubicInOut),
                CartSmall.FadeTo(0, easing: Easing.CubicInOut)
            );

            MenuBarTitle.IsVisible = false;
            MenuBarIcon.IsVisible = false;
            TopGradient.IsVisible = false;
            CartSmall.IsVisible = false;
        }

        async Task SwipeGestureRecognizer_SwipedDown()
        {
            if (!_isCartVisible)
                return;

            _isCartVisible = false;

            MenuBarRow1.Height = 54;

            var animateGridRow1 = new Animation(v => GridRow1.HeightRequest = v, 80, _height);
            animateGridRow1.Commit(this, "GridRow1AnimationOut", 16, 200, Easing.CubicInOut);

            var animateGridRow2 = new Animation(v => GridRow2.HeightRequest = v, _height, 80);
            animateGridRow2.Commit(this, "GridRow2AnimationOut", 16, 200, Easing.CubicInOut);

            var animateCartSmall = new Animation(v => CartSmall.HeightRequest = v, 0, 54);
            animateCartSmall.Commit(this, "CartSmallAnimationIn", 16, 200, Easing.CubicInOut);

            MenuBarTitle.IsVisible = true;
            MenuBarIcon.IsVisible = true;
            TopGradient.IsVisible = true;
            CartSmall.IsVisible = true;

            await Task.WhenAll(
                MenuBarTitle.FadeTo(1, easing: Easing.CubicInOut),
                MenuBarIcon.FadeTo(1, easing: Easing.CubicInOut),
                TopGradient.FadeTo(1, easing: Easing.CubicInOut),
                CartSmall.FadeTo(1, easing: Easing.CubicInOut)
            );
        }
    }
}