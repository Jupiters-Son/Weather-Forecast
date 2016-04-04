using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RestSharp;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace WeatherForecast
{
    class RESThandler
    { 
        private string url;
        private IRestResponse response;

        public RESThandler()
        {
            url = "";
        }


        public RESThandler(string lurl)
        {
            url = lurl;
        }

        //Gets the information from the Responce class
        public Response.Current ExecuteRequest()
        {
            var client = new RestClient(url);
            var request = new RestRequest();

            response = client.Execute(request);

            XmlSerializer serializer = new XmlSerializer(typeof(Response.Current));
            Response.Current objRss;

            TextReader sr = new StringReader(response.Content);
            objRss = (Response.Current)serializer.Deserialize(sr);
            return objRss;
        }


        public Responcefivedays.Weatherdata executeRequestfiveday()
        {
            var client = new RestClient(url);
            var request = new RestRequest();

            response = client.Execute(request);

            XmlSerializer serializer = new XmlSerializer(typeof(Responcefivedays.Weatherdata));
            Responcefivedays.Weatherdata objRss;

            TextReader sr = new StringReader(response.Content);
            objRss = (Responcefivedays.Weatherdata)serializer.Deserialize(sr);
            return objRss;
        }

    }
}
