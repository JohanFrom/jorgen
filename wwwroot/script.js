// Google
const YOUR_CLIENT_ID = '501142797428-ksq5fut5ocb23qpm0aeps8gekk24vkbk.apps.googleusercontent.com';
const YOUR_REDIRECT_URI = 'https://jorgen-67273.azurewebsites.net';
var fragmentString = location.hash.substring(1);

function fetchData() {
    // New elements
    const pingTag = document.createElement("p");
    const img = document.createElement("img");

    // Get elements
    const imageContainer = document.getElementById("img-container");
    const mainConainer = document.getElementById("main");

    const temp = document.getElementById("temp");
    const pressure = document.getElementById("pressure");
    const humidity = document.getElementById("humidity");
    const speed = document.getElementById("speed");
    const hometowm = document.getElementById("hometown");

    // Set attributes
    img.setAttribute("src", "https://jorgen-67273.azurewebsites.net/Jorgen");
    img.setAttribute("height", "250");
    img.setAttribute("width", "250");
    img.setAttribute("alt", "jorgen_img");
    imageContainer.appendChild(img);

    try {
        fetch("https://jorgen-67273.azurewebsites.net/Weather/getweather", {
            headers: {
                method: 'GET',
                "Content-Type": "text/plain"
            }
        }).then(response => {
            if (!response.ok) {
                throw new Error("HTTP error", + response.status);
            }

            return response.json();
        }).then(json => {
            hometowm.innerHTML = "Jörgens stad Veberöd";
            temp.innerHTML = "Temperature: " + json.temp + " °C";
            pressure.innerHTML = "Pressure: " + json.pressure + " hPa";
            humidity.innerHTML = "Humidity: " + json.humidity + " %";
            speed.innerHTML = "Speed: " + json.speed + " m/s";
            getStatusOfBeard(json.temp)

        }).catch(error => {
            pingTag.innerHTML = `Could not get weather data inside try: ${error.message}`;
            mainConainer.appendChild(pingTag)
        })
    } catch (error) {
        pingTag.innerHTML = `Could not get weather dataoutside try: ${error.message}`;
    }
}

function getStatusOfBeard(temp) {
    const status = document.getElementById("status");
    try {
        fetch(`https://jorgen-67273.azurewebsites.net/Jorgen/statusOfBeard?temp=${temp}`, {
            headers: {
                method: 'GET',
                "Content-Type": "text/plain"
            }
        }).then(response => {

            if (!response.ok) {
                throw new Error("HTTP error", + response.status);
            }

            return response.json()
        }).then(json => {
            status.innerHTML = "Status på jörgens skägg: " + json
        }).catch(error => {
            pingTag.innerHTML = `Could not get weather data inside try: ${error.message}`;
            mainConainer.appendChild(pingTag)
        })
    } catch (error) {
        pingTag.innerHTML = `Could not get weather dataoutside try: ${error.message}`;
    }
}

//Parse query string to see if page request is coming from OAuth 2.0 server.
function setItem() {
    var params = {};
    var regex = /([^&=]+)=([^&]*)/g, m;
    while (m = regex.exec(fragmentString)) {
        params[decodeURIComponent(m[1])] = decodeURIComponent(m[2]);
    }
    if (Object.keys(params).length > 0) {
        localStorage.setItem('oauth2-test-params', JSON.stringify(params));
    }
    else {
        oauth2SignOut()
    }
}



function oauth2SignOut() {
    localStorage.clear();
    window.location.href = "https://jorgen-67273.azurewebsites.net"

    //window.location.reload();
}

function oauth2SignIn() {
    // Google's OAuth 2.0 endpoint for requesting an access token
    var oauth2Endpoint = 'https://accounts.google.com/o/oauth2/v2/auth';

    // Create element to open OAuth 2.0 endpoint in new window.
    var form = document.createElement('form');
    form.setAttribute('method', 'GET'); // Send as a GET request.
    form.setAttribute('action', oauth2Endpoint);

    // Parameters to pass to OAuth 2.0 endpoint.
    var params = {
        'client_id': YOUR_CLIENT_ID,
        'redirect_uri': YOUR_REDIRECT_URI,
        'scope': 'https://www.googleapis.com/auth/drive.metadata.readonly',
        'state': 'try_sample_request',
        'include_granted_scopes': 'true',
        'response_type': 'token'
    };

    // Add form parameters as hidden input values.
    for (var p in params) {
        var input = document.createElement('input');
        input.setAttribute('type', 'hidden');
        input.setAttribute('name', p);
        input.setAttribute('value', params[p]);
        form.appendChild(input);
    }

    // Add form to page and submit it to open the OAuth 2.0 endpoint.
    document.body.appendChild(form);
    form.submit();
}


// Check for localStorage oauth params
if (localStorage.getItem("oauth2-test-params") === null) {
    document.getElementById("logout").style.display = "none"
}

if (document.URL.length > 50 && document.URL.indexOf("#state=try_sample_request&access_token=") >= 0 && document.URL.indexOf("userinfo.profile%20") >= 0) {
    setItem();
    var params = JSON.parse(localStorage.getItem('oauth2-test-params'));
    if (params.access_token !== "") {
        document.getElementById("login").style.display = "none"
        document.getElementById("logout").style.display = "block"
        fetchData()
    }
}