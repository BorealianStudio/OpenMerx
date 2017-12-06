using System.Collections.Generic;


public class ParamHolder{

    [Newtonsoft.Json.JsonProperty]
    private Dictionary<string, string> _data = new Dictionary<string, string>();

    public void Set(string name, int value) {
        if (!_data.ContainsKey(name))
            _data.Add(name, value.ToString());
        else
            _data[name] = value.ToString();
    }

    public void Set(string name, string value) {
        if (!_data.ContainsKey(name)) {
            _data.Add(name, value);
        } else {
            _data[name] = value;
        }
    }

    public void Set(string name, float value) {
        if (!_data.ContainsKey(name)) {
            _data.Add(name, value.ToString());
        } else {
            _data[name] = value.ToString();
        }
    }

    public void Set(string name, double value) {
        if (!_data.ContainsKey(name)) {
            _data.Add(name, value.ToString());
        } else {
            _data[name] = value.ToString();
        }
    }

    // ********************************************************
    //                  get stuff
    // ********************************************************

    public int GetInt(string name, int defaultValue) {
        if (_data.ContainsKey(name)) {
            int result = defaultValue;
            if(int.TryParse(_data[name],out result)){
                return result;
            } else {
                throw new System.Exception("Cannot parse int : " + _data[name]);
            }
        }
        return defaultValue;
    }

    public int GetInt(string name) {
        if (_data.ContainsKey(name)) {
            int result = 0;
            if (int.TryParse(_data[name], out result)) {
                return result;
            } else {
                throw new System.Exception("Cannot parse int : " + _data[name]);
            }
        }
        throw new System.Exception("key not present in paramHolder : " + name);
    }

    public string GetString(string name, string defaultValue) {
        if (_data.ContainsKey(name)) {
            return _data[name];        
        }
        return defaultValue;
    }

    public string GetString(string name) {
        if (_data.ContainsKey(name)) {
            return _data[name];
        }
        throw new System.Exception("key not present in paramHolder : " + name);
    }
}
