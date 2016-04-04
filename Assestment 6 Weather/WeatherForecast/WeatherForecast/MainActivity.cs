using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Net;
using Android.Graphics;

namespace WeatherForecast
{
    [Activity(Label = "WeatherForecast", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        RESThandler objRest;

        TextView txtLocation;
        TextView txtTemperature;
        TextView txtWeather;
        TextView txtMax;
        TextView txtMin;
        TextView txtDateTime;
        Button btnSearch;
        AlertDialog alertdialog;
        string textFindLocation;
        ListView listWeather;
        ImageView imgIcon;
        string todayDateTime;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            textFindLocation = "Hamilton";
            todayDateTime = DateTime.Now.ToString();

            // Get our button from the layout resource,
            // and attach an event to it

            txtLocation = FindViewById<TextView>(Resource.Id.txtLocation);
            txtTemperature = FindViewById<TextView>(Resource.Id.txtTemperature);
            txtWeather = FindViewById<TextView>(Resource.Id.txtWeather);
            txtMax = FindViewById<TextView>(Resource.Id.txtMax);
            txtMin = FindViewById<TextView>(Resource.Id.txtMin);
            btnSearch = FindViewById<Button>(Resource.Id.btnSearch);
            listWeather = FindViewById<ListView>(Resource.Id.listviewWeather);
            imgIcon = FindViewById<ImageView>(Resource.Id.imgIcon);
            txtDateTime = FindViewById<TextView>(Resource.Id.txtDateTime);

            LoadLocationAsync();



            btnSearch.Click += BtnSearch_Click;
        } 
        
        //opens a window to search for another location around the world. For Example London.

        private void BtnSearch_Click(object sender, EventArgs e)
    {

            openprompt();

        }
       
            public void openprompt()
        {
            alertdialog = new AlertDialog.Builder(this).Create();
            EditText txtCallLocation = new EditText(this);

            alertdialog.SetTitle("enter Location");
            alertdialog.SetView(txtCallLocation);
            alertdialog.SetButton("Find", (s, ev) =>
            {
                Toast.MakeText(this, txtCallLocation.Text, ToastLength.Long).Show();
                textFindLocation = txtCallLocation.Text;
                LoadLocationAsync();
            });
   
            alertdialog.SetCancelable(false);
            alertdialog.Show();

        }

        // gets the Weather icons from the internet
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

        //Loads the current Weatehr information on the phone and fires a function to load the next five days aswell
        private void LoadLocationAsync()
        {
            objRest = new RESThandler(@"http://api.openweathermap.org/data/2.5/weather?APPID=e44aa6ea2f31203ad91da4660b11fced&mode=xml&q=" + textFindLocation + "%2CNZ");
            var Response = objRest.ExecuteRequest();
            var imageIcon = GetImageBitmapFromUrl("http://openweathermap.org/img/w/" + Response.Weather.Icon + ".png");
            imgIcon.SetImageBitmap(imageIcon);

            txtLocation.Text = Response.City.Name;
            txtTemperature.Text = Convert.ToString(Convert.ToDouble(Response.Temperature.Value.ToString())-273.15 + "°C");
            txtWeather.Text = Response.Clouds.Name;
            txtMax.Text = "Max=" + Convert.ToString(Convert.ToDouble(Response.Temperature.Max.ToString())-273.15 + "°C");
            txtMin.Text = "Min=" + Convert.ToString(Convert.ToDouble(Response.Temperature.Min.ToString())-273.15 + "°C");
            txtDateTime.Text = todayDateTime;
            LoadLocationfiveAsync();
        }

        // Loads the next five days of Weather information and displays it in a listview formatted by the Custom Row
        private void LoadLocationfiveAsync()
        {
            objRest = new RESThandler(@"http://api.openweathermap.org/data/2.5/forecast/daily?q=" + textFindLocation + ",NZ&mode=xml&units=metric&cnt=5&appid=e44aa6ea2f31203ad91da4660b11fced");
            var Response = objRest.executeRequestfiveday();

            listWeather.Adapter = new DataAdapter(this, Response.Forecast.Time);

        }



    }
}

