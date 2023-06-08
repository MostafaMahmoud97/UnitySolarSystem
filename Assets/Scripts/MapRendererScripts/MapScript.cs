using Microsoft.Geospatial;
using Microsoft.Maps.Unity;
using System.Collections;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using WeatherHelper;

public class MapScript : MonoBehaviour
{
    [Header("Map Render")]
    [SerializeField] private MapRenderer map;
    [SerializeField] private double lat;
    [SerializeField] private double lang;

    [Header("Date Display")]
    [SerializeField] private GameObject DisplayTime;
    [SerializeField] private GameObject DisplayDate;

    [Header("Weather Display")]
    [SerializeField] private GameObject DisplayWeather;
    [SerializeField] private Image ImageWeather;
    [SerializeField] private GameObject TempDisplay;
    [SerializeField] private GameObject WeatherPanelGameObject;



    private string baseUrl = "https://api.openweathermap.org/data/2.5/weather";
    private readonly string apiKey = "f1c4115f230a7818226e1e309b1d6cb8";
    private string getURL = "";

    private string ImageBaseUrl = "http://openweathermap.org/img/wn/";
    private Sprite ImageLogoSpirate;

    

    // Start is called before the first frame update
    void Start()
    {
        this.InitMap();
        this.ShowDate();
        this.InitUrl();
        StartCoroutine(SimpleGetRequest());
        
    }

    // Update is called once per frame
    void Update()
    {
        this.ShowTime();
    }

    private void InitMap()
    {
        map = GetComponent<MapRenderer>();

        //Debug.Log(map.Center);
        var newCenter = new LatLon(this.lat, this.lang);
        map.Center = newCenter;
    }

    private void ShowTime()
    {
        int hour = System.DateTime.Now.Hour;
        int Minutes = System.DateTime.Now.Minute;
        int Seconds = System.DateTime.Now.Second;
        DisplayTime.GetComponent<Text>().text = "" + hour + ":" + Minutes + ":" + Seconds;
    }

    private void ShowDate()
    {
        int day = System.DateTime.Now.Day;
        int month = System.DateTime.Now.Month;
        int year = System.DateTime.Now.Year;
        DisplayDate.GetComponent<Text>().text = "" + day + "/" + month + "/" + year;
    }

    private void ShowWeather(WeatherHelpClass Weather)
    {
        DisplayWeather.GetComponent<Text>().text = Weather.main_weather;
        TempDisplay.GetComponent<Text>().text = Weather.temp + " F°";

        ImageBaseUrl += Weather.icon_weather + "@2x.png";
        StartCoroutine(GetImageWeather(ImageBaseUrl));
        ShowDataInWeatherPanel(Weather);
    }

    private void ShowDataInWeatherPanel(WeatherHelpClass Weather)
    {
        //Debug.Log(WeatherPanelGameObject.transform.GetChild(1).gameObject);
        // Weather Text
        WeatherPanelGameObject.transform.GetChild(1).gameObject.GetComponent<Text>().text = Weather.main_weather;
        // Weather Temp
        WeatherPanelGameObject.transform.GetChild(2).gameObject.GetComponent<Text>().text = Weather.temp + " F°";
        // Weather Description
        WeatherPanelGameObject.transform.GetChild(4).gameObject.GetComponent<Text>().text = Weather.description_weather;
        // Wind Speed
        WeatherPanelGameObject.transform.GetChild(6).gameObject.GetComponent<Text>().text = "speed : "+Weather.speed_wind + " (miles/hour)";
        // Wind Deg
        WeatherPanelGameObject.transform.GetChild(7).gameObject.GetComponent<Text>().text = "deg : "+Weather.deg_wind + " wind direction";
        // Guest Wind
        WeatherPanelGameObject.transform.GetChild(8).gameObject.GetComponent<Text>().text = "guest : " + Weather.gust_wind + " (miles/hour)";
        // cloudness
        WeatherPanelGameObject.transform.GetChild(10).gameObject.GetComponent<Text>().text = "Cloudiness : " + Weather.all_cloud + " %";
        // rain
        WeatherPanelGameObject.transform.GetChild(12).gameObject.GetComponent<Text>().text = "1h : " + Weather.rain_1h;
        WeatherPanelGameObject.transform.GetChild(13).gameObject.GetComponent<Text>().text = "3h : " + Weather.rain_3h;
        // snow
        WeatherPanelGameObject.transform.GetChild(15).gameObject.GetComponent<Text>().text = "1h : " + Weather.snow_1h;
        WeatherPanelGameObject.transform.GetChild(16).gameObject.GetComponent<Text>().text = "3h : " + Weather.snow_3h;
        //Main
        WeatherPanelGameObject.transform.GetChild(18).gameObject.GetComponent<Text>().text = "temp : " + Weather.temp + " F°";
        WeatherPanelGameObject.transform.GetChild(19).gameObject.GetComponent<Text>().text = "feel_like : " + Weather.feels_like + " F°";
        WeatherPanelGameObject.transform.GetChild(20).gameObject.GetComponent<Text>().text = "temp_min : " + Weather.temp_min + " F°";
        WeatherPanelGameObject.transform.GetChild(21).gameObject.GetComponent<Text>().text = "temp_max : " + Weather.temp_max + " F°";
        WeatherPanelGameObject.transform.GetChild(22).gameObject.GetComponent<Text>().text = "pressure : " + Weather.pressure + " hPa";
        WeatherPanelGameObject.transform.GetChild(23).gameObject.GetComponent<Text>().text = "humidity : " + Weather.humidity + " %";
        WeatherPanelGameObject.transform.GetChild(24).gameObject.GetComponent<Text>().text = "see_level : " + Weather.sea_level + " hPa";
        WeatherPanelGameObject.transform.GetChild(25).gameObject.GetComponent<Text>().text = "grnd_level : " + Weather.grnd_level + " hPa";
        WeatherPanelGameObject.transform.GetChild(26).gameObject.GetComponent<Text>().text = "visibility : " + Weather.visibility + " KM";

    }

    private void InitUrl()
    {
        this.getURL = this.baseUrl + "?lat=" + this.lat + "&lon=" + this.lang + "&units=imperial" + "&appid=" + this.apiKey;
    }

    IEnumerator SimpleGetRequest()
    {
        UnityWebRequest www = UnityWebRequest.Get(this.getURL);

        yield return www.SendWebRequest();

        if(www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(www.error);
        }
        else
        {

            WeatherHelpClass weather = new WeatherHelpClass();
            string response = weather.FixStringJson(www.downloadHandler.text);
            weather = JsonUtility.FromJson<WeatherHelpClass>(response);
            ShowWeather(weather);

        }
    }

    IEnumerator GetImageWeather(string path)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(path);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(www.error);
        }
        else
        {
            Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            ImageLogoSpirate = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f));
            ImageWeather.sprite = ImageLogoSpirate;

            GameObject ImageLogoPanel = WeatherPanelGameObject.transform.GetChild(0).gameObject;
            ImageLogoPanel.GetComponent<Image>().sprite = ImageLogoSpirate;
        }
    }

}
