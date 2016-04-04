using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Net;
using Android.Graphics;
using Java.IO;
using Android.Graphics.Drawables;
using Android.Util;
using System.Net;
using System.IO;


namespace WeatherForecast
{

    public class DataAdapter : BaseAdapter<Responcefivedays.Time>
    {

        List<Responcefivedays.Time> items;

        Activity context;
        public DataAdapter(Activity context, List<Responcefivedays.Time> items)
            : base()
        {
            this.context = context;
            this.items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override Responcefivedays.Time this[int position]
        {
            get { return items[position]; }
        }
        public override int Count
        {
            get { return items.Count; }
        }
        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }


        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];
            View view = convertView;
            if (view == null) // no view to re-use, create new
                view = context.LayoutInflater.Inflate(Resource.Layout.CustomRow, null);

            view.FindViewById<TextView>(Resource.Id.txtDate).Text = item.Day;
            view.FindViewById<TextView>(Resource.Id.txtMin).Text = item.Temperature.Min;
            view.FindViewById<TextView>(Resource.Id.txtMax).Text = item.Temperature.Max;
            view.FindViewById<TextView>(Resource.Id.txtWeather).Text = item.Clouds.Value;
            var Image = GetImageBitmapFromUrl("http://openweathermap.org/img/w/" + item.Symbol.Var + ".png");
            view.FindViewById<ImageView>(Resource.Id.Image).SetImageBitmap(Image);
            //if (item.Enclosure.Count > 0)

            //{
            //    var imageBitmap = GetImageBitmapFromUrl(item.Enclosure[0].Url);

            //}

            return view;
        }

        //private Bitmap GetImageBitmapFromUrl(string url)
        //{
        //    Bitmap imageBitmap = null;
        //    if (!(url == "null"))
        //        using (var webClient = new WebClient())
        //        {
        //            var imageBytes = webClient.DownloadData(url);
        //            if (imageBytes != null && imageBytes.Length > 0)
        //            {
        //                imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
        //            }
        //        }

        //    return imageBitmap;
        //}

    }
}
