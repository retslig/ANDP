var BrowserDetect = {
    init: function () {
        this.browser = this.searchString(this.dataBrowser) || "An unknown browser";
        this.browserVersion = this.searchBrowserVersion(navigator.userAgent)
			|| this.searchBrowserVersion(navigator.appVersion)
			|| "an unknown version";
        this.OS = this.searchString(this.dataOS) || "an unknown OS";
        this.OSVersion = this.searchOSVersion(this.dataOSVersion) || "an unknown OS";
    },
    searchString: function (data) {
        for (var i = 0; i < data.length; i++) {
            var dataString = data[i].string;
            var dataProp = data[i].prop;
            this.versionSearchString = data[i].versionSearch || data[i].identity;
            if (dataString) {
                if (dataString.indexOf(data[i].subString) != -1)
                    return data[i].identity;
            }
            else if (dataProp)
                return data[i].identity;
        }
    },
    searchBrowserVersion: function (dataString) {
        var index = dataString.indexOf(this.versionSearchString);
        if (index == -1) return;
        return parseFloat(dataString.substring(index + this.versionSearchString.length + 1));
    },
    searchOSVersion: function (data) {
        for (var i = 0; i < data.length; i++) {
            var dataString = data[i].string;
            var dataProp = data[i].prop;
            if (dataString) {
                if (dataString.indexOf(data[i].subString) != -1) {
                    if (data[i].identity == "Android") {
                        return this.getAndroidOS();
                    } else {
                        return data[i].identity;
                    }
                }
                    
            }
            else if (dataProp) {
                if (data[i].identity == "Android") {
                    return this.getAndroidOS();
                } else {
                    return data[i].identity;
                }
            }
        }
    },
    getAndroidOS: function () {
        var start = navigator.appVersion.substring(navigator.appVersion.indexOf("Android"));
        var endIndex = start.indexOf(";");
        return start.substring(0, endIndex);
    },
    dataBrowser: [
		{
		    string: navigator.userAgent,
		    subString: "Chrome",
		    identity: "Chrome"
		},
		{ string: navigator.userAgent,
		    subString: "OmniWeb",
		    versionSearch: "OmniWeb/",
		    identity: "OmniWeb"
		},
		{
		    string: navigator.vendor,
		    subString: "Apple",
		    identity: "Safari",
		    versionSearch: "Version"
		},
		{
		    prop: window.opera,
		    identity: "Opera",
		    versionSearch: "Version"
		},
		{
		    string: navigator.vendor,
		    subString: "iCab",
		    identity: "iCab"
		},
		{
		    string: navigator.vendor,
		    subString: "KDE",
		    identity: "Konqueror"
		},
		{
		    string: navigator.userAgent,
		    subString: "Firefox",
		    identity: "Firefox"
		},
		{
		    string: navigator.vendor,
		    subString: "Camino",
		    identity: "Camino"
		},
		{		// for newer Netscapes (6+)
		    string: navigator.userAgent,
		    subString: "Netscape",
		    identity: "Netscape"
		},
		{
		    string: navigator.userAgent,
		    subString: "MSIE",
		    identity: "Explorer",
		    versionSearch: "MSIE"
		},
		{
		    string: navigator.userAgent,
		    subString: "Gecko",
		    identity: "Mozilla",
		    versionSearch: "rv"
		},
		{ 		// for older Netscapes (4-)
		    string: navigator.userAgent,
		    subString: "Mozilla",
		    identity: "Netscape",
		    versionSearch: "Mozilla"
		}
	],
    dataOS: [
		{
		    string: navigator.platform,
		    subString: "Win",
		    identity: "Windows",
		},
		{
		    string: navigator.platform,
		    subString: "Mac",
		    identity: "Mac",
		},
		{
		    string: navigator.userAgent,
		    subString: "iPhone",
		    identity: "iPhone/iPod",
		},
		{
		    string: navigator.platform,
		    subString: "Linux",
		    identity: "Linux",
		}
    ],
    dataOSVersion: [
        {
            //string: navigator.appVersion,
            string: navigator.appVersion,
            subString: "Android",
            identity: "Android",
        },
        {
            string: navigator.appVersion,
            subString: "Windows NT 6.2",
            identity: "8",
        },
        {
            string: navigator.appVersion,
            subString: "Windows NT 6.1",
            identity: "7",
        },
        {
            string: navigator.appVersion,
            subString: "Windows NT 6.0",
            identity: "Vista",
        },
        {
            string: navigator.appVersion,
            subString: "Windows NT 5.2",
            identity: "Server 2003; Windows XP x64 Edition",
        },
        {
            string: navigator.appVersion,
            subString: "Windows NT 5.1",
            identity: "XP",
        },
        {
            string: navigator.appVersion,
            subString: "Windows NT 5.01",
            identity: "2000, Service Pack 1 (SP1)",
        },
        {
            string: navigator.appVersion,
            subString: "Windows NT 5.0",
            identity: "2000",
        },
        {
            string: navigator.appVersion,
            subString: "98; Win 9x 4.90",
            identity: "Millennium Edition (Windows Me)",
        },
        {
            string: navigator.appVersion,
            subString: "Windows 98",
            identity: "98",
        },
        {
            string: navigator.appVersion,
            subString: "Windows 95",
            identity: "95",
        },
        {
            string: navigator.appVersion,
            subString: "Windows CE",
            identity: "CE",
        }
    ]

};
BrowserDetect.init();

