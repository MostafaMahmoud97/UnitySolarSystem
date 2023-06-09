


using System.Diagnostics;

namespace WeatherHelper
{
    public class WeatherHelpClass
    {

        //coord
        public double lon;
        public double lat;

        //weather
        public int id_weather;
        public string main_weather;
        public string description_weather;
        public string icon_weather;

        //weather Main
        public double temp;
        public double feels_like;
        public double temp_min;
        public double temp_max;
        public double pressure;
        public double humidity;
        public double sea_level;
        public double grnd_level;

        //wind
        public double speed_wind;
        public double deg_wind;
        public double gust_wind;

        //cloud
        public double all_cloud;

        //rain
        public double rain_1h;
        public double rain_3h;

        //snow
        public double snow_1h;
        public double snow_3h;

        //Visibility
        public double visibility;




        public string FixStringJson(string JsonString)
        {
            string response = JsonString;

            response = response.Replace("\"coord\":{", "");
            response = response.Replace("\"main\":{", "");
            response = response.Replace("},\"weather\":[{\"id\"", ",\"id_weather\"");
            response = response.Replace("main", "main_weather");
            response = response.Replace("description", "description_weather");
            response = response.Replace("icon", "icon_weather");
            response = response.Replace("}],\"base\":\"stations\"", "");
            response = response.Replace("\"wind\":{", "");
            response = response.Replace("speed", "speed_wind");
            response = response.Replace("deg", "deg_wind");
            response = response.Replace("gust", "gust_wind");
            response = response.Replace("},\"clouds\":{\"all\"", ",\"all_cloud\"");
            response = response.Replace("\"rain\":{\"1h\"", "\"rain_1h\"");
            response = response.Replace("\"rain\":{\"3h\"", "\"rain_3h\"");
            response = response.Replace("\"snow\":{\"3h\"", "\"snow_1h\"");
            response = response.Replace("\"snow\":{\"3h\"", "\"snow_3h\"");
            response = response.Replace("\"sys\":{", "");
            response = response.Replace("},", ",");

            return response;
        }

    }


    
}


