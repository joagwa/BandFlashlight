using Android.App;
using Android.Widget;
using Android.OS;
using Com.Microsoft.Band;
using Android.Graphics;
using Com.Microsoft.Band.Tiles;
using Java.Util;
using Com.Microsoft.Band.Tiles.Pages;
using System.Threading.Tasks;

namespace BandFlashlight
{
	enum TileLayoutIndex{MessagesLayout};
	enum TileMessagesPageElementId{ Message1 = 1, Message2 = 2};

	[Activity (Label = "BandFlashlight", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		int count = 1;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);
			var pairedBands = BandClientManager.Instance.GetPairedBands ();
			var bandCLient = BandClientManager.Instance.Create (this, pairedBands [0]);

			var pendingResult = bandCLient.Connect ();
			try {
				ConnectionState state;
				var pending = Task.Run(()=> pendingResult.Await ());
				while (!pending.IsCompleted) {

				}
				state = (ConnectionState)pending.Result;
				if (state == ConnectionState.Connected) {
					
					try {
						var tiles = bandCLient.TileManager.Tiles.Await ();
					} catch (System.Exception ex) {
						System.Diagnostics.Debug.WriteLine (ex);
					}

					try {
						var tileCapacity = bandCLient.TileManager.RemainingTileCapacity.Await ();
					} catch (System.Exception ex) {
						System.Diagnostics.Debug.WriteLine (ex);

					}

					var smallIconBitmap = Bitmap.CreateBitmap (24, 24, Bitmap.Config.Argb4444);
					var smallIcon = BandIcon.ToBandIcon (smallIconBitmap);

					var tileIconBitmap = Bitmap.CreateBitmap (46, 46, Bitmap.Config.Argb4444);
					var tileIcon = BandIcon.ToBandIcon (tileIconBitmap);

					var tileUuid = UUID.RandomUUID ();

		


					var panel = new FilledPanel (new PageRect (0, 0, 245, 102));
					panel.SetHorizontalAlignment (HorizontalAlignment.Left);
					panel.SetVerticalAlignment (VerticalAlignment.Top);
					panel.SetBackgroundColor (0);
					var layout = new PageLayout (panel);
					BandTile tile = new BandTile.Builder (tileUuid, "FlashLight", tileIcon).SetPageLayouts (layout).Build ();
					try {
						bandCLient.TileManager.AddTile (this, tile).Await ();
					} catch (System.Exception ex) {
						System.Diagnostics.Debug.WriteLine (ex);

					}
				}
			} catch (System.Exception ex) {
				System.Diagnostics.Debug.WriteLine (ex);

			}
		}
	}
}