/*
Google Nexus

Mozilla/5.0 (Linux; U; Android 2.2; en-us; Nexus One Build/FRF91) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1

Mozilla/5.0 (Linux; U; Android 2.1; en-us; Nexus One Build/ERD62) AppleWebKit/530.17 (KHTML, like Gecko) Version/4.0 Mobile Safari/530.17


Dell

Mozilla/5.0 (Linux; U; Android 1.6; en-gb; Dell Streak Build/Donut AppleWebKit/528.5+ (KHTML, like Gecko) Version/3.1.2 Mobile Safari/ 525.20.1


HTC

Mozilla/5.0 (Linux; U; Android 2.1-update1; de-de; HTC Desire 1.19.161.5 Build/ERE27) AppleWebKit/530.17 (KHTML, like Gecko) Version/4.0 Mobile Safari/530.17

Mozilla/5.0 (Linux; U; Android 2.1-update1; en-us; ADR6300 Build/ERE27) AppleWebKit/530.17 (KHTML, like Gecko) Version/4.0 Mobile Safari/530.17

Mozilla/5.0 (Linux; U; Android 1.6; en-us; WOWMobile myTouch 3G Build/unknown) AppleWebKit/528.5+ (KHTML, like Gecko) Version/3.1.2 Mobile Safari/525.20.1

Mozilla/5.0 (Linux; U; Android 2.2; nl-nl; Desire_A8181 Build/FRF91) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1

HTC_Dream Mozilla/5.0 (Linux; U; Android 1.5; en-ca; Build/CUPCAKE) AppleWebKit/528.5+ (KHTML, like Gecko) Version/3.1.2 Mobile Safari/525.20.1


Motorola

Mozilla/5.0 (Linux; U; Android 2.2; en-us; DROID2 GLOBAL Build/S273) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1

Mozilla/5.0 (Linux; U; Android 3.0; en-us; Xoom Build/HRI39) AppleWebKit/534.13 (KHTML, like Gecko) Version/4.0 Safari/534.13

Mozilla/5.0 (Linux; U; Android 2.2; en-us; Droid Build/FRG22D) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1

Mozilla/5.0 (Linux; U; Android 2.2; en-us; DROID2 GLOBAL Build/S273) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1

Mozilla/5.0 (Linux; U; Android 2.1-update1; en-us; DROIDX Build/VZW) AppleWebKit/530.17 (KHTML, like Gecko) Version/4.0 Mobile Safari/530.17 480X854 motorola DROIDX

Mozilla/5.0 (Linux; U; Android 2.1-update1; en-us; Droid Build/ESE81) AppleWebKit/530.17 (KHTML, like Gecko) Version/4.0 Mobile Safari/530.17

Mozilla/5.0 (Linux; U; Android 2.0; en-us; Droid Build/ESD20) AppleWebKit/530.17 (KHTML, like Gecko) Version/4.0 Mobile Safari/ 530.17 

Samsung

Mozilla/5.0 (Linux; U; Android 2.2; en-gb; GT-P1000 Build/FROYO) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1

Mozilla/5.0 (Linux; U; Android 2.2; en-ca; SGH-T959D Build/FROYO) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1

Mozilla/5.0 (Linux; U; Android 2.2; en-gb; GT-P1000 Build/FROYO) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1

Mozilla/5.0 (Linux; U; Android 2.0.1; en-us; Droid Build/ESD56) AppleWebKit/530.17 (KHTML, like Gecko) Version/4.0 Mobile Safari/530.17

 

Sony

Mozilla/5.0 (Linux; U; Android 2.1-update1; de-de; E10i Build/2.0.2.A.0.24) AppleWebKit/530.17 (KHTML, like Gecko) Version/4.0 Mobile Safari/530.17

Mozilla/5.0 (Linux; <Android Version>; <Build Tag etc.>)
AppleWebKit/<WebKit Rev> (KHTML, like Gecko) Chrome/<Chrome Rev> Mobile
Safari/<WebKit Rev>
Tablet UA:

Mozilla/5.0 (Linux; <Android Version>; <Build Tag etc.>)
AppleWebKit/<WebKit Rev>(KHTML, like Gecko) Chrome/<Chrome Rev>
Safari/<WebKit Rev>
Here's an example of the Chrome user agent string on a Galaxy Nexus:

Mozilla/5.0 (Linux; Android 4.0.4; Galaxy Nexus Build/IMM76B)
AppleWebKit/535.19 (KHTML, like Gecko) Chrome/18.0.1025.133 Mobile
Safari/535.19

*/