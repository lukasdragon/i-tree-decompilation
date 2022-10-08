// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.WeatherStationValidator
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using Eco.Util.Services;
using i_Tree_Eco_v6.Resources;
using System.Text.RegularExpressions;

namespace i_Tree_Eco_v6
{
  public class WeatherStationValidator
  {
    private string[] _errors;
    private WeatherService _weatherService;
    private ProgramSession _ps;

    public WeatherStationValidator()
    {
      this._errors = new string[2]
      {
        Strings.ErrWeatherStationInvalid,
        Strings.ErrWeatherStationVerification
      };
      this._ps = ProgramSession.GetInstance();
      this._weatherService = new WeatherService(this._ps.LocSp);
    }

    public int Validate(int year, string station_id)
    {
      if (!new Regex("^[A-Z0-9]{6}-[A-Z0-9]{5}$").IsMatch(station_id))
        return 1;
      try
      {
        if (this._weatherService.GetWeatherYearData(station_id, year) == null)
          return 1;
      }
      catch
      {
        return 2;
      }
      return 0;
    }

    public string TranslateError(int error) => error <= 0 || error > this._errors.Length ? string.Empty : this._errors[error - 1];
  }
}
