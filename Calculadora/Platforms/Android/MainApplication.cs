using Android.App;
using Android.Content.Res;
using Android.Runtime;

namespace Calculadora
{
    [Application]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(Entry), (handle, view) =>
            {
                if (view is Entry)
                {
                    //Remove underline
                    handle.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
                    handle.PlatformView.ShowSoftInputOnFocus = false;
                }
            });
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
