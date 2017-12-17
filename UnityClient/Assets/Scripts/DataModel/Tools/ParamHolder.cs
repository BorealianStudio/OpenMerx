// MIT License

// Copyright(c) 2017 Andre Plourde
// part of https://github.com/BorealianStudio/OpenMerx

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.


// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

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
