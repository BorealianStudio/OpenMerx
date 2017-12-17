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

public class LinkInfo {

    public int FromID { get; set; }
    public string FromParam { get; set; }
    public int ToID { get; set; }
    public string ToParam { get; set; }
    public ParamInfos.ParamType LinkType { get; set; }

    public LinkInfo(int fromID, string fromParam, int toID, string toParam, ParamInfos.ParamType linkType) {
        FromID = fromID;
        FromParam = fromParam;
        ToID = toID;
        ToParam = toParam;
        LinkType = linkType;
    }
}
